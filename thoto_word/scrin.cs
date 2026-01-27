using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace thoto_word
{

    public class scrin
    {
        static Bitmap ScreenShot = null;
        public static void scrin1() 
        {
            double r = 1;
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            //r = (double)ScreenShot.Width / (double)bounds.Width;
            ScreenShot = ScreenCapture();
            // Clone a portion of the Bitmap object.
            RectangleF cloneRect = new RectangleF((int)(Static.LocPress.X * r), (int)(Static.LocPress.Y * r), (int)(Static.LocUp.X * r - Static.LocPress.X * r), (int)(Static.LocUp.Y * r - Static.LocPress.Y * r));
            PixelFormat format = ScreenShot.PixelFormat;

            try
            {
                Bitmap cloneBitmap = ScreenShot.Clone(cloneRect, format);
                cloneBitmap.Save("85.png");
                Clipboard.SetImage(cloneBitmap);
            }
            catch
            {
                MessageBox.Show("a");
            }
        }
        public static Bitmap ScreenCapture()
        {
            // Initialize the virtual screen to dummy values
            int screenLeft = int.MaxValue;
            int screenTop = int.MaxValue;
            int screenRight = int.MinValue;
            int screenBottom = int.MinValue;

            Bitmap bmp = null;

            // Enumerate system display devices
            int deviceIndex = 0;
            while (true)
            {
                NativeUtilities.DisplayDevice deviceData = new NativeUtilities.DisplayDevice { cb = Marshal.SizeOf(typeof(NativeUtilities.DisplayDevice)) };
                if (NativeUtilities.EnumDisplayDevices(null, deviceIndex, ref deviceData, 0) != 0)
                {
                    // Get the position and size of this particular display device
                    NativeUtilities.DEVMODE devMode = new NativeUtilities.DEVMODE();
                    if (NativeUtilities.EnumDisplaySettings(deviceData.DeviceName, NativeUtilities.ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        // Update the virtual screen dimensions
                        screenLeft = Math.Min(screenLeft, devMode.dmPositionX);
                        screenTop = Math.Min(screenTop, devMode.dmPositionY);
                        screenRight = Math.Max(screenRight, devMode.dmPositionX + devMode.dmPelsWidth);
                        screenBottom = Math.Max(screenBottom, devMode.dmPositionY + devMode.dmPelsHeight);
                    }
                    deviceIndex++;
                }
                else
                    break;
            }

            // Create a bitmap of the appropriate size to receive the screen-shot.
            bmp = new Bitmap(screenRight - screenLeft, screenBottom - screenTop);

            Graphics g = Graphics.FromImage(bmp);

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bmp, new Rectangle(Point.Empty, bmp.Size));
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);

            return bmp;
        }
    }
}
