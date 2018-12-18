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

        static public int StringMonthToInt(string StringMonth)
        {
            //dạng nhập vào tháng 12

            if (string.IsNullOrEmpty(StringMonth))
            {
                return 0;
            }

            //Tách chuỗi
            var tokens = StringMonth.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //Lấy chuỗi thứ 2
            int Int = int.Parse(tokens[1]);

            return Int;
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

        
        static public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder(); // chuỗi tổng

            int number = new int(); // số 2 chữ số
            StringBuilder CHARS = new StringBuilder(); // chuỗi in hoa
            StringBuilder chars = new StringBuilder(); // chuỗi in thường

            //random số có 2 chữ số
            Random random = new Random();

            number = random.Next(10, 99);      
            builder.Append(number); //nối vào chuỗi tổng


            //random kí tự in hoa
            
            char CH;
            for (int i = 0; i < 2; i++)
            {
                CH = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                CHARS.Append(CH);
            }
            builder.Append(CHARS); //nối vào chuỗi tổng


            //random kí tự in thường        
            char ch;
            for (int i = 0; i < 2; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));                
                chars.Append(ch);
            }
            builder.Append(chars.ToString().ToLower()); //nối vào chuỗi tổng

            return builder.ToString();
        }

        public static string Base64Encode(string plainText) //Hàm mã hóa pass sang base64
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData) //Hàm giải mã pass
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
