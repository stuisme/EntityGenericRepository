using System;

namespace EntityGenericRepository.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Gets the default value for a given type
        /// </summary>
        /// <param name="type">type of default value to return</param>
        /// <returns>default value of the specified type</returns>
        internal static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
