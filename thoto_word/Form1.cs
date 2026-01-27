using Emgu;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Drawing.Drawing2D;

namespace thoto_word
{
    public partial class Form1 : Form
    {
        int xDown = 0;
        int yDown = 0;
        int xUp = 0;
        int yUp = 0;
        Rectangle rectCropArea = new Rectangle();
        MemoryStream ms = new MemoryStream();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Bitmap originalImage = new Bitmap("a.jpg");
            //// Создайте новую bitmap по размеру PictureBox
            //Bitmap highQualityImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            //using (Graphics g = Graphics.FromImage(highQualityImage))
            //{
            //    // Установите высокое качество
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(originalImage, 0, 0, pictureBox1.Width, pictureBox1.Height);
            //}
            //pictureBox1.Image = highQualityImage;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Invalidate();

            xDown = e.X;
            yDown = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //pictureBox1.Image.Clone();
            xUp = e.X;
            yUp = e.Y;
            Rectangle rec = new(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
            using (Pen pen = new(Color.YellowGreen, 3))
            {

                pictureBox1.CreateGraphics().DrawRectangle(pen, rec);
            }
            rectCropArea = rec;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Show();

            //scrinshot();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            ras();
        }

        private void scrinshot()
        {
            using (Bitmap bounds = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bounds))
                {
                    g.CopyFromScreen(0, 0, 0, 0, bounds.Size);
                }
                bounds.Save("6.jpg");
            }
            Bitmap originalImage = new Bitmap("6.jpg");
            // Создайте новую bitmap по размеру PictureBox
            Bitmap highQualityImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics g = Graphics.FromImage(highQualityImage))
            {
                // Установите высокое качество
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            pictureBox1.Image = highQualityImage;
            originalImage.Dispose();
            File.Delete("6.jpg");
        }

        private void ras()
        {
            // 1. Инициализация движка Tesseract
            // Необходимо указать путь к папке tessdata и язык
            using (Tesseract ocr = new Tesseract(@"D:\testData", "rus", OcrEngineMode.TesseractLstmCombined))
            {
                // 2. Загрузка изображения с помощью Emgu CV
                using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(@"5.png"))
                {
                    // Можно выполнить предварительную обработку, например, перевод в серый цвет и бинаризацию
                    Mat grayImage = new Mat();
                    CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                    CvInvoke.Threshold(grayImage, grayImage, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

                    // 3. Установка изображения для распознавания
                    // Если использовалась предобработка, передайте обработанное изображение
                    ocr.SetImage(image.Mat);

                    // 4. Выполнение распознавания
                    ocr.Recognize();

                    // 5. Получение распознанного текста
                    string recognizedText = ocr.GetUTF8Text();
                    label1.Text = recognizedText;
                }
                File.Delete("5.png");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            scrin.scrin1();
        }
    }
}
