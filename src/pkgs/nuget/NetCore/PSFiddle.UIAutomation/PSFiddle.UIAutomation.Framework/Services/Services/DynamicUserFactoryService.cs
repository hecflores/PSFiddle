using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class DynamicUserFactoryService : IDynamicUserFactoryService
    {
        private List<DynamicUserType> dynamicUserTypes;
        private String jsonFile;

        public DynamicUserFactoryService(String jsonFileLocation)
        {
            this.jsonFile = jsonFileLocation;
            this.Initialize();
        }
        public void Initialize()
        {
            if (!File.Exists(this.jsonFile))
                throw new Exception($"Json file {this.jsonFile} doesnt exists");

            this.dynamicUserTypes = JsonConvert.DeserializeObject<List<DynamicUserType>>(File.ReadAllText(this.jsonFile));

            // Needed to know if we can free or not...
            this.dynamicUserTypes.ForEach(b => b.fromPool = true); 
        }
        public void FreeUser(string email)
        {
            var user = this.dynamicUserTypes.Where(b => b.email == email).FirstOrDefault();
            if (user == null)
                throw new Exception($"Dynamic user '{email}' was not found");

            user.inUse = false;
        }

        public DynamicUserType TakeUser()
        {
            var user = this.dynamicUserTypes.Where(b =>!b.inUse).FirstOrDefault();
            if (user == null)
                throw new Exception($"No users are free to take");

            user.inUse = true;
            return user;
        }
    }
}
