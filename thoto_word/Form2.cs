using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace thoto_word
{
    public partial class Form2 : Form
    {
        bool RectangleFlag = false;
        Form3 newForm = new Form3();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            Static.LocPress = e.Location;
            try
            {
                newForm.Show();
                newForm.Location = Static.LocPress;
                newForm.Size = new Size(0, 0);
                newForm.Opacity = 0.3;
                //if (ForwSitrting.BC != Color.Empty)
                //    newForm.BackColor = ForwSitrting.BC;
                RectangleFlag = true;
            }
            catch { }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            Static.LocNow = e.Location;

            if (RectangleFlag)
            {
                newForm.Size = new Size((int)(Static.LocNow.X - Static.LocPress.X), (int)(Static.LocNow.Y - Static.LocPress.Y));

                Graphics gra = newForm.CreateGraphics();

                gra.Clear(newForm.BackColor);

                // Create a new pen.
                Pen skyPen = new Pen(ChangeColorBrightness(newForm.BackColor, 0.5f));

                // Set the pen's width.
                skyPen.Width = 4.0F;

                // Set the LineJoin property.
                skyPen.LineJoin = LineJoin.Bevel;

                // Draw a rectangle.
                gra.DrawRectangle(skyPen, new Rectangle(0, 0, (int)(Static.LocNow.X - Static.LocPress.X), (int)(Static.LocNow.Y - Static.LocPress.Y)));

                //Dispose of the pen.
                skyPen.Dispose();
            }
        }

        private Color ChangeColorBrightness(Color color, float v)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;
            if (v < 0)
            {
                v = 1 + v;
                red *= v; green *= v; blue *= v;
            }
            else
            {
                red = (255 - red) * v + red;
                green = (255 - green) * v + green;
                blue = (255 - blue) * v + blue;
            }
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            Static.LocUp = e.Location;


            newForm.Opacity = 0.3;
            this.Opacity = 0.01;

            scrin.scrin1();


            RectangleFlag = false;

            Thread.Sleep(500);

            newForm.Hide();
            this.Opacity = 0;
            this.ActiveControl = null;
            Close();
        }
    }
    class NativeUtilities
    {
        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            // The device is part of the desktop.
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            // This is the primary display.
            PrimaryDevice = 0x4,
            // Represents a pseudo device used to mirror application drawing for remoting or other purposes.
            MirroringDriver = 0x8,
            // The device is VGA compatible.
            VGACompatible = 0x16,
            // The device is removable; it cannot be the primary display
            Removable = 0x20,
            // The device has more display modes than its output devices support.
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DisplayDevice
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        public const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        [DllImport("User32.dll")]
        public static extern int EnumDisplayDevices(string lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);
    }
}
