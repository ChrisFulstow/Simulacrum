using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fractals;

namespace MandelbrotUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var area = new Rect(
                new Point(-2.4, -1.5),
                new Point(0.8, 1.5));

            var sw = Stopwatch.StartNew();
            
            MandelbrotImage.Source = DrawMandelbrot(area);
            
            var elapsedMs = sw.ElapsedMilliseconds;
            //MessageBox.Show("Render duration: " + elapsedMs + " ms");
        }

        private ImageSource DrawMandelbrot(Rect area)
        {
            var pixelHeight = (int)MandelbrotImage.Height;
            var pixelWidth = (int)MandelbrotImage.Width;
            var bitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null);

            var bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            var pixels = new byte[pixelHeight * pixelWidth * bytesPerPixel];
            var stride = pixelWidth * bytesPerPixel;

            var xScale = (area.Right - area.Left) / pixelWidth;
            var yScale = (area.Top - area.Bottom) / pixelHeight;
            
            var mandelbrotSolver = new MandelbrotSolver(maxIterations: 50);

            var pixelIndices = Enumerable.Range(0, pixels.Length).Where(i => i % 4 == 0);

            Parallel.ForEach(pixelIndices, i =>
            {
                var yPixel = i / stride;
                var xPixel = i % stride / bytesPerPixel;

                var xCoord = area.Left + xPixel * xScale;
                var yCoord = area.Top - yPixel * yScale;

                var c = new Complex(xCoord, yCoord);

                var mandelbrotResult = mandelbrotSolver.IsMemberIterative(c);

                var colour = IterationColour(mandelbrotResult.Iterations);
                pixels[i] = colour.B;
                pixels[i + 1] = colour.G;
                pixels[i + 2] = colour.R;
                pixels[i + 3] = colour.A;
            });

            var sourceRect = new Int32Rect(0, 0, pixelWidth, pixelHeight);
            bitmap.WritePixels(sourceRect, pixels, stride, 0);
            return bitmap;
        }


        private static Color IterationColour(int iteration)
        {
            // scale the iteration into a color shade
            var colour = new Color
            {
                A = 255,
                B = (byte)(iteration / 100 * 25),
                G = (byte)((iteration % 100) % 10 * 25),
                R = (byte)((iteration % 100) % 10 * 25)
            };
            return colour;
        }
    }
}
