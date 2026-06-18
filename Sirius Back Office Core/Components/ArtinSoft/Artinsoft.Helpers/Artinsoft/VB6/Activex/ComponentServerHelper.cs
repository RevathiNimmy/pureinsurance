using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;

namespace Artinsoft.VB6.Activex
{
    /// <summary>
    /// The ComponentServerHelper provides support to migration from VB6 ActiveX Dll/Exe components. These component
    /// provides some functionality like initialization of global variables and app domains handling.
    /// </summary>
    public class ComponentServerHelper
    {
        /// <summary>
        /// Singleton storing the ComponentServerHelper instance
        /// </summary>
        private static ComponentServerHelper _instance = null;
        /// <summary>
        /// Works like a references count, storing the all instances created for the component
        /// </summary>
        private Dictionary<AppDomain, List<object>> instances = new Dictionary<AppDomain, List<object>>();
        /// <summary>
        /// Contains the InitGlobalVarsDelegate's invoked when all references of this component were freed
        /// </summary>
        private List<InitGlobalVarsDelegate> initGlobalVarsDels = new List<InitGlobalVarsDelegate>();
        /// <summary>
        /// Indicates if ComponentServerHelper works using AppDomains or just a simple ClassFactory
        /// </summary>
        private static bool useDomains = false;
        /// <summary>
        /// The first and Main Domain loaded
        /// </summary>
        private AppDomain mainDomain = null;
        /// <summary>
        /// The last Domain loaded
        /// </summary>
        private AppDomain curDomain = null;

        /// <summary>
        /// Obtains the singleton instance of the ComponentServerHelper
        /// </summary>
        /// <returns>The singleton instance of the ComponentServerHelper</returns>
        public static ComponentServerHelper GetInstance()
        {
            if (_instance == null)
                _instance = new ComponentServerHelper();

            return _instance;
        }

        /// <summary>
        /// Obtains the singleton instance of the ComponentServerHelper
        /// </summary>
        /// <param name="useDomainsValue">Indicates if the ComponentServerHelper should handle AppDomains or just works like a ClassFactory</param>
        /// <returns>The singleton instance of the ComponentServerHelper</returns>
        public static ComponentServerHelper GetInstance(bool useDomainsValue)
        {
            useDomains = useDomainsValue;
            return GetInstance();
        }

        /// <summary>
        /// Creates an instance of T in the corresponding space of memory or AppDomain
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>An instance of type T</returns>
        public T Create<T>() where T : ComponentClassHelper
        {
            return Create<T>(null);
        }

        /// <summary>
        /// Creates an instance of T in the corresponding space of memory or AppDomain
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <param name="oldInstance">The instance being freed if it had a referenced instance</param>
        /// <returns>An instance of type T</returns>
        public T Create<T>(object oldInstance) where T : ComponentClassHelper
        {
            if (InstanceIsCreated(oldInstance)) RemoveInstance(oldInstance);
            Type type = typeof(T);
            T newInstance = null;
            if (useDomains)
            {
                // VB6 SingleUse classes are created in a different Domain (component server)
                if ((type.BaseType.Name == "ComponentSingleUseClassHelper") ||
                    (type.BaseType.Name == "GlbComponentSingleUseClassHelper"))
                    newInstance = CreateInstanceInAvailableDomain<T>();
                else
                    newInstance = CreateInstanceInMainDomain<T>();
            }
            else
                newInstance = CreateInstanceNoDomain<T>();
            newInstance.RegisterInitGlobalVarsDelegates();
            AddInstance(curDomain, newInstance);
            return newInstance;
        }

        /// <summary>
        /// Creates an instance of T in Main Domain
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>An instance of type T</returns>
        private T CreateInstanceInMainDomain<T>() where T : ComponentClassHelper
        {
            Type type = typeof(T);
            if (mainDomain == null)
                mainDomain = AppDomain.CurrentDomain;
            ObjectHandle oh = mainDomain.CreateInstance(type.Assembly.FullName, type.FullName);
            curDomain = mainDomain;
            return (T)(oh.Unwrap());
        }

        /// <summary>
        /// Searches a Domain without a specific class type (SingleUse/GlbSingleUse) where to create the type, else
        /// it is created in a new Domain
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>An instance of type T</returns>
        private T CreateInstanceInAvailableDomain<T>() where T : ComponentClassHelper
        {
            AppDomain aDomain = GetDomainToCreate<T>();
            if (aDomain != null)
            {
                Type type = typeof(T);
                curDomain = aDomain;
                ObjectHandle oh = curDomain.CreateInstance(type.Assembly.FullName, type.FullName);
                return (T)(oh.Unwrap());
            }
            else
                return CreateInstanceInNewDomain<T>();
        }

        /// <summary>
        /// Creates an instance of T in a New Domain
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>An instance of type T</returns>
        private T CreateInstanceInNewDomain<T>() where T : ComponentClassHelper
        {
            Type type = typeof(T);
            curDomain = AppDomain.CreateDomain("NewDomain", null, null);
            ObjectHandle oh = curDomain.CreateInstance(type.Assembly.FullName, type.FullName);
            return (T)(oh.Unwrap());
        }

        /// <summary>
        /// Creates an instance of T in the addresses space of the main application
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>An instance of type T</returns>
        private T CreateInstanceNoDomain<T>() where T : ComponentClassHelper
        {
            curDomain = AppDomain.CurrentDomain;
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Registers a InitGlobalVarsDelegate to be invoked when all component references are freed.
        /// This method is invoked to each created instance just if UseDomain is false.
        /// </summary>
        /// <param name="del">The delegate to be registered</param>
        public void RegisterInitGlobalVarsDelegate(InitGlobalVarsDelegate del)
        {
            if (!initGlobalVarsDels.Contains(del))
                initGlobalVarsDels.Add(del);
        }

        /// <summary>
        /// Frees a component instance and checks if Domain/GlobalVars should be initialized
        /// </summary>
        /// <param name="instance">The instance to be freed</param>
        public T Dispose<T>(T instance) where T : ComponentClassHelper
        {
            if (InstanceIsCreated(instance)) RemoveInstance(instance);
            if (instances.Count == 0)
            {
                // TODO: check AppDomain.UnLoad
                if (useDomains)
                    mainDomain = null;
                foreach (InitGlobalVarsDelegate del in initGlobalVarsDels)
                {
                    del.Invoke();
                }
            }
            return null;
        }

        /// <summary>
        /// Looks for a AppDomain to create a specific (SingleUse/GlbSingleUse) type
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <returns>The AppDomain where to create the type or null if all AppDomain have the specific type</returns>
        public AppDomain GetDomainToCreate<T>() where T : ComponentClassHelper
        {
            Type type = typeof(T);
            string baseName = type.BaseType.Name;
            foreach (AppDomain aDomain in instances.Keys)
            {
                bool available = true;
                foreach (object inst in instances[aDomain])
                {
                    Type instType = inst.GetType();
                    if (instType.BaseType.Name == baseName)
                    {
                        available = false;
                        break;
                    }
                }
                if (available) return aDomain;
            }
            return null;
        }

        /// <summary>
        /// Finds an instance in ComponentServer and returns it
        /// </summary>
        /// <typeparam name="T">The type of the class being created</typeparam>
        /// <param name="instance">The instance to be found</param>
        /// <returns>The instance if it was found or a new instance of type T</returns>
        public T FindInstance<T>(T instance) where T : ComponentClassHelper
        {
            if (InstanceIsCreated(instance))
                return instance;
            else
                return Create<T>();
        }

        /// <summary>
        /// Looks for an instance in instances Dictionary
        /// </summary>
        /// <param name="instance">The instance to be found</param>
        /// <returns>True if instance is found</returns>
        private bool InstanceIsCreated(object instance)
        {
            foreach (AppDomain aDomain in instances.Keys)
            {
                if (instances[aDomain].Contains(instance)) return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the instance in the instances Dictionary related to its corresponding AppDomain
        /// </summary>
        /// <param name="theDomain">The AppDomain where the instance was created</param>
        /// <param name="instance">The instance to be added</param>
        private void AddInstance(AppDomain theDomain, object instance)
        {
            if (instances.ContainsKey(theDomain))
            {
                instances[theDomain].Add(instance);
            }
            else
            {
                List<object> list = new List<object>();
                list.Add(instance);
                instances.Add(theDomain, list);
            }
        }

        /// <summary>
        /// Removes the instance from the instances Dictionary
        /// </summary>
        /// <param name="instance">The instance to be removed</param>
        private void RemoveInstance(object instance)
        {
            foreach (AppDomain aDomain in instances.Keys)
            {
                if (instances[aDomain].Contains(instance))
                {
                    instances[aDomain].Remove(instance);
                    if (instances[aDomain].Count == 0) instances.Remove(aDomain);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Delegate that points to a method, which function is to initialize the global vars to the corresponding module.
    /// This delegate should be registered (RegisterInitGlobalVarsDelegate) if UseDomain flag is Off
    /// </summary>
    public delegate void InitGlobalVarsDelegate();

    /// <summary>
    /// Marks a class like ComponentClassHelper type, it means this class could be instantiated via the ComponentServerHelper
    /// </summary>
    public class ComponentClassHelper : MarshalByRefObject
    {
        /// <summary>
        /// Register init global variables delegates
        /// </summary>
        public virtual void RegisterInitGlobalVarsDelegates()
        {
        }
    }

    /// <summary>
    /// Marks a class like ComponentSingleUseClassHelper type, it means this class could be instantiated via the ComponentServerHelper
    /// and it behaves like a VB6 SingleUse class. It could be created in a new domain
    /// </summary>
    public class ComponentSingleUseClassHelper : ComponentClassHelper
    {
    }

    /// <summary>
    /// Marks a class like GlbComponentSingleUseClassHelper type, it means this class could be instantiated via the ComponentServerHelper
    /// and it behaves like a VB6 GlbSingleUse class. It could be created in a new domain
    /// </summary>
    public class GlbComponentSingleUseClassHelper : ComponentClassHelper
    {
    }
}
