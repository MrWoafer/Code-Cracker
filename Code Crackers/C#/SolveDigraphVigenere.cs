using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDigraphVigenere
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

            //Console.Write("-- C# Digraph Vigenère Solver --");
            Console.Write("-- C# N-Graph Vigenère Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            //string ciphertext = System.IO.File.ReadAllText("--DigraphVigenereMessage.txt");
            string ciphertext = System.IO.File.ReadAllText("--NGraphVigenereMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int period;
            string alphabetInput;
            int nGraphLength;
            if (args.Length > 0)
            {
                period = Int32.Parse(args[0]);
                //alphabetInput = args[1];
                //alphabetInput = System.IO.File.ReadAllText("--NGramVigenereAlphabet.txt");
                alphabetInput = System.IO.File.ReadAllText("--NGraphVigenereAlphabet.txt");
                //nGraphLength = Int32.Parse(args[2]);
                nGraphLength = Int32.Parse(args[1]);
            }
            else
            {
                //period = 4;
                period = 3;
                alphabetInput = "aaabacadaeafagahaiajakalamanaoapaqarasatauavawaxayazbabbbcbdbebfbgbhbibjbkblbmbnbobpbqbrbsbtbubvbwbxbybzcacbcccdcecfcgchcicjckclcmcncocpcqcrcsctcucvcwcxcyczdadbdcdddedfdgdhdidjdkdldmdndodpdqdrdsdtdudvdwdxdydzeaebecedeeefegeheiejekelemeneoepeqereseteuevewexeyezfafbfcfdfefffgfhfifjfkflfmfnfofpfqfrfsftfufvfwfxfyfzgagbgcgdgegfggghgigjgkglgmgngogpgqgrgsgtgugvgwgxgygzhahbhchdhehfhghhhihjhkhlhmhnhohphqhrhshthuhvhwhxhyhziaibicidieifigihiiijikiliminioipiqirisitiuiviwixiyizjajbjcjdjejfjgjhjijjjkjljmjnjojpjqjrjsjtjujvjwjxjyjzkakbkckdkekfkgkhkikjkkklkmknkokpkqkrksktkukvkwkxkykzlalblcldlelflglhliljlklllmlnlolplqlrlsltlulvlwlxlylzmambmcmdmemfmgmhmimjmkmlmmmnmompmqmrmsmtmumvmwmxmymznanbncndnenfngnhninjnknlnmnnnonpnqnrnsntnunvnwnxnynzoaobocodoeofogohoiojokolomonooopoqorosotouovowoxoyozpapbpcpdpepfpgphpipjpkplpmpnpopppqprpsptpupvpwpxpypzqaqbqcqdqeqfqgqhqiqjqkqlqmqnqoqpqqqrqsqtquqvqwqxqyqzrarbrcrdrerfrgrhrirjrkrlrmrnrorprqrrrsrtrurvrwrxryrzsasbscsdsesfsgshsisjskslsmsnsospsqsrssstsusvswsxsysztatbtctdtetftgthtitjtktltmtntotptqtrtstttutvtwtxtytzuaubucudueufuguhuiujukulumunuoupuqurusutuuuvuwuxuyuzvavbvcvdvevfvgvhvivjvkvlvmvnvovpvqvrvsvtvuvvvwvxvyvzwawbwcwdwewfwgwhwiwjwkwlwmwnwowpwqwrwswtwuwvwwwxwywzxaxbxcxdxexfxgxhxixjxkxlxmxnxoxpxqxrxsxtxuxvxwxxxyxzyaybycydyeyfygyhyiyjykylymynyoypyqyrysytyuyvywyxyyyzzazbzczdzezfzgzhzizjzkzlzmznzozpzqzrzsztzuzvzwzxzyzz";
                //alphabetInput = "aaaacadaeafagahaiajakalamanaoapaqarasatauavawaxayazbabbbcbdbebfbgbhbibjbkblbmbnbobpbqbrbsbtbubvbwbxbybzcacbcccdcecfcgchcicjckclcmcncocpcqcrcsctcucvcwcxcyczdadbdcdddedfdgdhdidjdkdldmdndodpdqdrdsdtdudvdwdxdydzeaebecedeeefegeheiejekelemeneoepeqereseteuevewexeyezfafbfcfdfefffgfhfifjfkflfmfnfofpfqfrfsftfufvfwfxfyfzgagbgcgdgegfggghgigjgkglgmgngogpgqgrgsgtgugvgwgxgygzhahbhchdhehfhghhhihjhkhlhmhnhohphqhrhshthuhvhwhxhyhziaibicidieifigihiiijikiliminioipiqirisitiuiviwixiyizjajbjcjdjejfjgjhjijjjkjljmjnjojpjqjrjsjtjujvjwjxjyjzkakbkckdkekfkgkhkikjkkklkmknkokpkqkrksktkukvkwkxkykzlalblcldlelflglhliljlklllmlnlolplqlrlsltlulvlwlxlylzmambmcmdmemfmgmhmimjmkmlmmmnmompmqmrmsmtmumvmwmxmymznanbncndnenfngnhninjnknlnmnnnonpnqnrnsntnunvnwnxnynzoaobocodoeofogohoiojokolomonooopoqorosotouovowoxoyozpapbpcpdpepfpgphpipjpkplpmpnpopppqprpsptpupvpwpxpypzqaqbqcqdqeqfqgqhqiqjqkqlqmqnqoqpqqqrqsqtquqvqwqxqyqzrarbrcrdrerfrgrhrirjrkrlrmrnrorprqrrrsrtrurvrwrxryrzsasbscsdsesfsgshsisjskslsmsnsospsqsrssstsusvswsxsysztatbtctdtetftgthtitjtktltmtntotptqtrtstttutvtwtxtytzuaubucudueufuguhuiujukulumunuoupuqurusutuuuvuwuxuyuzvavbvcvdvevfvgvhvivjvkvlvmvnvovpvqvrvsvtvuvvvwvxvyvzwawbwcwdwewfwgwhwiwjwkwlwmwnwowpwqwrwswtwuwvwwwxwywzxaxbxcxdxexfxgxhxixjxkxlxmxnxoxpxqxrxsxtxuxvxwxxxyxzyaybycydyeyfygyhyiyjykylymynyoypyqyrysytyuyvywyxyyyzzazbzczdzezfzgzhzizjzkzlzmznzozpzqzrzsztzuzvzwzxzyzz";
                nGraphLength = 2;
            }
            //if (alphabetInput.Length % 2 != 0)
            if (alphabetInput.Length % nGraphLength != 0)
            {
                //Console.Write("ERROR: Provided alphabet does not have even length.");
                Console.Write("ERROR: Given N-Graph length does not divide the length of the provided alphabet.");
            }
            else
            {
                //string[] alphabet = new string[alphabetInput.Length / 2];
                string[] alphabet = new string[alphabetInput.Length / nGraphLength];

                //for (int i = 0; i + 1 < alphabet.Length; i += 2)
                //for (int i = 0; i + 1 < alphabetInput.Length; i += 2)
                for (int i = 0; i + 1 < alphabetInput.Length; i += nGraphLength)
                {
                    //alphabet[i / 2] = alphabetInput[i] + alphabet[i + 1];
                    //alphabet[i / 2] = alphabetInput[i].ToString() + alphabetInput[i + 1].ToString();
                    for (int j = 0; j < nGraphLength; j++)
                    {
                        alphabet[i / nGraphLength] += alphabetInput[i + j];
                    }
                }

                Console.Write("Alphabet:\n");
                if (alphabet.Length < 1000)
                {
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        Console.Write(alphabet[i] + " ");
                    }
                }
                else
                {
                    Console.Write("Alphabet is over 1000 ngraphs in length, so will not be displayed. Please check the --NGraphVigenereAlphabet.txt file if you would like to see the alphabet.");
                }
                Console.Write("\n\n");
                Console.Write("Period: " + period);
                Console.Write("\n\n");
                Console.Write("N-Graph Length: " + nGraphLength);
                Console.Write("\n\n");
                Console.Write("-----------------------\n\n");

                //Console.Write(CipherLib.NGramVigenere.Decrypt(ciphertext, new int[] { 40, 658, 122 }, alphabet));
                //Console.Write(CipherLib.NGramVigenere.CrackReturnKey(ciphertext, period, alphabet).Item1);

                /// Create a dictionary of the indices of the ngrams. Put in the ngram; get out the index.
                Dictionary<string, int> indices = new Dictionary<string, int>();
                for (int i = 0; i < alphabet.Length; i++)
                {
                    indices[alphabet[i]] = i;
                }

                Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[period]);

                //result = CipherLib.NGramVigenere.CrackReturnKey(ciphertext, period, alphabet);
                result = CipherLib.NGramVigenere.CrackReturnKey(ciphertext, period, alphabet, indices);

                Console.Write("Best Key:\n\n");
                CipherLib.Utils.DisplayArray(result.Item2);
                Console.Write("\n");
                Console.Write("(");
                /// Display the key as ngrams(/ngraphs) instead of integers;
                for (int i = 0; i < result.Item2.Length; i++)
                {
                    //Console.Write(alphabet[result.Item2[i]]);
                    Console.Write(alphabet[result.Item2[i]].ToUpper());
                    if (i < result.Item2.Length - 1)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write(")");
                Console.Write("\n\n");
                Console.Write(result.Item1);
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
