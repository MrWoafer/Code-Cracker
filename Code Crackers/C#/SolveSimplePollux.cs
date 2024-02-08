using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSimplePollux
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Simple Pollux Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--SimplePolluxMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            string keyAlphabet;
            if (args.Length > 0)
            {
                alphabet = args[0];
                keyAlphabet = args[1];
            }
            else
            {
                alphabet = "0123456789";
                //keyAlphabet = "---....///";
                keyAlphabet = "....---///";
            }

            Console.Write("Ciphertext Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Morse Key Alphabet: " + keyAlphabet);
            Console.Write("\n\n");
            Console.Write("-----------------------\n\n");

            //if (true)
            if (false)
            {
                Console.Write(CipherLib.Pollux.DecryptSimplePollux(ciphertext, "0123456789".ToCharArray(), "./-../.--/".ToCharArray()));
                Console.Write("\n\n");
            }

            /*int[] keyAlphabetAsNums = new int[keyAlphabet.Length];
            for (int i = 0; i < keyAlphabet.Length; i++)
            {
                if (keyAlphabet[i] == '.')
                {
                    keyAlphabetAsNums[i] = 0;
                }
                else if (keyAlphabet[i] == '-')
                {
                    keyAlphabetAsNums[i] = 1;
                }
                else if (keyAlphabet[i] == '/')
                {
                    keyAlphabetAsNums[i] = 2;
                }
                else
                {
                    Console.Write("ERROR: Unknown character in the key alphabet!");
                }
            }*/

            int[][] perms = CipherLib.Annealing.Permutations(keyAlphabet.Length);
            //int[][] perms = CipherLib.Annealing.Permute(keyAlphabetAsNums);
            //int displayPeriod = CipherLib.Annealing.Factorial(keyAlphabet.Length - 1);
            //int displayPeriod = CipherLib.Annealing.Factorial(5);
            //int displayPeriod = CipherLib.Annealing.Factorial(6);
            int displayPeriod = CipherLib.Annealing.Factorial(7);
            //int displayPeriod = perms.Length / 50;

            char[] currentKey = keyAlphabet.ToCharArray();
            char[] bestKey = keyAlphabet.ToCharArray();

            //float bestScore = float.NegativeInfinity;
            float bestScore = float.PositiveInfinity;
            float currentScore;

            string decryption;

            for (int i = 0; i < perms.Length; i++)
            {
                if (i % displayPeriod == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                    Console.Write("Searched " + i.ToString() + " / " + perms.Length.ToString() + " keys...");
                }
                
                for (int j = 0; j < keyAlphabet.Length; j++)
                {
                    currentKey[j] = keyAlphabet[perms[i][j]];
                    /*if (perms[i][j] == 0)
                    {
                        currentKey[j] = '.';
                    }
                    else if (perms[i][j] == 1)
                    {
                        currentKey[j] = '-';
                    }
                    else if (perms[i][j] == 2)
                    {
                        currentKey[j] = '/';
                    }*/
                }

                decryption = CipherLib.Pollux.DecryptSimplePollux(ciphertext, alphabet.ToCharArray(), currentKey);

                if (decryption != null)
                {
                    currentScore = CipherLib.Annealing.ChiSquared(decryption);
                    
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        CipherLib.Utils.CopyArray(currentKey, bestKey);

                        Console.Write("\n\n");
                        Console.Write("New Best Key:\n");
                        Console.Write(alphabet);
                        Console.Write("\n");
                        //Console.Write(bestKey.ToString());
                        Console.Write(new string(bestKey));
                        Console.Write("\n\n");
                        Console.Write(decryption);
                        Console.Write("\n\n-----------------------\n\n");
                    }
                }
            }

            Console.Write("Searched " + perms.Length.ToString() + " / " + perms.Length.ToString() + " keys...");

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.\n\n");
            Console.Write("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
