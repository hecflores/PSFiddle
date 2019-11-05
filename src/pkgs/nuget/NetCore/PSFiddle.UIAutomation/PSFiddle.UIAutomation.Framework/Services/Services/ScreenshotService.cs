using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class ScreenCaptureHelper
    {
        private static bool FirstTime = true;
        private static DirectoryInfo imagesFolder;
        private static DirectoryInfo ImagesFolder
        {
            get
            {
                imagesFolder = imagesFolder != null ? imagesFolder : (new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Images")));
                return imagesFolder;
            }
        }

        public static void CleanDirectory()
        {
            // Image File Path
            if (ImagesFolder.Exists)
            {
                try
                {
                    ImagesFolder.Delete(true);
                    ImagesFolder.Refresh();
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to delete folder 'Images'", e);
                }
            }

            ImagesFolder.Create();
            ImagesFolder.Refresh();
        }
        
        public static String GenerateFilePath(String Name, String SubName)
        {
            int ImageCount = ImagesFolder.GetFiles().Length;
            String imageFilePath;
            do
            {
                imageFilePath = String.Format(Path.Combine(ImagesFolder.FullName, "{0:00000000000}-{1}-{2}-Screenshot.jpg"), ImageCount++, Name, SubName);
            }
            while (File.Exists(imageFilePath));

            return imageFilePath;
        }
        public enum CaptureType
        {
            Informative = 0,
            Error = 1,
            Success = 2
        }
        public String CaptureScreen(String Name, String SubName, String Title, CaptureType type, IWebDriver webDriver)
        {
            // First time
            if (FirstTime)
            {
                //Clean Directory
                CleanDirectory();

                FirstTime = false;
            }

            ITakesScreenshot screenCapture = (ITakesScreenshot)webDriver;

            // Generate File Path
            String FilePath = GenerateFilePath(Name, SubName);

            try
            {
                // Capture
                var capture = screenCapture.GetScreenshot();
                capture.SaveAsFile(FilePath);
               
            }
            catch (Exception e)
            {
                String logPath = Path.ChangeExtension(FilePath, "big.err");
                File.WriteAllText(logPath, $"Screen Capture Failed\nMessage...:{e.Message}\n{e.StackTrace}");

                FilePath = logPath;
            }


            return FilePath;
        }
    }
}
