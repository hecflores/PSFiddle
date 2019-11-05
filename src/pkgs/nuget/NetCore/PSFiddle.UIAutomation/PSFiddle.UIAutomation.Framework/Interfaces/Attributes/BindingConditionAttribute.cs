using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class BindingConditionAttribute : Attribute
    {
        private readonly InstructionBindingTypes type;
        private readonly string name;
        private readonly object value;

        public BindingConditionAttribute(InstructionBindingTypes Type, String Name, Object Value = null)
        {
            type = Type;
            name = Name;
            value = Value;
        }

        public bool TestCondition(IResolver resolver)
        {
            var context = resolver.Resolve<IExpectedVsActualService>();

            if (this.type.Equals(InstructionBindingTypes.Actuals) && value != null)
                return context.Actual(name).Equals(value);

            if (this.type.Equals(InstructionBindingTypes.Data) && value != null)
                return context.Data(name).Equals(value);

            if (this.type.Equals(InstructionBindingTypes.Expected) && value != null)
                return context.Expected(name).Equals(value);


            if (this.type.Equals(InstructionBindingTypes.Actuals) && value == null && !context.HasActual(name))
                return false;

            if (this.type.Equals(InstructionBindingTypes.Data) && value == null)
                return context.Data(name).Equals(value);

            if (this.type.Equals(InstructionBindingTypes.Expected) && value == null)
                return context.Expected(name).Equals(value);

            throw new Exception($"Unexpected Instruction Binding Type {this.type}");
        }
    }
}
