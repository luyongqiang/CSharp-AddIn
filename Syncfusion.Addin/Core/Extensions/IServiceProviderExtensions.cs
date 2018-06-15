using System;

namespace     Syncfusion.Core.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
