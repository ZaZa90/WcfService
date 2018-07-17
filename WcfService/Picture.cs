using System;
using System.Collections.Generic;
using System.Drawing;
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

        private ResultPoint[] resPts;

        public void CreatePicture(byte[] imageBytes,string name)
        {

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            Bitmap bitMap = new Bitmap(ms);
            image = (Image)bitMap;
            String path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + name + ".jpg";

            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            this.name = name;
            this.path = path;
            url = "..\\Images\\" + name + ".jpg";

        }

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
            // create an in memory bitmap
            var barcodeBitmap = (Bitmap)Bitmap.FromFile(path);
            // decode the barcode from the in memory bitmap
            var barcodeResult = barcodeReader.Decode(barcodeBitmap);
            try
            {
                QR = barcodeResult.Text;
                resPts = barcodeResult.ResultPoints;
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

        
        public float getAngle() {
            float vx, vy, ax, ay, bx, by, m12, m13, m23, p1, p2, p3, p4;
            float selectedCoeff1, selectedCoeff2;
            float angleRad, displacement;
            float[] x, y;

            x = new float[3];
            y = new float[3];

            for (int i=0; i < 3; i++){
                x[i] = resPts[i].X;
                y[i] = resPts[i].Y;
            }
            
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
        }
    }
}