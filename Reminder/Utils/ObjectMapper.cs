using Data.Entities;
using Reminder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Utils
{
    /// <summary>
    /// Just mapping TSource to TDestination using reflections
    /// </summary>
    public static class ObjectMapper
    {
        public static TDestination Map<TSource, TDestination>(this TSource source, Action<TSource, TDestination> customMaps = null) where TDestination : new()
        {
            var srcType = source.GetType();
            var srcProps = srcType.GetProperties();

            var tType = typeof(TDestination);
            var tProps = tType.GetProperties();

            var dest = new TDestination();

            foreach (var tProp in tProps)
            {
                var srcProp = srcProps
                    .FirstOrDefault(p => string.Equals(p.Name, tProp.Name, StringComparison.OrdinalIgnoreCase));

                if (srcProp != null && tProp.CanWrite)
                {
                    var value = srcProp.GetValue(source);

                    if (value != null && tProp.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                    {
                        tProp.SetValue(dest, value);
                    }
                    else if (value != null)
                    {
                        try
                        {
                            var converted = Convert.ChangeType(value, tProp.PropertyType);
                            tProp.SetValue(dest, converted);
                        }
                        catch
                        {
                            // Could not convert — silently skip or handle
                        }
                    }
                }
            }

            if (customMaps != null)
            {
                customMaps(source, dest);
            }

            return dest;
        }
    }
}
