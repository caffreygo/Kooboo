﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo
{
    internal class OptionsEntry
    {
        private object _lock = new object();

        public Type Type { get; set; }

        private List<Action<object>> _configure;
        public List<Action<object>> Configure
        {
            get
            {
                if (_configure == null)
                {
                    _configure = new List<Action<object>>();
                }
                return _configure;
            }
        }

        private object _value;
        public object Value
        {
            get
            {
                if (_value == null)
                {
                    lock (_lock)
                    {
                        if (_value == null)
                        {
                            var options = Activator.CreateInstance(Type);
                            foreach (var each in Configure)
                            {
                                each(options);
                            }
                            _value = options;
                        }
                    }
                }
                return _value;
            }
        }
    }
}
