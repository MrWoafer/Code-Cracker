using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTranspoSub
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

            Console.Write("-- C# Transpo-Sub (Monoalphabetic Substitution + Transposition) Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--TranspoSubMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            CipherLib.TranspositionType transpoType;
            int columnNum;
            if (args.Length > 0)
            {
                alphabet = args[0];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
                columnNum = Int32.Parse(args[2]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                //transpoType = CipherLib.TranspositionType.column;
                transpoType = CipherLib.TranspositionType.myszkowski;
                columnNum = 7;
            }

            //Console.Write("Using Alphabet:\n" + alphabet);
            Console.Write("Using Alphabet: " + alphabet);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            //else
            else if (transpoType == CipherLib.TranspositionType.row)
            {
                Console.Write("Row");
            }
            else if (transpoType == CipherLib.TranspositionType.myszkowski)
            {
                Console.Write("Myszkowski");
            }
            else if (transpoType == CipherLib.TranspositionType.amsco)
            {
                Console.Write("AMSCO");
            }
            else
            {
                Console.Write("Unknown (I'm probably about to crash)");
            }
            Console.Write("\n\n-----------------------\n\n");

            float bestScore = Int32.MinValue;
            string bestDecryption = "";
            //string bestDecryptionAfterTranspo = "";
            string bestMonoSubKey = "";
            int[] bestTranspoKey = new int[columnNum];

            float newScore;
            int[] newTranspoKey = new int[columnNum];

            int countdown = 10;

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[columnNum]);

            string decryption;

            for (int i = 0; i < countdown; i++)
            {
                //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + (countdown - i).ToString() + Enumerable.Repeat(" ", countdown.ToString().Length - (countdown - i).ToString().Length));
                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + (countdown - i).ToString() + new String(' ', countdown.ToString().Length - (countdown - i).ToString().Length));

                //if (false)
                if (true)
                {
                    //string initialKey = CipherLib.Annealing.OrderedFrequencyKey(ciphertext);
                    string initialKey = CipherLib.Annealing.CustomOrderedFrequencyKey(ciphertext, alphabet);

                    //string decryption = CipherLib.Annealing.DecodeMonoSub(ciphertext, initialKey);
                    //decryption = CipherLib.Annealing.DecodeMonoSub(ciphertext, initialKey);
                    decryption = CipherLib.Annealing.DecodeCustomMonoSub(ciphertext, initialKey, alphabet);
                    //string decryption = CipherLib.Annealing.CrackMonoSub(ciphertext, 1000, CipherLib.SolutionType.ChiSquared);
                    //string decryption = CipherLib.Annealing.CrackMonoSub(ciphertext, 10000, CipherLib.SolutionType.ChiSquared);

                    if (transpoType == CipherLib.TranspositionType.row)
                    {
                        //decryption = CipherLib.Annealing.CrackRowTranspo(decryption, 7, 1000);
                        //decryption = CipherLib.Annealing.CrackRowTranspo(decryption, columnNum, 1000);
                        result = CipherLib.Annealing.CrackRowTranspoReturnKey(decryption, columnNum, 1000);
                    }
                    else if (transpoType == CipherLib.TranspositionType.column)
                    {
                        //decryption = CipherLib.Annealing.CrackColumnTranspo(decryption, 7, 1000);
                        //decryption = CipherLib.Annealing.CrackColumnTranspo(decryption, columnNum, 1000);
                        result = CipherLib.Annealing.CrackColumnTranspoReturnKey(decryption, columnNum, 1000);
                    }
                    else if (transpoType == CipherLib.TranspositionType.myszkowski)
                    {
                        result = CipherLib.Annealing.CrackMyszkowskiTranspoReturnKey(decryption, columnNum, 10000);
                    }
                    else if (transpoType == CipherLib.TranspositionType.amsco)
                    {
                        result = CipherLib.Annealing.CrackAMSCOReturnKey(decryption, columnNum, new int[] { 0, 1 }, 10000);
                    }
                    //string decryptionAfterTranspo = decryption;
                    decryption = result.Item1;
                    Array.Copy(result.Item2, newTranspoKey, columnNum);

                    //string decryptionAfterTranspo = decryption;

                    //Tuple<string, string> result2;

                    decryption = CipherLib.Annealing.CrackMonoSub(decryption, 1000);
                    //result2 = CipherLib.Annealing.CrackCustomMonoSubReturnKey(decryption, alphabet, 1000);

                    //decryption = result2.Item1;

                    newScore = CipherLib.Annealing.QuadgramScore(decryption);

                    if (i == 0 || newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestDecryption = decryption;
                        //bestDecryptionAfterTranspo = decryptionAfterTranspo;
                        Array.Copy(newTranspoKey, bestTranspoKey, columnNum);
                        if (transpoType == CipherLib.TranspositionType.column)
                        {
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeColumnTranspo(ciphertext, bestTranspoKey), ciphertext);
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(decryptionAfterTranspo, CipherLib.Annealing.DecodeColumnTranspo(ciphertext, bestTranspoKey));
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(decryption, CipherLib.Annealing.DecodeColumnTranspo(ciphertext, bestTranspoKey));
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeColumnTranspo(ciphertext, bestTranspoKey), decryption);
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.IncompleteColumnarTranspo(ciphertext, bestTranspoKey), decryption);
                        }
                        else if (transpoType == CipherLib.TranspositionType.row)
                        {
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeRowTranspo(ciphertext, bestTranspoKey), ciphertext);
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(decryptionAfterTranspo, CipherLib.Annealing.DecodeRowTranspo(ciphertext, bestTranspoKey));
                            //bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(decryption, CipherLib.Annealing.DecodeRowTranspo(ciphertext, bestTranspoKey));
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeRowTranspo(ciphertext, bestTranspoKey), decryption);
                        }
                        else if (transpoType == CipherLib.TranspositionType.myszkowski)
                        {
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeMyszkowskiTranspo(ciphertext, bestTranspoKey), decryption);
                        }
                        else if (transpoType == CipherLib.TranspositionType.amsco)
                        {
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, bestTranspoKey, new int[] { 0, 1 }), decryption);
                        }
                        //bestMonoSubKey = result2.Item2;
                    }
                }
                else
                {
                    decryption = CipherLib.Annealing.CrackMonoSub(ciphertext, 10000, CipherLib.SolutionType.ChiSquared);

                    if (transpoType == CipherLib.TranspositionType.row)
                    {
                        result = CipherLib.Annealing.CrackRowTranspoReturnKey(decryption, columnNum, 1000);
                    }
                    else if (transpoType == CipherLib.TranspositionType.column)
                    {
                        result = CipherLib.Annealing.CrackColumnTranspoReturnKey(decryption, columnNum, 1000);
                    }

                    decryption = result.Item1;

                    Array.Copy(result.Item2, newTranspoKey, columnNum);

                    newScore = CipherLib.Annealing.QuadgramScore(decryption);

                    if (i == 0 || newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestDecryption = decryption;
                        Array.Copy(newTranspoKey, bestTranspoKey, columnNum);
                        if (transpoType == CipherLib.TranspositionType.column)
                        {
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeColumnTranspo(ciphertext, bestTranspoKey), decryption);
                        }
                        else if (transpoType == CipherLib.TranspositionType.row)
                        {
                            bestMonoSubKey = CipherLib.Annealing.GetKeyFromPlaintext(CipherLib.Annealing.DecodeRowTranspo(ciphertext, bestTranspoKey), decryption);
                        }
                    }
                }
            }

            Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write("Best substitution key:\n\n");
            //Console.Write("a b c d e f g h i j k l m n o p q r s t u v w x y z");
            //CipherLib.Utils.DisplayArray(CipherLib.Annealing.GetKeyFromPlaintext(bestDecryption, bestDecryptionAfterTranspo));
            Console.Write("Plaintext:  abcdefghijklmnopqrstuvwxyz\n");
            Console.Write("Ciphertext: ");
            //Console.Write(CipherLib.Annealing.GetKeyFromPlaintext(bestDecryption, bestDecryptionAfterTranspo).ToUpper());
            Console.Write(bestMonoSubKey.ToUpper());
            Console.Write("\n\n");
            //Console.Write("Best transposition key key:\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey);
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");

            Console.Write(bestDecryption);

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
