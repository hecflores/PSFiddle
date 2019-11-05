using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services
{
    [AthenaRegister(typeof(IInstructionsDiscovery), Model.Enums.AthenaRegistrationType.Singleton)]
    public class InstructionsDiscovery : IInstructionsDiscovery
    {
        public class InstructionItem
        {
            public String UnityName { get; set; }
            public String InstructionName { get; set; }
            public List<BindingConditionAttribute> Conditions { get; set; }
        }

        private readonly List<InstructionItem> _bindings = new List<InstructionItem>();
        private readonly IResolver resolver;

        public InstructionsDiscovery(IResolver resolver)
        {
            this.resolver = resolver;
        }
        public void Find(string InstructionName)
        {
            var bindings = _bindings.Where(b => b.InstructionName == InstructionName).ToList();
            if (bindings.Count == 0)
                throw new Exception($"No Instruction Binding by the name {InstructionName} was found");

            bindings = bindings.Where(b => b.Conditions.All(c => c.TestCondition(resolver))).ToList();
            if (bindings.Count == 0)
                throw new Exception($"No Instruction Binding by the name {InstructionName} and the conditions provided");

            if (bindings.Count > 1)
                throw new Exception($"More then one instruction binding found by the name {InstructionName} and the conditions provided");

        }

        public void Setup()
        {
            var logger = resolver.Resolve<ILogger>();
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                logger.LogTrace($"Discovering Instructions: Assembly {a.FullName}");

                // Get all interceptorTypes (Classes with the attribute given in A)
                try
                {
                    a.GetTypes()
                        .Where(b => b.IsClass)
                        .Select(b => new KeyValuePair<Type, BindingInstructionAttribute>(b, (BindingInstructionAttribute)Attribute.GetCustomAttribute(b, typeof(BindingInstructionAttribute))))
                        .Where(b => b.Value != null)
                        .ToList()
                        .ForEach((b) =>
                        {
                            b.Value.Register(resolver, b.Key);

                            //var containerName = b.Value.Name;
                            //var instructionName = b.Value.GetInstructionName();
                            //var conditions = Attribute.GetCustomAttributes(b.Key, typeof(BindingConditionAttribute)).Cast<BindingConditionAttribute>().ToList();
                            //this._bindings.Add(new InstructionItem()
                            //{
                            //    Conditions = conditions,
                            //    InstructionName = instructionName,
                            //    UnityName = containerName
                            //});


                        });
                }
                catch (Exception e)
                {
                    logger.LogError($"Unable to load assembly {a.FullName}");
                    logger.LogError(e);
                }
            }
        }
    }
}
