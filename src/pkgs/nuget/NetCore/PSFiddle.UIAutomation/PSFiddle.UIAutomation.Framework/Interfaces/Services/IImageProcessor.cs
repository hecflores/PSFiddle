using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.UIAutomation.Framework.Interfaces.Services
{
    public interface IImageProcessor
    {
        String CompressImagesToGif(Bitmap[] images, String FilePathToExport = null);
        String CompressImagesToGif(String[] imagePath, String FilePathToExport = null);
        String CompressImagesFromFolderToGif(String imageFolder, String FilePathToExport = null);

    }
}
