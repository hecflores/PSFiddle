using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.UI.Types;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using System.IO;
using MC.Track.TestSuite.Toolkit.Dependencies.Builders;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Attributes;

namespace MC.Track.TestSuite.Toolkit.Dependencies
{
    [AthenaRegister(typeof(IFilesDependencies), AthenaRegistrationType.Singleton)]
    public class FilesDependencies : IFilesDependencies
    {
        private readonly IFilesBuilder filesBuilder;

        public FilesDependencies(IFilesBuilder filesBuilder)
        {
            this.filesBuilder = filesBuilder;
        }
        public IFilesBuilder Factory()
        {
            return filesBuilder;
        }
    }
}
