using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Toolkit.Extensions;
using MC.Track.TestSuite.Interfaces.Config;
using AE.Net.Mail;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.UI.Types;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using System.IO;
using MC.Track.TestSuite.Toolkit.Dependencies.Shared;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Toolkit.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Repositories;
using MC.Track.TestSuite.Model.Helpers;
using System.Diagnostics;
using System.Data;

namespace MC.Track.TestSuite.Toolkit.Dependencies.Builders
{
    [AthenaRegister(typeof(IFilesBuilder), AthenaRegistrationType.Type)]
    public class FilesBuilder : BaseListBuilder<GenericFileType, IFilesBuilder>, IFilesBuilder
    {
        private readonly IConfiguration config;
        private readonly IResolver resolver;
        
        public FilesBuilder(IConfiguration config, IResolver resolver)
        {
            this.config = config;
            this.resolver = resolver;
            

        }
        public IFilesBuilder Create(String Content, Action<IFileBuilder> buildIt = null)
        {
            return this.AddBuildStep("Creating a File", (files) =>
            {
                var filePath = Path.GetTempFileName();
                File.WriteAllText(filePath, Content);

                UseFile(filePath, buildIt);
                return files;
            });
        }
        public IFilesBuilder UseFile(string FilePath, Action<IFileBuilder> buildIt = null)
        {
            return this.AddBuildStep("Use File", (files) =>
            {
                if (files == null) files = new List<GenericFileType>();

                var genericFile = new GenericFileType();
                genericFile.FileName = Path.GetFileName(FilePath);
                genericFile.FilePath = Path.Combine(config.SolutionPath, FilePath);
                genericFile.Uploaded = new List<FileMetadata>();

                var fileBuilder = resolver.ApplyIntercepts<IFileBuilder>(new FileBuilder(genericFile, this.resolver));
                buildIt?.Invoke(fileBuilder);
                genericFile = fileBuilder.Build();

                files.Add(genericFile);
                return files;
            });
        }

    }
    public class FileBuilder : BaseBuilder<GenericFileType, IFileBuilder>, IFileBuilder
    {
        private readonly IFilesBuilder filesBuilder;
        private readonly IDisposableTracker disposableTracker;
        private readonly IFileManager fileManager;
        private readonly IRawRepository rawRepository;

        private String _activeTask = "Nothing really...";
        private UserType _savedUser = null;

        public FileBuilder(GenericFileType initial, IResolver resolver) : base(initial)
        {
            this.filesBuilder = resolver.Resolve<IFilesBuilder>();
            this.disposableTracker = resolver.Resolve<IDisposableTracker>();
            this.fileManager = resolver.Resolve<IFileManager>();
            this.rawRepository = resolver.Resolve<IRawRepository>();
        }

        [BuilderTearDown]
        public void TearDown()
        {
            _savedUser = null;
        }
        public IUploadedFileBuilder AddBuildStepRequiringUser(String name, Func<GenericFileType, GenericFileType> action)
        {
            return this.AddBuildStep(name, (file) =>
            {
                Assert.IsNotNull(_savedUser, "Unable to use builder method because a user was not setup. Please use the method 'UsingUser' to prep the user");
                action(file);
                return file;
            });
        }
        public IFileBuilder Done()
        {
            return this.AddBuildStep($"Done with {_activeTask}", (file) =>
            {
                return file;
            });
        }

        
       
        public IFileBuilder UsingUser(UserType user)
        {
            return this.AddBuildStep($"Using User {user.Email}", (file) =>
            {
                _savedUser = user;
                return file;
            });
        }
        
        public IFileBuilder RunAsPowershellContent(int timeout = 1000000)
        {
            return this.AddBuildStep("Running as powershell content", (file) =>
            {
                var processStartInfo = new ProcessStartInfo()
                {
                    Arguments = file.FilePath,
                    FileName = "Powershell.exe",
                    CreateNoWindow = true
                };
                
                var process = Process.Start(processStartInfo);
                process.WaitForExit(timeout);
                if(process.ExitCode > 0)
                {
                    throw new Exception($"Execuiting Powershell file {file.FilePath} failed - With Exit Code {process.ExitCode}");
                }
                return file;
            });
        }
        public IFileBuilder DeleteOnExit()
        {
            return this.AddBuildStep("Delete on exit", (file) =>
            {
                this.disposableTracker.Add(new DisposableResourceType()
                {
                    resource = file.FilePath,
                    disposeDelegate = (resFile) => File.Delete((String)resFile)
                });
                return file;
            });
        }
        public IFileBuilder DeleteNow()
        {
            return this.AddBuildStep("Delete on exit", (file) =>
            {
                File.Delete(file.FilePath);
                return file;
            });
        }

        public IFileBuilder MakeCopy()
        {
            return this.AddBuildStep("Make Copy", (file) =>
            {
                var newFilePath = fileManager.GenerateFileName("CopiedFiles", file.FileName, Path.GetExtension(file.FilePath));
                File.Copy(file.FilePath, newFilePath);
                return filesBuilder.UseFile(newFilePath).BuildSingle();
            });
        }

        public IFileBuilder Rename(string Name)
        {
            return this.AddBuildStep($"Rename file to {Name}", (file) =>
            {
                var newDirectory = Path.GetTempPath();
                var filePath = Path.Combine(newDirectory, Name);
                File.Copy(file.FilePath, filePath);
                File.Delete(file.FilePath);
                return filesBuilder.UseFile(filePath).BuildSingle();
            });
        }

        public IFileBuilder RenameToRandomName(string Extension)
        {
            return this.Rename($"{Guid.NewGuid().ToString()}.{Extension}");
        }

        public IFileBuilder TokenReplace(string TokenName, string TokenValue)
        {
            return this.AddBuildStep("Token Replace", (file) =>
            {
                var allText = File.ReadAllText(file.FilePath);
                allText = allText.Replace(TokenName, TokenValue);
                File.WriteAllText(file.FilePath,allText);
                return file;
            });
        }
        public IFileBuilder ExecuteAsSqlStatment(Action<DataSet> callback = null)
        {
            return this.AddBuildStep("Execute as Sql Statement", (file) =>
            {
                var dataSet = this.rawRepository.ExecuteReaderQuery(File.ReadAllText(file.FilePath));
                callback?.Invoke(dataSet);
                return file;
            });
        }

        
    }
}
