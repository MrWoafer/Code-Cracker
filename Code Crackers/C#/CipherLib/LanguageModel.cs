using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    public struct NGram
    {
        public string ngram;
        public double logProbability;
        public int length;

        public NGram(string nGram, double score)
        {
            ngram = nGram;
            logProbability = score;
            length = ngram.Length;
        }

        public int Length()
        {
            return length;
        }
        public double Score()
        {
            return logProbability;
        }
        public string Text()
        {
            return ngram;
        }
    }

    public class LanguageModel
    {
        private Dictionary<string, int> ngramProbs = new Dictionary<string, int>();
        private int ngramProbsTotal;
        //private Dictionary<string[], int> ngramWordProbs = new Dictionary<string[], int>();
        private Dictionary<string, int> ngramWordProbs = new Dictionary<string, int>();
        private int ngramWordProbsTotal;
        private Dictionary<string, int> ngramWordProbs2 = new Dictionary<string, int>();
        private int ngramWordProbs2Total;

        private Dictionary<int, List<string>> dictionaryWords = new Dictionary<int, List<string>>();

        //private static readonly string[] corporaLocations = new string[] {
        private static readonly List<string> corporaLocationsList = new List<string> {
            "C:\\Users\\woafe\\Documents\\Coding\\Cipher Challenge 2019\\Text Corpora\\Dracula.txt",
            "C:\\Users\\woafe\\Documents\\Coding\\Cipher Challenge 2019\\Text Corpora\\Frankenstein.txt",
            "C:\\Users\\woafe\\Documents\\Coding\\Cipher Challenge 2019\\Text Corpora\\Jekyll And Hyde.txt" };

        private static string[] corporaLocations;

        private string corpus;
        private string baseCorpus;

        private int n;

        string ngram;

        public LanguageModel(int nGramLength, string dictionaryName = "==FullDictionary.txt")
        {
            n = nGramLength;

            Initialise();
            LoadDictionary(dictionaryName);
        }

        public void Analyse(bool displayLetters = false, bool displayWords = false)
        {
            AnalyseLetters(displayLetters);
            n -= 1;
            AnalyseWords(displayWords);
            //ngramWordProbs2 = ngramWordProbs;
            ngramWordProbs2 = new Dictionary<string, int>(ngramWordProbs);
            ngramWordProbs = new Dictionary<string, int>();
            ngramWordProbs2Total = ngramWordProbsTotal;
            ngramWordProbsTotal = 0;
            n += 1;
            AnalyseWords(displayWords);

            /*foreach (string i in ngramWordProbs2.Keys)
            {
                Console.Write("\n" + i);
            }
            Console.Write("\nngramProbs2Total: " + ngramWordProbs2Total);
            Console.Write("\nngramProbsTotal: " + ngramWordProbsTotal);
            foreach (string i in ngramWordProbs.Keys)
            {
                Console.Write("\n" + i);
            }*/
        }

        private void Initialise()
        {
            string[] corpusFiles = System.IO.Directory.GetFiles("C:\\Users\\woafe\\Documents\\Coding\\Cipher Challenge 2019\\Text Corpora\\corpus1\\", "*txt");

            foreach (string i in corpusFiles)
            {
                //Console.Write(i);
                corporaLocationsList.Add(i);
            }

            corporaLocations = corporaLocationsList.ToArray();

            //int[][] perms = Annealing.Permutations(n);
            int[][] perms = Annealing.NToTheMArrangements(26, n);

            string key = "";

            for (int i = 0; i < perms.Length; i++)
            {
                key = "";

                for (int j = 0; j < perms[i].Length; j++)
                {
                    key += Annealing.ALPHABET[perms[i][j]];
                }

                //Console.Write("\n" + key);

                ngramProbs[key] = 1;
                //ngramProbs[key] = 0;
            }
        }

        /*public void AnalyseLetters()
        {
            int total = 0;            
            string[] lines = System.IO.File.ReadAllLines("C:\\Users\\woafe\\Documents\\Coding\\Cipher Challenge 2019\\Text Corpora\\english_trigrams.txt");
            string ngramCount;

            for (int i = 0; i < lines.Length; i++)
            {
                ngram = "";
                ngramCount = "";

                for (int j = 0; j < n; j++)
                {
                    ngram += lines[i][j].ToString().ToLower();                    
                }

                for (int j = 4; j < lines[i].Length; j++)
                {
                    ngramCount += lines[i][j].ToString();
                }

                //Console.Write("\n" + ngram + " " + ngramCount);

                //ngramProbs[ngram] += Int32.Parse(ngramCount);
                ngramProbs[ngram] += Int32.Parse(ngramCount) / 10;

                //total += Int32.Parse(ngramCount);
                total += Int32.Parse(ngramCount) / 10;

                //Console.Write("\n" + total);
            }



            //ngramProbs["-=Total=-"] = total;
            ngramProbsTotal = total;

            /*int[][] perms = Annealing.NToTheMArrangements(26, n);
            string key;

            for (int i = 0; i < perms.Length; i++)
            {
                key = "";

                for (int j = 0; j < perms[i].Length; j++)
                {
                    key += Annealing.ALPHABET[perms[i][j]];
                }

                //ngramProbs[key] = ngramProbs[key] / total;

                //Console.Write("\n" + key + " " + ngramProbs[key]);
            }*/
        /*}*/

        public void AnalyseWords(bool displayThem = false)
        {
            int total = 0;
            //string[] wordgram = new string[n];
            string wordgram;
            for (int i = 0; i < corporaLocations.Length; i++)
            {
                baseCorpus = System.IO.File.ReadAllText(corporaLocations[i]);

                string[] words = baseCorpus.Split(null);

                /*foreach (string j in words)
                {
                    Console.Write("\n" + j);
                }*/

                for (int j = 0; j <= words.Length - n; j++)
                {
                    //wordgram = new string[n];
                    wordgram = "";

                    for (int k = 0; k < n; k++)
                    {
                        //wordgram[k] = words[j + k];
                        wordgram += words[j + k];
                        if (k < n - 1)
                        {
                            wordgram += " ";
                        }
                    }

                    /*foreach (string k in wordgram)
                    {
                        Console.Write(k + " ");
                        //Console.Write("\n");
                    }
                    Console.Write("\n");*/

                    //if (wordgram == new string[3] { "and", "that", "was" })
                    /*if (wordgram.SequenceEqual(new string[3] { "and", "that", "was" }))
                    {
                        Console.Write("Yeah!");
                    }*/

                    if (ngramWordProbs.ContainsKey(wordgram))
                    //if (existsA)
                    {
                        //Console.Write("Yippee!");
                        ngramWordProbs[wordgram] += 1;
                    }
                    else
                    {
                        ngramWordProbs[wordgram] = 1;
                    }

                    if (ngramWordProbs.ContainsKey(wordgram))
                    //if (existsA)
                    {
                        //Console.Write("Yippee!");
                        ngramWordProbs[wordgram] += 1;
                    }
                    else
                    {
                        ngramWordProbs[wordgram] = 1;
                    }

                    total += 1;
                }                

                //Console.Write(corpus);
            }

            ngramWordProbsTotal = total;

            if (displayThem)
            {
                foreach (string i in ngramWordProbs.Keys.ToArray<string>())
                {
                    if (ngramWordProbs[i] > 1)
                    {
                        Console.Write("\n");
                        /*foreach (string j in i)
                        {
                            Console.Write(j + " ");
                        }*/
                        Console.Write(i);
                        Console.Write(" " + ngramWordProbs[i]);
                    }
                }
                Console.Write("\n\nTotal: " + ngramWordProbsTotal);
            }
        }

        public void AnalyseLetters(bool displayThem = false)
        {
            int total = 0;
            string lettergram;
            for (int i = 0; i < corporaLocations.Length; i++)
            {
                corpus = System.IO.File.ReadAllText(corporaLocations[i]);

                for (int j = 0; j <= corpus.Length - n; j++)
                {
                    lettergram = "";

                    for (int k = 0; k < n; k++)
                    {
                        lettergram += corpus[j + k];
                    }

                    if (ngramProbs.ContainsKey(lettergram))
                    {
                        ngramProbs[lettergram] += 1;
                    }
                    else
                    {
                        ngramProbs[lettergram] = 1;
                    }

                    total += 1;
                }
            }

            ngramProbsTotal = total;

            if (displayThem)
            {
                foreach (string i in ngramProbs.Keys.ToArray<string>())
                {
                    if (ngramProbs[i] > 1)
                    {
                        Console.Write("\n");
                        Console.Write(i);
                        Console.Write(" " + ngramProbs[i]);
                    }
                }
                Console.Write("\n\nTotal: " + ngramProbsTotal);
            }
        }

        public double WordNGramProb(string wordNGram, int ngramLength)
        {
            if (ngramLength == n)
            {
                if (ngramWordProbs.ContainsKey(wordNGram))
                {
                    return (double)ngramWordProbs[wordNGram] / ngramWordProbsTotal;
                }
                else
                {
                    return 1d / ngramWordProbsTotal;
                }
            }
            else if (ngramLength == n-1)
            {
                if (ngramWordProbs2.ContainsKey(wordNGram))
                {
                    return (double)ngramWordProbs2[wordNGram] / ngramWordProbs2Total;
                }
                else
                {
                    return 1d / ngramWordProbs2Total;
                }
            }
            else
            {
                throw new ArgumentException("ngramLength is invalid!");
            }
        }

        public double LetterNGramProb(string letterNGram)
        {
            if (ngramProbs.ContainsKey(letterNGram))
            {
                //Console.Write("\n" + ngramProbs[letterNGram] + " " + ngramProbsTotal);
                return (double)ngramProbs[letterNGram] / ngramProbsTotal;
            }
            else
            {
                return 1d / ngramProbsTotal;
            }
        }

        public double NGramsAnalysedCount(int ngramLength)
        {
            if (ngramLength == n)
            {
                return ngramProbsTotal;
            }
            else if(ngramLength == n - 1)
            {
                return ngramWordProbs2Total;
            }
            else
            {
                throw new ArgumentException("ngramLength is invalid!");
            }
        }

        public string RandomWord(int length)
        {
            if (dictionaryWords.ContainsKey(length))
            {
                return dictionaryWords[length][Annealing.rand.Next() % dictionaryWords[length].Count];
            }
            else
            {
                throw new ArgumentException("Dictionary does no have any words of length " + length + ".");
            }
        }

        private void LoadDictionary(string dictionaryName = "==FullDictionary.txt")
        {
            string[] dictionary = Annealing.GetDictionary(dictionaryName);

            for (int i = 0; i < dictionary.Length; i++)
            {
                if (dictionaryWords.ContainsKey(dictionary[i].Length))
                {
                    dictionaryWords[dictionary[i].Length].Add(dictionary[i]);
                }
                else
                {
                    dictionaryWords[dictionary[i].Length] = new List<string>();
                    dictionaryWords[dictionary[i].Length].Add(dictionary[i]);
                }
            }
        }
    }
}
