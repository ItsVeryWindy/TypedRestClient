using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using TypedRestClient.Filters;
using TypedRestClient.Filters.Exceptions;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Responses;
using TypedRestClient.Filters.Validation;

namespace TypedRestClient.Generator
{
    public static class TypedRestClientGenerator
    {
        public static TInterface CreateInstance<TInterface>(string uri)
        {
            if (uri == null)
                throw new ArgumentNullException(uri);

            return CreateInstance<TInterface>(new HttpClient()
            {
                BaseAddress = new Uri(uri)
            });
        }

        public static TInterface CreateInstance<TInterface>(Uri uri)
        {
            return CreateInstance<TInterface>(new HttpClient()
            {
                BaseAddress = uri
            });
        }

        public static TInterface CreateInstance<TInterface>(HttpClient client)
        {
            return CreateFactory<TInterface>(new FactoryConfiguration()).Create(client);
        }

        public static TInterface CreateInstance<TInterface>(HttpClient client, TypedRestClientConfiguration configuration)
        {
            return CreateFactory<TInterface>(new FactoryConfiguration()).Create(client, configuration);
        }

        public static ITypedRestClientFactory<TInterface> CreateFactory<TInterface>()
        {
            return CreateFactory<TInterface>(new FactoryConfiguration());
        }

        public static ITypedRestClientFactory<TInterface> CreateFactory<TInterface>(FactoryConfiguration factoryConfiguration)
        {
            var callDetails = new List<CallDetails>();

            var type = CreateType<TInterface>(callDetails, factoryConfiguration);

            return new TypedRestClientFactory<TInterface>(type, new HttpClientProxy(callDetails));
        }

        private static TypeInfo CreateType<TInterface>(List<CallDetails> callDetails, FactoryConfiguration factoryConfiguration)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            var typeBuilder = moduleBuilder.DefineType("Type");

            typeBuilder.AddInterfaceImplementation(typeof(TInterface));

            var proxyField = CreateField<IHttpClientProxy>(typeBuilder, "_proxy");
            var httpClientField = CreateField<HttpClient>(typeBuilder, "_client");
            var filtersListField = CreateField<List<Filters>>(typeBuilder, "_filters");

            CreateConstructor(typeBuilder, proxyField, httpClientField, filtersListField);

            var methods = typeof(TInterface).GetMethods();

            for (var i = 0; i < methods.Length; i++)
            {
                callDetails.Add(CreateMethod<TInterface>(typeBuilder, httpClientField, proxyField, filtersListField, i, methods[i], factoryConfiguration));
            }

            return typeBuilder.CreateTypeInfo();
        }

        private static FieldBuilder CreateField<T>(TypeBuilder builder, string name)
        {
            return builder.DefineField(name, typeof(T), FieldAttributes.Private);
        }

        private static CallDetails CreateMethod<TInterface>(TypeBuilder builder, FieldInfo httpClientField, FieldInfo proxyField, FieldInfo filtersListField, int methodId, MethodInfo method, FactoryConfiguration factoryConfiguration)
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

            var proxyMethod = typeof(IHttpClientProxy).GetMethod(nameof(IHttpClientProxy.CallClient))?.MakeGenericMethod(genericArg);

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
            generator.Emit(OpCodes.Ldfld, filtersListField);
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

            var callDetails = new CallDetails
            {
                ParameterIndices = method.GetParameters().ToDictionary(pi => pi.Name, pi => pi.Position)
            };

            var parameterDictionary = method.GetParameters().ToDictionary(x => x.Name, x => new RequestParameter(x.ParameterType, x.GetCustomAttributes()));

            var validationEventArgs = new ValidationFilterEventArgs(parameterDictionary, genericArg);

            PopulateCallDetails(callDetails, validationEventArgs, factoryConfiguration.Filters);

            PopulateCallDetails(callDetails, validationEventArgs, typeof(TInterface).GetCustomAttributes());

            PopulateCallDetails(callDetails, validationEventArgs, method.GetCustomAttributes());

            PopulateRequestParameterCallDetails(callDetails, validationEventArgs, method);

            return callDetails;
        }

        private static void PopulateCallDetails(CallDetails callDetails, IValidationFilterEventArgs eventArgs, IEnumerable<object> filters)
        {
            foreach (var filter in filters)
            {
                if (filter is IValidationFilter validationFilter && !validationFilter.Validate(eventArgs))
                    throw new TypeLoadException();

                if (filter is IFilterFactory filterFactory)
                {
                    callDetails.FilterFactories.Add(filterFactory);
                }

                if (filter is IRequestFilter requestFilter)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(new RequestFilterWrapperAsync(requestFilter)));
                }

                if (filter is IRequestFilterAsync requestFilterAsync)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(requestFilterAsync));
                }

                if (filter is IResponseFilter responseFilter)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(new ResponseFilterWrapperAsync(responseFilter)));
                }

                if (filter is IResponseFilterAsync responseFilterAsync)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(responseFilterAsync));
                }

                if (filter is IExceptionFilter exceptionFilter)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(new ExceptionFilterWrapperAsync(exceptionFilter)));
                }

                if (filter is IExceptionFilterAsync exceptionFilterAsync)
                {
                    callDetails.FilterFactories.Add(new SelfFilterFactory(exceptionFilterAsync));
                }
            }
        }

        private static void PopulateRequestParameterCallDetails(CallDetails callDetails, IValidationFilterEventArgs eventArgs, MethodInfo method)
        {
            foreach (var parameter in method.GetParameters())
            {
                foreach (var attribute in parameter.GetCustomAttributes())
                {
                    if (attribute is IValidationFilter validationFilter && !validationFilter.Validate(eventArgs))
                        throw new TypeLoadException();

                    if(attribute is IFilterFactory filterFactory)
                    {
                        callDetails.FilterFactories.Add(new RequestParameterFilterFactory(filterFactory, parameter.Name));
                    }

                    if (attribute is IRequestParameterFilter requestFilter)
                    {
                        callDetails.FilterFactories.Add(new SelfFilterFactory(new RequestParameterFilterRequestFilterAsyncWrapper(requestFilter, parameter.Name)));
                    }

                    if (attribute is IRequestParameterFilterAsync requestFilterAsync)
                    {
                        callDetails.FilterFactories.Add(new SelfFilterFactory(new RequestParameterFilterAsyncRequestFilterAsyncWrapper(requestFilterAsync, parameter.Name)));
                    }
                }
            }
        }

        private static void CreateConstructor(TypeBuilder builder, FieldBuilder proxyField, FieldBuilder httpClientField, FieldBuilder filtersListField)
        {
            var constructor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IHttpClientProxy), typeof(HttpClient), typeof(TypedRestClientConfiguration) });

            var generator = constructor.GetILGenerator();

            var configurationField = CreateField<TypedRestClientConfiguration>(builder, "_configuration");

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, proxyField);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Stfld, httpClientField);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_3);
            generator.Emit(OpCodes.Stfld, configurationField);

            generator.Emit(OpCodes.Ldarg_0);

            // call Initialize method
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, proxyField);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, httpClientField);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, configurationField);

            var proxyMethod = typeof(IHttpClientProxy).GetMethod(nameof(IHttpClientProxy.Initialize));

            generator.Emit(OpCodes.Call, proxyMethod);

            generator.Emit(OpCodes.Stfld, filtersListField);

            generator.Emit(OpCodes.Ret);
        }
    }
}
