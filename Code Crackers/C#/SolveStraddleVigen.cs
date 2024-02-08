using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStraddleVigenere
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

            Console.Write("-- C# Straddle Vigenère Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--StraddleVigenereMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int period;
            int rows;
            int columns;
            string alphabet;
            bool isAllNumbers;
            if (args.Length > 0)
            {
                period = Int32.Parse(args[0]);
                rows = Int32.Parse(args[1]);
                columns = Int32.Parse(args[2]);
                alphabet = args[3];
            }
            else
            {
                //period = 8;
                //period = 4;
                //period = 5;
                period = 4;
                rows = 3;
                //rows = 2;
                columns = 10;
                //alphabet = "abcdefghijklmnopqrstuvwxyz#/";
                alphabet = "abcdefghijklmnopqrstuvwxyz./";
            }
            isAllNumbers = System.Text.RegularExpressions.Regex.IsMatch(ciphertext, @"^\d+$");

            Console.Write("Ciphertext Was Converted Back Into Characters: ");
            if (isAllNumbers)
            {
                Console.Write("False");
            }
            else
            {
                Console.Write("True");
            }
            Console.Write("\n\n");
            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Checkerboard Height: " + rows);
            Console.Write("\n\n");
            Console.Write("Checkerboard Width: " + columns);
            Console.Write("\n\n");
            Console.Write("Period: " + period);
            Console.Write("\n\n");

            /*char[][] testKey = new char[][] {
                new char[] { '.', 'n', 'l', '.', 'p', 't', 'v', 'j', 's', 'c' },
                new char[] { 'k', 'w', 'f', 'd', 'e', 'h', 'i', 'a', 'g', 'o' },
                new char[] { 'x', 'y', 'b', 'z', 'm', 'q', 'r', 'u', '.', '.' }};*/
            //Console.Write("\n\n");
            //Console.Write(CipherLib.Straddle.Decrypt(ciphertext, testKey));
            //Console.Write("\n\n");
            //Console.Write(CipherLib.StraddleVigenere.Decrypt(ciphertext, testKey, "05452"));
            //Console.Write("\n\n");
            //Console.Write(CipherLib.Straddle.Crack(ciphertext));
            //Console.Write(CipherLib.Straddle.Crack(ciphertext, numOfTrials: 10000));
            //Console.Write(CipherLib.Straddle.Crack(ciphertext, numOfTrials: 1000));
            //Console.Write(CipherLib.Straddle.Crack(ciphertext));
            //Console.Write(CipherLib.Straddle.Crack2(ciphertext));
            //Console.Write(CipherLib.Straddle.Crack2(ciphertext, numOfTrials: 100));
            //Console.Write(CipherLib.Straddle.Crack2(ciphertext, numOfTrials: 10000));
            //Console.Write(CipherLib.Straddle.Crack2(ciphertext, numOfTrials: 1000));
            /*DateTime startTime = DateTime.Now;
            for (int i = 0; i < 676; i++)
            {
                Console.Write(i + "\n");
                CipherLib.Straddle.Crack(ciphertext, numOfTrials: 10000);
            }
            Console.Write(DateTime.Now - startTime);*/
            //Console.Write(CipherLib.Straddle.Crack3(ciphertext, numOfTrials: 1000));
            //Console.Write(CipherLib.Straddle.Crack())
            //Console.Write("\n\n\n\n\n\n\n");

            /*char[] currentKey = Enumerable.Repeat('0', period).ToArray();
            char[] bestKey = new char[period];
            Array.Copy(currentKey, bestKey, period);

            float bestScore = float.NegativeInfinity;
            float currentScore;

            string currentPlaintext;
            string bestPlaintext = "";

            int[][] perms = CipherLib.Annealing.NToTheMArrangements(10, 2);

            for (int i = 0; i < period; i += 2)
            {
                for (int j = 0; j < perms.Length; j++)
                {
                    Console.Write(i * perms.Length / 2 + j);
                    Console.Write("\n");
                    currentKey[i] = (char)(perms[j][0] + '0');
                    currentKey[i + 1] = (char)(perms[j][1] + '0');

                    currentPlaintext = CipherLib.Straddle.Crack(CipherLib.StraddleVigenere.UndoPeriodicKey(ciphertext, new string(currentKey)));
                    currentScore = CipherLib.Annealing.QuadgramScore(currentPlaintext);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, period);
                        bestPlaintext = currentPlaintext;
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, period);
                    }
                }
            }

            Console.Write("\n");
            Console.Write(bestPlaintext);
            Console.Write("\n\n");
            CipherLib.Utils.DisplayArray(bestKey);*/

            /*char[][] testKey = new char[][] {
                new char[] { ' ', 'n', 'l', ' ', 'p', 't', 'v', 'j', 's', 'c' },
                new char[] { 'k', 'w', 'f', 'd', 'e', 'h', 'i', 'a', 'g', 'o' },
                new char[] { 'x', 'y', 'b', 'z', 'm', 'q', 'r', 'u', '.', '/' }};

            Console.Write(CipherLib.StraddleVigenere.DecryptConvertedBack(ciphertext, testKey, "0545"));*/

            /*char[][] testKey = new char[][] {
                new char[] { ' ', 'n', 'd', ' ', 'p', 't', 'v', 'j', 's', 'c' },
                new char[] { 'k', 'w', 'f', 'l', 'e', 'h', 'i', 'a', 'g', 'o' },
                new char[] { 'x', 'y', 'b', 'z', 'm', 'q', 'r', 'u', '.', '/' }};*/

            //Console.Write(CipherLib.StraddleVigenere.DecryptConvertedBack(ciphertext, testKey, "0545"));
            //Console.Write(CipherLib.StraddleVigenere.Decrypt(ciphertext, testKey, "0545"));

            /// Check that the provided alphabet fits the size of the grid
            if (rows * columns != alphabet.Length + rows - 1)
            {
                Console.Write("-----------------------\n\n");
                Console.Write("ERROR: Size of provided alphabet does not match the provided grid size.");
            }
            else
            {
                char[][] sampleBoard = new char[rows][];
                //string sampleAlphabet = new string('.', rows - 1) + alphabet;
                string sampleAlphabet = new string(' ', rows - 1) + alphabet;

                int index = 0;
                for (int i = 0; i < sampleBoard.Length; i++)
                {
                    sampleBoard[i] = new char[columns];
                    for (int j = 0; j < sampleBoard[i].Length; j++)
                    {
                        sampleBoard[i][j] = sampleAlphabet[index];
                        index++;
                    }
                }
                Console.Write("Sample Checkerboard:\n");
                Console.Write("\n");
                CipherLib.Straddle.DisplayKey(sampleBoard);
                Console.Write("\n\n");

                if (!isAllNumbers)
                {
                    Console.Write("-----------------------\n\n");
                    Console.Write("WARNING: The solver doesn't really work if the ciphertext was converted back to characters.");
                    Console.Write("\n\n");
                }

                float currentScore = float.NegativeInfinity;
                float bestScore = currentScore;

                char[][] bestCheckerboard = new char[rows][];
                for (int i = 0; i < bestCheckerboard.Length; i++)
                {
                    bestCheckerboard[i] = new char[columns];
                }
                string bestPeriodicKey = "";

                /// In the format (plaintext, checkerboard, periodic key);
                Tuple<string, char[][], string> result = new Tuple<string, char[][], string>("", new char[rows][], "");
                string plaintext;

                int trial = 0;
                while (true)
                {

                    Console.Write("-----------------------\n\n");
                    Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                    //result = CipherLib.StraddleVigenere.CrackReturnKey(ciphertext, period, rows, columns, alphabet, true);
                    //result = CipherLib.StraddleVigenere.CrackReturnKey(ciphertext, period, rows, columns, alphabet, 10000, true);
                    result = CipherLib.StraddleVigenere.CrackReturnKey(ciphertext, period, rows, columns, alphabet, isAllNumbers, 10000, true);
                    //result = CipherLib.StraddleVigenere.CrackReturnKey(ciphertext, period, rows, columns, alphabet, 100000, true);

                    plaintext = result.Item1;

                    currentScore = CipherLib.Annealing.QuadgramScore(plaintext);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;

                        bestPeriodicKey = result.Item3;
                        CipherLib.Utils.Copy2DArray(result.Item2, bestCheckerboard);

                        Console.Write("New best checkerboard:\n\n");
                        CipherLib.Straddle.DisplayKey(bestCheckerboard);
                        Console.Write("\n\n");
                        Console.Write("New best periodic key:\n\n");
                        Console.Write(bestPeriodicKey);
                        Console.Write("\n\n");
                        Console.Write("Score: " + bestScore + "\n\n");
                        Console.Write("Decipherment: " + plaintext);
                        Console.Write("\n\n");
                    }
                    else
                    {
                        Console.Write("Didn't find a better key...");
                        Console.Write("\n\n");
                    }

                    trial++;
                }
            }

            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }
    }
}
