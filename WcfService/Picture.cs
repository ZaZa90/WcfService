using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;

namespace WcfService
{
    public class Picture
    {

        public String base64code;
        private string name;
        private Image image;
        private string url;
        private string path;
        private string QR = "Error";
        private int angle = -1;

        private ResultPoint[] resPts;

        public string getPts()
        {
            string str = "";
            if (resPts != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (resPts[i] != null) str = str + resPts[i].X.ToString() + " " + resPts[i].Y.ToString() + "; ";
                }
            }
            return str;
        }

        public void CreatePicture(byte[] imageBytes,string name)
        {

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            Bitmap bitMap = new Bitmap(ms);

//            Bitmap grayBM = MakeGrayscale3(bitMap);
  //          Bitmap newBM = AdjustContrast(grayBM, 100.0f);

            image = (Image)MakeGrayscale3(bitMap);
            //Image imageFinal = (Image)newBM;

            String path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + name + ".jpg";

            //String pathGray = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + name + "Gray.jpg";
            //String pathFinal = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + name + "Final.jpg";

            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

            //imageGray.Save(pathGray, System.Drawing.Imaging.ImageFormat.Jpeg);
            //imageFinal.Save(pathFinal, System.Drawing.Imaging.ImageFormat.Jpeg);

            this.name = name;
            this.path = path;
            url = "..\\Images\\" + name + ".jpg";

        }

        private static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /*        private static Bitmap AdjustContrast(Bitmap Image, float Value)
                {
                    Value = (100.0f + Value) / 100.0f;
                    Value *= Value;
                    Bitmap NewBitmap = (Bitmap)Image.Clone();
                    BitmapData data = NewBitmap.LockBits(
                        new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                        ImageLockMode.ReadWrite,
                        NewBitmap.PixelFormat);
                    int Height = NewBitmap.Height;
                    int Width = NewBitmap.Width;

                    unsafe
                    {
                        for (int y = 0; y < Height; ++y)
                        {
                            byte* row = (byte*)data.Scan0 + (y * data.Stride);
                            int columnOffset = 0;
                            for (int x = 0; x < Width; ++x)
                            {
                                byte B = row[columnOffset];
                                byte G = row[columnOffset + 1];
                                byte R = row[columnOffset + 2];

                                float Red = R / 255.0f;
                                float Green = G / 255.0f;
                                float Blue = B / 255.0f;
                                Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                                Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                                Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

                                int iR = (int)Red;
                                iR = iR > 255 ? 255 : iR;
                                iR = iR < 0 ? 0 : iR;
                                int iG = (int)Green;
                                iG = iG > 255 ? 255 : iG;
                                iG = iG < 0 ? 0 : iG;
                                int iB = (int)Blue;
                                iB = iB > 255 ? 255 : iB;
                                iB = iB < 0 ? 0 : iB;

                                row[columnOffset] = (byte)iB;
                                row[columnOffset + 1] = (byte)iG;
                                row[columnOffset + 2] = (byte)iR;

                                columnOffset += 4;
                            }
                        }
                    }

                    NewBitmap.UnlockBits(data);

                    return NewBitmap;
                }
        */
        public string getUrl()
        {
            return url;
        }

        public Image getImage()
        {
            return image;
        }

        public string getBase64Code()
        {
            return base64code;
        }

        public string getQRCode()
        {
            return this.QR;
        }

        public int ScanQR()
        {
            var barcodeReader = new BarcodeReader();
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
            barcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            barcodeReader.AutoRotate = false;
            barcodeReader.Options.TryHarder = true;
            
            // create an in memory bitmap
            var barcodeBitmap = (Bitmap)Bitmap.FromFile(path);
            // decode the barcode from the in memory bitmap
            var barcodeResult = barcodeReader.Decode(barcodeBitmap);
            try
            {
                QR = barcodeResult.Text;
                resPts = barcodeResult.ResultPoints;
                angle = (int)calculateAngle();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        /*public void GenerateBase64String()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                String path =  System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\picture.jpg";

                image = Image.FromFile(path, true);

                // Convert Image to byte[]
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                url = "..\\Images\\picture.jpg";
                base64code = base64String;
            }
        }*/
        /*public void CreatePictureByCode(string code,string name){
        // Convert Base64 String to byte[]
        byte[] imageBytes = Convert.FromBase64String(code);
        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

        // Convert byte[] to Image
        ms.Write(imageBytes, 0, imageBytes.Length);
        image = Image.FromStream(ms, true);
        String path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + name + ".jpg"; ;
        image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        url = "..\\Images\\" + name + ".jpg";
        base64code = code;
    }*/

        public int getAngle() { return angle; }

        public float calculateAngle() {
            float angle, displacement;
            double[] x, y;

            x = new double[3];
            y = new double[3];
            
            for (int i=0; i < 3; i++){
                x[i] = (double)resPts[i].X;
                y[i] = (double)resPts[i].Y;
            }
/*
                                    //Angular Coefficients
                                    m12 = (y[1] - y[0]) / (x[1] - x[0]);
                                    m13 = (y[2] - y[0]) / (x[2] - x[0]);
                                    m23 = (y[2] - y[1]) / (x[2] - x[1]);

                                    // First i find the vertex of the triangle where the angle is 90 degree
                                    if (m12 * m13 == -1)
                                    {
                                        vx = x[0];
                                        vy = y[0];
                                        ax = x[1];
                                        ay = y[1];
                                        bx = x[2];
                                        by = y[2];
                                        selectedCoeff1 = m12;
                                        selectedCoeff2 = m13;
                                    }
                                    else if (m12 * m23 == -1)
                                    {
                                        vx = x[1];
                                        vy = y[1];
                                        ax = x[0];
                                        ay = y[0];
                                        bx = x[2];
                                        by = y[2];
                                        selectedCoeff1 = m12;
                                        selectedCoeff2 = m23;
                                    }
                                    else
                                    {
                                        vx = x[2];
                                        vy = y[2];
                                        ax = x[1];
                                        ay = y[1];
                                        bx = x[0];
                                        by = y[0];
                                        selectedCoeff1 = m13;
                                        selectedCoeff2 = m23;
                                    }

                                    p1 = ax - vx;
                                    p2 = ay - vy;
                                    p3 = by - vy;

                                    angleRad = (selectedCoeff1 <= 0) ? Math.Abs(selectedCoeff1) : Math.Abs(selectedCoeff2);
                                    if (p1 >= 0 && p2 >= 0)
                                    {
                                        if (p3 >= 0) displacement = 180;
                                        else displacement = 270;
                                    }
                                    else if (p1 < 0 && p2 < 0)
                                    {
                                        if (p3 >= 0) displacement = 90;
                                        else displacement = 0;
                                    }
                                    else if (p1 >= 0 && p2 < 0)
                                    {
                                        if (p3 >= 0) displacement = 270;
                                        else displacement = 0;
                                    }
                                    else
                                    {

                                        if (p3 >= 0) displacement = 180;
                                        else displacement = 90;
                                    }
                                    return displacement + angleRad * (float)(180.0 / Math.PI);
                            */
            if (x[2] - x[1] == 0)
            {
                if (x[1] - x[0] > 0) return 270;
                else return 90;
            }
            angle = (float)(Math.Atan((y[1] - y[2]) / (x[2] - x[1])) * (180 / Math.PI));
            if(((angle > 0) && (y[2] - y[1] > 0)) || ((angle < 0) && (y[2] - y[1] < 0))) angle = 180 + angle;
            else if (angle < 0) angle = 360 + angle;
            return angle;    
        }
    }
}