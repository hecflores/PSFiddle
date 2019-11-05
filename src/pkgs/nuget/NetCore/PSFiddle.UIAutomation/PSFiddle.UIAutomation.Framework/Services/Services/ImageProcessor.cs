using PSFiddle.UIAutomation.Framework.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PSFiddle.UIAutomation.Framework.Services.Services
{
    [AthenaRegister(typeof(IImageProcessor))]
    public class ImageProcessor : IImageProcessor
    {
        private readonly IFileManager fileManager;

        public ImageProcessor(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }
        public string CompressImagesFromFolderToGif(string imageFolder, string FilePathToExport = null)
        {
            throw new NotImplementedException();
        }

        public string CompressImagesToGif(Bitmap[] images, string FilePathToExport = null)
        {
            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();
            FilePathToExport = FilePathToExport ?? fileManager.GenerateFileName("Images", "Gif Generation", "gif");
            foreach (System.Drawing.Bitmap bmpImage in images)
            {
                IntPtr bmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
                
                // DeleteObject(bmp); // recommended, handle memory leak
            }
            using (FileStream fs = new FileStream(FilePathToExport, FileMode.Create))
            {
                gEnc.Save(fs);
            }

            return FilePathToExport;
        }

        public string CompressImagesToGif(string[] imagePath, string FilePathToExport = null)
        {
            throw new NotImplementedException();
        }
    }
}
