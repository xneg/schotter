using System;
using System.Text;
using static System.Math;

namespace Schotter
{
    public class CanvasRenderer
    {
        public Canvas DrawSchotter(int consoleCols, int squaresPerRow, int squaresPerCol)
        {
            var canvasWidth = consoleCols * 2;
            var padding = canvasWidth > 4 ? 2 : 0;
            var squareSide = (canvasWidth - padding * 2f) / squaresPerRow;

            var canvasHeight = (int)squareSide * squaresPerRow + padding * 2;

            var canvas = new Canvas(canvasWidth, canvasHeight);

            var rnd = new Random();

            for (var y = 0; y < squaresPerCol; y++)
                for (var x = 0; x < squaresPerRow; x++)
                {
                    var sx = (int)(x * squareSide + squareSide / 2 + padding);
                    var sy = (int)(y * squareSide + squareSide / 2 + padding);

                    /* Rotate and translate randomly as we go down to lower rows. */

                    double angle = 0;

                    if (y > 1)
                    {
                        var r1 = rnd.NextDouble() / squaresPerCol * y;
                        var r2 = rnd.NextDouble() / squaresPerCol * y;
                        var r3 = rnd.NextDouble() / squaresPerCol * y;

                        if (rnd.Next() > 0.5f)
                            r1 = -r1;
                        if (rnd.Next() > 0.5f)
                            r2 = -r2;
                        if (rnd.Next() > 0.5f)
                            r3 = -r3;

                        angle = r1;
                        sx += (int)Round(r2 * squareSide / 3);
                        sy += (int)Round(r3 * squareSide / 3);
                    }
                    canvas.DrawSquare(sx, sy, squareSide, angle);
                }

            return canvas;
        }

        public string RenderCanvas(Canvas canvas)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < canvas.Height; y += 4)
            {
                for (var x = 0; x < canvas.Width; x += 2)
                {
                    /* We need to emit groups of 8 bits according to a specific
                    * arrangement. See lwTranslatePixelsGroup() for more info. */
                    byte b = 0;
                    if (canvas.GetPixel(x, y) == 1)
                        b |= (1 << 0);
                    if (canvas.GetPixel(x, y + 1) == 1)
                        b |= (1 << 1);
                    if (canvas.GetPixel(x, y + 2) == 1)
                        b |= (1 << 2);
                    if (canvas.GetPixel(x + 1, y) == 1)
                        b |= (1 << 3);
                    if (canvas.GetPixel(x + 1, y + 1) == 1)
                        b |= (1 << 4);
                    if (canvas.GetPixel(x + 1, y + 2) == 1)
                        b |= (1 << 5);
                    if (canvas.GetPixel(x, y + 3) == 1)
                        b |= (1 << 6);
                    if (canvas.GetPixel(x + 1, y + 3) == 1)
                        b |= (1 << 7);

                    sb.Append(TranslatePixelsGroup(b));
                }

                if (y != canvas.Height - 1) sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private static string TranslatePixelsGroup(byte b)
        {
            var code = 0x2800 + b;

            return Encoding.UTF8.GetString(new[]
            {
                (byte)(0xE0 | (code >> 12)),
                (byte)(0x80 | ((code >> 6) & 0x3F)),
                (byte)(0x80 | (code & 0x3F))
            });
        }
    }
}
