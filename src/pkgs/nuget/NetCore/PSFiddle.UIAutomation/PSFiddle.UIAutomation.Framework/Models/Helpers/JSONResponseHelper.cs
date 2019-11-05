using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Helpers
{
    public class JSONResponseHelper
    {
        public RootObject LoadJSONData(string json)
        {
            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);
            return rootObject;
        }
        public class Organization
        {
            public string OrganizationID { get; set; }
            public string OrganizationName { get; set; }
        }

        public class SourceFileStatus
        {
            public string StatusCode { get; set; }
        }

        public class Debtor
        {
            public string OrganizationID { get; set; }
            public string OrganizationName { get; set; }
        }

        public class Creditor
        {
            public string OrganizationID { get; set; }
            public string OrganizationName { get; set; }
        }

        public class InstructionStatus
        {
            public string StatusCode { get; set; }
            public string StatusDetails { get; set; }
            public string StatusAction { get; set; }
        }

        public class Instruction
        {
            public Debtor Debtor { get; set; }
            public Creditor Creditor { get; set; }
            public string EndToEndIdentification { get; set; }
            public object UUID { get; set; }
            public int InstructionIdentification { get; set; }
            [JsonConverter(typeof(SingleValueArrayConverter<InstructionStatus>))]
            public List<InstructionStatus> InstructionStatus { get; set; }
        }

        public class RootObject
        {
            public Organization Organization { get; set; }
            public string CorrelationID { get; set; }
            public string SourceFileID { get; set; }
            public SourceFileStatus SourceFileStatus { get; set; }
            public string SourceFileIdentifier { get; set; }
            [JsonConverter(typeof(SingleValueArrayConverter<Instruction>))]
            public List<Instruction> Instructions { get; set; }
        }
        public class SingleValueArrayConverter<T> : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                object retVal = new Object();
                if (reader.TokenType == JsonToken.StartObject)
                {
                    T instance = (T)serializer.Deserialize(reader, typeof(T));
                    retVal = new List<T>() { instance };
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    retVal = serializer.Deserialize(reader, objectType);
                }
                return retVal;
            }

            public override bool CanConvert(Type objectType)
            {
                return true;
            }
        }
    }
}
