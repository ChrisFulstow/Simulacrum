using System;
using System.Numerics;
using Fractals;

namespace MandelbrotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            double x1 = -2.25, x2 = 0.8, xd = 0.04D;
            double y1 = 1.25, y2 = -1.25, yd = 0.05D;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            DrawAsciiMandelbrotSet(x1, y1, x2, y2, xd, yd);
            Console.Read();
        }


        static void DrawAsciiMandelbrotSet(double x1, double y1, double x2, double y2, double xd, double yd)
        {
            var mandelbrotSolver = new MandelbrotSolver();
            Func<Complex, bool> solver = x => mandelbrotSolver.IsMemberIterative(x).IsMember;

            for (var y = y1; y >= y2; y -= yd)
            {
                for (var x = x1; x <= x2; x += xd)
                {
                    var c = new Complex(x, y);
                    var outputChar = solver(c) ? "#" : ".";
                    Console.Write(outputChar);
                }
                Console.WriteLine();
            }
        }
    }
}
