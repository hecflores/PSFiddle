using MC.Track.TestSuite.Interfaces.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;
using MC.Track.TestSuite.Model.Helpers;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Services.Factories;
using MC.Track.TestSuite.Interfaces.Services;

namespace MC.Track.TestSuite.Services.Resources
{
    public class TemporaryFileResource : ITemporaryFileResource
    {
        public FileInfo File { get; private set; }
        public TemporaryFileSettings Settings { get; private set; }
        private readonly IFileSaverService fileSaverService;
        private readonly IStorageService storageService;

        public TemporaryFileResource(FileInfo file, TemporaryFileSettings settings, IFileSaverService fileSaverService, IStorageService storageService)
        {
            this.File = file;
            this.Settings = settings;
            this.fileSaverService = fileSaverService;
            this.storageService = storageService;
        }
        public void AppendLine(String text)
        {
            this.Append(text + "\r\n");
        }
        public void Append(String text)
        {
            using(var streamer = File.AppendText())
            {
                streamer.Write(text);
            }
        }

        public void UpdateUri(Uri uri)
        {
            XConsole.WriteLine($"{Settings.Title} - {uri}");
        }
        public Uri Flush()
        {
            if (Settings.SaveFileWhenFinished)
            {
                this.fileSaverService.Save(File.FullName);
            }
            if (Settings.UploadToTestCloadStorageWhenFinished)
            {
                var filePath = File.FullName;
                var path = $"{Path.GetFileName(filePath).Replace(" ", "_")}";

                Uri result;// storageService.UploadFile("testlogs", path, filePath).GetAwaiter().GetResult();

                if (Settings.FileLifetime == TemporaryFileLifetimes.PROGRAM_LIFETIME && TemporaryFileFactory.StaticlySavedFiles.ContainsKey(Settings.Name))
                {
                    result = storageService.AppendToFile(TemporaryFileFactory.StaticlySavedFiles[Settings.Name], filePath).GetAwaiter().GetResult();
                    System.IO.File.WriteAllText(filePath, "");
                }
                else if (Settings.FileLifetime == TemporaryFileLifetimes.PROGRAM_LIFETIME && !TemporaryFileFactory.StaticlySavedFiles.ContainsKey(Settings.Name))
                {
                    result = storageService.AppendToFile("testlogs", path, filePath).GetAwaiter().GetResult();
                    TemporaryFileFactory.StaticlySavedFiles.Add(Settings.Name, result);
                    System.IO.File.WriteAllText(filePath, "");
                }
                else
                {
                    result = storageService.UploadFile("testlogs", path, filePath).GetAwaiter().GetResult();
                }
                return result;
            }

            return null;
        }
    }
}
