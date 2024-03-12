using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stormgate_Op_Db
{
    public class ScreenCapture
    {
        private Bitmap _capturedImg;
        private readonly string _capturedImgPath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\_capturedImg.png";

        public void CaptureImg(Rectangle screenSpace)
        {
            try
            {
                using (Graphics graphics = Graphics.FromImage(_capturedImg))
                {
                    graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Capturing image: ", ex.Message);
            }
        }

        public void SaveCurrentImg()
        {
            if (_capturedImg != null)
            {
                _capturedImg.Save(_capturedImgPath);
            }
            else throw new Exception("No Image Currently Stored in Object!");
        }


        public bool QImgMatch(string reffPath)
        {
            if (reffPath.EndsWith(".png") && _capturedImgPath.EndsWith(".png"))
            {
                if (File.ReadAllBytes(reffPath) == File.ReadAllBytes(_capturedImgPath))
                {
                    return true;
                }

                return false;
            }
            else throw new Exception("Wrong image type!");
        }

    }
}
