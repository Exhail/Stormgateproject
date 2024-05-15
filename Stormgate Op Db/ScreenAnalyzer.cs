using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Threading;

namespace Stormgate_Op_Db
{
    class ScreenAnalyzer
    {
        private Bitmap _textExtractionImage;

        private readonly string _tessDataPath = @"C:\\Users\\ellio\\Desktop\\C# Learning Fun times\\Text Recognition Software\\tesseract-5.3.4\\tessdata";

        private readonly string _textImgTestSavePath2 = @"C:\\Users\\ellio\\Desktop\\C# Learning Fun times\\Projects\\Stormgate Op Db\\Cache\\TestSave2.png";
        private readonly string _textImgTestSavePath = @"C:\\Users\\ellio\\Desktop\\C# Learning Fun times\\Projects\\Stormgate Op Db\\Cache\\TestSave.png";
        private readonly string _comparisonImagePath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\comparisonImage.png";

        public void SaveImg(Rectangle screenSpace)
        {
            var comparisonImage = new Bitmap(screenSpace.Width, screenSpace.Height);

            try
            {
                using (Graphics graphics = Graphics.FromImage(comparisonImage))
                {
                    graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);
                }


                comparisonImage.Save(_comparisonImagePath, System.Drawing.Imaging.ImageFormat.Png);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Capturing image: ", ex.Message);
            }
        }

        public bool QImgMatch(Rectangle screenSpace, string reffImgPath)
        {
            var reffImg = new Bitmap(reffImgPath);
            Bitmap targetImg = ConvertRectangle(screenSpace);
            var reffPixelCount = PixelCount(reffImg);
            var targetPixelCount = PixelCount(targetImg);

            bool pixelCountCheck = false;

            if (targetPixelCount >= reffPixelCount - (reffPixelCount * 0.2) && targetPixelCount <= reffPixelCount + (reffPixelCount * 0.2))
            {
                pixelCountCheck = true;
            }

            var reffPixelsPerColor = CountColours(reffImg);
            var targetPixelsPerColor = CountColours(targetImg);

            int reffColourCount = 0;
            foreach (KeyValuePair<Color, int> Colour in reffPixelsPerColor)
            {
                reffColourCount++;
            }
            int similarColourCount = 0;
            foreach (KeyValuePair<Color,int> reffcolour in reffPixelsPerColor)
            {
                foreach (KeyValuePair<Color,int> targetcolour in targetPixelsPerColor)
                {
                    if (reffcolour.Key == targetcolour.Key)
                    {
                        var reffPixelColour = reffcolour.Value;
                        var numOfPixels2 = targetcolour.Value;

                        if (targetcolour.Value <= (reffcolour.Value + (reffcolour.Value * 0.2)) && targetcolour.Value >= reffcolour.Value + -(reffcolour.Value * 0.2))
                        {
                            similarColourCount++;
                        }
                    }
                }
            }

            bool similarColourCheck = false;

            if (similarColourCount >= (reffColourCount - (reffColourCount * 0.2)) & (similarColourCount <= (reffColourCount + (reffColourCount * 0.2))))
            {
                similarColourCheck = true;
            }

            if (similarColourCheck && pixelCountCheck)
            {
                return true;
            }
            return false;
        }

        public string Read(Rectangle screenSpace)
        {
            Bitmap _textExtractionImage = ConvertRectangle(screenSpace);

            using (var TessEngine = new TesseractEngine(_tessDataPath, "Eng", EngineMode.Default)) 
            {
                Page page = TessEngine.Process(_textExtractionImage);
                string output = page.GetText().Trim().ToLower();

                if (output.Length < 1)
                {
                    return null;
                }
                return output;
            }
        }

        public string ReadCS(Rectangle screenSpace)
        {
            Bitmap _textExtractionImage = ConvertRectangle(screenSpace);

            using (var TessEngine = new TesseractEngine(_tessDataPath, "Eng", EngineMode.Default))
            {
                Page page = TessEngine.Process(_textExtractionImage);
                string output = page.GetText().Trim();

                if (output.Length < 1)
                {
                    return null;
                }

                return output;
            }
        }

        private Dictionary<Color, int> CountColours(Bitmap img)
        {
            var colourCount = new Dictionary<Color, int>();

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color pixelColour = img.GetPixel(x, y);

                    if (colourCount.ContainsKey(pixelColour))
                    {
                        colourCount[pixelColour]++;
                    }
                    else colourCount.Add(pixelColour, 1);
                }
            }
            return colourCount;
        }

        private int PixelCount(Bitmap img)
        {
            int pixelCount = 0;

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixelCount++;
                }
            }
            return pixelCount;
        }

        private Bitmap ConvertRectangle(Rectangle screenSpace)
        {
            var bitmap = new Bitmap(screenSpace.Width, screenSpace.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);
            }

            return bitmap;
        }
    }
}
