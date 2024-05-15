using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stormgate_Op_Db
{
    class Hash
    {
        public byte[] HashImage_SHA1(Bitmap image)
        {
            var ByteImage = BitToByte(image);
            var hash = new SHA1CryptoServiceProvider().ComputeHash(ByteImage);

            return hash;
        }

        public bool QSameHash(byte[] hash1, byte[] hash2)
        {
            //attempt to rewrite this to foreach loop (+Readability?)
            if (hash1.Length == hash2.Length)
            {
                int i = 0;
                while ((i < hash1.Length) && (hash1[i] == hash2[i]))
                {
                    i += 1;
                }
                if (i == hash1.Length)
                {
                    return true;
                }
            }
            return false;
        }

        private byte[] BitToByte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                byte[] bitmapData = stream.ToArray();
                return bitmapData;
            }
        }
    }
}
