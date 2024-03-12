﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace Stormgate_Op_Db
{

    public class ScreenReader
    {
        private Bitmap _imgCache;
        private readonly string _tessDataPath = "C:\\Users\\ellio\\Desktop\\C# Learning Fun times\\Text Recognition Software\\tesseract-5.3.4\\tessdata";
        private readonly string _testSavePath = @"C:\\Users\\ellio\\Desktop\\C# Learning Fun times\\Projects\\Stormgate Op Db\\Cache\\TestSave.png";


        public string Read(Rectangle screenSpace)
        {
            _imgCache = new Bitmap(screenSpace.Width, screenSpace.Height);

            using (Graphics graphics = Graphics.FromImage(_imgCache))                                       //create a graphics object from the bitmap
            {
                graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);               //Capture the specified area of the screen

            }

            using (var TessEngine = new TesseractEngine(_tessDataPath, "Eng", EngineMode.Default))              //process bitmap into text
            {
                Page page = TessEngine.Process(_imgCache);
                string output = page.GetText().Trim().ToLower();

                if (output.Length < 1)
                {
                    return null;
                }

                return output;                                                                      
            }
            
        }

        public string ReadCaseSensitive(Rectangle screenSpace)
        {
            _imgCache = new Bitmap(screenSpace.Width, screenSpace.Height);

            using (Graphics graphics = Graphics.FromImage(_imgCache))                                       //create a graphics object from the bitmap
            {
                graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);               //Capture the specified area of the screen

            }

            using (var TessEngine = new TesseractEngine(_tessDataPath, "Eng", EngineMode.Default))              //process bitmap into text
            {
                Page page = TessEngine.Process(_imgCache);
                string output = page.GetText().Trim();

                if (output.Length < 1)
                {
                    return null;
                }

                return output;
            }

        }
        public void Save(Rectangle screenSpace)
        {
            _imgCache = new Bitmap(screenSpace.Width, screenSpace.Height);

            //create a graphics object from the bitmap
            using (Graphics graphics = Graphics.FromImage(_imgCache))
            {
                //Capture the specified area of the screen
                graphics.CopyFromScreen(screenSpace.Location, Point.Empty, screenSpace.Size);
            }
            //Save to file
            _imgCache.Save(_testSavePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    


}
