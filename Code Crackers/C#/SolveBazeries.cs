using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBazeries
{
    class Program
    {
        const int maxNumber = 1000000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Bazeries Solver-Bruter Hybrid --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--BazeriesMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int n = 0;

            string alphabet;
            if (args.Length > 0)
            {
                alphabet = args[0];
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
            }

            Console.Write("Using Alphabet:\n" + alphabet);
            Console.Write("\n\n-----------------------\n\n");

            /*Console.Write(CipherLib.Bazeries.Decrypt(ciphertext, 45632, alphabet));
            Console.Write("\n\n");
            //Console.Write(CipherLib.Annealing.NGramIOCAlphanumeric(CipherLib.Bazeries.ReverseBlocks(ciphertext, 45632), 2));
            Console.Write(CipherLib.Annealing.IOCAlphanumeric(CipherLib.Bazeries.ReverseBlocks(ciphertext, 45632)));
            Console.Write("\n\n");
            Console.Write(CipherLib.Annealing.BigramIOCAlphanumeric(CipherLib.Bazeries.ReverseBlocks(ciphertext, 45632)));
            Console.Write("\n\n");
            Console.ReadLine();*/

            string originalCiphertext = ciphertext;

            //ciphertext = CipherLib.Annealing.CrackCustomMonoSub(ciphertext, alphabet, 0);
            //ciphertext = CipherLib.Annealing.DecodeCustomMonoSub(ciphertext, CipherLib.Annealing.CustomOrderedFrequencyKey(ciphertext, alphabet), alphabet);
            //ciphertext = CipherLib.Annealing.DecodeCustomMonoSub(ciphertext, alphabet, CipherLib.Annealing.CustomOrderedFrequencyKey(ciphertext, alphabet));
            //Console.Write(ciphertext);
            //Console.ReadLine();
            ciphertext = CipherLib.Annealing.DecodeCustomMonoSub(ciphertext, CipherLib.Annealing.CustomOrderedFrequencyKey(ciphertext, alphabet), alphabet);

            string decipherment;

            //int displayPeriod = 100;
            int displayPeriod = 1000;

            Console.Write("Searching all " + maxNumber.ToString() + " keys...");
            Console.Write("\n\n");

            bool isNormalMonoSub;
            List<int> possibleKeys = new List<int>();

            float currentScore = float.MinValue;
            float bestScore = currentScore;

            bool justGotNewBestKey = false;

            for (n = 1; n <= maxNumber; n++)
            {
                //if ((n + 1) % displayPeriod == 0 || n == 0 || justGotNewBestKey)
                if ((n + 1) % displayPeriod == 0 || n == 1 || justGotNewBestKey)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSearched: " + (n + 1) + " keys");
                    justGotNewBestKey = false;
                }

                isNormalMonoSub = CipherLib.Annealing.IsNormalMonoSub(CipherLib.Bazeries.ReverseBlocks(ciphertext, n));

                if (isNormalMonoSub)
                {
                    /*if (n == 45632)
                    {
                        Console.Write("We got 'im");
                    }*/

                    possibleKeys.Add(n);

                    //decipherment = CipherLib.Bazeries.Decrypt(ciphertext, n, alphabet, 10000);
                    //decipherment = CipherLib.Bazeries.DecryptCrackMonoSub(ciphertext, n, alphabet, 1000);
                    decipherment = CipherLib.Bazeries.Decrypt(ciphertext, n, alphabet);

                    currentScore = CipherLib.Annealing.QuadgramScore(decipherment);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;

                        //decipherment = CipherLib.Bazeries.DecryptCrackMonoSub(ciphertext, n, alphabet);
                        decipherment = CipherLib.Bazeries.DecryptCrackMonoSub(ciphertext, n, alphabet, 10000);

                        Console.Write("\n\n--------------------------------------");
                        Console.Write("\n\n");
                        Console.Write("New best key:\n\n");
                        Console.Write("Score: " + bestScore + "\n");
                        Console.Write("\n");
                        Console.Write("Key: " + n.ToString());
                        Console.Write("\n\n");
                        Console.Write("PLA: " + alphabet);
                        Console.Write("\n");
                        //Console.Write("CIP: " + CipherLib.Annealing.GetKeyFromPlaintext(decipherment, CipherLib.Bazeries.ReverseBlocks(ciphertext, n), alphabet));
                        Console.Write("CIP: " + CipherLib.Annealing.GetKeyFromPlaintext(decipherment, CipherLib.Bazeries.ReverseBlocks(originalCiphertext, n), alphabet));
                        Console.Write("\n\n");
                        Console.Write(decipherment);
                        Console.Write("\n\n");
                        Console.Write("--------------------------------------\n\n");

                        justGotNewBestKey = true;
                    }
                }
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("All keys checked.");
            Console.Write("\n\n");
            Console.Write("I identified " + possibleKeys.Count() + " possible keys:");
            Console.Write("\n\n");
            Console.Write("Press ENTER to see them: ");
            Console.ReadLine();
            Console.Write("\n");

            for (int i = 0; i < possibleKeys.Count(); i++)
            {
                //Console.Write(n);
                Console.Write(possibleKeys[i]);
                Console.Write("\n");
            }

            Console.Write("\n--------------------------------------\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
