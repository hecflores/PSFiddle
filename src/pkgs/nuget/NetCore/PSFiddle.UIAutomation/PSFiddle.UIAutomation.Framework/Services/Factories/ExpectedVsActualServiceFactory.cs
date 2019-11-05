using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(IExpectedVsActualService), arguments: new Type[] { typeof(String) })]
    public class ExpectedVsActualServiceFactory : BaseMagicFactory<IExpectedVsActualService, String, String>
    {
        private readonly IResolver resolver;

        public ExpectedVsActualServiceFactory(IDisposableTracker disposableTracker, IResolver resolver) : base(disposableTracker)
        {
            this.resolver = resolver;
        }

        protected override IExpectedVsActualService OnCreate(string name, String description)
        {
            return new ExpectedVsActualService(this.resolver, name, description);
        }

        protected override void OnDestroy(IExpectedVsActualService obj)
        {
            obj.VerifyExpectations();
        }
    }
}
