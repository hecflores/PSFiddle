using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Extensions
{
    public static class TypeExtensions
    {
        public static MethodInfo GetGenericMethod(this Type t, string name, Type[] genericArgTypes, Type[] argTypes, Type returnType)
        {
            MethodInfo foo1 = (from m in t.GetMethods(BindingFlags.Public)
                               where m.Name == name
                               && m.GetGenericArguments().Length == genericArgTypes.Length
                               && m.GetParameters().Select(pi => pi.ParameterType.IsGenericType ? pi.ParameterType.GetGenericTypeDefinition() : pi.ParameterType).SequenceEqual(argTypes) &&
                               (returnType == null || (m.ReturnType.IsGenericType ? m.ReturnType.GetGenericTypeDefinition() : m.ReturnType) == returnType)
                               select m).FirstOrDefault();
            if (foo1 != null)
            {
                return foo1.MakeGenericMethod(genericArgTypes);
            }
            return null;
        }
    }
}
