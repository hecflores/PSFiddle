using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Instructions.Shared;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Instructions.Shared
{
    public class BaseInstruction<T> : IInstruction<T> where T : IInstruction<T>
    {
        public IDependencies Dependencies { get; private set; }
        private readonly IInstructions instructions;
        private readonly IExpectedVsActualService expectedVsActualService;
        protected readonly IResolver Resolver;
        public BaseInstruction(IResolver resolver)
        {
            this.expectedVsActualService = resolver.Resolve<IExpectedVsActualService>();
            this.Resolver = resolver;
            this.Dependencies = resolver.Resolve<IDependencies>();

        }
        protected T Me()
        {
            return (T)((IInstruction<T>)this);
        }
        public T Actual(string Name, string Value)
        {
            this.expectedVsActualService.Actual(Name, Value);
            return Me();
        }
        public T Actual(string Name, bool Value)
        {
            this.expectedVsActualService.Actual(Name, Value);
            return Me();
        }

        public string Actual(string Name)
        {
            return (String)this.expectedVsActualService.Actual(Name);
        }

        public IInstructions All()
        {
            return this.instructions;
        }

        public T Data(string Name, string Value)
        {
            this.expectedVsActualService.Data(Name, Value);
            return Me();
        }
        public T Data(string Name, bool Value)
        {
            this.expectedVsActualService.Data(Name, Value);
            return Me();
        }

        public string Data(string Name)
        {
            return (String)this.expectedVsActualService.Data(Name);
        }

        public T Expected(string Name, string Value)
        {
            this.expectedVsActualService.Expected(Name, Value);
            return Me();
        }
        public T Expected(string Name, bool Value)
        {
            this.expectedVsActualService.Expected(Name, Value);
            return Me();
        }

        public string Expected(string Name)
        {
            return (String)this.expectedVsActualService.Expected(Name);
        }
    }
}
