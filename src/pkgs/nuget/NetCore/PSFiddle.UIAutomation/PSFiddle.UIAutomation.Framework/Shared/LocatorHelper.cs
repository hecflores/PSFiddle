using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PSFiddle.UIAutomation.Framework.Shared;
namespace MC.Track.Shared
{
    public class LocatorHelper
    {
        public static void Validate()
        {
            throw new NotImplementedException();
        }

        public static String GetLocationName(String Name)
        {
            var associatedField = typeof(LocatorNames).GetProperties().ToList()
               .Where(b => b.PropertyType.Equals(typeof(String)))
               .Where(b => b.Name.Equals(Name))
               .FirstOrDefault();

            if (associatedField == null)
            {
                throw new Exception($"Element id {Name} not found");
            }

            return associatedField.GetValue(null) as String;
        }
        public static String GetLocationSelector(String Name)
        {
            var associatedField = typeof(LocatorSelectors).GetProperties().ToList()
               .Where(b => b.PropertyType.Equals(typeof(String)))
               .Where(b => b.Name.Equals(Name))
               .FirstOrDefault();

            if (associatedField == null)
            {
                throw new Exception($"Element id {Name} not found");
            }

            return associatedField.GetValue(null) as String;
        }
        public static String GetLocationFieldFromName(String Name)
        {
            var associatedField = typeof(LocatorNames).GetProperties().ToList()
                .Where(b => b.PropertyType.Equals(typeof(String)))
                .Where(b => b.GetValue(null).Equals(Name))
                .FirstOrDefault();

            if (associatedField == null)
            {
                throw new Exception($"Element Name {Name} not found");
            }

            return associatedField.Name;
        }
        public static String GetLocationPathFromName(ref String Name)
        {
            var match = Regex.Match(Name, @"^UIElement \|(.+)\|\=\=\|(.+)\|$");
            if (match.Success)
            {
                Name = match.Groups[1].Value;
                return match.Groups[2].Value;
            }
            String LocationId = GetLocationFieldFromName(Name);
            return GetLocationSelector(LocationId);
        }
    }
}
