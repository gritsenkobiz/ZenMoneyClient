using System;
using System.Reflection;

namespace Gritsenko.Universal.Extensions
{
    /// <summary>
    /// Helper methods for properties
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Возвращает пользоватлеьский атрибут, если таковой присобачен к свойству
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute TryGetAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            var attr = propertyInfo.GetCustomAttribute<TAttribute>();
                //.FirstOrDefault(x=>x is TAttribute);

            return attr;
        }
    }
}