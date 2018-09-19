using static System.Math;

namespace Schotter
{
    public class Canvas
    {
        public int Width { get; }

        public int Height { get; }

        public int[] Pixels { get; }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;

            Pixels = new int[width * height];
        }

        public void DrawPixel(int x, int y, int color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            Pixels[x + y * Width] = color;
        }

        public int GetPixel(int x, int y)
        {
            if (x < 0 || x > Width || y < 0 || y > Height)
                return 0;

            return Pixels[x + (y * Width)];
        }

        /* Draw a line from x1,y1 to x2,y2 using the Bresenham algorithm. */
        public void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            var dx = Abs(x2 - x1);
            var dy = Abs(y2 - y1);
            var sx = (x1 < x2) ? 1 : -1;
            var sy = (y1 < y2) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                DrawPixel(x1, y1, color);
                if (x1 == x2 && y1 == y2)
                    break;

                var e2 = err * 2;

                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public void DrawSquare(int x, int y, double size, double angle)
        {
            var px = new int[4];
            var py = new int[4];

            size /= 1.4142135623;
            size = Round(size);

            var k = PI / 4 + angle;

            for (var i = 0; i < 4; i++)
            {
                px[i] = (int)Round(Sin(k) * size + x);
                py[i] = (int)Round(Cos(k) * size + y);

                k += PI / 2;
            }

            for (var i = 0; i < 4; i++)
                DrawLine(px[i], py[i], px[(i + 1) % 4], py[(i + 1) % 4], 1);
        }
    }
}
