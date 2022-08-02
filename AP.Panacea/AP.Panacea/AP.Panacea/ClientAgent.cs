using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    [Serializable]
    public class ClientAgent
    {
        private readonly string _name;
        private readonly string _version;
        
        public ClientAgent(string name, string version = "1.0.0.0")
        {
            _name = name;
            _version = version;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Version
        {
            get { return _version; }
        }
    }
}
