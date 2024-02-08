from Alphabet import *
from operator import *
from WordFunctions import *
from FindWords import *

def AnalyseText(Text):

    Frequencies = {}

    for x in alphabetlist:

        Frequencies[x] = 0

    for letter in Text:

        if letter in Frequencies:

            Frequencies[letter] += 1

        #else:

            #Frequencies[letter] = 1

    print ("\nFrequencies:\n")

    for letter in alphabetlist:

        print (letter, "  ", Frequencies[letter])

    SortedFrequencies = sorted(Frequencies.items(), key = lambda a: a[1])

    #print ("\nThe Most Common Letter is:  ", SortedFrequencies[0])
    #print ("\nThe Most Common Letter is:  ", SortedFrequencies[-1][0])

    print ("\nIn Order, The Frequencies Are:\n")

    for letter in reversed(SortedFrequencies):

        #print (letter, "  ", Frequencies[letter])
        print (letter[0], "  ", letter[1])

    print ("\nThe Most Common Letter is:  ", SortedFrequencies[-1][0])

    return SortedFrequencies



def AnalyseWords(Text, LetterFrequencies):

    Frequencies = {}

    TextCopy = Text

    Text = Text.split(" ")

    for word in Text:

        if word in Frequencies:

            #Frequencies[letter] += 1
            Frequencies[word] += 1

        else:

            Frequencies[word] = 1

    SortedFrequencies = sorted(Frequencies.items(), key = lambda a: a[1])

    print ("\nIn Order, The Frequencies Are:\n")

    for word in reversed(SortedFrequencies):

        #print (word[0], "  ", word[1])
        #print (word[0], "\t\t\t\t", word[1])
        print (word[0], " "*(20-len(word[0])), word[1])

    print ("\nThe Most Common Word is:  ", SortedFrequencies[-1][0])


    Key = {}

    for x in alphabetlist:

        Key[x] = ""

    #Key["e"] = SortedFrequencies[-1][0]
    #Key["e"] = LetterFrequencies[-1][0]
    Key["e"] = SortedFrequencies[-1][0][-1]

    for word in Text:

        #if len(word) == 3 and word[-1] == SortedFrequencies[-1][0]:
        #if len(word) == 3 and word[-1] == LetterFrequencies[-1][0]:
        if len(word) == 3 and word[-1] == Key["e"]:

            Key["t"] = word[0]
            Key["h"] = word[1]

    for word in Text:

        if len(word) == 2 and word[0] == Key["t"]:

            Key["o"] = word[1]

    for word in Text:

        if len(word) == 4 and word[0] == Key["t"] and word[1] == Key["h"] and word[3] == Key["t"]:

            Key["a"] = word[2]

    for word in Text:

        if len(word) == 3 and word[0] == Key["a"]:

            Key["n"] = word[1]
            Key["d"] = word[2]

    #if False:
    if True:

        #for trial in range(0, 100):
        for trial in range(0, 26):

            #print (trial, Key)

            for word in Text:

                newword = ""

                for letter in word:

                    Found = False

                    #for character in Keys:
                    for character in Key:

                        #if Keys[character] == letter:
                        if Key[character] == letter:

                            Found = True

                            #newword = newword + Key[letter]
                            #newword = newword + Key[character]
                            newword = newword + character

                    if Found == False:

                        newword = newword + "_"

                    else:

                        #newword = newword + letter
                        pass

                    #print (newword)

                try:    

                    PotentialWords = FindPotentialWords(newword, Dictionary, "")

                    if len(PotentialWords) == 1:

                        for letter in range(0, len(PotentialWords[0])):

                            #print (letter, PotentialWords[0][letter], word)

                            #Keys[PotentialWords[0][letter]] = word[letter]
                            Key[PotentialWords[0][letter]] = word[letter]

                except:

                    #print (newword, word, letter, PotentialWords)
                    pass

                else:

                    NewPotentialWords = []

                    for word2 in PotentialWords:

                        for letter in range(0, len(word)):

                            if newword[letter] == "_" and Key[word2[letter]] == "":

                                NewPotentialWords.append(word2)

                    if len(NewPotentialWords) == 1:

                        for letter in range(0, len(NewPotentialWords[0])):

                            Key[NewPotentialWords[0][letter]] = word[letter]

                            

    BreakText()

    print ("\n\nI Am Led To Believe:")

    print ("")

    for x in alphabetlist:

        print (x, "->", Key[x])


    NewMsg = ""

    #for letter in Text:
    for letter in TextCopy:

        Found = False

        for character in Key:

            if Key[character] == letter:

                NewMsg = NewMsg + character

                Found = True

        if not Found:

            NewMsg = NewMsg + letter.upper()

    print ("\n\nNew Message:\n")

    print (NewMsg)

    return


def BreakText():

    print ("\n" + "-"*25 + "\n")

    return



Text = RemovePunctuation(open(input("Enter File Name: "), "r").read().lower())

BreakText()

#print ("LETTERS:\n")
print ("\nLETTERS:\n")

LetterFrequencies = AnalyseText(Text)


#DictionaryName = input ("Enter Dictionary Name: ")
#DictionaryName = "Medium Dictionary.txt"
DictionaryName = "Large Dictionary.txt"

Dictionary = open(DictionaryName, "r").readlines()

for item in range(0, len(Dictionary)):

    Dictionary[item] = RemovePunctuation(Dictionary[item].lower())

Dictionary = set(Dictionary)


BreakText()

print ("\n\nWORDS:\n")
#print ("\n\n\nWORDS:\n")

AnalyseWords(Text, LetterFrequencies)

#print ("\n\nPress ENTER To Exit...")
EXIT = input ("\n\nPress ENTER To Exit...")
