using System.Numerics;

namespace Fractals
{
    public class MandelbrotSolver
    {
        public readonly int MaxIterations = 1000;

        public MandelbrotSolver(int maxIterations = 1000)
        {
            MaxIterations = maxIterations;
        }

        /// <summary>
        /// Recursive solution to determine whether complex number belongs to the Mandelbrot set
        /// </summary>
        public MandelbrotResult IsMemberRecursive(Complex c, Complex? z = null, int iteration = 0)
        {
            if (iteration == MaxIterations) return new MandelbrotResult(iteration, true);
            if (z.HasValue && z.Value.Magnitude > 2.0) return new MandelbrotResult(iteration, false);

            var newZ = z.HasValue ? Complex.Pow(z.Value, 2) + c : Complex.Zero;
            return IsMemberRecursive(c, newZ, iteration + 1);
        }


        public MandelbrotResult IsMemberIterative(Complex c)
        {
            var iteration = 0;
            var z = Complex.Zero;
            while (iteration < MaxIterations && z.Magnitude < 2)
            {
                z = z * z + c;
                iteration++;
            }
            var isMember = (iteration == MaxIterations);
            return new MandelbrotResult(iteration, isMember);
        }


        public struct MandelbrotResult
        {
            public int Iterations;
            public bool IsMember;

            public MandelbrotResult(int iterations, bool isMember)
            {
                Iterations = iterations;
                IsMember = isMember;
            }
        }
    }
}
