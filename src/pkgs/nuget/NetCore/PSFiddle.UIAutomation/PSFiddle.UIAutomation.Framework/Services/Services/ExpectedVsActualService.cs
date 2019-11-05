using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    [AthenaRegister(typeof(IExpectedVsActualService), Model.Enums.AthenaRegistrationType.Singleton)]
    public class DefaultExpectedVsActualService : ExpectedVsActualService
    {
        public DefaultExpectedVsActualService(IResolver resolver) : base(resolver, "Default", "Validation of current test case")
        {
        }
    }

    public class ExpectedVsActualService : IExpectedVsActualService
    {
        private readonly Dictionary<String, Object> _actual   = new Dictionary<string, object>();
        private readonly Dictionary<String, Object> _expected = new Dictionary<string, object>();
        private readonly Dictionary<String, Object> _data     = new Dictionary<string, object>();

        private readonly string name;
        private readonly string description;

        private bool _verified = false;
        private bool _allowOverwriteExpected = true;
        private bool _allowOverwriteActual = false;
        private bool _allowOverwriteData = true;
        private bool _strictVerifications  = true;

        private readonly IReflectionHelperService reflectionHelperService;

        public ExpectedVsActualService(IResolver resolver, String Name, String Description)
        {
            this.name = Name;
            this.description = Description;
            this.reflectionHelperService = resolver.Resolve<IReflectionHelperService>();
        }
        public String Name() => name;
        public String Description() => description;
        public bool AllowExpectedOverwrites() => _allowOverwriteExpected;
        public void AllowExpectedOverwrites(bool overwriteOrNot)=> this._allowOverwriteExpected = overwriteOrNot;
        public bool AllowDataOverwrites() => _allowOverwriteExpected;
        public void AllowDataOverwrites(bool overwriteOrNot) => this._allowOverwriteExpected = overwriteOrNot;
        public bool AllowActualsOverwrites() => _allowOverwriteData;
        public void AllowActualsOverwrites(bool overwriteOrNot) => this._allowOverwriteData = overwriteOrNot;
        public void StrictVerifications(bool strict) => _strictVerifications = strict;
        public bool StrictVerifications() => _strictVerifications;

        public void Actual(string Name, object Value)
        {
            if (this._actual.ContainsKey(Name) && !this.AllowActualsOverwrites())
                throw new Exception($"Actual Variable {Name} has already been set to {Actual(Name)}. Cannot set to {Value}.\nTo AllowActualsOverwrites, use the AllowOverwrites() method");

            if (!this._actual.ContainsKey(Name))
                this._actual.Add(Name, null);

            this._actual[Name] = Value;

            _verified = false; // We need to re-verify

            // Checks to ensure that we have good logging and consisting comparing
            if (Value.GetType().GetMethod("ToString", new Type[] { }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Actual Value to a object that does not override the method ToString()");

            if (Value.GetType().GetMethod("Equals", new Type[] { typeof(Object) }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Actual Value to a object that does not override the method Equals()");
            
        }
        public object Actual(string Name)
        {
            if (!this._actual.ContainsKey(Name) && this.StrictVerifications())
                throw new Exception($"Actual Variable {Name} was not found");

            if (!this._actual.ContainsKey(Name))
                return null;

            return this._actual[Name];
        }
        public bool HasActual(String Name)
        {
            return this._actual.ContainsKey(Name);
        }
        public void Expected(string Name, object Value)
        { 
            if (this._expected.ContainsKey(Name) && !this.AllowExpectedOverwrites())
                throw new Exception($"Expected Variable {Name} has already been set to {Expected(Name)}. Cannot set to {Value}.\nTo AllowOverwrites, use the AllowOverwrites() method");

            if (!this._expected.ContainsKey(Name))
                this._expected.Add(Name, null);

            this._expected[Name] = Value;

            _verified = false; // We need to re-verify

            // Checks to ensure that we have good logging and consisting comparing
            if (Value.GetType().GetMethod("ToString", new Type[] { }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Actual Value to a object that does not override the method ToString()");

            if (Value.GetType().GetMethod("Equals", new Type[] { typeof(Object) }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Actual Value to a object that does not override the method Equals()");
        }
        public object Expected(string Name)
        {
            if (!this._expected.ContainsKey(Name) && this.StrictVerifications())
                throw new Exception($"Expected Variable {Name} was not found");

            if (!this._expected.ContainsKey(Name))
                return null;

            return this._expected[Name];
        }
        public bool HasExpected(String Name)
        {
            return this._expected.ContainsKey(Name);
        }
        public void Data(string Name, object Value)
        {
            if (this._data.ContainsKey(Name) && !this.AllowDataOverwrites())
                throw new Exception($"Data Variable {Name} has already been set to {Data(Name)}. Cannot set to {Value}.\nTo AllowDataOverwrites, use the AllowDataOverwrites() method");

            if (!this._data.ContainsKey(Name))
                this._data.Add(Name, null);

            this._data[Name] = Value;

            _verified = false; // We need to re-verify

            // Checks to ensure that we have good logging and consisting comparing
            if (Value.GetType().GetMethod("ToString",new Type[] { }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Data Value to a object that does not override the method ToString()");

            if (Value.GetType().GetMethod("Equals", new Type[] { typeof(Object) }).DeclaringType != Value.GetType())
                throw new Exception("TESTING DEVELOPER BUG: Setting Data Value to a object that does not override the method Equals()");
        }
        public object Data(string Name)
        {
            if (!this._data.ContainsKey(Name) && this.StrictVerifications())
                throw new Exception($"Data Variable {Name} was not found");

            if (!this._data.ContainsKey(Name))
                return null;

            return this._data[Name];
        }
        public bool HasData(String Name)
        {
            return this._data.ContainsKey(Name);
        }



        public void VerifyExpectations()
        {
            if (_verified) {
                return;
            }
            _verified = true;

            Dictionary<String, Exception> failures = new Dictionary<string, Exception>();
            XConsole.WriteLine($"Verifications: {Name()} - {Description()}");
            foreach(var exp in this._expected)
            {
                Object expected = null;
                Object actual = null;
                String name = exp.Key;
                try
                {
                    expected = Expected(name);
                    actual = Actual(name);

                    if (expected.Equals(actual))
                        XConsole.WriteLine($"  Ok    - {name}: [{expected}] == [{actual}]");
                    else
                        throw new Exception($"[{expected}] != [{actual}]");
                }
                catch(Exception e)
                {
                    XConsole.WriteLine($"*BROKE* - {name}: {e.Message} ");
                    failures.Add(exp.Key, e);
                }
            }

            if(failures.Count > 0)
            {
                XConsole.WriteLine($"*Verifications Failed*");
                foreach(var fail in failures)
                {
                    XConsole.WriteLine($"  {fail.Key}:\n   {fail.Value.Message}\n   {fail.Value.StackTrace}");
                }
                throw new Exception($"Verification for {Name()} - {Description()} failed");
            }
        }

        
    }
}
