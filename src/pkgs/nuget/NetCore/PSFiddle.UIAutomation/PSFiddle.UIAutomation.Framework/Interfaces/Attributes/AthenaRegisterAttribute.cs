using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Extensions;
namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AthenaRegisterAttribute : Attribute
    {
        public Type InputType;
        public String Name;
        public AthenaRegistrationType RegistrationType;
        public Object[] ContructorArguments;

        public AthenaRegisterAttribute(Type InputType, 
                                       AthenaRegistrationType RegistrationType = AthenaRegistrationType.Type, 
                                       String Name = null, 
                                       params Object[] ContructorArguments) {
            this.InputType = InputType;
            this.Name = Name;
            this.RegistrationType = RegistrationType;
            this.ContructorArguments = ContructorArguments!=null? ContructorArguments:new object[0];
        }
        public virtual void Register(IResolver resolver, Type classFound)
        {
            Console.WriteLine($"   Registering {InputType.Name} to {classFound.Name} {(Name == null ? "" : ($"with name {Name}"))}");
            // Builds Method
            var methodName = $"Register{RegistrationType.ToString()}";
            var arguments = new List<KeyValuePair<Type, Object>>();

            // Classes
            arguments.Add(new KeyValuePair<Type, Object>(typeof(Type), InputType));
            arguments.Add(new KeyValuePair<Type, Object>(typeof(Type), classFound));

            // Add Name
            arguments.Add(new KeyValuePair<Type, object>(typeof(String), Name));
            
            // Add Constructor Arguments
            arguments.Add(new KeyValuePair<Type, Object>(typeof(Object[]), ContructorArguments));

            // Get Method
            var method = resolver.GetType().GetMethod(methodName, arguments.Select(b=>b.Key).ToArray());

            // Invoke
            method.Invoke(resolver, arguments.Select(b => b.Value).ToArray());

        }
    }
}
