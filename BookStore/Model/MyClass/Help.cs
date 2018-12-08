using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookStore.Model.MyClass
{
    static class Help
    {     
        static public Byte[] ImageToByte(BitmapImage imageSource)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        static public BitmapImage ByteToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        static public string StandardizeName(string str)
        {

            var fullname = str;
            const string Space = " ";
            var tokens = fullname.Split(new string[] { Space },
                StringSplitOptions.RemoveEmptyEntries)
                .Select(token =>
                {
                    token = token.Trim().ToLower();
                    var firstChar = token.Substring(0, 1).ToUpper();
                    var remaining = token.Substring(1, token.Length - 1);
                    return firstChar + remaining;
                });
            bool isEmpty = !tokens.Any();
            string resuft;
            if (isEmpty)
            {
                resuft = "";
            }
            else
            {
                var builder = new System.Text.StringBuilder();
                foreach (var token in tokens)
                {
                    builder.Append(token);
                    builder.Append(Space);
                }
                builder.Remove(builder.Length - 1, 1);
                resuft = builder.ToString();
            }
            return resuft;

        }
    }
}
