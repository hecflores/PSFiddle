using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class FileSaverService : IFileSaverService
    {
        private Action<String> saveAction;
        public FileSaverService(Action<String> saveAction)
        {
            this.saveAction = saveAction;
        }

        public void Save(string FilePath)
        {
            this.saveAction(FilePath);
        }
    }
}
