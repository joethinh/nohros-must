using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    public sealed class ModuleActivator
    {
        static ListDictionary modules_ = new ListDictionary();
        static object lock_ = new object();

        public static ILoginModule GetInstance(Type type)
        {
            ILoginModule login_module = null;

            lock (lock_)
            {
                login_module = modules_[type] as ILoginModule;

                if (login_module == null) {
                    login_module = Activator.CreateInstance(type) as ILoginModule;
                    if (login_module != null)
                        modules_[type] = login_module;
                }
            }
            return login_module;
        }
    }
}