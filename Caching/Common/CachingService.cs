using Caching.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Caching.Common
{
    public class CachingService
    {
        static CachingService()
        {
            var section = (UnityConfigurationSection) ConfigurationManager.GetSection("unity.caching");
            IUnityContainer container = new UnityContainer();
            container.LoadConfiguration(section);
            Store = container.Resolve<ICachingService>();
        }

        public static ICachingService Store { get; }
    }
}