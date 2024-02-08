using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHomophonicBigram
{
    class Program
    {
        //private const int numberOfInitialKeyTrials = 40;
        private const int numberOfInitialKeyTrials = 1;
        //private const int numberOfInitialKeyTrials = 500;
        //private const int numberOfInitialKeyTrials = 100;

        //private const bool repeatIfBetter = false;
        private const bool repeatIfBetter = true;

        //private const bool matrixIsFreq = true;
        private const bool matrixIsFreq = false;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            //Console.Write("-- C# Homophonic Substitution Bigram Solver --");
            Console.Write("-- C# Homophonic Substitution Quadgram Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            //string[] msg = System.IO.File.ReadAllText("--HomophonicBigramMessage.txt").Split(null);
            string[] msg = System.IO.File.ReadAllText("--HomophonicQuadgramMessage.txt").Split(null);

            int[] ciphertext = new int[msg.Length];
            HashSet<string> symbols = new HashSet<string>();

            foreach (string i in msg)
            {
                symbols.Add(i);
            }

            string[] symbolArray = symbols.ToArray();

            for (int i = 0; i < msg.Length; i++)
            {
                for (int j = 0; j < symbolArray.Length; j++)
                {
                    if (symbolArray[j] == msg[i])
                    {
                        ciphertext[i] = j;
                        break;
                    }
                }
            }

            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");

            for (int i = 0; i < ciphertext.Length; i++)
            {
                Console.Write(msg[i]);
                if (i < ciphertext.Length - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            if (args.Length > 0)
            {
                alphabet = args[0];
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            float[] alphabetExpectedFrequencies = CipherLib.Annealing.averageletterfrequencies;
            for (int i = 0; i < alphabetExpectedFrequencies.Length; i++)
            {
                alphabetExpectedFrequencies[i] /= 100f;
            }

            char[] bestKey = new char[symbolArray.Length];
            char[] bestInitKey = new char[symbolArray.Length];

            Dictionary<int, int[]> startN = new Dictionary<int, int[]>();

            int[] n = new int[26];

            ///                      e  t  a  o  i  n  s  r  h  d  l  c  u  m  f  w  g  y  p  b  v  k  x  j  q  z ///
            startN[1] = new int[]  { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[2] = new int[]  { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[3] = new int[]  { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[4] = new int[]  { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[5] = new int[]  { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[6] = new int[]  { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[7] = new int[]  { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[8] = new int[]  { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[9] = new int[]  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[10] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[11] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[12] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[13] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[14] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[15] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[16] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[17] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[18] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            startN[19] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            startN[20] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            startN[21] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
            startN[22] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
            startN[23] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
            startN[24] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
            startN[25] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
            startN[26] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[27] = new int[] { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[28] = new int[] { 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[29] = new int[] { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[30] = new int[] { 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[31] = new int[] { 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[32] = new int[] { 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[33] = new int[] { 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[34] = new int[] { 4, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[35] = new int[] { 4, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[36] = new int[] { 4, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[37] = new int[] { 4, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[38] = new int[] { 4, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[39] = new int[] { 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[40] = new int[] { 4, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[41] = new int[] { 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[42] = new int[] { 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[43] = new int[] { 5, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[44] = new int[] { 5, 4, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[45] = new int[] { 5, 4, 3, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            ///                      e  t  a  o  i  n  s  r  h  d  l  c  u  m  f  w  g  y  p  b  v  k  x  j  q  z ///

            startN[46] = new int[] { 5, 5, 3, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[47] = new int[] { 5, 5, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[48] = new int[] { 6, 5, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[49] = new int[] { 6, 5, 4, 4, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[50] = new int[] { 6, 5, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[51] = new int[] { 7, 5, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[52] = new int[] { 7, 5, 4, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[53] = new int[] { 7, 5, 4, 4, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[54] = new int[] { 7, 5, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[55] = new int[] { 7, 5, 4, 4, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[56] = new int[] { 7, 5, 5, 4, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[57] = new int[] { 7, 5, 5, 4, 4, 4, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[58] = new int[] { 7, 5, 5, 5, 4, 4, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[59] = new int[] { 7, 6, 5, 5, 4, 4, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[60] = new int[] { 7, 6, 5, 5, 4, 4, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[61] = new int[] { 7, 6, 5, 5, 4, 4, 4, 4, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[62] = new int[] { 7, 6, 5, 5, 4, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[63] = new int[] { 7, 6, 5, 5, 5, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[64] = new int[] { 8, 6, 5, 5, 5, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[65] = new int[] { 8, 6, 5, 5, 5, 4, 4, 4, 4, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[66] = new int[] { 8, 6, 5, 5, 5, 4, 4, 4, 4, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[67] = new int[] { 8, 6, 6, 5, 5, 4, 4, 4, 4, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[68] = new int[] { 8, 7, 6, 5, 5, 4, 4, 4, 4, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[69] = new int[] { 8, 7, 6, 5, 5, 5, 4, 4, 4, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[70] = new int[] { 8, 7, 6, 5, 5, 5, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[71] = new int[] { 8, 7, 6, 6, 5, 5, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[72] = new int[] { 9, 7, 6, 6, 5, 5, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[73] = new int[] { 9, 7, 6, 6, 5, 5, 5, 4, 4, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[74] = new int[] { 9, 7, 6, 6, 5, 5, 5, 4, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[75] = new int[] { 9, 7, 6, 6, 5, 5, 5, 4, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[76] = new int[] { 10,7, 6, 6, 5, 5, 5, 4, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[77] = new int[] { 10,7, 7, 6, 5, 5, 5, 4, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[78] = new int[] { 10,8, 7, 6, 5, 5, 5, 4, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[79] = new int[] { 10,8, 7, 6, 6, 5, 5, 4, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[80] = new int[] { 10,8, 7, 6, 6, 5, 5, 5, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[81] = new int[] { 10,8, 7, 7, 6, 5, 5, 5, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[82] = new int[] { 11,8, 7, 7, 6, 5, 5, 5, 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[83] = new int[] { 11,8, 7, 7, 6, 5, 5, 5, 5, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[84] = new int[] { 11,8, 7, 7, 6, 6, 5, 5, 5, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[85] = new int[] { 11,8, 7, 7, 6, 6, 5, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[86] = new int[] { 11,9, 7, 7, 6, 6, 5, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[87] = new int[] { 11,9, 7, 7, 7, 6, 5, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[88] = new int[] { 11,9, 7, 7, 7, 6, 6, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[89] = new int[] { 12,9, 7, 7, 7, 6, 6, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[90] = new int[] { 12,9, 8, 7, 7, 6, 6, 5, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[91] = new int[] { 12,9, 8, 7, 7, 6, 6, 6, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[92] = new int[] { 12,9, 8, 7, 7, 7, 6, 6, 5, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[93] = new int[] { 12,9, 8, 7, 7, 7, 6, 6, 5, 4, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[94] = new int[] { 12,9, 8, 7, 7, 7, 6, 6, 5, 4, 4, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[95] = new int[] { 12,9, 8, 7, 7, 7, 6, 6, 5, 4, 4, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            startN[96] = new int[] { 12,9, 8, 8, 7, 7, 6, 6, 5, 4, 4, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[97] = new int[] { 12,9, 8, 8, 7, 7, 7, 6, 5, 4, 4, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[98] = new int[] { 12,9, 8, 8, 7, 7, 7, 6, 5, 4, 4, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[99] = new int[] { 12,9, 8, 8, 7, 7, 7, 6, 5, 4, 4, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            startN[100] = new int[]{ 12,10,8, 8, 7, 7, 7, 6, 5, 4, 4, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            ///                      e  t  a  o  i  n  s  r  h  d  l  c  u  m  f  w  g  y  p  b  v  k  x  j  q  z ///
            ///                      
            //startN[209] = new int[]{ 26,19,18,16,15,15,14,13,12,8, 8, 5, 5, 5, 4, 4, 4, 4, 4, 3, 2, 1, 1, 1, 1, 1 };

            for (int i = Enumerable.Min(startN.Keys); i <= Enumerable.Max(startN.Keys); i++)
            {
                if (!startN.ContainsKey(i))
                {
                    Console.Write("We have an error: no int[] n for " + i.ToString() + " unique symbols.");
                    Console.Write("\n\n");
                    Console.Write("Press ENTER to continue (the program will probably crash)...");
                    Console.ReadLine();
                }
            }
            foreach (int i in startN.Keys)
            {
                int total = 0;
                for (int j = 0; j < startN[i].Length; j++)
                {
                    total += startN[i][j];
                }
                if (total != i)
                {
                    Console.Write("We have an error: the int[] n for " + i.ToString() + " unique symbols does not total to " + i.ToString() + ".");
                    Console.Write("\n\n");
                    Console.Write("Press ENTER to continue (the program will probably crash)...");
                    Console.ReadLine();
                }
            }
            Array.Copy(startN[26], n, n.Length);
            if (startN.ContainsKey(symbols.Count))
            {
                Array.Copy(startN[symbols.Count], n, n.Length);
            }
            else
            {
                Console.Write("Program cannot yet support " + symbols.Count.ToString() + " unique symbols.");
                Console.Write("\n\n");
                Console.Write("Press ENTER to continue (the program will crash)...");
                Console.ReadLine();
            }

            Console.Write("Using alphabet: " + alphabet + "\n\n");
            Console.Write("Length of ciphertext (in terms of symbols): " + ciphertext.Length.ToString() + "\n\n");
            Console.Write("Number of symbols: " + symbols.Count.ToString() + "\n\n");
            Console.Write("-----------------------");
            Console.Write("\n\n");
            string time = DateTime.Now.ToString("HH:mm;ss");
            Console.Write("Program started at: " + time.Replace(":", "h ").Replace(";", "m ") + "s");
            Console.Write("\n\n");

            Console.Write("Starting...");

            Tuple<float, char[]> result = RandomInitialKey(n, ciphertext);

            float bestScore = result.Item1;
            Array.Copy(result.Item2, bestInitKey, result.Item2.Length);

            Array.Copy(bestInitKey, bestKey, bestInitKey.Length);

            int[] m = new int[n.Length];
            Array.Copy(n, m, n.Length);

            float newScore;
            bool betterScore = false;
            bool betterScoreThisPass = false;

            Console.Write("\n\n");
            Console.Write("Initial key:\n");
            Console.Write("------------\n\n");
            Console.Write("Score: " + bestScore);
            Console.Write("\n\n");
            Console.Write("Key in WSACC form:\n\n");
            DisplayKeyWSACCFormat(bestKey, symbolArray);
            Console.Write("\n\n");
            DisplayKey(bestKey, symbolArray);
            Console.Write("\n\n");
            Console.Write(CipherLib.Homophonic.Homophonic.Decrypt(ciphertext, bestKey));
            //Console.Write("\n\n");
//            DisplayKey(bestKey, symbolArray);
            Console.Write("\n\n--------------------------------------\n\n");

            //Console.Write("Do you want to continue the program [y/n]:");
            Console.Write("Do you want to continue the program [y/n]: ");
            string input = Console.ReadLine();
            input = input.ToLower();

            //if (true)
            //if (false)
            if (input == "y")
            {
                Console.Write("\n");
                for (int i = 1; i < 2; i++)
                {
                    betterScoreThisPass = false;

                    for (int j = 0; j < 26 - i; j++)
                    //for (int j = 0; j < 1; j++)
                    {
                        //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bi: " + (25 - i).ToString() + "   " + "(" + (100f * (j + 26f * (i - 1) - i * (i - 1f) / 2f) / (26f * (26f - 1f) / 2f)).ToString("n2") + "%)");
                        //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: " + ((float)j / (26f - i)).ToString("n2") + "%)");
                        //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: " + ((float)j / (26f - i)).ToString("n2") + "%");
                        //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: " + ((float)j * 100f / (26f - i)).ToString("n2") + "%");
                        CipherLib.Utils.ClearLine();
                        Console.Write("Progress: " + ((float)j * 100f / (26f - i)).ToString("n2") + "%");
                        switch (j % 4)
                        {
                            case 1:
                                //Console.Write(".  ");
                                Console.Write(".");
                                break;
                            case 2:
                                //Console.Write(".. ");
                                Console.Write("..");
                                break;
                            case 3:
                                Console.Write("...");
                                break;
                            default:
                                //Console.Write("   ");
                                break;
                        }

                        betterScore = false;

                        if (m[j] > 0)
                        {
                            Array.Copy(n, m, n.Length);

                            OuterSwap(ref m[j + i], ref m[j]);

                            result = RandomInitialKey(m, ciphertext);

                            newScore = result.Item1;

                            Array.Copy(result.Item2, bestInitKey, result.Item2.Length);

                            if (newScore < bestScore)
                            {
                                Array.Copy(m, n, m.Length);
                                bestScore = newScore;
                                Array.Copy(bestInitKey, bestKey, bestInitKey.Length);

                                betterScore = true;
                                betterScoreThisPass = true;
                            }
                        }

                        if (!betterScore)
                        {
                            if (m[j + i] > 0)
                            {
                                Array.Copy(n, m, n.Length);

                                OuterSwap(ref m[j], ref m[j + i]);

                                result = RandomInitialKey(m, ciphertext);

                                newScore = result.Item1;

                                Array.Copy(result.Item2, bestInitKey, result.Item2.Length);
                                if (newScore < bestScore)
                                {
                                    Array.Copy(m, n, m.Length);
                                    bestScore = newScore;
                                    Array.Copy(bestInitKey, bestKey, bestInitKey.Length);

                                    betterScore = true;
                                    betterScoreThisPass = true;
                                }
                            }
                        }

                        if (betterScore)
                        {
                            CipherLib.Utils.ClearLine();

                            Console.Write("\n\n");
                            Console.Write("New best key in WSACC form:\n\n");
                            DisplayKeyWSACCFormat(bestKey, symbolArray);
                            Console.Write("\n\n");

                            Console.Write("New best key:\n\n");
                            DisplayKey(bestKey, symbolArray);
                            Console.Write("\n\n");

                            Console.Write("New best score: " + bestScore);
                            Console.Write("\n\n");
                            Console.Write(CipherLib.Homophonic.Homophonic.Decrypt(ciphertext, bestKey));                            

                            Console.Write("\n\n--------------------------------------\n\n");
                        }
                    }

                    /*if (betterScoreThisPass)
                    {
                        // Do the loop again;
                        i = 0;
                    }
                    else
                    {
                        // i will increment and cause the loop to stop;
                    }*/
                }
            }
            else
            {
                Console.Write("\n");
            }

            //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: 100%");
            //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: 100.00%");
            /*Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bProgress: 100.00%  ");

            Console.Write("\n\n");
            Console.Write("Final score: " + bestScore);
            Console.Write("\n\n");
            Console.Write(CipherLib.Homophonic.Homophonic.Decrypt(ciphertext, bestKey));

            Console.Write("\n\n");
            Console.Write("Best key:\n\n");
            DisplayKey(bestKey, symbolArray);

            Console.Write("\n\n");
            Console.Write("Best key in WSACC form:\n\n");
            DisplayKeyWSACCFormat(bestKey, symbolArray);

            Console.Write("\n\n--------------------------------------\n\n");
            //DisplayKeyWSACCFormat(bestKey, symbolArray);*/

            CipherLib.Utils.ClearLine();
            Console.Write("Progress: 100.00%  ");
            Console.Write("\n\n--------------------------------------\n\n");

            time = DateTime.Now.ToString("HH:mm;ss");
            Console.Write("Program finished at: " + time.Replace(":", "h ").Replace(";", "m ") + "s");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        private static void OuterSwap(ref int mi, ref int mj)
        {
            if (mj > 1)
            {
                mi += 1;
                mj -= 1;
            }
        }

        private static Tuple<float, char[]> RandomInitialKey(int[] n, int[] ciphertext)
        {
            float bestInitScore = Int32.MaxValue;
            char[] bestInitKey = new char[Enumerable.Sum(n)];
            char[] key = new char[Enumerable.Sum(n)];
            HashSet<int> charsRemaining;

            int[] m = new int[n.Length];

            int index;
            int[][] Dp;
            float initScore;

            for (int r  = 0; r < numberOfInitialKeyTrials; r++)
            {
                /// Randomly initialise k_1, k_2, ..., k_n, satisfying n_a, n_b, ..., n_z
                charsRemaining = new HashSet<int>(CipherLib.Utils.IntRangeArray(0, 25));
                Array.Copy(n, m, n.Length);
                for (int i = 0; i < 26; i++)
                {
                    if (m[i] <= 0)
                    {
                        charsRemaining.Remove(i);
                    }
                }
                for (int i = 0; i < key.Length; i++)
                {
                    index = CipherLib.Utils.PickRandom(charsRemaining);

                    key[i] = CipherLib.Annealing.orderedlettersonavgfreq[index];

                    m[index] -= 1;

                    if (m[index] <= 0)
                    {
                        charsRemaining.Remove(index);
                    }
                }

                Tuple<float, char[]> result = InnerHillClimb(key, ciphertext);

                initScore = result.Item1;

                Array.Copy(result.Item2, key, result.Item2.Length);

                if (r == 0 || initScore < bestInitScore)
                {
                    bestInitScore = initScore;
                    Array.Copy(key, bestInitKey, key.Length);
                }
            }

            return new Tuple<float, char[]>(bestInitScore, bestInitKey);
        }

        private static Tuple<float, char[]> InnerHillClimb(char[] key, int[] ciphertext)
        //private static Tuple<float, char[]> InnerHillClimb2(char[] key, int[] ciphertext)
        {
            float bestInnerScore = score(ciphertext, key);
            float newInnerScore;

            char temp;

            for (int i = 1; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length - i; j++)
                {
                    temp = key[j];
                    key[j] = key[j + i];
                    key[j + i] = temp;

                    newInnerScore = score(ciphertext, key);

                    if (newInnerScore < bestInnerScore)
                    {
                        bestInnerScore = newInnerScore;

                        if (repeatIfBetter)
                        {
                            i = 1;
                            j = 0;
                        }
                    }
                    else
                    {
                        temp = key[j];
                        key[j] = key[j + i];
                        key[j + i] = temp;
                    }
                }
            }

            return new Tuple<float, char[]>(bestInnerScore, key);
        }

        private static Tuple<float, char[]> InnerHillClimb2(char[] key, int[] ciphertext)
        //private static Tuple<float, char[]> InnerHillClimb(char[] key, int[] ciphertext)
        {
            float bestInnerScore = score(ciphertext, key);
            float newInnerScore;

            char temp;
            int index1, index2;

            for (int i = 0; i < 1000; i++)
            {
                index1 = CipherLib.Utils.rand.Next() % key.Length;
                index2 = CipherLib.Utils.rand.Next() % key.Length;
                while (index2 == index1)
                {
                    index2 = CipherLib.Utils.rand.Next() % key.Length;
                }

                temp = key[index1];
                key[index1] = key[index2];
                key[index2] = temp;

                newInnerScore = score(ciphertext, key);

                if (newInnerScore < bestInnerScore)
                {
                    bestInnerScore = newInnerScore;

                    if (repeatIfBetter)
                    {
                        i = 0;
                    }
                }
                else
                {
                    temp = key[index1];
                    key[index1] = key[index2];
                    key[index2] = temp;
                }
            }

            return new Tuple<float, char[]>(bestInnerScore, key);
        }

        private static float score(int[] ciphertext, char[] key)
        {
            string plaintext = CipherLib.Homophonic.Homophonic.Decrypt(ciphertext, key);

            float score = CipherLib.Annealing.QuadgramScore(plaintext);
            //float score = 0f;

            return -score;
        }

        private static void DisplayMatrix(int[][] matrix, string[] alphabet, int spaceNum = 3)
        {
            Console.Write("  ");
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int k = 0; k < spaceNum - alphabet[i].ToString().Length; k++)
                {
                    Console.Write(" ");
                }
                Console.Write(alphabet[i]);
            }
            Console.Write("\n--");
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < spaceNum; j++)
                {
                    Console.Write("-");
                }
            }
            Console.Write("\n");
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write(alphabet[i]);
                for (int j = 0; j < 2 - alphabet[i].ToString().Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("| ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j]);
                    if (j < matrix[i].Length - 1)
                    {
                        for (int k = 0; k < spaceNum - matrix[i][j].ToString().Length; k++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                if (i < matrix.Length-1)
                {
                    Console.Write("\n");
                }
            }
        }

        private static void DisplayKey(char[] key, string[] symbolArray)
        {
            for (int k = 0; k < key.Length; k++)
            {
                Console.Write(" " + symbolArray[k].ToString());
            }
            Console.Write("\n");
            for (int l = 0; l < key.Length; l++)
            {
                for (int k = 0; k < symbolArray[l].ToString().Length; k++)
                {
                    Console.Write(" ");
                }
                Console.Write(key[l].ToString());
            }
        }

        private static char[] DeduceKey(string plaintext, int[] ciphertext, int symbolNum)
        {
            char[] key = new char[symbolNum];
            for (int i = 0; i < plaintext.Length; i++)
            {
                key[ciphertext[i]] = plaintext[i];
            }
            return key;
        }

        private static void DisplayKeyWSACCFormat(char[] key, string[] symbolArray)
        {
            for (int k = 0; k < key.Length; k++)
            {
                Console.Write(symbolArray[k].ToString() + " " + key[k]);
                if (k < key.Length - 1)
                {
                    Console.Write("\n");
                }
            }
        }
    }
}