using MC.Track.TestSuite.Interfaces.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PSFiddle.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether this instance can intercept the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can intercept the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanIntercept(this Type type)
        {
            return type.IsPublic && type.IsInterface && !type.IsAnonymousType() && type != typeof(IEventBasedProxy) && type != typeof(IUnityContainer);//  && !type.FullName.Contains("Microsoft.Extensions") && !type.FullName.Contains("Microsoft.AspNetCore") && !type.FullName.Contains("Swashbuckle.AspNetCore");
        }

        /// <summary>
        /// Determines whether [is anonymous type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is anonymous type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
    }
}
