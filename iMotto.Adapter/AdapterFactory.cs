using System;
using System.Collections.Generic;
//using log4net;

namespace iMotto.Adapter
{
    public class AdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();
        
        public AdapterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
                
        public IAdapter GetAdapter(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length < 4)
            {
                return new AdapterBase();
            }

            return LoadAdapter(code);
        }

        private IAdapter LoadAdapter(string code)
        {
            var adCode = code.Substring(0, 3);

            if (!_adapters.ContainsKey(adCode))
            {
                _adapters.Add(adCode, new AdapterBase());
            }

            return _adapters[adCode];
        }
        
        public void Register(string key, Type adapterType)
        {
            if (_adapters.ContainsKey(key))
            {
                throw new Exception("Duplicated adapter code.");
            }

            var adapter = _serviceProvider.GetService(adapterType) as IAdapter;

            if (adapter == null)
            {
                throw new Exception("Unknown Adapter Type.");
            }

            _adapters.Add(key, adapter);
        }
    }

}
