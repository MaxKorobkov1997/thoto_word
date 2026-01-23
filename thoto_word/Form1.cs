using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IO;
using System.Reflection.Metadata;
using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;

namespace thoto_word
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Инициализация движка Tesseract
            // Необходимо указать путь к папке tessdata и язык
            using (Tesseract ocr = new Tesseract(@"D:\testData", "rus", OcrEngineMode.TesseractLstmCombined))
            {
                // 2. Загрузка изображения с помощью Emgu CV
                using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(@"C:\Users\Максим\Downloads\1.jpg"))
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
            }
        }
    }
}
