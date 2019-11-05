using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Enums;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AthenaFactoryRegisterAttribute : AthenaRegisterAttribute
    {
        public AthenaFactoryRegisterAttribute(Type InputType, Type[] arguments = null, AthenaRegistrationType RegistrationType = AthenaRegistrationType.Type, string Name = null, params object[] ContructorArguments) : base(InputType, RegistrationType, Name, ContructorArguments)
        {
            var listArgs = new List<Type>();
            listArgs.Add(InputType);
            listArgs.AddRange(arguments == null ? new Type[0] : arguments);

            switch (listArgs.Count)
            {
                case 1:
                    this.InputType = typeof(IMagicFactory<>).MakeGenericType(listArgs.ToArray());
                    break;
                case 2:
                    this.InputType = typeof(IMagicFactory<,>).MakeGenericType(listArgs.ToArray());
                    break;
                case 3:
                    this.InputType = typeof(IMagicFactory<,,>).MakeGenericType(listArgs.ToArray());
                    break;
                case 4:
                    this.InputType = typeof(IMagicFactory<,,,>).MakeGenericType(listArgs.ToArray());
                    break;
                case 5:
                    this.InputType = typeof(IMagicFactory<,,,,>).MakeGenericType(listArgs.ToArray());
                    break;
                default:
                    throw new Exception("Unexpected number of arguments to be {listArgs.Count}");
            }
           
        }

        public override void Register(IResolver resolver, Type classFound)
        {
            base.Register(resolver, classFound);

            
        }
    }
}
