using System;
using System.Collections.Generic;
using System.Linq;
using _build.ConfigScripts;

namespace _build.Scripts
{
    public class ServiceAccessUtils
    {
        public static readonly Dictionary<string, ServiceDefinition> ServiceDictionary = ServicesConfig.Services
            .ToDictionary(s => s.ServiceName);

        public static bool ServiceExists(string serviceName)
        {
            return ServiceDictionary.ContainsKey(serviceName);
        }

        public static void Execute(IEnumerable<ServiceDefinition> services, Action<ServiceDefinition> action)
        {
            foreach (var s in services)
            {
                action(s);
            }
        }

        public static IEnumerable<ServiceDefinition> ServicesList => ServiceDictionary.Values;
        
    }
}
