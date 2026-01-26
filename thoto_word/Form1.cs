using Emgu;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection.Metadata;

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
            Bitmap originalImage = new Bitmap("a.jpg");
            // Создайте новую bitmap по размеру PictureBox
            Bitmap highQualityImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics g = Graphics.FromImage(highQualityImage))
            {
                // Установите высокое качество
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            pictureBox1.Image = highQualityImage;
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
            Rectangle rec = new Rectangle(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
            using (Pen pen = new Pen(Color.YellowGreen, 3))
            {

                pictureBox1.CreateGraphics().DrawRectangle(pen, rec);
            }
            rectCropArea = rec;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            scrinshot();
        }

        private void obres()
        {
            try
            {
                pictureBox2.Refresh();
                //Prepare a new Bitmap on which the cropped image will be drawn
                Bitmap sourceBitmap = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                Graphics g = pictureBox2.CreateGraphics();

                //Draw the image on the Graphics object with the new dimesions
                g.DrawImage(sourceBitmap, new Rectangle(0, 0, rectCropArea.Width, rectCropArea.Height), rectCropArea, GraphicsUnit.Pixel);
                sourceBitmap.Dispose();
                var path = Environment.CurrentDirectory.ToString();
                ms = new MemoryStream();
                byte[] ar = new byte[ms.Length];
                var timeout = ms.WriteAsync(ar, 0, ar.Length);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обрезке: {ex.Message}");
            }
            finally
            {
                // sourceBitmap создается как новая копия из pictureBox1.Image, 
                // поэтому его можно безопасно удалить
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //pictureBox1.Refresh();
            //var size = GetTrueScreenSize(Screen.PrimaryScreen);
            //Bitmap bmp = new Bitmap(size.Width, size.Height);
            //using (var graph = Graphics.FromImage(bmp))
            //    graph.CopyFromScreen(0, 0, 0, 0, size);
            //pictureBox1.Image = bmp;
            obres();
            MessageBox.Show("a");
        }

        private void scrinshot()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap screenBmp = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(screenBmp))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                screenBmp.Save("6.jpg");
            }
            Bitmap originalImage = new Bitmap("6.jpg");
            // Создайте новую bitmap по размеру PictureBox
            Bitmap highQualityImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics g = Graphics.FromImage(highQualityImage))
            {
                // Установите высокое качество
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            pictureBox1.Image = highQualityImage;
            originalImage.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Инициализация движка Tesseract
            // Необходимо указать путь к папке tessdata и язык
            using (Tesseract ocr = new Tesseract(@"D:\testData", "rus", OcrEngineMode.TesseractLstmCombined))
            {
                // 2. Загрузка изображения с помощью Emgu CV
                using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(@"6.jpg"))
                {
                    // Можно выполнить предварительную обработку, например, перевод в серый цвет и бинаризацию
                    // Mat grayImage = new Mat();
                    // CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                    // CvInvoke.Threshold(grayImage, grayImage, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

                    // 3. Установка изображения для распознавания
                    // Если использовалась предобработка, передайте обработанное изображение
                    ocr.SetImage(image.Mat);

                    // 4. Выполнение распознавания
                    ocr.Recognize();

                    // 5. Получение распознанного текста
                    string recognizedText = ocr.GetUTF8Text();
                    label1.Text = recognizedText;
                }
                File.Delete("6.jpg");
            }
        }
    }
}
