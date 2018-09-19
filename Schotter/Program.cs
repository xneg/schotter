using System;
using System.Text;

namespace Schotter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var cols = 66;
            var squaresPerRow = 4;
            var squaresPerCol = 4;

            var canvasRenderer = new CanvasRenderer();

            var canvas = canvasRenderer.DrawSchotter(cols, squaresPerRow, squaresPerCol);

            for (var y = 0; y < canvas.Height; y++)
            {
                for (var x = 0; x < canvas.Width; x++)
                {
                    Console.Write(canvas.GetPixel(x, y));
                }
                Console.Write(Environment.NewLine);
            }

            var rendered = canvasRenderer.RenderCanvas(canvas);

            Console.WriteLine(rendered);

            Console.ReadLine();
        }
    }
}
