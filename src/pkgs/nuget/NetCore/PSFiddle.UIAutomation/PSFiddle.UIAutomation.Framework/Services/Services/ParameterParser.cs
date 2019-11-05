using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class ParameterParser : IParameterParser
    {
        private IStateManagment stateManagment;
        public ParameterParser(IStateManagment stateManagment)
        {
            this.stateManagment = stateManagment;
        }
        public string Filter(string content)
        {
            return Regex.Replace(content, @"\<(.*)\>", (regex) =>
            {
                var variableName = regex.Groups[1].Value;

                var splits = variableName.Split('.');

                var obj = this.stateManagment.Get(splits[0]);
                for(var i = 1; i < splits.Length; i++)
                {
                    Type type = obj.GetType();
                    var field = type.GetProperty(splits[i]);
                    if(field == null)
                    {
                        throw new Exception($"Field {splits[i]} doesnt exists for obj {splits[i-1]}");
                    }
                    obj = field.GetValue(obj);

                    if(obj == null)
                    {
                        throw new Exception($"Field {splits[i]} in obj {splits[i - 1]} is null");
                    }
                }

                return obj.ToString();
            });
           
        }
    }
}
