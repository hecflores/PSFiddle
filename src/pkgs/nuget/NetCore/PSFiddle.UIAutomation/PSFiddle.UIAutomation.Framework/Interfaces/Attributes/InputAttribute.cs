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
    public class InputAttribute : Attribute
    {
        private readonly InstructionBindingTypes instructionBindingTypes;
        private readonly string name;
        private Object value;

        public InputAttribute(InstructionBindingTypes instructionBindingTypes, String Name)
        {
            this.instructionBindingTypes = instructionBindingTypes;
            name = Name;
        }
        public String Name() => name;
        public Object Value() => value;
        public void Verify(IResolver resolver)
        {
            var context = resolver.Resolve<IExpectedVsActualService>();

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Actuals) && !context.HasActual(name))
                throw new Exception($"Input was expecting: Actuals variable named [{name}]. Was not found");

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Data) && !context.HasData(name))
                throw new Exception($"Input was expecting: Data variable named [{name}]. Was not found");

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Expected) && !context.HasExpected(name))
                throw new Exception($"Input was expecting: Expected variable named [{name}]. Was not found");

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Actuals) && context.HasActual(name))
                value = $"Actual '{context.Actual(name)}'";

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Data) && context.HasData(name))
                value = $"Data '{context.Data(name)}'";

            if (this.instructionBindingTypes.Equals(InstructionBindingTypes.Expected) && context.HasExpected(name))
                value = $"Expected '{context.Expected(name)}'";
        }
    }
}
