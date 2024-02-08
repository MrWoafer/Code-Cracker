#####################################################################

from Alphabet import *
from WordFunctions import *
from math import *
from time import sleep
from random import randint

def LoadQuadgramFrequencyTable(ListOrDictionary, CountUpTo):

    List = open("Quadgram Frequencies.txt", "r").read()

    if ListOrDictionary == "List":

        FrequencyTable = []

    elif ListOrDictionary == "Dictionary":

        FrequencyTable = {}

    TotalFreq = 0

    CurrentQuadgram = ""

    CurrentNumber = ""

    NumberOfQuadgrams = 0

    for character in List:

        if CountUpTo == "All" or NumberOfQuadgrams < CountUpTo:

            letter = character.lower()

            if letter in alphabetset:

                if CurrentNumber != "":

                    if ListOrDictionary == "List":

                        FrequencyTable.append([CurrentQuadgram, int(CurrentNumber)])

                    elif ListOrDictionary == "Dictionary":

                        FrequencyTable[CurrentQuadgram] = int(CurrentNumber)

                    TotalFreq += int(CurrentNumber)
                    
                    CurrentQuadgram, CurrentNumber = letter, ""

                    NumberOfQuadgrams += 1

                else:

                    CurrentQuadgram = CurrentQuadgram + letter

            elif letter == " ":

                pass

            elif letter in digitset:

                CurrentNumber = CurrentNumber + letter


    for item in FrequencyTable:

        if ListOrDictionary == "List":

            item[1] = item[1]*100/TotalFreq

        if ListOrDictionary == "Dictionary":

            FrequencyTable[item] = FrequencyTable[item]*100/TotalFreq

    if ListOrDictionary == "List":

        FrequencyTable.insert(0, TotalFreq)

    if ListOrDictionary == "Dictionary":

        FrequencyTable["Total"] = TotalFreq

    return FrequencyTable



def QuadgramScore(Msg, QuadgramFrequencies):

    NumberOfQuadgrams = len(QuadgramFrequencies)

    Msg = RemoveSpaces(RemovePunctuation(Msg))

    Score = 0

    Quadgram = ""

    UsedQuadgrams = set()

    for letter in range(0, len(Msg)):
            
        if letter <= len(Msg)-4:

            Quadgram = Msg[letter:letter+4]

            if Quadgram in QuadgramFrequencies and Quadgram not in UsedQuadgrams:
                try:
                    Score += log10(QuadgramFrequencies[Quadgram]/100)
                except:
                    pass

            else:
                try:
                    Score += log10(0.01/QuadgramFrequencies["Total"])
                except:
                    pass

    return Score

QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

##############################################

letters = {'a':'1','b':'2','c':'3','d':'4','e':'5','f':'6','g':'7','h':'8','i':'9','j':'10','k':'11','l':'12','m':'13','n':'14','o':'15','p':'16','q':'17','r':'18','s':'19','t':'20','u':'21','v':'22','w':'23','x':'24','y':'25','z':'26'}
#numbers =  {v: k for k, v in letters.iteritems()}
numbers =  {v: v for v in letters.items()}

def Numberify(cipher_text):

    global letters

    cipher_nums = []

    for char in cipher_text:

        cipher_nums.append(int(letters[char])-1)

    return cipher_nums

def Decrypt(text,key):
    
    plaintext = ""
    n = 0
    temp_key = key

    while len(text) > n:

        new_letter = (text[n]-temp_key[n])%26
    
        #plaintext = plaintext + numbers[str(new_letter+1)]
        #plaintext = plaintext + alphabetlist[str(new_letter+1)]
        #plaintext = plaintext + alphabet[str(new_letter+1)]
        plaintext = plaintext + alphabet[new_letter+1]

        temp_key.append(new_letter)

        n += 1

        #print(key)
    
    temp_key = key
    return plaintext

def IncrementKey(key,layer):

    if len(key) == layer + 1 and key[layer] == 25:

        key[layer] = 0
        return key
    
    if key[layer] == 25:

        key[layer] = 0
        key = IncrementKey(key,layer+1)
        return key

    else:

        key[layer] += 1

        return key

#def AutoKeyHillClimb(Msg):
def AutoKeyHillClimb(Msg, RangeStart, RangeEnd):

    top_score = -10000000

    #\cipher_text = Numberify(raw_input("Enter Cipher Text: "))s
    cipher_text = Numberify(Msg)

    #BestMsg = "Couldn't Find It!"
    BestMsg = Msg

    BestKey = []

    #Hill climbing for longer keys
    #for key_length in range(1,27):
    for key_length in range(RangeStart,RangeEnd+1):

        length_score = -10000000
        
        #print("Key Length: " + str(key_length))
        print("Key Length: " + str(key_length))
        print ("-------")
        print ()

        key = []
        for b in range(0,key_length):

            key.append(randint(0,25))

        x = 26^key_length
        if x > 5000:
            x = 5000
        for loops in range(0,x):

            improved = True

            while improved == True:

                improved = False

                for a in range(0,len(key)):

                    top_value = key[a]

                    for i in range(0,26):

                        key[a] = i

                        key_score = QuadgramScore(Decrypt(cipher_text,key),QuadgramFrequencies)

                        #print(key_score)
                        if key_score > length_score:
                            """if key_score > top_score:
                                top_score = key_score
                                #//print("TOP")
                                print("New Best Key!")

                                print ("\nDecipherment: " + Decrypt(cipher_text,key))
                                print()
                                BestMsg = Decrypt(cipher_text,key)"""
                            length_score = key_score
                            top_value = i
                            #print(str(key[0:key_length]),key_score)
                            #BestKey = key
                            #for x in key:
                            #for x in key[0:keylength]:
                            """for x in key[0:key_length]:

                                print (alphabetlist[int(x)] + " ", end = "")"""
                                
                            improved = True

                            if key_score > top_score:
                                top_score = key_score
                                #print("New Best Key!")
                                print("New Best Key!  ", end = "")
                                for x in key[0:key_length]:
                                    #print (alphabetlist[int(x)] + " ", end = "")
                                    print (alphabetlist[int(x)], end = "")
                                #print ("\nDecipherment: " + Decrypt(cipher_text,key))
                                print ("\n\nDecipherment: " + Decrypt(cipher_text,key))
                                print()
                                BestMsg = Decrypt(cipher_text,key)
                                print()
                                BestKey = key
                                BestKeyLength = key_length

                        key = key[0:key_length]

                    key[a] = top_value

            key = []
            for b in range(0,key_length):

                key.append(randint(0,25))


        #print ("\n------------\n")
        print ("\n----------------------------\n")



    #####################
    #return BestMsg
    #return BestMsg

    #######print(BestKey)

    BestKeyStr = ""

    #for x in key:
    #for x in key[0:key_length]:
    for x in BestKey[0:BestKeyLength]:

        BestKeyStr = BestKeyStr + alphabetlist[int(x)]

    return BestKeyStr
