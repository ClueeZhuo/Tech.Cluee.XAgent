using NewLife;

using System;
using System.Security.Cryptography;
using System.Text;

namespace Tech.Cluee.XAgent
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //new PrintService().Main(args);

            var buf = "123".GetBytes(Encoding.UTF8);

            var pass = "123".GetBytes(Encoding.UTF8);

            var mode = (CipherMode)Enum.Parse(typeof(CipherMode), CipherMode.CBC + "");
            var padding = (PaddingMode)Enum.Parse(typeof(PaddingMode), PaddingMode.PKCS7 + "");
            var aes = new AesCryptoServiceProvider();
            buf = aes.Encrypt(buf, pass, mode, padding);

            Console.WriteLine(System.Text.Encoding.UTF8.GetString(buf));

            Console.ReadKey();
        }
    }
}
