using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class ParameterDictionary : IReadOnlyDictionary<string, object>
    {
        private readonly object[] _values;
        private readonly IReadOnlyDictionary<string, int> _indicies;

        public ParameterDictionary(object[] values, IReadOnlyDictionary<string, int> indicies)
        {
            _values = values ?? throw new ArgumentNullException(nameof(values));
            _indicies = indicies ?? throw new ArgumentNullException(nameof(indicies));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var index in _indicies)
            {
                yield return new KeyValuePair<string, object>(index.Key, _values[index.Value]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _indicies.Count;

        public bool ContainsKey(string key) => _indicies.ContainsKey(key);

        public bool TryGetValue(string key, out object value)
        {
            value = null;

            if (!_indicies.TryGetValue(key, out var index))
                return false;

            if (index >= _values.Length)
                return false;

            value = _values[index];

            return true;
        }

        public object this[string key]
        {
            get
            {
                var index = _indicies[key];

                if(index >= _values.Length)
                    throw new KeyNotFoundException();

                return _values[index];
            }
        }

        public IEnumerable<string> Keys => _indicies.Keys;

        public IEnumerable<object> Values => _values;
    }
}
