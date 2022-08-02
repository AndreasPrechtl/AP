using System;
using System.Drawing;

namespace AP.Drawing
{
    /// <summary>
    /// Extension class for image manipulations.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Scales an image maintaining the original aspect ratio.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maximumSize">The maximum dimensions for the image.</param>
        /// <param name="callback">Provides a callback method to prematurely stop scaling.</param>
        /// <returns>The scaled image.</returns>
        public static Image Scale(this Image image, Size maximumSize, Image.GetThumbnailImageAbort callback = null)
        {
            Size imageSize = image.Size;
            double width = imageSize.Width;
            double height = imageSize.Height;

            double factor = Math.Min(maximumSize.Width / width, maximumSize.Height / height);
                        
            return image.GetThumbnailImage((int)Math.Round(width * factor), (int)Math.Round(height * factor), callback, IntPtr.Zero);
        }
    }
}
