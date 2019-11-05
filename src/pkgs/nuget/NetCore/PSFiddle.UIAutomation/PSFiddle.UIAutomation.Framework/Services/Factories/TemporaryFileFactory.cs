using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Resources;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Services.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;
using Unity.Attributes;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Services.Factories
{
    [AthenaFactoryRegister(typeof(ITemporaryFileResource),arguments:new Type[] { typeof(TemporaryFileSettings) })]
    public class TemporaryFileFactory : BaseMagicFactory<ITemporaryFileResource, TemporaryFileSettings>
    {
        public  static Dictionary<String, Uri> StaticlySavedFiles = new Dictionary<string, Uri>();
        private IFileManager fileManager;
        private readonly IFileSaverService fileSaverService;
        private readonly IResolver resolver;
        private readonly IStorageService storageService;

        public TemporaryFileFactory(IDisposableTracker disposableTracker,
                                    IFileManager fileManager, 
                                    IFileSaverService fileSaverService,
                                    IResolver resolver,
                                    [Dependency(StorageServiceTypes.TestServices)] IStorageService storageService) : base(disposableTracker)
        {
            this.fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            this.fileSaverService = fileSaverService ?? throw new ArgumentNullException(nameof(fileSaverService));
            this.resolver = resolver;
            this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        protected override ITemporaryFileResource OnCreate(TemporaryFileSettings settings)
        {

            String fileName=this.fileManager.GenerateFileName("TempFiles", settings.Name, settings.Extension);
            if (settings.FileLifetime == TemporaryFileLifetimes.PROGRAM_LIFETIME && settings.UploadToTestCloadStorageWhenFinished && StaticlySavedFiles.ContainsKey(settings.Name))
            {
                // Do nothing... We want to append what we have when we upload...
            }
            else if(settings.InitialContent != null)
            {
                File.WriteAllText(fileName, settings.InitialContent);
            }
            var tempFileResource =  new TemporaryFileResource(new FileInfo(fileName), settings, fileSaverService, storageService);
            return resolver.ApplyIntercepts<ITemporaryFileResource>(tempFileResource);
        }

        protected override void OnDestroy(ITemporaryFileResource obj)
        {
            obj.Flush();
        }
    }
}
