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
    public class AthenaFactoryRegisterGenericAttribute : AthenaFactoryRegisterAttribute
    {
        private Type GenericType;
        public AthenaFactoryRegisterGenericAttribute(Type InputType,Type GenericType, AthenaRegistrationType RegistrationType = AthenaRegistrationType.Type, string Name = null, params object[] ContructorArguments) : base(InputType, null, RegistrationType, Name, ContructorArguments)
        {
            this.GenericType = GenericType;
        }
        public override void Register(IResolver resolver, Type classFound)
        {
            base.Register(resolver, GenericType.MakeGenericType(classFound));
        }
    }
}
