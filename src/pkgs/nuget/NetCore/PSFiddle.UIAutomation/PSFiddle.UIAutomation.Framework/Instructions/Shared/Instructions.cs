using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Instructions.Shared;
namespace MC.Track.TestSuite.Toolkit.Instructions.Shared
{
    public class Instructions : BaseInstruction<IInstructions>, IInstructions, IInstruction<IInstructions>
    {
        public Instructions(IResolver resolver) : base(resolver)
        {
           
        }

        
    }
}
