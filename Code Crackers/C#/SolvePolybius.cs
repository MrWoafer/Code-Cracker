using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPolybius
{
    class Program
    {
        //const int numOfTrials = 1000;
        const int numOfTrials = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Polybius Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--PolybiusMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            /// n is the length of the ngrams
            int n;
            string alphabet;
            if (args.Length > 0)
            {
                n = Int32.Parse(args[0]);
                alphabet = args[1];
            }
            else
            {
                n = 2;
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }
            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("N-Gram Length: " + n);
            Console.Write("\n\n-----------------------\n\n");

            string subCiphertext = "";

            Dictionary<string, char> ngramToLetter = new Dictionary<string, char>();
            Dictionary<char, string> letterToNgram = new Dictionary<char, string>();

            string ngram;
            int index = 0;
            for (int i = 0; i < ciphertext.Length; i += n)
            {
                ngram = "";
                for (int j = 0; j < n; j++)
                {
                    ngram += ciphertext[i + j];
                }
                if (!ngramToLetter.ContainsKey(ngram))
                {
                    ngramToLetter[ngram] = alphabet[index];
                    letterToNgram[alphabet[index]] = ngram;
                    index++;
                }
                subCiphertext += ngramToLetter[ngram];
            }

            string solution = CipherLib.Annealing.CrackCustomMonoSub(subCiphertext, alphabet, numOfTrials);

            /// form the grid to display the key
            /*if (n == 2 && CipherLib.Annealing.NumOfDistinctChars(ciphertext) == 10)
            {
                char[][] key = new char[5][];
                for (int i = 0; i < 5; i++)
                {
                    key[i] = new char[5];
                }
            }*/

            //Console.Write("Key found: ");
            Console.Write("Key found:\n");
            /// Display the key
            Dictionary<string, char> nGramsToPlaintext = new Dictionary<string, char>();
            for (int i = 0; i < solution.Length; i++)
            {
                ngram = "";
                for (int j = 0; j < n; j++)
                {
                    ngram += ciphertext[i * n + j];
                }
                if (!nGramsToPlaintext.ContainsKey(ngram))
                {
                    nGramsToPlaintext[ngram] = solution[i];
                }
            }
            foreach (string i in nGramsToPlaintext.Keys)
            {
                Console.Write(i + " ");
            }
            //Console.Write(" ");
            Console.Write("\n");
            foreach (char i in nGramsToPlaintext.Values)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(i);
                Console.Write(" ");
            }
            Console.Write("\n\n");

            Console.Write(solution);

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
