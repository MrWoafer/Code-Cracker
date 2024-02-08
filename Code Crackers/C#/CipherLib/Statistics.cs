using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Statistics
    {
        public static Random rand = new Random();

        public static double RandomFromNormal(double mean, double stdDev)
        {
            double x1 = 1d - rand.NextDouble();
            double x2 = 1d - rand.NextDouble();

            return mean + stdDev * Math.Sqrt(-2d * Math.Log(x1)) * Math.Sin(2d * Math.PI * x2);
        }

        public static float ChiSquared(float[] observed, float[] expected)
        {
            float chiSquared = 0f;
            for (int i = 0; i < observed.Length; i++)
            {
                chiSquared += (observed[i] - expected[i]) * (observed[i] - expected[i]) / expected[i];
            }
            return chiSquared;
        }
    }
}
