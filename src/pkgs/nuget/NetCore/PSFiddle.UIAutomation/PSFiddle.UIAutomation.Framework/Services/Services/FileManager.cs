using CSharpFunctionalExtensions;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class FileManager : IFileManager
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public FileManager(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }
        private String FixPath(string path)
        {
            var finalString = Regex.Replace(path, @"[^\w\d\- ]", "");
            return finalString.Substring(0, Math.Min(20, finalString.Length));
        }
        public void ClearDirectory(string DirectoryName)
        {
            DirectoryName = Path.Combine(Directory.GetCurrentDirectory(), DirectoryName);

            lock (DirectoryName)
            {
                // Image File Path
                if (Directory.Exists(DirectoryName))
                {
                    try
                    {
                        Directory.Delete(DirectoryName, true);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to delete folder 'Images'", e);
                    }
                }

                Directory.CreateDirectory(DirectoryName);
            }
        }

        public string GenerateFileName(String DirectoryName, string Name, String Extension)
        {
            Name = FixPath(Name);
            DirectoryName = Path.Combine(Directory.GetCurrentDirectory(), DirectoryName);
            
            lock (DirectoryName)
            {
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }
                int ImageCount = Directory.GetFiles(DirectoryName).Length;
                String filePath;
                do
                {
                    filePath = String.Format(Path.Combine(DirectoryName, "{0:00000000000}-{1}.{2}"), ImageCount++, Name, Extension);
                }
                while (File.Exists(filePath));

                return filePath;
            }
        }
        public bool CreateFileIfNotExistsOrContentChanged(String DirectoryName, string Name, String Extension, String content)
        {
            Name = FixPath(Name);
            DirectoryName = Path.Combine(Directory.GetCurrentDirectory(), DirectoryName);

            lock (DirectoryName)
            {
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }
                int ImageCount = Directory.GetFiles(DirectoryName).Length;
                String filePath;
                filePath = String.Format(Path.Combine(DirectoryName, "{0:00000000000}-{1}.{2}"), ImageCount++, Name, Extension);

                if (File.Exists(filePath))
                {
                    if(File.ReadAllText(filePath) == content)
                        return false;
                }

                File.WriteAllText(filePath, content);
                return true;
            }
        }
        public String ResolvePath(String RelativeFileName)
        {
            return Path.Combine(config.SolutionPath, RelativeFileName);
        }
        public Result<String> GetAllContent(String RelativeFileName)
        {
            try
            {
                return Result.Ok(File.ReadAllText(Path.Combine(config.SolutionPath, RelativeFileName)));
            }
            catch(Exception e)
            {
                logger.LogError(e);
                return Result.Fail<String>($"Failed to read all content for file {RelativeFileName}: \n{e.Message}");
            }
        }
    }
}
