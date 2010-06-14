using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    public sealed class ModuleActivator
    {
        static ListDictionary _modules = new ListDictionary();
        static object _lock = new object();

        public static ILoginModule GetInstance(Type type)
        {
            ILoginModule newObject = null;

            lock (_lock)
            {
                newObject = _modules[type] as ILoginModule;

                if (newObject == null)
                {
                    newObject = Activator.CreateInstance(type) as ILoginModule;
                    if (newObject != null)
                        _modules[type] = newObject;
                }
            }
            return newObject;
        }
    }
}