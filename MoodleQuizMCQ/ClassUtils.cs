using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MoodleQuizMCQ
{
    internal static class ClassUtils
    {
        public static byte[] ImageToByte(Image image)
        {
            using MemoryStream ms = new();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray(); ;
        }

        public static Image ByteToImage(byte[] imageBytes)
        {
            using MemoryStream ms = new(imageBytes);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static System.Collections.Generic.IEnumerable<TControl> GetChildControls<TControl>(this System.Windows.Forms.Control control) where TControl : System.Windows.Forms.Control
        {
            var children = (control.Controls != null) ? control.Controls.OfType<TControl>() : Enumerable.Empty<TControl>();
            return children.SelectMany(c => GetChildControls<TControl>(c)).Concat(children);
        }

        public static string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using MemoryStream ms = new();
            // Convert Image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        public static bool IsBase64String(string base64)
        {
            base64 = base64.Trim();
            return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
