using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace thoto_word
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int showCmds);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int MOD_ALT = 0x1;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public const int WM_HOTKEY = 0x312;

        int xDown = 0;
        int yDown = 0;
        int xUp = 0;
        int yUp = 0;
        Rectangle rectCropArea = new Rectangle();
        MemoryStream ms = new MemoryStream();
        public Form1()
        {
            InitializeComponent();
            bool res = RegisterHotKey(Handle, 1, MOD_SHIFT, (uint)Keys.Z);
            if (res == false) MessageBox.Show("as");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                scrin.scrin1();
                m.Result = (IntPtr)0;
                return;
            }
            base.WndProc(ref m);
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ras();
        }

        private void ras()
        {
            // 1. Инициализация движка Tesseract
            // Необходимо указать путь к папке tessdata и язык
            using (Tesseract ocr = new Tesseract(@"D:\testData", "rus", OcrEngineMode.TesseractLstmCombined))
            {
                // 2. Загрузка изображения с помощью Emgu CV
                using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(@"85.png"))
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
