using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BindingInstructionAttribute : AthenaRegisterAttribute
    {

        public BindingInstructionAttribute(Type interfaceType) : base(interfaceType)
        {
        }
        public override void Register(IResolver resolver, Type classFound)
        {
            var context = resolver.Resolve<IExpectedVsActualService>();

            RegistrationType = AthenaRegistrationType.Singleton;
            base.Register(resolver, classFound);
            var proxy = resolver.Resolve<IEventBasedProxy>();
            proxy.BeforeCalled((arg) =>
            {
                if (InputType.IsInstanceOfType(arg.TargetObj))
                {
                    var inputs  = arg.MethodBase.GetCustomAttributes<InputAttribute>();

                    bool allGood = true;
                    foreach(var input in inputs)
                    {
                        try
                        {
                            input.Verify(resolver);
                            XConsole.WriteLine($"  Ok    - Input {input.Name()}: [{input.Value()}]");
                        }
                        catch(Exception e)
                        {
                            XConsole.WriteLine($"*BROKE* - {input.Name()}: {e.Message} ");
                            allGood = false;
                        }
                    }

                    if(!allGood)
                        throw new Exception($"Not all inputs passed inspection for this particular instruction\n{arg.TargetObj.GetType().Name}.{arg.MethodBase.Name}()");
                }
            });
            proxy.AfterCalled((arg) =>
            {
                if (arg.Exception != null)
                    return;

                if (InputType.IsInstanceOfType(arg.TargetObj))
                {
                    var outputs = arg.MethodBase.GetCustomAttributes<OutputAttribute>();

                    bool allGood = true;
                    foreach (var output in outputs)
                    {
                        try
                        {
                            output.Verify(resolver);
                            XConsole.WriteLine($"  Ok    - Output {output.Name()}: [{output.Value()}]");
                        }
                        catch (Exception e)
                        {
                            XConsole.WriteLine($"*BROKE* - {output.Name()}: {e.Message} ");
                            allGood = false;
                        }
                    }

                    if (!allGood)
                        throw new Exception($"Not all outputs passed inspection for this particular instruction\n{arg.TargetObj.GetType().Name}.{arg.MethodBase.Name}()");

                }
            });
        }
       
    }
}
