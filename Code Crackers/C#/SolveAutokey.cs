using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAutokey
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

            Console.Write("-- C# Autokey Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");
            
            string ciphertext = System.IO.File.ReadAllText("--AutokeyMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int keyLength;
            string alphabet;
            if (args.Length > 0)
            {
                keyLength = Int32.Parse(args[0]);
                alphabet = args[1];
            }
            else
            {
                keyLength = 9;
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            Console.Write("Alphabet: " + alphabet + "\n\n");
            Console.Write("Key Length: " + keyLength.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            string key = "";

            float bestScore = float.MinValue;
            int bestKey;
            float newScore;

            for (int i = 0; i < keyLength; i++)
            {
                bestKey = 0;
                //bestScore = CipherLib.Annealing.ChiSquared(DecodeAutokeyPartial(ciphertext, 0, keyLength, alphabet));
                bestScore = CipherLib.Annealing.ChiSquared(DecodeAutokeyPartial(ciphertext, i, 0, keyLength, alphabet));

                for (int j = 1; j < alphabet.Length; j++)
                {
                    //newScore = CipherLib.Annealing.ChiSquared(DecodeAutokeyPartial(ciphertext, j, keyLength, alphabet));
                    newScore = CipherLib.Annealing.ChiSquared(DecodeAutokeyPartial(ciphertext, i, j, keyLength, alphabet));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestKey = j;
                    }
                }

                key += alphabet[bestKey];
            }

            Console.Write("Best Key:\n\n");
            Console.Write(key);
            Console.Write("\n\n");
            Console.Write(DecodeAutokey(ciphertext, key, alphabet));
            Console.Write("\n\n--------------------------------------\n\n");
            Console.Write("Program finished.\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        public static string DecodeAutokeyPartial(string ciphertext, int keyColumn, int shift, int keyLength, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            //for (int i = 0; i < ciphertext.Length; i += keyLength)
            for (int i = keyColumn; i < ciphertext.Length; i += keyLength)
            {
                plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(ciphertext[i]) - shift, alphabet.Length)]);
                //shift = alphabet.IndexOf(ciphertext[i]);
                shift = alphabet.IndexOf(plaintext[plaintext.Length - 1]);
            }
            return plaintext.ToString();
        }

        public static string DecodeAutokey(string ciphertext, string key, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(ciphertext[i]) - alphabet.IndexOf(key[i]), alphabet.Length)]);
                key += plaintext[plaintext.Length - 1];
            }
            return plaintext.ToString();
        }
    }
}
