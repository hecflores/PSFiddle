#if NETCORE
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Attributes;
using PSFiddle.Common.Extensions;
using System.Linq;
namespace PSFiddle.Common.Services
{
    public class InterceptableChangeToken : IChangeToken
    {
        public IChangeToken originalConfig;

#pragma warning disable CA2213 // SpecFlow Binding Contraints
        private readonly IUnityContainer container;
#pragma warning restore CA2213 // SpecFlow Binding Contraints

        public bool HasChanged => originalConfig.HasChanged;

        public bool ActiveChangeCallbacks => originalConfig.ActiveChangeCallbacks;

        public InterceptableChangeToken(IChangeToken originalConfig, IUnityContainer container)
        {
            this.originalConfig = originalConfig;
            this.container = container;
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return originalConfig.RegisterChangeCallback(callback, state);
        }
    }
    public class InterceptableConfigurationSection : IConfigurationSection
    {
        public IConfigurationSection originalConfig;

#pragma warning disable CA2213 // SpecFlow Binding Contraints
        private readonly IUnityContainer container;
#pragma warning restore CA2213 // SpecFlow Binding Contraints


        public string Key => originalConfig.Key;

        public string Path => originalConfig.Path;

        public string Value { get => originalConfig.Value; set => originalConfig.Value = value; }

        public string this[string key] { get => originalConfig[key]; set => originalConfig[key] = value; }

        public InterceptableConfigurationSection(IConfigurationSection originalConfig, IUnityContainer container)
        {
            this.originalConfig = originalConfig;
            this.container = container;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return originalConfig.GetChildren().Select(b => container.MakeInterceptable<IConfigurationSection>(new InterceptableConfigurationSection(b, container)));
        }

        public IChangeToken GetReloadToken()
        {
            return container.MakeInterceptable<IChangeToken>(new InterceptableChangeToken(originalConfig.GetReloadToken(), container));
        }

        public IConfigurationSection GetSection(string key)
        {
            return container.MakeInterceptable<IConfigurationSection>(new InterceptableConfigurationSection(originalConfig.GetSection(key), container));
        }
    }

    public class InterceptableConfiguration : IConfiguration
    {
        public IConfiguration originalConfig;

#pragma warning disable CA2213 // SpecFlow Binding Contraints
        private readonly IUnityContainer container;
#pragma warning restore CA2213 // SpecFlow Binding Contraints

        public InterceptableConfiguration(IConfiguration originalConfig, IUnityContainer container)
        {
            this.originalConfig = originalConfig;
            this.container = container;
        }
        public string this[string key] { get => originalConfig[key]; set => originalConfig[key]=value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return originalConfig.GetChildren().Select(b=>container.MakeInterceptable<IConfigurationSection>(new InterceptableConfigurationSection(b, container)));
        }

        public IChangeToken GetReloadToken()
        {
            return container.MakeInterceptable<IChangeToken>(new InterceptableChangeToken(originalConfig.GetReloadToken(), container));
        }

        public IConfigurationSection GetSection(string key)
        {
            return container.MakeInterceptable<IConfigurationSection>(new InterceptableConfigurationSection(originalConfig.GetSection(key), container));
        }
    }
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration originalConfig;

        public ConfigurationService(IConfiguration originalConfig)
        {
            this.originalConfig = originalConfig;
        }

        public string Get(string config)
        {
            return originalConfig[config];
        }
    }
}
#endif