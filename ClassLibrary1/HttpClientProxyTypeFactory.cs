using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using ClassLibrary1.Filters;

namespace ClassLibrary1
{
    public class HttpClientProxyTypeFactory : HttpClientProxyTypeFactory<EmptyConstructorParameters>
    {
    }

    public class HttpClientProxyTypeFactory<TConstructorParameters>
    {
        public IHttpClientProxyFactory<TInterface, TConstructorParameters> CreateFactory<TInterface>()
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            var typeBuilder = moduleBuilder.DefineType("Type");

            typeBuilder.AddInterfaceImplementation(typeof(TInterface));

            var proxyField = CreateField<IHttpClientProxy<TConstructorParameters>>(typeBuilder, "_proxy");
            var httpClientField = CreateField<HttpClient>(typeBuilder, "_client");
            var additionalParametersField = CreateField<TConstructorParameters>(typeBuilder, "_parameters");

            CreateConstructor(typeBuilder, proxyField, httpClientField, additionalParametersField);

            var methods = typeof(TInterface).GetMethods();

            var callDetails = new List<CallDetails<TConstructorParameters>>();

            for(var i = 0; i < methods.Length; i++)
            {
                callDetails.Add(CreateMethod<TInterface>(typeBuilder, httpClientField, additionalParametersField, proxyField, i, methods[i]));
            }

            var info = typeBuilder.CreateTypeInfo();

            return new HttpClientProxyFactory<TInterface, TConstructorParameters>(info, new HttpClientProxy<TConstructorParameters>(callDetails));
        }

        private static FieldBuilder CreateField<T>(TypeBuilder builder, string name)
        {
            return builder.DefineField(name, typeof(T), FieldAttributes.Private);
        }

        private static void CreateConstructor(TypeBuilder builder, FieldInfo proxyField, FieldInfo httpClientField, FieldInfo additionalParameters)
        {
            var constructor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IHttpClientProxy<TConstructorParameters>), typeof(HttpClient), typeof(TConstructorParameters) });

            var generator = constructor.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, proxyField);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Stfld, httpClientField);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_3);
            generator.Emit(OpCodes.Stfld, additionalParameters);

            generator.Emit(OpCodes.Ret);
        }

        private CallDetails<TConstructorParameters> CreateMethod<TInterface>(TypeBuilder builder, FieldInfo httpClientField, FieldInfo constructorParameters, FieldInfo proxyField, int methodId, MethodInfo method)
        {
            var parameters = method.GetParameters();

            var defineMethod = builder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, method.ReturnType, method.GetParameters().Select(p => p.ParameterType).ToArray());

            int? cancellationTokenParameter = null;

            foreach (var parameter in parameters)
            {
                defineMethod.DefineParameter(parameter.Position + 1, ParameterAttributes.None, parameter.Name);

                if (parameter.ParameterType == typeof(CancellationToken))
                {
                    cancellationTokenParameter = parameter.Position;
                }
            }

            var generator = defineMethod.GetILGenerator();

            var genericArg = method.ReturnType.GenericTypeArguments[0];

            var proxyMethod = typeof(IHttpClientProxy<TConstructorParameters>).GetMethod(nameof(IHttpClientProxy<TConstructorParameters>.CallClient))?.MakeGenericMethod(genericArg);

            var cancellationTokenMethod = typeof(CancellationToken).GetProperty(nameof(CancellationToken.None)).GetMethod;

            var localParameterArray = generator.DeclareLocal(typeof(object[]));

            // create an array and assign to local variable
            generator.Emit(OpCodes.Ldc_I4, parameters.Length);
            generator.Emit(OpCodes.Newarr, typeof(object));
            generator.Emit(OpCodes.Stloc, localParameterArray);

            // add each parameter to the array
            for (var i = 0; i < parameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldloc, localParameterArray);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Ldarg, i + 1);
                generator.Emit(OpCodes.Stelem, typeof(object));
            }

            LocalBuilder localCancellationToken = null;

            if (cancellationTokenParameter == null)
            {
                localCancellationToken = generator.DeclareLocal(typeof(CancellationToken));

                generator.Emit(OpCodes.Call, cancellationTokenMethod);
                generator.Emit(OpCodes.Stloc, localCancellationToken);
            }

            // call CallClient method
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, proxyField);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, httpClientField);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, constructorParameters);
            generator.Emit(OpCodes.Ldc_I4, methodId);
            generator.Emit(OpCodes.Ldloc, localParameterArray);

            if (cancellationTokenParameter == null)
            {
                generator.Emit(OpCodes.Ldloc, localCancellationToken);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg, cancellationTokenParameter.Value);
            }

            generator.Emit(OpCodes.Call, proxyMethod);

            // return value from CallClient method
            generator.Emit(OpCodes.Ret);

            var callDetails = new CallDetails<TConstructorParameters>
            {
                ParameterIndices = method.GetParameters().ToDictionary(pi => pi.Name, pi => pi.Position)
            };

            var parameterDictionary = method.GetParameters().ToDictionary(x => x.Name, x => new RequestParameter(x.ParameterType, x.GetCustomAttributes()));

            var validationEventArgs = new ValidationFilterEventArgs(typeof(TConstructorParameters), parameterDictionary, genericArg);

            PopulateCallDetails(callDetails, validationEventArgs, typeof(TInterface).GetCustomAttributes());

            PopulateCallDetails(callDetails, validationEventArgs, method.GetCustomAttributes());

            PopulateRequestParameterCallDetails(callDetails, validationEventArgs, method);

            return callDetails;
        }

        private static void PopulateCallDetails(CallDetails<TConstructorParameters> callDetails, IValidationFilterEventArgs eventArgs, IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                if (attribute is IValidationFilter validationFilter && !validationFilter.Validate(eventArgs))
                    throw new TypeLoadException();

                if (attribute is IRequestFilter<TConstructorParameters> requestFilterWithArgs)
                {
                    callDetails.RequestFilters.Add(new RequestFilterWrapperAsync<TConstructorParameters>(requestFilterWithArgs));
                }

                if (attribute is IRequestFilterAsync<TConstructorParameters> requestFilterWithArgsAsync)
                {
                    callDetails.RequestFilters.Add(requestFilterWithArgsAsync);
                }

                if (attribute is IRequestFilter requestFilter)
                {
                    callDetails.RequestFilters.Add(new RequestFilterWrapperAsync<TConstructorParameters>(new RequestFilterWrapper<TConstructorParameters>(requestFilter)));
                }

                if (attribute is IRequestFilterAsync requestFilterAsync)
                {
                    callDetails.RequestFilters.Add(new RequestFilterAsyncWrapper<TConstructorParameters>(requestFilterAsync));
                }

                if (attribute is IResponseFilter<TConstructorParameters> responseFilterWithArgs)
                {
                    callDetails.ResponseFilters.Add(new ResponseFilterWrapperAsync<TConstructorParameters>(responseFilterWithArgs));
                }

                if (attribute is IResponseFilterAsync<TConstructorParameters> responseFilterWithArgsAsync)
                {
                    callDetails.ResponseFilters.Add(responseFilterWithArgsAsync);
                }

                if (attribute is IResponseFilter responseFilter)
                {
                    callDetails.ResponseFilters.Add(new ResponseFilterWrapperAsync<TConstructorParameters>(new ResponseFilterWrapper<TConstructorParameters>(responseFilter)));
                }

                if (attribute is IResponseFilterAsync responseFilterAsync)
                {
                    callDetails.ResponseFilters.Add(new ResponseFilterAsyncWrapper<TConstructorParameters>(responseFilterAsync));
                }

                if (attribute is IExceptionFilter<TConstructorParameters> exceptionFilterWithArgs)
                {
                    callDetails.ExceptionFilters.Add(new ExceptionFilterWrapperAsync<TConstructorParameters>(exceptionFilterWithArgs));
                }

                if (attribute is IExceptionFilterAsync<TConstructorParameters> exceptionFilterWithArgsAsync)
                {
                    callDetails.ExceptionFilters.Add(exceptionFilterWithArgsAsync);
                }

                if (attribute is IExceptionFilter exceptionFilter)
                {
                    callDetails.ExceptionFilters.Add(new ExceptionFilterWrapperAsync<TConstructorParameters>(new ExceptionFilterWrapper<TConstructorParameters>(exceptionFilter)));
                }

                if (attribute is IExceptionFilterAsync exceptionFilterAsync)
                {
                    callDetails.ExceptionFilters.Add(new ExceptionFilterAsyncWrapper<TConstructorParameters>(exceptionFilterAsync));
                }
            }
        }

        private static void PopulateRequestParameterCallDetails(CallDetails<TConstructorParameters> callDetails, IValidationFilterEventArgs eventArgs, MethodInfo method)
        {
            foreach (var parameter in method.GetParameters())
            {
                foreach (var attribute in parameter.GetCustomAttributes())
                {
                    if (attribute is IValidationFilter validationFilter && !validationFilter.Validate(eventArgs))
                        throw new TypeLoadException();

                    if (attribute is IRequestParameterFilter<TConstructorParameters> requestFilterWithArgs)
                    {
                        callDetails.RequestFilters.Add(new RequestParameterFilterRequestFilterWrapperAsync<TConstructorParameters>(requestFilterWithArgs, parameter.Name));
                    }

                    if (attribute is IRequestParameterFilterAsync<TConstructorParameters> requestFilterWithArgsAsync)
                    {
                        callDetails.RequestFilters.Add(new RequestParameterFilterAsyncOfTRequestFilterAsyncWrapper<TConstructorParameters>(requestFilterWithArgsAsync, parameter.Name));
                    }

                    if (attribute is IRequestParameterFilter requestFilter)
                    {
                        callDetails.RequestFilters.Add(
                            new RequestFilterWrapperAsync<TConstructorParameters>(
                                new RequestParameterFilterRequestFilterWrapper<TConstructorParameters>(requestFilter, parameter.Name)));
                    }

                    if (attribute is IRequestParameterFilterAsync requestFilterAsync)
                    {
                        callDetails.RequestFilters.Add(
                            new RequestParameterFilterAsyncRequestFilterAsyncWrapper<TConstructorParameters>(requestFilterAsync, parameter.Name));
                    }
                }
            }
        }
    }
}
