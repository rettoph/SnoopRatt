using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Extensions
{
    public static class IConfigurationExtensions
    {
        public static T Bind<T>(this IConfiguration configuration, string key)
            where T : class, new()
        {
            var instance = new T();

            configuration.Bind(key, instance);

            return instance;
        }
    }
}
