from Alphabet import *
from WordFunctions import *
import random
import copy
from math import *
from NumberFunctions import *
import subprocess
from time import sleep
import AutokeyDecrypter
#import scipy
from scipy import stats
from CharFunctions import *

def GetKeyOfValue(Value, Dictionary):

    for entry in Dictionary:

        if Dictionary[entry] == Value:

            return entry

    return None
        

def Decode(Msg, Key):

    OriginalMsg = Msg

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    #Msg = Msg.lower()

    NewMsg = ""

    if isinstance(Key, list) or isinstance(Key, tuple):

        NewMsg = DecodePolyalphabet(Msg, Key)

    else:

        for letter in Msg:

            DecodedLetter = GetKeyOfValue(letter, Key)

            #if letter in Key:
            if DecodedLetter != None:

                #NewMsg = NewMsg + Key[letter]
                NewMsg = NewMsg + DecodedLetter

            else:

                NewMsg = NewMsg + letter.upper()

            #print (NewMsg)

    NewNewMsg = ""

    letter = 0
    letter2 = 0

    while letter < len(NewMsg):

        if OriginalMsg[letter2] in alphabetset:

            NewNewMsg = NewNewMsg + NewMsg[letter]

            letter += 1

        else:

            NewNewMsg = NewNewMsg + OriginalMsg[letter2]

        letter2 += 1

    #return NewMsg
    return NewNewMsg


def Encode(Msg, Key):

    OriginalMsg = Msg

    NewMsg = ""

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    if isinstance(Key, list) or isinstance(Key, tuple):

        NewMsg = EncodePolyalphabet(Msg, Key)

    else:

        for letter in Msg:

            if letter in Key and Key[letter] != "":

                NewMsg = NewMsg + Key[letter]

            else:

                NewMsg = NewMsg + letter.upper()


    NewNewMsg = ""

    letter = 0
    letter2 = 0

    while letter < len(NewMsg):

        if OriginalMsg[letter2] in alphabetset:

            NewNewMsg = NewNewMsg + NewMsg[letter]

            letter += 1

        else:

            NewNewMsg = NewNewMsg + OriginalMsg[letter2]

        letter2 += 1

    #return NewMsg
    return NewNewMsg


#def LetterFrequencies(Msg):
#def LetterFrequencies(Msg, KeyWordLength, KeyWordLetter):
def LetterFrequencies(Msg, KeyWordLength = 1, KeyWordLetter = 1):

    KeyWordLetter -= 1

    if KeyWordLetter < 0:

        KeyWordLetter = 0

    #Msg = RemoveSpaces(RemovePunctuation(Msg))
    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    LetterFreq = {}

    for letter in alphabetlist:

        LetterFreq[letter] = 0

    """for letter in Msg:

        if letter in LetterFreq:

            LetterFreq[letter] += 1

        elif letter in alphabetset:

            LetterFreq[letter] = 1"""

    Length = 0

    for letter in range(0, len(Msg)):

        if letter%KeyWordLength == KeyWordLetter:

            Length += 1

            if Msg[letter] in LetterFreq:

                LetterFreq[Msg[letter]] += 1

            elif Msg[letter] in alphabetset:

                LetterFreq[Msg[letter]] = 1

    LetterFreq = list(reversed(sorted(LetterFreq.items(), key = lambda a: a[1])))

    #for letter in LetterFreq:

        #letter.append(letter[1]/len(Msg))
        #letter = list(letter).append(letter[1]/len(Msg))

        #print (letter)

    #for letter in in range(0, len(LetterFreq)):
    for letter in range(0, len(LetterFreq)):

        #print (letter, LetterFreq[letter])

        #LetterFreq[letter] = list(LetterFreq[letter]).append(LetterFreq[letter][1]/len(Msg))
        LetterFreq[letter] = list(LetterFreq[letter])

        if len(Msg) > 0:
            
            #LetterFreq[letter].append(LetterFreq[letter][1]/len(Msg))
            LetterFreq[letter].append(LetterFreq[letter][1]/Length)
            LetterFreq[letter].append(0)

        #print (letter, LetterFreq[letter])

    return LetterFreq


def WordFrequencies(Msg):

    WordFreq = {}

    if len(RemovePunctuation(RemoveSpaces(Msg))) > 0:

        for word in RemovePunctuation(Msg).split(" "):

            if RemoveSpaces(word) != "":

                if word in WordFreq:

                    WordFreq[word] += 1

                else:

                    WordFreq[word] = 1

        WordFreq = list(reversed(sorted(WordFreq.items(), key = lambda a: a[1])))

        return WordFreq

    else:

            WordFreq = [["Nothing", "There's No Text!"]]

            return WordFreq



def TrigramFrequencies(Msg):

    TrigramFreq = {}

    #Msg = RemovePunctuation(RemoveSpaces(Msg))
    Msg = RemoveSpaces(Msg.lower())

    if len(Msg) > 0:

        for letter in range(0, len(Msg)-2):

            trigram = Msg[letter:letter+3]

            if trigram in TrigramFreq:

                TrigramFreq[trigram] += 1

            else:

                TrigramFreq[trigram] = 1

        TrigramFreq = list(reversed(sorted(TrigramFreq.items(), key = lambda a: a[1])))

        return TrigramFreq

    else:

        TrigramFreq = [["Nothing", "There's No Text!"]]

        return TrigramFreq



def BigramFrequencies(Msg):

    BigramFreq = {}

    #Msg = RemovePunctuation(RemoveSpaces(Msg))
    Msg = RemoveSpaces(Msg.lower())

    if len(Msg) > 0:

        for letter in range(0, len(Msg)-1):

            trigram = Msg[letter:letter+2]

            if trigram in BigramFreq:

                BigramFreq[trigram] += 1

            else:

                BigramFreq[trigram] = 1

        BigramFreq = list(reversed(sorted(BigramFreq.items(), key = lambda a: a[1])))

        return BigramFreq

    else:

        BigramFreq = [["Nothing", "There's No Text!"]]

        return BigramFreq


def QuadgramFrequencies(Msg):

    QuadgramFreq = {}

   # Msg = RemovePunctuation(RemoveSpaces(Msg))
    Msg = RemoveSpaces(Msg.lower())

    if len(Msg) > 0:

        for letter in range(0, len(Msg)-3):

            quadgram = Msg[letter:letter+4]

            if quadgram in QuadgramFreq:

                QuadgramFreq[quadgram] += 1

            else:

                QuadgramFreq[quadgram] = 1

        QuadgramFreq = list(reversed(sorted(QuadgramFreq.items(), key = lambda a: a[1])))

        return QuadgramFreq

    else:

        QuadgramFreq = [["Nothing", "There's No Text!"]]

        return QuadgramFreq



def QuintgramFrequencies(Msg):

    QuintgramFreq = {}

    #Msg = RemovePunctuation(RemoveSpaces(Msg))
    Msg = RemoveSpaces(Msg.lower())

    if len(Msg) > 0:

        for letter in range(0, len(Msg)-4):

            quintgram = Msg[letter:letter+5]

            if quintgram in QuintgramFreq:

                QuintgramFreq[quintgram] += 1

            else:

                QuintgramFreq[quintgram] = 1

        QuintgramFreq = list(reversed(sorted(QuintgramFreq.items(), key = lambda a: a[1])))

        return QuintgramFreq

    else:

        QuintgramFreq = [["Nothing", "There's No Text!"]]

        return QuintgramFreq


def IndexOfCoincidence(Msg, LetterFreq):

    if LetterFreq == None:

        LetterFreq = LetterFrequencies(Msg)

    Total = 0

    for x in LetterFreq:

        Total += x[1]

    if Total > 0:

        ###IOC - Index Of Coincidence
        IOC = 0

        for x in LetterFreq:

            if Total**2-Total != 0:

                IOC += (x[1]**2-x[1])/(Total**2-Total)

            else:

                IOC += 0

        return IOC

    else:

        #return "There's No Text!"
        return -1


def AnalyseText(Msg):

    DashNum = 20

    Report = ""

    """LetterFreq = {}

    for letter in Msg:

        if letter in LetterFreq:

            LetterFreq[letter] += 1

        elif letter in alphabetset:

            LetterFreq[letter] = 1

    #LetterFreq = sorted(LetterFreq, key = lambda a: LetterFreq[a])
    LetterFreq = list(reversed(sorted(LetterFreq.items(), key = lambda a: a[1])))"""

    Msg = Msg.strip("\n")

    LetterFreq = LetterFrequencies(Msg)

    Report = Report + "Letter Frequencies:\n\n"

    for x in LetterFreq:

        #Report = Report + "Letter Frequencies:\n\n"

        #Report = Report + x[0] + " - " + str(x[1]) + "\n"
        #Report = Report + x[0] + " - " + str(x[1]) + "\t" + str("%.1f" % x[2]) + "\t" + str(averageletterfrequencies[x[0]]) + "\n"
        #Report = Report + x[0] + " - " + str(x[1]) + "\t"*2 + str("%.3f" % x[2]) + "\t" + str(averageletterfrequencies[x[0]]) + "\n"
        #Report = Report + x[0] + " - " + str(x[1]) + "\t"*2 + str("%.3f" % x[2]) + "\t" + str("%.3f" % averageletterfrequencies[x[0]]/100) + "\n"
        #Report = Report + x[0] + " - " + str(x[1]) + "\t"*2 + str("%.3f" % x[2]) + "\t" + str("%.3f" % (averageletterfrequencies[x[0]]/100)) + "\n"
        Report = Report + x[0] + " - " + str(x[1]) + "\t"*2 + str("%.1f" % (x[2]*100)) + "\t" + str("%.1f" % averageletterfrequencies[x[0]]) + "\n"

        #Report = Report + "-"*DashNum + "\n\n"

    #Report = Report + "-"*DashNum + "\n\n"
        #Report = Report + "\n" + "-"*DashNum + "\n\n"
    Report = Report + "\n" + "-"*DashNum + "\n\n"



    #TabLength = 20
    TabLength = 30

    """WordFreq = {}

    #print (RemovePunctuation(Msg).split(" "))

    for word in RemovePunctuation(Msg).split(" "):

        if word in WordFreq:

            WordFreq[word] += 1

        else:

            WordFreq[word] = 1

    WordFreq = list(reversed(sorted(WordFreq.items(), key = lambda a: a[1])))"""

    WordFreq = WordFrequencies(Msg)

    Report = Report + "Word Frequencies:\n\n"

    for x in WordFreq:

        #Report = Report + x[0] + " - " + str(x[1]) + "\n"
        #Report = Report + x[0] + " - " + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"
        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"

    

    IOC = IndexOfCoincidence(Msg, LetterFreq)

    Report = Report + "Index Of Coincidence:\n\n"

    Report = Report + str(IOC) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"


    TrigramFreq = TrigramFrequencies(Msg)

    Report = Report + "Trigram Frequencies: " + str(len(TrigramFreq)) + "\n\n"

    for x in TrigramFreq:

        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"


    BigramFreq = BigramFrequencies(Msg)

    Report = Report + "Bigram Frequencies: " + str(len(BigramFreq)) + "\n\n"

    for x in BigramFreq:

        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"



    QuadgramFreq = QuadgramFrequencies(Msg)

    Report = Report + "Quadgram Frequencies: " + str(len(QuadgramFreq)) + "\n\n"

    for x in QuadgramFreq:

        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"


    QuintgramFreq = QuintgramFrequencies(Msg)

    Report = Report + "Quintgram Frequencies: " + str(len(QuintgramFreq)) + "\n\n"

    for x in QuintgramFreq:

        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"



    IOCThreshold = 0.05

    """Report = Report + "Summary:\n\n"

    Report = Report + "Most Common Letter: " + str(LetterFreq[0][0]) + "\n"
    
    Report = Report + "Most Common Word: " + str(WordFreq[0][0]) + "\n"

    Report = Report + "Most Common Trigram: " + str(TrigramFreq[0][0]) + "\n"

    Report = Report + "Most Common Bigram: " + str(BigramFreq[0][0]) + "\n"
    
    Report = Report + "Most Common Quadgram: " + str(QuadgramFreq[0][0]) + "\n"

    Report = Report + "Most Common Quintgram: " + str(QuintgramFreq[0][0]) + "\n"

    Report = Report + "Index Of Coincidence: " + str(IOC) + "\n"

    if isinstance(IOC, float):

        if IOC < IOCThreshold:

            Report = Report + "It is probably a more complicated cipher.\n"

        else:

            Report = Report + "It is probably a less complicated cipher.\n"

    else:

        Report = Report + "Put some text in!\n"

    #FactorisedLength = Factorise(len(RemovePunctuation(RemoveSpaces(Msg))))
    #FactorisedLength = Factorise(len(RemovePunctuation(RemoveSpaces(Msg))))[0]
    FactorisedLength = Factorise(len(RemovePunctuation(RemoveSpaces(Msg))))[1]

    Report = Report + "Length Of Text: " + str(len(RemovePunctuation(RemoveSpaces(Msg)))) + "\n"

    #Report = Report + "Factors Of Length: "
    Report = Report + "Factorised Length: "

    for factor in range(0, len(FactorisedLength)):

        if factor == len(FactorisedLength)-1:

            #Report = Report + FactorisedLength[factor]
            Report = Report + str(FactorisedLength[factor])

        else:

            #Report = Report + FactorisedLength[factor] + "x"
            Report = Report + str(FactorisedLength[factor]) + "x"

    Report = Report + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"
    """


    #IOCThreshold = 0.05
    IOCThreshold = 0.06

    Summary = ""

    Summary = Summary + "Summary:\n\n"

    Summary = Summary + "Most Common Letter: " + str(LetterFreq[0][0]) + "\n"

    if " " in Msg:
        
        Summary = Summary + "Most Common Word: " + str(WordFreq[0][0]) + "\n"

    else:

        Summary = Summary + "Most Common Word: Cannot identify any words\n"

    Summary = Summary + "Most Common Trigram: " + str(TrigramFreq[0][0]) + "\n"

    Summary = Summary + "Most Common Bigram: " + str(BigramFreq[0][0]) + "\n"
    
    Summary = Summary + "Most Common Quadgram: " + str(QuadgramFreq[0][0]) + "\n"

    Summary = Summary + "Most Common Quintgram: " + str(QuintgramFreq[0][0]) + "\n"

    Summary = Summary + "Index Of Coincidence: " + str(IOC) + "\n"

    if isinstance(IOC, float):

        if IOC < IOCThreshold:

            Summary = Summary + "It is probably a more complicated cipher.\n"

        else:

            Summary = Summary + "It is probably a less complicated cipher.\n"

    else:

        Summary = Summary + "Put some text in!\n"

    lengthOfText = len(RemoveSpaces(Msg.rstrip("\n")))

    #FactorisedLength = Factorise(len(RemovePunctuation(RemoveSpaces(Msg))))[1]
    #FactorisedLength = Factorise(len(RemoveSpaces(Msg)))[1]
    if lengthOfText < 50000:
        
        FactorisedLength = Factorise(lengthOfText)[1]

    else:

        FactorisedLength = [lengthOfText]

    #Summary = Summary + "Length Of Text: " + str(len(RemovePunctuation(RemoveSpaces(Msg)))) + "\n"
    #Summary = Summary + "Length Of Text: " + str(len(RemoveSpaces(Msg))) + "\n"
    #Summary = Summary + "Length Of Text: " + str(len(RemoveSpaces(Msg.rstrip("\n")))) + "\n"
    
    Summary = Summary + "Length Of Text: " + str(lengthOfText) + "\n"

    Summary = Summary + "Factorised Length: "

    for factor in range(0, len(FactorisedLength)):

        if factor == len(FactorisedLength)-1:

            Summary = Summary + str(FactorisedLength[factor])

        else:

            Summary = Summary + str(FactorisedLength[factor]) + "x"

    Summary = Summary + "\n"

    Summary = Summary + "\n" + "-"*DashNum + "\n\n"



    Report = Summary + Report

    return Report



def AttemptToSolve(Msg, Key, Dictionary):

    #NewKey = Key

    Msg = RemovePunctuation(RemoveSpaces(Msg))

    MinLength = 7

    for trial in range(0, 26):
    #for trial in range(0, 1000):
    #for trial in range(0, 1):

        NewKey = copy.deepcopy(Key)

        UsedLetters = set()

        for x in NewKey:

            if NewKey[x] != "":

                #UsedLetters.add(x)
                UsedLetters.add(NewKey[x])

        for x in NewKey:

            #print (UsedLetters)

            if NewKey[x] == "":

                index = random.randint(0, 25)

                while alphabetlist[index] in UsedLetters:

                    index = random.randint(0, 25)

                NewKey[x] = alphabetlist[index]

                UsedLetters.add(NewKey[x])

        Msg = Decode(Msg, NewKey)

        English = ContainsEnglish(Msg, Dictionary, MinLength)

        if English[0]:

            print (English[1])

            return NewKey

    print ("No Luck")

    #return Key
    return None



#def CalculateCaesarShift(Key):

    #KeyedValues = []

    #for x in Key:

        #if Key[x] != "" and len(KeyedValues) < 2::

            #KeyedValues.append(x, Key[x])


def CalculateCaesarShift(Key):

    Shifts = []

    for x in Key:

        if Key[x] != "":

            Shifts.append(alphabetlist.index(Key[x])-alphabetlist.index(x))

    if len(Shifts) > 0:

        for y in range(0, len(Shifts)):

            while Shifts[y] < 0:

                Shifts[y] += 26

        #print (Shifts)

        MatchShift = Shifts[0]

        for a in Shifts:

            if a != MatchShift:

                return (False, MatchShift)

        return (True, MatchShift)

    else:

        #return None
        return (False, 0)


def CreateCaesarKey(Key, Shift):

    for x in Key:

        #Key[x] = alphabetlist[(alphabetlist.index(x)+Shift-1)%26+1]

        Key[x] = alphabetlist[(alphabetlist.index(x)+Shift)%26]

    return Key





ModularMultiplicativeInverses = {1: 1,
                                 3: 9,
                                 5: 21,
                                 7: 15,
                                 9: 3,
                                 11: 19,
                                 15: 7,
                                 17: 23,
                                 19: 11,
                                 21: 5,
                                 23: 17,
                                 25: 25}

def CalculateAffineShift(Key):

    Shifts = []

    for x in Key:

        if Key[x] != "" and len(Shifts) < 2:

            Shifts.append([alphabetlist.index(x), alphabetlist.index(Key[x])])
            #Shifts.append([alphabet[x], alphabetlist[Key[x]]])
            #Shifts.append([alphabet[x], alphabet[Key[x]]])


    if len (Shifts) == 2:

        #Multiplier = Shifts[0][0] - Shifts[1][1]
        Multiplier = Shifts[0][0] - Shifts[1][0]

        Result = Shifts[0][1] - Shifts[1][1]

        if Multiplier < 0:

            Multiplier, Result = -Multiplier, -Result

        while Result < 0:

            Result += 26

        if Multiplier in ModularMultiplicativeInverses:

            #pass
            a = Result*ModularMultiplicativeInverses[Multiplier]

            while a > 26:

                a -= 26

            b = Shifts[0][1]-Shifts[0][0]*a

            while b < 0:

                b += 26

            while b > 25:

                b -= 26

            return (True, a, b)
            #return (True, a, b)

        else:

            #return (False, 1, 0)
            return (False, Multiplier, 0)

    else:

        #return (False, None)
        #(False, 1, 0)
        return (False, 1, 0)


def CreateAffineKey(Key, ShiftA, ShiftB):

    UsedLetters = set()

    for x in Key:

        #Key[x] = alphabetlist[(alphabetlist.index(x)*ShiftA+ShiftB)%26]

        newvalue = alphabetlist[(alphabetlist.index(x)*ShiftA+ShiftB)%26]

        if True:
        #if False:

            if Key[x] != "":

                if Key[x] != newvalue:

                    return (False)

        Key[x] = newvalue

        UsedLetters.add(newvalue)

    if True:
    #if False:

        if len(UsedLetters) < 26:

            return False

    return Key


def Transpose(Msg, RowOrColumn, Length):
    """"""
    if RowOrColumn != "Switch Columns":
    #if RowOrColumn != "Switch Columns" or True:

        #Msg = RemovePunctuation(RemoveSpaces(Msg)).lower()
        #Msg = RemoveSpaces(Msg).lower()
        #Msg = RemoveSpaces(Msg).rstrip("\n")
        Msg = RemoveSpaces(Msg).replace("\n","")

    NewMsg = ""

    if len(Msg) > 0:

        if RowOrColumn == "Column":
        #if RowOrColumn == "Row":

            Columns = Length
            #Rows = Length

            Rows = ceil(len(Msg)/Columns)
            #Columns = ceil(len(Msg)/Rows)

            for row in range(0, Rows):

                for column in range(0, Columns):

                    index = column*Rows+row

                    if index < len(Msg):

                        NewMsg = NewMsg + Msg[index]

                NewMsg = NewMsg + "\n"
                

        #elif RowsOrColumn == "Row":
        elif RowOrColumn == "Row":
        #elif RowOrColumn == "Column":

            """Rows = Length
            #Columns = Length

            #Columns = floor(Rows//Length)

            Columns = ceil(len(Msg)/Rows)"""
            #Rows = ceil(len(Msg)/Columns)

            Columns = Length

            Rows = ceil(len(Msg)/Columns)

            for row in range(0, Rows):

                for column in range(0, Columns):

                    index = column+Columns*row

                    if index < len(Msg):

                        NewMsg = NewMsg + Msg[index]

                NewMsg = NewMsg + "\n"


        elif RowOrColumn == "Switch Columns":

            Msg = Msg.split("\n")

            for line in range(0, len(Msg)):

                #Msg[line] = RemovePunctuation(RemoveSpaces(Msg[line])).lower()
                #Msg[line] = RemoveSpaces(Msg[line]).rstrip("\n")
                Msg[line] = RemoveSpaces(Msg[line]).replace("\n","")

            #ColumnOrder = Msg.split(",")
            ColumnOrder = Length.split(",")

            for integer in range(0, len(ColumnOrder)):

                ColumnOrder[integer] = int(ColumnOrder[integer])

            NewMsg = ""

            for line in Msg:

                #print (line)

                NewLine = ""

                for column in ColumnOrder:

                    #if column < len(line):
                    if column <= len(line):

                        #NewLine = NewLine + line[column]
                        NewLine = NewLine + line[column-1]

                #print (NewLine)
                    
                NewMsg = NewMsg + NewLine + "\n"
        

    return NewMsg


def AnalyseColumns(Msg):

    DashNum = 20

    Msg = RemovePunctuation(RemoveSpaces(Msg))
    #Msg = RemoveSpaces(Msg)

    Report = ""

    #Report = Report + "\n" + "-"*DashNum + "\n\n"

    #Report = Report + "Key Word Length:\n"
    #Report = Report + "Key Word Length:\n\n"
    #Report = Report + "Key Word Length:\tIOC:\n\n"
    #Report = Report + "Key Word Length:    IOC\n\n"
    Report = Report + "Key Word Length:  IOC\n\n"

    #for columnsnum in range(0, 10):
    #for columnsnum in range(1, 10):
    #for rowsnum in range(0, 10):
    #for rowsnum in range(1, 10):

    if len(Msg) > 0:
        
        #for rowsnum in range(1, 21):
        for rowsnum in range(1, 31):

            columnsnum = ceil(len(Msg)/rowsnum)

            #TranposedMsg = Transpose(Msg, Columns, columnsnum)
            #TranposedMsg = Transpose(Msg, Columns, columnsnum).split("\n")
            #TranposedMsg = Transpose(Msg, "Column", columnsnum).split("\n")
            TransposedMsg = Transpose(Msg, "Column", columnsnum).split("\n")

            #print (rowsnum, columnsnum, TransposedMsg)

            IOC = 0

            #LinesNum += 1
            LinesNum = 0

            for line in TransposedMsg:

                if line != "" and line != " " and line != "\n":

                    #print (IndexOfCoincidence(TransposedMsg, None))

                    #IOC += IndexOfCoincidence(TransposedMsg, None)
                    IOC += IndexOfCoincidence(line, None)
                    #IOC += CharIOC(line)

                    LinesNum += 1

            #IOC /= len(TransposedMsg)
            IOC /= LinesNum

            #print (IOC)

            #if columnsnum == 1:

                #Report = Report + "Key Word Length

            #Report = Report + str(columnsnum) + ": \t" + str(IOC) + "\n"
            Report = Report + str(rowsnum) + ": \t" + str(IOC) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"

    return Report



def SolveSubstitutionCipher(Msg, Key, Dictionary):

    NewMsg = ""

    Msg = RemovePunctuation(RemoveSpaces(Msg))

    #EnglishScore = 0
    EngScore = 0

    #OldKey = Key
    #OldEngScore = EnglishScore(Msg, Dictionary)

    BestKey = Key
    BestEngScore = EnglishScore(Msg, Dictionary)

    #while EnglishScore < 100:
    #while EngScore < 100:
    #for x in range(0, 1000):
    for x in range(0, 100000):

        if x % 100 == 0:

            print (x)

        CurrentKey = Key

        for letter in Key:

            CurrentKey[letter] = alphabetlist[random.randint(0, 25)]

        #EnglishScore = EnglishScore(Msg, Dictionary)
        #EngScore = EnglishScore(Msg, Dictionary)
        EngScore = EnglishScore(Decode(Msg, Key), Dictionary)

        #if EngScore > OldEngScore:
        if EngScore > BestEngScore:

            BestKey = CurrentKey

    #return NewMsg, Key
    #return Key
    return BestKey



def EnglishScore(Msg, Dictionary):

    Msg = RemoveSpaces(RemovePunctuation(Msg))

    Score = 0

    MinLength = 4

    Word = ""

    for letter in Msg:

        Word = Word+letter

        if Word not in Dictionary:

            Word = Word[:-1]

            if len(Word) >= MinLength:

                Score += 1

                word = letter

    LetterFreq = LetterFrequencies(Msg)

    Score2 = 0

    for letter in LetterFreq:

        Score2 += (letter[1]/len(Msg))/(averageletterfrequencies[letter[0]])
        

    FinalScore = Score + Score2

    return FinalScore



def Reverse(Msg):

    #return str(reversed(list(Msg)))
    #return str(list(reversed(list(Msg))))

    ReversedMsg = ""

    for letter in Msg:

        ReversedMsg = letter + ReversedMsg

    return ReversedMsg



#def FindPotentialWords(Input):
#def FindPotentialWords(Input, DictionaryType):
def FindPotentialWords(Input, Dictionary):

    Input = Input.lower()

    #print (Input)
    
    Words = []

    #Dictionary = set(open("Large Dictionary.txt", "r").readlines())
    #Dictionary = open("Large Dictionary.txt", "r").readlines()
    #Dictionary = open("Full Dictionary.txt", "r").lower().readlines()
    #Dictionary = open("Full Dictionary.txt", "r").readlines()
    """Dictionary = open(DictionaryType + " Dictionary.txt", "r").readlines()

    #for word in Dictionary:
    for word in range(0, len(Dictionary)):

        #Dictionary[word] = RemovePunctuation(Dictioary[word])
        #Dictionary[word] = RemovePunctuation(Dictioary[word]).lower()
        Dictionary[word] = RemovePunctuation(Dictionary[word]).lower()

    Dictionary = set(Dictionary)"""

    for word in Dictionary:

        #NumberSubstitutions = {}

        if len(word) == len(Input):
            #
            NumberSubstitutions = {}

            Match = True

            for letter in range(0, len(word)):

                if Match:

                    if Input[letter] == "-":

                        pass

                    elif Input[letter] in digitset:
                    #elif False and Input[letter] in digitset:

                        #print (NumberSubstitutions)

                        #if letter in NumberSubstitutions:
                        if Input[letter] in NumberSubstitutions:

                            #print (NumberSubstitutions)

                            #if NumberSubstitutions[letter] != word[letter]:
                            if NumberSubstitutions[Input[letter]] != word[letter]:

                                Match = False

                        else:

                            #NumberSubstitutions[letter] = word[letter]
                            NumberSubstitutions[Input[letter]] = word[letter]

                    #elif Match and word[letter] != Input[letter]:
                    elif word[letter] != Input[letter]:

                        Match = False

                #elif Input[letter] in NumberList:
                """elif Input[letter] in digitset:

                    if letter in NumberSubstitutions:

                        if NumberSubstitutions[letter] != word[letter]:

                            Match = False

                    else:

                        NumberSubstitutions[letter] = word[letter]"""

                        

            if Match:

                Words.append(word)

    return Words


            
def DecodePolyalphabet(Msg, Keys):

    NewMsg = ""

    KeyNum = len(Keys)

    CurrentKey = 0

    for letter in Msg:

        DecodedLetter = GetKeyOfValue(letter, Keys[CurrentKey])

        if DecodedLetter != None:

            NewMsg = NewMsg + DecodedLetter

        else:

            NewMsg = NewMsg + letter.upper()

        CurrentKey = (CurrentKey+1)%KeyNum

    return NewMsg



def EncodePolyalphabet(Msg, Keys):

    NewMsg = ""

    KeyNum = len(Keys)

    CurrentKey = 0

    for letter in Msg:

        if letter in Keys[CurrentKey] and Keys[CurrentKey][letter] != "":

            NewMsg = NewMsg + Keys[CurrentKey][letter]

        else:

            NewMsg = NewMsg + letter.upper()

        CurrentKey = (CurrentKey+1)%KeyNum

    return NewMsg



def CipherSpecificAnalyse(Msg):

    Report = ""

    Report = Report + AnalyseColumns(Msg)

    Report = Report + AnalyseTypeOfCipher(Msg)

    return Report



def AnalyseTypeOfCipher(Msg):

    #OriginalMsg = copy.deepcopy(Msg)
    #OriginalMsg = copy.deepcopy(Msg[:-2])
    #OriginalMsg = copy.deepcopy(Msg[:-4])
    OriginalMsg = copy.deepcopy(Msg[:-1])

    DashNum = 20

    Msg = RemovePunctuation(RemoveSpaces(Msg))
    #Msg = Msg.lower()

    Report = ""

    IOCThreshold = 0.055

    IOC = IndexOfCoincidence(Msg, LetterFrequencies(Msg))

    if isinstance(IOC, int) or isinstance(IOC, float):

        Report = Report + "IOC is " + str(IOC) + ": "

    #if IOC != None:
    #if isinstance(IOC, int) or isinstance(IOC, float):

        if IOC >= IOCThreshold:

            Report = Report + "Perhaps a simpler cipher"

        else:

            Report = Report + "Perhaps a more complex cipher"

    else:

        Report = Report + "Unable to calculate IOC."

    Report = Report + "\n\n"


    VigenereIOCThreshold = 0.06

    #if IsVigenere(Msg):
    if IsVigenere(Msg, VigenereIOCThreshold):

        #Report = Report + "Regular IOC > 0.06; Perhaps a Vigenère cipher\n\n"
        Report = Report + "Regular IOC > " + str(VigenereIOCThreshold) + "; Perhaps a Vigenère cipher\n\n"



    Playfair = True

    for letter in range(0, len(Msg)):

        #if letter != len(Msg)-1:
        if letter % 2 == 0 and letter != len(Msg)-1:

            if Msg[letter] == Msg[letter+1]:

                Playfair = False

    if Playfair:

        #Report = Report += "No same-letter digrams: Perhaps a Playfair cipher"
        #Report = Report + "No same-letter digrams: Perhaps a Playfair cipher"
        Report = Report + "No same-letter digrams: Perhaps a Playfair cipher\n\n"


    #print (OriginalMsg)

    Polybius = True

    #for letter in range(0, len(Msg)):
    for letter in range(0, len(OriginalMsg)):

        #if Msg[letter] not in digitset:
        if OriginalMsg[letter] not in digitset:

            Polybius = False

    #if len(Msg)%2 == 1:
    if len(OriginalMsg)%2 == 1:
    #if len(OriginalMsg)%2 == 0:

        Polybius = False

    if Polybius:

        Report = Report + "Only digits; Even length: Perhaps a Polybius cipher\n\n"

    #Report = Report + "\n" + "-"*DashNum + "\n\n"
    Report = Report + "-"*DashNum + "\n\n"



    TabLength = 30
    
    """PlayfairFreq = NGramFrequencies(Msg, 2)

    Report = Report + "Playfair:\n\n"

    for x in PlayfairFreq:

        Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"
    """


    DigraphFreq = NGramFrequencies(Msg, 2)

    Report = Report + "Digraph:\n\n"

    DigraphIOC = IndexOfCoincidence(Msg, DigraphFreq)

    Report = Report + "IOC: " + str(DigraphIOC) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"

    #if True:
    if False:

        FIle = open("Digraph Frequencies File.txt", "w")

        UsedDigraphs = set()

        for x in DigraphFreq:

            #FIle.write(x[0])
            FIle.write(x[0]+"\n")

            #UsedDigraphs.add(x)
            UsedDigraphs.add(x[0])

        for x in range(0, 26):

            for y in range(0, 26):

                d = alphabetlist[x]+alphabetlist[y]

                if d not in UsedDigraphs:

                    FIle.write(d+"\n")

        FIle.close()

    

    PlayfairFreq = NGramFrequencies(Msg, 2)

    Report = Report + "Playfair:\n\n"

    for x in PlayfairFreq:

        #Report = Report + x[0] + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"
        #Report = Report + str(x[0]) + " "*(TabLength-len(str(x[1])+x[0])) + str(x[1]) + "\n"
        Report = Report + str(x[0]) + " "*(TabLength-len(str(x[1])+str(x[0]))) + str(x[1]) + "\n"

    Report = Report + "\n" + "-"*DashNum + "\n\n"
    

    return Report



#def AttemptToSolve2(Msg, Key, Dictionary):
def AttemptToSolve2(Msg, Key, Dictionary, SolveMethod):

    #if WordSpacingRetained(Msg):
    if SolveMethod == "Words":

        """print ("Spaces Reatined!")"""

        #NewMsg, Key = SolveSubstitutionSpacesRetained(Msg, Key)
        NewMsg, NewKey = SolveSubstitutionSpacesRetained(Msg, Key)

    #else:
    elif SolveMethod == "Quadgrams":

        """print ("Speaces Not Reetteened!")"""

        #NewMsg = SolveSubstitutionAnnealing(Msg)
        #NewMsg = SolveSubstitutionSpacesNotRetained(Msg, Key)
        #Msg, Key = SolveSubstitutionSpacesNotRetained(Msg, Key)
        NewMsg, NewKey = SolveSubstitutionSpacesNotRetained(Msg, Key)

    elif SolveMethod == "Fast Bigrams":

        NewMsg, NewKey = SolveMonoSubFastBigrams(Msg)

    else:

        assert ValueError("Invalid solve method: " + str(SolveMethod))

    #return Key
    return NewKey


def SolveSubstitutionSpacesRetained(Msg, Key):

    LetterFreq = LetterFrequencies(Msg)

    DictionaryType = "Large"

    Dictionary = open(DictionaryType + " Dictionary.txt", "r").readlines()

    for word in range(0, len(Dictionary)):

        Dictionary[word] = RemovePunctuation(Dictionary[word]).lower()

    Dictionary = set(Dictionary)

    Msg = RemovePunctuation(Msg)

    #print(LetterFrequencies(Msg))

    #Key = AddToKey(Key, "e",  LetterFrequencies(Msg)[0])
    Key = AddToKey(Key, "e",  LetterFrequencies(Msg)[0][0])

    Msg = Msg.split(" ")

    for word in Msg:

        if len(word) == 3 and word[-1] == Key["e"]:

            Key = AddToKey(Key, "t", word[0])
            Key = AddToKey(Key, "h", word[1])

    for word in Msg:

        if len(word) == 2 and word[0] == Key["t"]:

            Key = AddToKey(Key, "o", word[1])

    for word in Msg:

        if len(word) == 4 and word[0] == Key["t"] and word[1] == Key["h"] and word[3] == Key["t"]:

            Key = AddToKey(Key, "a", word[2])

    for word in Msg:

        if len(word) == 3 and word[0] == Key["a"]:

            Key = AddToKey(Key, "n", word[1])
            Key = AddToKey(Key, "d", word[2])

    #print (Key)

    trial = -1

    SolvedALetter = True

    #for trial in range(0, 26):
    #for trial in range(0, 3):
    #for trial in range(0, 5):
    while not KeySolved(Key, LetterFreq) and SolvedALetter:

        SolvedALetter = False

        trial += 1

        #print (Key)

        print ("Trial: " + str(trial+1))

        for word in Msg:

            DigitToAdd = 0

            UsedDigits = {}

            newword = ""

            for letter in word:

                Found = False

                for character in Key:

                    if Key[character] == letter:

                        Found = True

                        newword = newword + character

                if Found == False:

                    if True:
        
                        newword = newword + "-"
    

                    if False:

                        if letter in UsedDigits:

                            #newword = newword + UsedDigits[letter]
                            newword = newword + str(UsedDigits[letter])

                        else:

                            newword = newword + str(DigitToAdd)

                            #UsedDigits[DigitToAdd] = letter
                            UsedDigits[letter] = DigitToAdd

                            DigitToAdd += 1

                else:

                    pass

            try:    

                #PotentialWords = FindPotentialWords(newword, "Large")
                #PotentialWords = FindPotentialWords(newword, "Medium")
                PotentialWords = FindPotentialWords(newword, Dictionary)

                if len(PotentialWords) == 1:

                    for letter in range(0, len(PotentialWords[0])):

                        Key[PotentialWords[0][letter]] = word[letter]

            except:

                pass

            else:

                NewPotentialWords = []

                for word2 in PotentialWords:

                    for letter in range(0, len(word)):

                        #if newword[letter] == "_" and Key[word2[letter]] == "":
                        #if newword[letter] in digitset and Key[word2[letter]] == "":
                        if (newword[letter] in digitset or newword[letter] == "-") and Key[word2[letter]] == "":

                            NewPotentialWords.append(word2)

                if len(NewPotentialWords) == 1:

                    for letter in range(0, len(NewPotentialWords[0])):

                        Key[NewPotentialWords[0][letter]] = word[letter]

                        SolvedALetter = True


    return Msg, Key
    #return Key


def AddToKey(Key, Index, Value):

    #if Key[Index] != "":
    if Key[Index] == "":

        Key[Index] = Value

    return Key


def WordSpacingRetained(Msg):

    OldWordLength = None

    CurrentWordLength = 0

    for letter in Msg:

        if letter != " ":

            CurrentWordLength += 1

        else:

            if OldWordLength != None and CurrentWordLength != OldWordLength:

                return True

            OldWordLength = CurrentWordLength

            CurrentWordLength = 0

    return False


def KeySolved(Key, LetterFreq):

    UnsolvedLetters = []

    for letter in alphabetlist:

        Unsolved = True

        for item in Key:

            if Key[item] == letter:

                Unsolved = False

        if Unsolved:

            UnsolvedLetters.append(letter)
            

    for letter in UnsolvedLetters:

        for letterfreq in LetterFreq:

        #if LetterFreq[letter] != 0:

            #return False

            if letterfreq[0] == letter:

                if letterfreq[0] != 0:

                    return False

    return True


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

    #for letter in List:
    #for letter.lower() in List:
    for character in List:

        if CountUpTo == "All" or NumberOfQuadgrams < CountUpTo:

            letter = character.lower()
            
            #CurrentQuadgram = ""

            #CurrentNumber = ""

            if letter in alphabetset:

                if CurrentNumber != "":

                    if ListOrDictionary == "List":

                        FrequencyTable.append([CurrentQuadgram, int(CurrentNumber)])

                    elif ListOrDictionary == "Dictionary":

                        FrequencyTable[CurrentQuadgram] = int(CurrentNumber)

                    TotalFreq += int(CurrentNumber)

                    #CurrentQuadgram, CurrentNumber = "", ""
                    CurrentQuadgram, CurrentNumber = letter, ""

                    NumberOfQuadgrams += 1

                else:

                    CurrentQuadgram = CurrentQuadgram + letter

            elif letter == " ":

                pass

            elif letter in digitset:

                CurrentNumber = CurrentNumber + letter

            #print(CurrentQuadgram, CurrentLetter)
            #print(CurrentQuadgram, CurrentNumber)


    for item in FrequencyTable:

        if ListOrDictionary == "List":

            item[1] = item[1]*100/TotalFreq

            #FrequencyTable.insert(0, TotalFreq)

        if ListOrDictionary == "Dictionary":

            FrequencyTable[item] = FrequencyTable[item]*100/TotalFreq

            #FrequencyTable["Total"] = TotalFreq

    if ListOrDictionary == "List":

        FrequencyTable.insert(0, TotalFreq)

    if ListOrDictionary == "Dictionary":

        FrequencyTable["Total"] = TotalFreq

    return FrequencyTable


def SolveSubstitutionSpacesNotRetained(Msg, Key):

    return SolveSubstitutionAnnealing(Msg, Key)


def SolveSubstitutionAnnealing(Msg, Key):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #Msg = RemoveSpaces(RemovePunctuationSpaces)

    #QuadgramFrequencies = LoadQuadgramFrequencyTable("List")
    #QuadgramFrequencies = LoadQuadgramFrequencyTable("List", 20)
    #QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", 20)
    #QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary")
    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    Key = CreateOrderedFrequencyKey(Msg)

    #print (Key)

    #print (QuadgramFrequencies)

    #print (QuadgramScore(Msg, QuadgramFrequencies))

    NewKey = copy.deepcopy(Key)

    #BestScore = QuadgramScore(Decode(Msg, NewKey))
    BestScore = QuadgramScore(Decode(Msg, NewKey), QuadgramFrequencies)

    BestKey = copy.deepcopy(NewKey)

    #NumOfTrials = 10000
    NumOfTrials = 1000

    #for trial in range(0, 1000):
    #for trial in range(0, 10000):
    for trial in range(0, NumOfTrials):

        #SwapLetter1 = random.randint(0, 25)
        SwapIndex1 = random.randint(0, 25)

        #SwapLetter2 = SwapLetter1
        SwapIndex2 = SwapIndex1

        #while SwapLetter2 == SwapLetter1:
        while SwapIndex2 == SwapIndex1:

            #SwapLetter2 = random.randint(0, 25)
            SwapIndex2 = random.randint(0, 25)

        #SwapLetter1 = alphabetlist[SwapLetter1]
        SwapLetter1 = alphabetlist[SwapIndex1]

        #SwapLetter2 = alphabetlist[SwapLetter2]
        SwapLetter2 = alphabetlist[SwapIndex2]

        #SwapEncodeAs1 = NewKey[alphabetlist[SwapLetter1]]
        SwapEncodeAs1 = NewKey[alphabetlist[SwapIndex1]]

        #SwapEncodeAs2 = NewKey[alphabetlist[SwapLetter2]]
        SwapEncodeAs2 = NewKey[alphabetlist[SwapIndex2]]

        NewKey[SwapLetter1] = SwapEncodeAs2

        NewKey[SwapLetter2] = SwapEncodeAs1

        #NewScore = QuadgramScore(Decode(Msg, NewKey))
        NewScore = QuadgramScore(Decode(Msg, NewKey), QuadgramFrequencies)

        if NewScore > BestScore:

            BestScore = NewScore

            BestKey = copy.deepcopy(NewKey)
            #BestKey = NewKey

        else:

            #Probability = e**((NewScore-BestScore)/trial)
            Probability = e**((NewScore-BestScore)/(NumOfTrials-trial))

            NewKey = copy.deepcopy(BestKey)
            #NewKey = BestKey

    print (Decode(Msg, BestKey))

    #return NewKey
    #return BestKey
    #return Msg, BestKey
    return Decode(Msg, BestKey), BestKey



def QuadgramScore(Msg, QuadgramFrequencies):

    NumberOfQuadgrams = len(QuadgramFrequencies)

    #Msg = RemoveSpaces(RemovePunctuation(Msg))
    Msg = RemoveSpaces(Msg)

    Score = 0

    Quadgram = ""

    #UsedQuadgrams = set()

    #for letter in Msg:
    for letter in range(0, len(Msg)):

        '''Quadgram += letter

        if len(Quadgram) == 4:

            if Quadgram in QuadgramFrequencies and Quadgram not in UsedQuadgrams:

                #Score += log(QuadgramFrequencies[Quadgram])
                #Score += log(QuadgramFrequencies[Quadgram])/NumberOfQuadgrams
                #Score += log(QuadgramFrequencies[Quadgram]/NumberOfQuadgrams)

                #Score += log(QuadgramFrequencies[Quadgram]/QuadgramFrequencies["Total"])
                #Score += log(QuadgramFrequencies[Quadgram]/100)
                Score += log10(QuadgramFrequencies[Quadgram]/100)
                #
                UsedQuadgrams.add(Quadgram)

        #if len(Quadgram) == 4:

            Quadgram = ""'''
            
        if letter <= len(Msg)-4:

            Quadgram = Msg[letter:letter+4]

            #if Quadgram in QuadgramFrequencies and Quadgram not in UsedQuadgrams:
            if Quadgram in QuadgramFrequencies:

                Score += log10(QuadgramFrequencies[Quadgram]/100)
                #
                #UsedQuadgrams.add(Quadgram)

            else:

                Score += log10(0.01/QuadgramFrequencies["Total"])

    return Score


def CreateOrderedFrequencyKey(Msg):

    Key = {}

    LetterFreq = LetterFrequencies(Msg)

    for letter in range(0, len(LetterFreq)):

        #Key[orderlettersonaveragefrequency[letter]] = LetterFreq[letter][0]
        #Key[orderedlettersonaveragefrequency[letter]] = LetterFreq[letter][0]
        Key[orderedlettersonaveragefrequencies[letter]] = LetterFreq[letter][0]

    return Key


#def MakePolybiusSubstitution(Msg):
def MakePolybiusSubstitution(Msg, NgramLength):

    #Msg = RemoveSpaces(Msg)
    Msg = NumsToLetter(Msg)
    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))

    Digrams = {}

    CurrentLetter = 0

    NewMsg = ""

    for letter in range(0, len(Msg)-1):

        #if letter%2 == 0:
        if letter%NgramLength == 0:

            #Digram = Msg[letter:letter+1]
            #Digram = Msg[letter:letter+2]
            Digram = Msg[letter:letter+NgramLength]

            if Digram in Digrams:

                NewMsg = NewMsg + Digrams[Digram]

            else:

                if CurrentLetter <= 25:

                    Digrams[Digram] = alphabetlist[CurrentLetter]

                    NewMsg = NewMsg + Digrams[Digram]

                    CurrentLetter += 1

    return NewMsg



class Key():

    def __init__(self):

        self.name = ""


class PlayfairKey(Key):

    def __init__(self, Width, Height, StartingUsedLetter):

        Key.__init__(self)

        self.width, self.height = Width, Height

        #self.SetUpGrid(StartingUsedLetter)
        self.SetUpGrid(Width, Height, StartingUsedLetter)

        #print (self.GetKey())

    #def SetUpGrid(self, StartingUsedLetter):
    def SetUpGrid(self, Width, Height, StartingUsedLetter):

        self.key = {}

        CurrentLetter = 0

        UsedLetters = set()

        UsedLetters.add(StartingUsedLetter)

        for x in range(0, Width):

            for y in range(0, Height):

                #if CurrentLetter < 26:
                if CurrentLetter < 25:

                    Index = random.randint(0, 25)

                    while alphabetlist[Index] in UsedLetters:

                    #self.key[x, y] = alphabetlist[CurrentLetter]

                        Index = random.randint(0, 25)

                    self.key[x, y] = alphabetlist[Index]

                    UsedLetters.add(alphabetlist[Index])

                    CurrentLetter += 1

                else:

                    self.key[x, y] = ""

    def GetKey(self):

        return self.key

    def GetKeyItem(self, x, y):

        return self.key[x, y]

    def SetKeyItem(self, x, y, Value):

        self.key[x, y] = Value

    def GetWidth(self):

        return self.width

    def GetHeight(self):

        return self.height

    def SwapKeyItems(self, x1, y1, x2, y2):

        #Temp = self.key[x1, y1]
        Temp = self.GetKeyItem(x1, y1)

        #self.key[x1, y1] = self.key[x2, y2]

        #self.key[x2, y2] = Temp

        self.SetKeyItem(x1, y1, self.GetKeyItem(x2, y2))

        self.SetKeyItem(x2, y2, Temp)

    def DisplayKey(self):

        for y in range(0, self.GetHeight()):

            for x in range(0, self.GetWidth()):

                print (" " + self.GetKeyItem(x, y), end = "")

            print ("\n", end = "")


    #def DecodeMsg(self, Msg):
    def DecodeMsg(self, Msg, DecodeAsDoubleLetter):

        OriginalMsg = Msg

        Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

        NewMsg = ""

        Digrams = SplitIntoDigrams(Msg)

        for digram in Digrams:

            #print (digram)

            #if Stuff:

            #    pass

            #if digram[1] == "X":
            #if digram[1] == "x":
            if DecodeAsDoubleLetter and digram[1] == "x":

                NewMsg = NewMsg + digram[0]*2

            #"""
            #elif not DecodeAsDoubleLetter and digram[1] == "x":
#
            #    NewMsg = NewMsg + digram[0] + "x"
            #"""

            else:

                Letter1Coords = self.FindLetterInKey(digram[0])
                Letter2Coords = self.FindLetterInKey(digram[1])

                if Letter1Coords != False and Letter2Coords != False:

                    if Letter1Coords[1] == Letter2Coords[1]:

                        NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter1Coords[0]-1, 0, self.GetWidth()-1), Letter1Coords[1])
                        NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter2Coords[0]-1, 0, self.GetWidth()-1), Letter2Coords[1])

                    elif Letter1Coords[0] == Letter2Coords[0]:

                        NewMsg = NewMsg + self.GetKeyItem(Letter1Coords[0], CycleRound(Letter1Coords[1]-1, 0, self.GetHeight()-1))
                        NewMsg = NewMsg + self.GetKeyItem(Letter2Coords[0], CycleRound(Letter2Coords[1]-1, 0, self.GetHeight()-1))

                    else:

                        NewMsg = NewMsg + self.GetKeyItem(Letter2Coords[0], Letter1Coords[1])
                        NewMsg = NewMsg + self.GetKeyItem(Letter1Coords[0], Letter2Coords[1])

        #return NewMsg
        return CombineWithOriginalMsg(NewMsg, OriginalMsg)

            


    def EncodeMsg(self, Msg):

        OriginalMsg = Msg

        #Msg = RemoveSpaces(RemovePunctuation(Msg))
        Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

        NewMsg = ""

        Digrams = SplitIntoDigrams(Msg)

        #print (Digrams)

        for digram in Digrams:

            if digram[0] == digram[1]:

                NewMsg = NewMsg + digram[random.randint(0, 1)] + "x"

            else:

                #Coords1 = FindLetterInKey(digram[0])
                #Letter1Coords = FindLetterInKey(digram[0])
                #Letter2Coords = FindLetterInKey(digram[1])
                Letter1Coords = self.FindLetterInKey(digram[0])
                Letter2Coords = self.FindLetterInKey(digram[1])

            #print (Letter1Coords, Letter2Coords)

            #if Letter1Coords != False and Letter2Coords != False:
                if Letter1Coords != False and Letter2Coords != False:

                    #if Letter1Coords[0] == Letter2Coords[0]:
                    if Letter1Coords[1] == Letter2Coords[1]:

                        #print (CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()-1), Letter1Coords[1])
                        #print (self.GetKeyItem(CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()-1), Letter1Coords[1]))
                        #print (self.Key[CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()-1), Letter1Coords[1]])
                        #print (self.key[CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()-1), Letter1Coords[1]])

                        #NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()), Letter1Coords[1])
                        #NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter2Coords[0]+1, 0, self.GetWidth()), Letter2Coords[1])
                        NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter1Coords[0]+1, 0, self.GetWidth()-1), Letter1Coords[1])
                        NewMsg = NewMsg + self.GetKeyItem(CycleRound(Letter2Coords[0]+1, 0, self.GetWidth()-1), Letter2Coords[1])

                    #if Letter1Coords[1] == Letter2Coords[1]:
                    #elif Letter1Coords[1] == Letter2Coords[1]:
                    elif Letter1Coords[0] == Letter2Coords[0]:

                        #N#ewMsg = NewMsg + self.GetKeyItem(Letter1Coords[0], CycleRound(Letter1Coords[1]+1, 0, self.GetHeight()))
                        #NewMsg = NewMsg + self.GetKeyItem(Letter2Coords[0], CycleRound(Letter2Coords[1]+1, 0, self.GetHeight()))
                        NewMsg = NewMsg + self.GetKeyItem(Letter1Coords[0], CycleRound(Letter1Coords[1]+1, 0, self.GetHeight()-1))
                        NewMsg = NewMsg + self.GetKeyItem(Letter2Coords[0], CycleRound(Letter2Coords[1]+1, 0, self.GetHeight()-1))

                    else:

                        NewMsg = NewMsg + self.GetKeyItem(Letter2Coords[0], Letter1Coords[1])
                        NewMsg = NewMsg + self.GetKeyItem(Letter1Coords[0], Letter2Coords[1])

        #return Msg
        #return NewMsg
        return CombineWithOriginalMsg(NewMsg, OriginalMsg)


    def FindLetterInKey(self, Letter):

        #for x in range(0, self.GetWidth());:
        for x in range(0, self.GetWidth()):

            for y in range(0, self.GetHeight()):

                if self.GetKeyItem(x, y) == Letter:

                    return (x, y)

        return False


    def ReverseKey(self):

        self.newkey = {}

        for x in reversed(range(0, self.GetWidth())):

            for y in reversed(range(0, self.GetHeight())):

                #newx, newy = self.GetWidth-1-x, self.GetHeight-1-y
                newx, newy = self.GetWidth()-1-x, self.GetHeight()-1-y

                self.newkey[newx, newy] = self.GetKeyItem(x, y)

        self.SetKey(self.newkey)


    def SetKey(self, NewKey):

        self.key = copy.deepcopy(NewKey)
        #self.key = NewKey


    #def SwapRow(
    #def SwapRows(Row1, Row2):
    def SwapRows(self, Row1, Row2):

        TempRow = []

        for x in range(0, self.GetWidth()):

            TempRow.append(self.GetKeyItem(x, Row2))

        for x in range(0, self.GetWidth()):

            #self.SetKeyItem(x, Row2, self.GetKeyItem(x, Row1)))
            self.SetKeyItem(x, Row2, self.GetKeyItem(x, Row1))

            #self.SetKeyItem(x, Row1, TempRow[x]))
            self.SetKeyItem(x, Row1, TempRow[x])

    #def SwapColumnss(Column1, Column2):
    def SwapColumns(self, Column1, Column2):

        TempColumn = []

        for y in range(0, self.GetHeight()):

            TempColumn.append(self.GetKeyItem(Column2, y))

        for y in range(0, self.GetHeight()):

            #self.SetKeyItem(Column2, y, self.GetKeyItem(Column1, y)))
            self.SetKeyItem(Column2, y, self.GetKeyItem(Column1, y))

            #self.SetKeyItem(Column1, y, TempColumn[x]))
            self.SetKeyItem(Column1, y, TempColumn[x])

    def ReverseColumn(self, x):

        newcolumn = []

        for y in range(0, self.GetHeight()):

            newcolumn.insert(0, self.GetKeyItem(x, y))

        for y in range(0, self.GetHeight()):

            self.SetKeyItem(x, y, newcolumn[y])


    #def ReverseRow(self, x):
    def ReverseRow(self, y):

        newrow = []

        for x in range(0, self.GetWidth()):

            newrow.insert(0, self.GetKeyItem(x, y))

        for x in range(0, self.GetWidth()):

            self.SetKeyItem(x, y, newrow[x])


    def ShuffleKey(self):

        for swap in range(0, random.randint(0, 30)):

            Index1 = (random.randint(0, self.GetWidth()-1), random.randint(0, self.GetHeight()-1))

            Index2 = (random.randint(0, self.GetWidth()-1), random.randint(0, self.GetHeight()-1))

            while Index1 == Index2:

                Index2 = (random.randint(0, self.GetWidth()-1), random.randint(0, self.GetHeight()-1))

            self.SwapKeyItems(Index1[0], Index1[1], Index2[0], Index2[1])



#def SolvePlayfair(Msg, Key):
#def SolvePlayfair(Msg, Key, DecodeAsDoubleLetter):
#def SolvePlayfair(Msg, Key, DecodeAsDoubleLetter, Temperature, Step):
#def SolvePlayfair(Msg, Key, DecodeAsDoubleLetter, Temperature, Step, Count):
def PlayfairAnnealing(Msg, Key, DecodeAsDoubleLetter, Temperature, Step, Count, QuadgramFreq):

    #NewKey = PlayfairKey(5, 5, "j")

    #print (NewKey)

    #NewKey.DisplayKey()

    #QuadgramFreq = LoadQuadgramFrequencies()

    #BestKey = copy.deepcopy(NewKey())
    #BestKey = copy.deepcopy(NewKey)
    BestKey = copy.deepcopy(Key)

    BestScore = QuadgramScore(BestKey.DecodeMsg(Msg, DecodeAsDoubleLetter), QuadgramFreq)

    OverallBestScore = copy.deepcopy(BestScore)

    OverallBestKey = copy.deepcopy(BestKey)

    #print(OverallBestScore)

    while Temperature >= 0:

        """BestKey = copy.deepcopy(OverallBestKey)
        #BestKey = OverallBestKey.ShuffleKey()
        #BestKey = OverallBestKey.ShuffleKey()
        #BestKey = OverallBestKey
        BestKey.ShuffleKey()"""

        #BestScore = copy.deepcopy(OverallBestScore)
        """BestScore = QuadgramScore(BestKey.DecodeMsg(Msg, DecodeAsDoubleLetter), QuadgramFreq)"""

        #for trial in range(0, 1000):
        for trial in range(0, Count):

            CurrentKey = copy.deepcopy(BestKey)
            #CurrentKey = BestKey

            #print (CurrentKey)
            #CurrentKey.DisplayKey()
            #print ()

            #Index1 = (random.randint(0, Key.GetWidth()), random.randint(0, self.Key.GetHeight()))

            #Index2 = (random.randint(0, Key.GetWidth()), random.randint(0, self.Key.GetHeight()))
            #Index1 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, self.CurrentKey.GetHeight()))

            #Index2 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, self.CurrentKey.GetHeight()))
            #Index1 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, CurrentKey.GetHeight()))

            #Index2 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, CurrentKey.GetHeight()))
            Index1 = (random.randint(0, CurrentKey.GetWidth()-1), random.randint(0, CurrentKey.GetHeight()-1))

            Index2 = (random.randint(0, CurrentKey.GetWidth()-1), random.randint(0, CurrentKey.GetHeight()-1))

            while Index1 == Index2:

                #Index2 = (random.randint(0, Key.GetWidth()), random.randint(0, self.Key.GetHeight()))
                #Index2 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, self.CurrentKey.GetHeight()))
                #Index2 = (random.randint(0, CurrentKey.GetWidth()), random.randint(0, CurrentKey.GetHeight()))
                Index2 = (random.randint(0, CurrentKey.GetWidth()-1), random.randint(0, CurrentKey.GetHeight()-1))

            #CurrentKey.SwapItem(Index1[0], Index1[1], Index2[0], Index2[1])

            ### Swap Letter, Reverse Column, Reverse Row, Reverse Key, Swap Rows, Swap Columns, Shuffle Key
            #Manoeuvre = weightedrand((90, 3, 3, 4))
            #Manoeuvre = weightedrand((86, 3, 3, 2, 3, 3))
            Manoeuvre = weightedrand((88, 2, 2, 2, 2, 2, 2))

            if Manoeuvre == 0:
                
                CurrentKey.SwapKeyItems(Index1[0], Index1[1], Index2[0], Index2[1])

            elif Manoeuvre == 1:

                #CurrentKey.SwapColumn(Index1[0])
                CurrentKey.ReverseColumn(Index1[0])

            elif Manoeuvre == 2:

                #CurrentKey.SwapRow(Index1[1])
                CurrentKey.ReverseRow(Index1[1])

            elif Manoeuvre == 3:

                CurrentKey.ReverseKey()

            elif Manoeuvre == 4:

                Row1 = Index1[1]

                Row2 = Index2[1]

                while Row1 == Row2:

                    #Row2 = random.randint(0, CurrentKey.GetHeight())
                    Row2 = random.randint(0, CurrentKey.GetHeight()-1)

                CurrentKey.SwapRows(Row1, Row2)

            elif Manoeuvre == 5:

                Column1 = Index1[0]

                Column2 = Index2[0]

                while Column1 == Column2:

                    #Column2 = random.randint(0, CurrentKey.GetWidth())
                    Column2 = random.randint(0, CurrentKey.GetWidth()-1)

                CurrentKey.SwapRows(Column1, Column2)

            elif Manoeuvre == 6:

                CurrentKey.ShuffleKey()

            #print (CurrentKey)
            #CurrentKey.DisplayKey()
            #print ()

            CurrentScore = QuadgramScore(CurrentKey.DecodeMsg(Msg, DecodeAsDoubleLetter), QuadgramFreq)

            if CurrentScore > BestScore:

                #print ("New Key!")
                #print ("Better Key!")
                #print ()

                BestKey = copy.deepcopy(CurrentKey)
                #BestKey = CurrentKey

                BestScore = copy.deepcopy(CurrentScore)
                #BestScore = CurrentScore

            elif CurrentScore < BestScore:

                Probability = AnnealingProbability(CurrentScore - BestScore, Temperature)

                #if AnnealingProbability(Probability):
                if AnnealingProbabilityResult(Probability):

                    BestKey = copy.deepcopy(CurrentKey)
                    #BestKey = CurrentKey

                    BestScore = copy.deepcopy(CurrentScore)
                    #BestScore = CurrentScore


        Temperature -= Step

        """if BestScore > OverallBestScore:

            #OverallBest = BestScore
            #OverallBest = copy.deepcopy(BestScore)
            OverallBestScore = copy.deepcopy(BestScore)

            OverallBestKey = copy.deepcopy(BestKey)
            #OverallBestKey = BestKey"""

        """#print ("Best Score: " + int(BestScore))
        print ("Temperature: " + str(Temperature))
        #print ("Best Score: " + str(BestScore))
        print ("Best Score: " + str(OverallBestScore))
        print ("Best Key:")
        #BestKey.DisplayKey()
        OverallBestKey.DisplayKey()
        print()
        #print (BestKey,DecodeMsg(Msg))
        #print (BestKey.DecodeMsg(Msg))
        #print (BestKey.DecodeMsg(Msg, DecodeAsDoubleLetter))
        print (OverallBestKey.DecodeMsg(Msg, DecodeAsDoubleLetter))
        print("\n")"""

            

    #return NewKey
    #return BestKey
    #return OverallBestKey
    return BestScore, BestKey


#def SplitIntoDigrams():
def SplitIntoDigrams(Msg):

    Digrams = []

    for letter in range(0, len(Msg)):

        if letter % 2 == 0:

            if letter < len(Msg)-1:

                Digrams.append(Msg[letter:letter+2])

    if len(Msg)%2 == 1:

        Digrams.append(Msg[-1])

    return Digrams


#def CycleNumber(Num, Min, Max):
def CycleRound(Num, Min, Max):

    if Min > Max:

        Temp = Max

        Max = Min

        Min = Temp

    while Num > Max:

        Num -= Max-Min+1

    while Num < Min:

        Num += Max-Min+1

    return Num


def AnnealingProbability(Difference, Temperature):

    return e**(Difference/Temperature)


def AnnealingProbabilityResult(AnnealingProbability):

    Num = random.randint(0, 1000000)/1000000

    return AnnealingProbability >= Num


"""def SolvePlayfair(Msg, Key, DecodeAsDoubleLetter, Temperature, Step, Count):

    BestKey = PlayfairKey(5, 5, "j")

    QuadgramFreq = LoadQuadgramFrequencyTable("Dictionary", "All")

    HighestScore = QuadgramScore(BestKey.DecodeMsg(Msg, DecodeAsDoubleLetter), QuadgramFreq)

    for iteration in range(0, 20):

        NewScore, NewKey = PlayfairAnnealing(Msg, BestKey, DecodeAsDoubleLetter, Temperature, Step, Count, QuadgramFreq)

        if NewScore > HighestScore:

            HighestScore = copy.deepcopy(NewScore)

            BestKey = copy.deepcopy(NewKey)

            print ("New Best Key!")
            print ("Found On Iteration:", iteration)
            print ("Best Score:", HighestScore)
            BestKey.DisplayKey()
            print (BestKey.DecodeMsg(Msg, DecodeAsDoubleLetter))
            print ("\n")

    return BestKey"""


def SolvePlayfair(Msg, Key, DecodeAsDoubleLetter, Temperature, Step, Count):

    #print ("Let's Call!")

    Msg = RemoveSpaces(RemovePunctuation(Msg))

    #File = open("Playfair Msg To Crack.txt", "w")
    File = open("Playfair Message.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call([Temperature, Step, Count, DecodeAsDoubleLetter, Msg])
    #subprocess.call(["a.exe", Temperature, Step, Count, DecodeAsDoubleLetter, Msg])
    #//subprocess.call(["a.exe", str(Temperature), str(Step), str(Count), str(DecodeAsDoubleLetter), str(Msg)])
    #subprocess.call(["PlayfairSolver.exe", str(Temperature), str(Step), str(Count), str(DecodeAsDoubleLetter)])
    subprocess.Popen(["PlayfairCreaker.exe", str(Temperature), str(Step), str(Count), str(DecodeAsDoubleLetter)])

    return


def WritePlayfairCrackFile(Msg, Key, DecodeAsDoubleLetter, Temperature, Step, Count):

    Msg = RemoveSpaces(RemovePunctuation(Msg))

    File = open("Playfair Msg To Crack.txt", "w")

    File.write(str(Temperature) + "\n" + str(Step) + "\n" + str(Count) + "\n" + str(DecodeAsDoubleLetter) + "\n" + Msg)

    File.close()


def SplitIntoWords(Msg, DictionaryType):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    Dictionary = open(DictionaryType + " Dictionary.txt", "r").readlines()

    for word in range(0, len(Dictionary)):

        Dictionary[word] = RemovePunctuation(Dictionary[word]).lower()

    #Dictionary = set(Dictionary)

    item = 0

    while item < len(Dictionary):

        if len(Dictionary[item]) == 1:

            del Dictionary[item]

        else:

            item += 1

    Dictionary = set(Dictionary)

    CurrentWord = ""

    SplitMsg = ""

    LastWordStop = 0

    letter = 0

    MaxLength = 10
    MaxLength = 15

    #for letter in Msg:
    #for letter in range(0, len(Msg)):
##    while letter < len(Msg):
##
##        #print (letter, CurrentWord)
##
##        #CurrentWord = CurrentWord + letter
##        CurrentWord = CurrentWord + Msg[letter]
##
##        #LastWordStop = 0
##
##        #if CurrentWord not in Dictionary:
##        if CurrentWord in Dictionary:
##
##            #print ("Yes!")
##
##            #SplitMsg = SplitMsg + CurrentWord[:-1] + " "
##            #SplitMsg = SplitMsg + CurrentWord[:] + " "
##            SplitMsg = SplitMsg + CurrentWord + " "
##
##            #CurrentWord = letter
##            #CurrentWord = Msg[letter]
##            CurrentWord = ""
##
##            #LastWordStop = letter
##            LastWordStop = letter+1
##
##        elif len(CurrentWord) > MaxLength:
##
##            #print ("Too Long!")
##
##            CurrentWord = ""
##
##            SplitMsg = SplitMsg + Msg[LastWordStop]
##
##            LastWordStop += 1
##
##            #letter = LastWordStop
##            letter = LastWordStop-1
##
##        letter += 1


    #for letter in range(0, len(Msg)-MaxLength):
##    letter = 0
##
##    while letter < len(Msg):
##
##        LongestWord = ""
##
##        for extra in range(0, MaxLength):
##
##            if letter+extra+1 < len(Msg):
##
##                CurrentWord = Msg[letter:letter+extra+1]
##
##                #print (CurrentWord)
##
##                if CurrentWord in Dictionary:
##
##                    LongestWord = CurrentWord
##
##        if LongestWord != "":
##
##            #SplitMsg += LongestWord
##            #SplitMsg = SplitMsg + LongestWord
##            SplitMsg = SplitMsg + LongestWord + " "
##
##            letter += len(LongestWord)
##
##        else:
##
##            SplitMsg = SplitMsg + Msg[letter]
##
##            letter += 1

    letter = len(Msg)-1

    while letter >= 0:

        LongestWord = ""

        for extra in range(0, MaxLength):

            if letter - extra >= 0:

                CurrentWord = Msg[letter - extra:letter + 1]

                if CurrentWord in Dictionary:

                    LongestWord = CurrentWord

        if LongestWord != "":

            SplitMsg = LongestWord + " " + SplitMsg

            letter -= len(LongestWord)

        else:

            #SplitMsg = SplitMsg + Msg[letter]
            SplitMsg = Msg[letter] + SplitMsg

            letter -= 1

    SplitMsg = SplitMsg.strip()
            

    #return SplitWord
    return SplitMsg


def Factorise(Num):

    Factors = {}
    FactorList = []

    #//for testfactor in range(0, floor(sqrt(Num))):
    #for testfactor in range(2, floor(sqrt(Num))):
    for testfactor in range(2, floor(sqrt(Num)) + 1):

        while Num%testfactor == 0:

            #print (testfactor)

            if testfactor in Factors:

                #factors[testfactor] += 1
                Factors[testfactor] += 1

            else:

                Factors[testfactor] = 1

            FactorList.append(testfactor)

            #Num//testfactor
            Num //= testfactor

    #print (FactorList)

    if Num > 1:

        if Num in Factors:

            Factors[Num] += 1

        else:

            Factors[Num] = 1

        #FactorList.append(testfactor)
        FactorList.append(Num)

    return (Factors, FactorList)


def Solve2x2Hill(Msg, Key):

    #Msg = RemoveSpaces(RemovePunctuation(Msg))
    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("2x2 Hill Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["2x2HillCreaker.exe"])

    return



#def Cofactors(Num1, Num2):
def Cofactor(Num1, Num2):

    Cofactor = 1

    Factors1 = Factorise(Num1)[1]
    Factors2 = Factorise(Num2)[1]

    #for x in range(0, len(

    #Length = min(len(Factors1), len(Factors2))

    #x = 0
    factor = 0

    #while factor < Length:
    while factor < len(Factors1):

        if Factors1[factor] in Factors2:

            Cofactor *= Factors1[factor]

            del Factors2[Factors2.index(Factors1[factor])]
            del Factors1[factor]
            #del Factors2[Factors2.index(Factors1[factor])]

        else:

            factor += 1

    return Cofactor



def ModularSimultaneous(x1, y1, c1, x2, y2, c2):

    Solutions = ["None", "None"]

    #X1, Y1, C1 = x1, y1, c1
    #X2, Y2, C2 = x2, y2, c2
    X1, Y1, C1 = x1%26, y1%26, c1%26
    X2, Y2, C2 = x2%26, y2%26, c2%26

    #### x1 * x + y1 * y = c1 (mod26)
    #### x2 * x  +  y2 * y  =  c2 (mod 26)

    #print (Cofactor(60, 13))
    #print (Cofactor(60, 28))
    #print (Cofactor(1020, 20))

    Cofactor1 = x2/Cofactor(x1, x2)
    #x1, y1, c1 *= Cofactor1
    x1 *= Cofactor1
    y1 *= Cofactor1
    c1 *= Cofactor1

    Cofactor2 = x1/Cofactor(x1, x2)
    #x2, y2, c2 *= Cofactor2
    x2 *= Cofactor2
    y2 *= Cofactor2
    c2 *= Cofactor2

    """DisplayModEquations(x1, y1, c1, x2, y2, c2)"""

    x3 = 0
    y3 = y1-y2
    c3 = c1-c2

    if y3 < 0:

        y3 *= -1
        c3 *= -1

    y3 %= 26
    c3 %= 26

    """DisplayModEquation(x3, y3, c3)"""

    y3Prime = Mod26Inverse(y3)

    if y3Prime == 0:

        Solutions[1] = "None"

        #return False

    else:
        
        c3 *= y3Prime

        
        c3 %= 26

        """DisplayModEquation(0, "", c3)"""

        Solutions[1] = c3

        x1 = X1
        y1 = Y1
        c1 = C1

        y1 *= Solutions[1]
        c1 -= y1
        c1 %= 26

        """DisplayModEquation(x1, 0, c1)"""

        x1Prime = Mod26Inverse(x1)

        if x1Prime == 0:

            #Solutions[0] = "None"

            x2 = X2
            y2 = Y2
            c2 = C2

            y2 *= Solutions[1]
            c2 -= y2
            c2 %= 26

            """DisplayModEquation(x2, 0, c2)"""

            x2Prime = Mod26Inverse(x2)

            if x2Prime == 0:

                Solutions[0] = "None"

            else:

                #c2 *= x1Prime
                c2 *= x2Prime
                c2 %= 26

                """DisplayModEquation("", 0, c2)"""

                Solutions[0] = c2

        else:

            c1 *= x1Prime
            c1 %= 26

            """sDisplayModEquation("", 0, c1)"""

            Solutions[0] = c1

    return Solutions



def DisplayModEquations(x1, y1, c1, x2, y2, c2):

    print (str(x1)+"x + " + str(y1) + "y = " + str(c1) + " (mod 26)")
    print (str(x2)+"x + " + str(y2) + "y = " + str(c2) + " (mod 26)")
    print ()


#def DisplayModEquations(x3, y3, c3):
def DisplayModEquation(x3, y3, c3):

    #print (str(x1)+"x + " + str(y1) + "y = " + str(c1) + " (mod 26)")
    #print (str(x2)+"x + " + str(y2) + "y = " + str(c2) + " (mod 26)")
    print (str(x3)+"x + " + str(y3) + "y = " + str(c3) + " (mod 26)")


def Mod26Inverse(Num):

    ModularMultiplicativeInverses = {1: 1,
                                     3: 9,
                                     5: 21,
                                     7: 15,
                                     9: 3,
                                     11: 19,
                                     15: 7,
                                     17: 23,
                                     19: 11,
                                     21: 5,
                                     23: 17,
                                     25: 25}

    if Num in ModularMultiplicativeInverses:

        return ModularMultiplicativeInverses[Num]

    else:

        return 0


def AddAffineKey(Key, a, b):

    #b -= 1

    for letter in Key:

        #index = alphabet[letter]
        index = alphabet[letter]-1

        newindex = (a*index + b)%26

        #newletter = alphabet[newindex]
        newletter = alphabet[newindex+1]

        Key[letter] = newletter

    return Key
    

def ModularLinear(a, b, c):

    a %= 26
    b %= 26
    c %= 26

    c -= b
    c %= 26

    AModInverse = Mod26Inverse(a)

    if AModInverse != 0:

        c *= AModInverse
        c %= 26

        return c

    else:

        #return "None"
        return None


def MakeNihilistSubstitution(Msg):

    #Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    Digrams = SplitIntoDigrams(Msg)

    #print (Digrams)

    #AssignedLetters = set()
    AssignedLetters = {}

    CurrentAssignLetter = 0

    NewMsg = ""

    for digram in Digrams:

        if len(digram) == 2 and digram[0] in digitset and digram[1] in digitset:

            #a = digram[0]
            #b = digram[1]
             #a = digram[0]
            a = int(digram[0])
            b = int(digram[1])

            while b > 5:

                b -= 5

                a += 1

            #if b <= 0:
            while b <= 0:
                b +=5

            while a > 5:

                a -= 5

            #if a <= 0:
            while a <= 0:

                a += 5

            Index = 5*(a-1)+b

            """if Index in AssignedLetters:

                NewMsg = NewMsg + AssignedLetters[Index]

            else:

                AssignedLetters[Index] = alphabetlist[CurrentAssignLetter]

                CurrentAssignLetter += 1

                NewMsg = NewMsg + AssignedLetters[Index]"""

            NewMsg = NewMsg + alphabet[Index]

    return NewMsg



def CreateVigenereFreqKey(Msg, KeyWordLength, KeyWordLetter):

    LetterFreqs = LetterFrequencies(Msg, KeyWordLength, KeyWordLetter)

    Key = {}

    #for letter in range(0, len(LetterFresq)):
    for letter in range(0, len(LetterFreqs)):

        Key[orderedlettersonaveragefrequencies[letter]] = LetterFreqs[letter][0]

    return Key


def SolveVigenere(Msg, Key, KeyWordLength):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("Vigenere Message.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["VigenereCreaker.exe", KeyWordLength])
    subprocess.Popen(["VigenereCreaker.exe", str(KeyWordLength)])

    return


#def MakeBifidSubstitution(Msg):
def MakeBifidSubstitution(Msg, Period):

    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))

    LetterSubs = {}

    ### abc
    a = 1

    b = 1

    c = 1

    NewMsg = ""

    Row1 = ""

    Row2 = ""

    Group = ""

    NewGroup = ""

    """for letter in Msg:

        if letter in LetterSubs:

            NewMsg = NewMsg + LetterSubs[letter]

        else:

            #LetterSubs[letter] = str(a) + str(b) + str(c)
            LetterSubs[letter] = str(b) + str(c)

            c += 1

            if c > 5:

                c = 1

                b += 1

            if b > 5:

                b = 1

                a += 1

            NewMsg = NewMsg + LetterSubs[letter]"""

    for letter in range(0, len(Msg)):

        if letter%Period == 0:

            Group = Msg[letter:rounddownto(letter+Period, len(Msg))]

            NewGroup = ""

            for item in Group:

                if item in LetterSubs:

                    NewGroup = NewGroup + LetterSubs[item]

                else:

                    LetterSubs[item] = str(b) + str(c)

                    c += 1

                    if c > 5:

                        c = 1

                        b += 1

                    NewGroup = NewGroup + LetterSubs[item]

            #Row1 = Row1 + NewGroup[:len(NewGroup)//2]
            #Row2 = Row2 + NewGroup[len(NewGroup)//2:]
            #for x in range(0, Period):
            for x in range(0, len(NewGroup)//2):

                Row1 = Row1 + NewGroup[x]
                #Row2 = Row2 + NewGroup[Period+x]
                Row2 = Row2 + NewGroup[len(NewGroup)//2+x]

    #NewMsg = Row1+Row2
    NewMsg = Row1+"\n"+Row2

    #print (NewMsg)

    """NewMsg = NumsToLetter(NewMsg)

    #print (NewMsg)

    #NewMsg = Transpose(NewMsg, "Columns", 2)
    NewMsg = Transpose(NewMsg, "Column", 2)

    #print (NewMsg)

    #return NewMsg

    NewNewMsg = ""

    #print (NewMsg)

    #NewMsg = NewMsg.split(" ")
    NewMsg = NewMsg.split("\n")

    #print (NewMsg)

    for pair in NewMsg:

        newpair = LettersToNum(pair)

        #NewNewMsg = NewNewMsg + NumSubs

        for sub in LetterSubs:

            #print (newpair, sub)

            #if sub == newpair:
            if LetterSubs[sub] == newpair:

                NewNewMsg = NewNewMsg + sub
    
    return NewNewMsg"""

    return NewMsg

def NumsToLetter(Msg):

    NewMsg = ""

    for letter in Msg:

        if letter in digitset:

            #NewMsg = NewMsg + alphabetlist[int(Msg)]
            NewMsg = NewMsg + alphabetlist[int(letter)]

        else:

            NewMsg = NewMsg + letter

    return NewMsg


#def NumsToLetter(Msg):
def LettersToNum(Msg):

    NewMsg = ""

    for letter in Msg:

        if letter in alphabetset:

            NewMsg = NewMsg + str(alphabet[letter]-1)

        else:

            #NewMsg = NewMsg + letetr
            NewMsg = NewMsg + letter

    return NewMsg


#def TranspositionDistance(Msg):
def TranspositionDistance(Msg, MinimumDistance):

    #Msg = RemovesSpaces(RemovePunctuation(Msg.lower()))
    #Msg = RemovesSpace(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    Distances = {}

    #for letter in range(0, Msg):
    for letter in range(0, len(Msg)):

        if Msg[letter] == "t":

            for otherletter in range(0, len(Msg)):

                if Msg[otherletter] == "h":

                    Distance = abs(letter-otherletter)

                    if Distance >= MinimumDistance:

                        if Distance in Distances:

                            Distances[Distance] += 1

                        else:

                            #Distances[Distance] = 0
                            Distances[Distance] = 1

    #print (Distances)

    LargestDistance = Distances[Distance]

    LargestDistanceItem = Distance

    for x in Distances:

        if Distances[x] > LargestDistance:

            LargestDistance = Distances[x]

            LargestDistanceItem = x

    return LargestDistanceItem


#def SolveTransposition(Msg, RowOrColumn, Length):
"""def CrackTransposition(Msg, RowOrColumn, Length):

    #Msg = TransposeMsg(RemoveSpaces(RemovePunctuation(Msg.lower())), RowOrColumn, Length)
    Msg = Transpose(RemoveSpaces(RemovePunctuation(Msg.lower())), RowOrColumn, Length)

    Columns = []

    for x in range(0, Length):

        Columns.append(1)

    #print (Columns)
        

    #BestScore = 0
    BestScore = None

    BestStr = ""

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    print ("Checking:", Length**Length)

    #for x in range(0, factorial(Length)):
    for x in range(0, Length**Length):

        if len(set(Columns)) == Length:

            NewMsg = Msg

            Str = ""

            for y in Columns:

                Str = Str + str(y) + ","

            Str = Str[:-1]

            #Msg = TransposeMsg(Msg, "Switch Columns", Str)
            #Msg = Transpose(Msg, "Switch Columns", Str)
            NewMsg = Transpose(NewMsg, "Switch Columns", Str)

            #Score = QuadgramScore(Msg, QuadgramFrequencies)
            Score = QuadgramScore(NewMsg, QuadgramFrequencies)

            #if Score > BestScore:
            #if Score > BestScore or BestScore == None:
            if BestScore == None or Score > BestScore:

                BestScore = Score

                BestStr = Str

                print (Str, x)#

            #Columns[-1] += 1

            #for y in range(0, len(Columns)):
            #for y in range(0, len(Columns)-1):
            """#for y in range(0, len(Columns)-2):
#
            #    if Columns[Length-1-y] > Length:
#
            #        Columns[Length-1-y] = 1
#
            #        Columns[Length-2-y] += 1
##
            #print (Columns)"""
"""
        Columns[-1] += 1

        #for y in range(0, len(Columns)-2):
        for y in range(0, len(Columns)-1):

            if Columns[Length-1-y] > Length:

                Columns[Length-1-y] = 1

                Columns[Length-2-y] += 1

        #print (Columns)

    #print (Columns)

    print ("Checked All!")

    return BestStr"""
    
        

def SolveTransposition(Msg):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower()).replace("\n","")

    File = open("Transposition Message.txt", "w")
    #File = open("--TranspositionMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["TranspositionCreaker.exe"])
    subprocess.Popen(["TranspositionCreaker.exe"])
    #subprocess.Popen(["SolveTransposition.exe"])

    return


def SolveTranspositionCSharp(Msg):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    #Msg = RemoveSpaces(Msg.lower())
    Msg = RemoveSpaces(Msg.lower()).replace("\n","")

    #File = open("Transposition Message.txt", "w")
    File = open("C# Programs\\--TranspositionMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["TranspositionCreaker.exe"])
    subprocess.Popen(["C# Programs\\SolveTransposition.exe"], cwd = "C# Programs")

    return





letters = {'A':'1','B':'2','C':'3','D':'4','E':'5','F':'6','G':'7','H':'8','I':'9','J':'10','K':'11','L':'12','M':'13','N':'14','O':'15','P':'16','Q':'17','R':'18','S':'19','T':'20','U':'21','V':'22','W':'23','X':'24','Y':'25','Z':'26'}
#numbers =  {v: k for k, v in letters.iteritems()}
#numbers =  {v: k for k, v in letters.keys()}
#numbers =  {v: k for k in letters.keys()}
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

        #new_letter = (text[n]-temp_key[n])%26
        #new_letter = alphabet[(alphabet[text[n]]-alphabet[temp_key[n]])%26]
        #new_letter = alphabet[(alphabet[text[n]]-temp_key[n])%26]
        new_letter = alphabetlist[(alphabet[text[n]]-temp_key[n])%26]
    
        #plaintext = plaintext + numbers[str(new_letter+1)]
        plaintext = plaintext + new_letter

        #temp_key.append(new_letter)
        temp_key.append(alphabet[new_letter]-1)

        n += 1

        #print(key)
    
    temp_key = key
    return plaintext

def IncrementKey(key,layer):

    #print(layer)
    
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



#def SolveAutoKey(cipher_text):
def SolveAutoKey2(cipher_text):

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    cipher_text = RemoveSpaces(RemovePunctuation(cipher_text))

    #cipher_text = Numberify(raw_input("Enter Cipher Text: "))

    key = [1,0,0,0,0,0,0]
    closest_score = -10000000000

    while key != [0,0,0,0,0,0,0]:

        key_score = Decrypt(cipher_text,key)
        #print(key_score)
        key_score = QuadgramScore(key_score,QuadgramFrequencies)
        #print(key_score)
        
        #sleep(0.5)
        
        if key_score > closest_score:

            closest_score = key_score
            print("New best: " + str(key))
            print(key_score)

        key = key[0:7]
        
        key = IncrementKey(key,0)

        #print(key)

        if key[0:4] == [0,0,0,0]:

            print(key)


def ReplaceBeaufort(Msg):

    NewMsg = ""

    for letter in Msg:

        if letter.lower() in alphabetset:

            #NewMsg = NewMsg + alphabet[26-alphabet[letter]]
            NewMsg = NewMsg + alphabet[27-alphabet[letter]]

        else:

            NewMsg = NewMsg + letter

    return NewMsg


def NGramFrequencies(Msg, NGramLength):

    NGramFreq = {}

    Msg = RemovePunctuation(RemoveSpaces(Msg))

    if len(Msg) > 0:

        #for letter in range(0, len(Msg)-NGramLength+1) and letter % NGramLength == 0:
        for letter in range(0, len(Msg)-NGramLength+1):

            #if letter % NGramLength == 0
            if letter % NGramLength == 0:

                ngram = Msg[letter:letter+NGramLength]

                if ngram in NGramFreq:

                    NGramFreq[ngram] += 1

                else:

                    NGramFreq[ngram] = 1

        NGramFreq = list(reversed(sorted(NGramFreq.items(), key = lambda a: a[1])))

        return NGramFreq

    else:

        #NGramFreq = [["Nothing", "There's No Text!"]]
        NGramFreq = [[-1, -1]]

        return NGramFreq


#def Solve4Square(Msg):
def Solve4Square(Msg, UseNormalAlphabet):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("4-Square Message.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["4-SquareCreaker.exe"])
    #subprocess.call(["4SquareCreaker.exe"])
    subprocess.Popen(["4SquareCreaker.exe", str(UseNormalAlphabet)])


def SolveDigraphSubstitution(Msg):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    BigramFreq = NGramFrequencies(Msg, 2)

    TextBigramFreq = list()

    for x in BigramFreq:

        TextBigramFreq.append(x[0])

    for x in range(0, 26):

        for y in range(0, 26):

            d = alphabetlist[x]+alphabetlist[y]

            if d not in TextBigramFreq:

                TextBigramFreq.append(d)

    NormalBigramFreq = open("Digraph Frequencies File.txt", "r").readlines()

    for x in range(0, len(NormalBigramFreq)):

        NormalBigramFreq[x] = NormalBigramFreq[x].replace("\n", "")

    #print (NormalBigramFreq)

    Correlations = dict()

    for x in range(0, len(NormalBigramFreq)):

        Correlations[NormalBigramFreq[x]] = TextBigramFreq[x]

    for x in range(0, 26):

        for y in range(0, 26):

            d = alphabetlist[x]+alphabetlist[y]

            #print (d + " " + Correlation[d])
            print (d + " " + Correlations[d])

    return


class DigraphSubKey(Key):

    def __init__(self):

        Key.__init__(self)

        self.links = {}
        
            #
    def ParseKey(self, Text):

        #print ("Text:", Text)

        #Text = Text[:-2]
        Text = Text[:-1]

        if len(Text) > 0:

            self.links = {}

            Text = Text.split("\n")

            for x in Text:

                y = x.split(" ")

                #self.links[y[0]] = y[1]
                #self.links[y[0]].lower() = y[1].lower()
                self.links[y[0].lower()] = y[1].lower()

        #print (self.links)

    def GetKey(self):

        return self.links

    def GetKeyText(self):

        #print (self.links)

        if len(self.links) > 0:

            Text = ""

            for x in self.links:

                #Text = x +  "" + self.links[x] + "\n"
                #Text = x + " " + self.links[x] + "\n"
                Text = Text + x + " " + self.links[x] + "\n"

        else:

            Text = ""

        #print (Text)

        return Text

    def Decode(self, Msg):

        OriginalMsg = Msg

        Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

        #Bigrams = SplitIntoBigrams(
        Bigrams = SplitIntoDigrams(Msg)

        NewMsg = ""

        for bigram in Bigrams:

            #DecodedBigram = GetKeyofValue(self.links, bigram)
            #DecodedBigram = GetKeyOfValue(self.links, bigram)
            DecodedBigram = GetKeyOfValue(bigram, self.links)

            if DecodedBigram != None:

                #NewMsg += DecodeBigram
                NewMsg = NewMsg + DecodedBigram

            else:

                NewMsg = NewMsg + bigram.upper()

        #return NewMsg
        #return CombineWithOriginalMsg(OriginalMsg, NewMsg)
        return CombineWithOriginalMsg(NewMsg, OriginalMsg)

#def IsVigenere(Msg):
#def IsVigenere(Msg,IOCLowerBound, IOCUpperBound = 100000000):
#def IsVigenere(Msg,IOCLowerBound, IOCUpperBound = 100000000, Limit = 11):
#def IsVigenere(Msg,IOCLowerBound, IOCUpperBound = 100000000, Limit = 12):
def IsVigenere(Msg,IOCLowerBound, IOCUpperBound = 100000000, Limit = 13):

    #IOCLowerBound = 0.06

    if len(Msg) > 0:
        
        #for KeyLength in range(1, 21):
        #for KeyLength in range(2, 12):
        for KeyLength in range(2, Limit + 1):

            #VigenereIOC(Msg, KeyLength)
            IOC = VigenereIOC(Msg, KeyLength)
            
            #if IOC > IOCLowerBound:
            if IOC > IOCLowerBound and IOC < IOCUpperBound:

                Yes = True

                #if VigenereIOC(Msg, KeyLength*2) > IOCLowerBound:

                    #return True

                #for x in range(2, 3+1):
                for x in range(2, 3):
                    
                    if VigenereIOC(Msg, KeyLength*x) < IOCLowerBound:

                        Yes = False

                if Yes:

                    return True

    return False


def IsNGramVigenere(Msg, NGramLength, IOCLowerBound, IOCUpperBound, Limit = 13):

    if len(Msg) > 0:
        
        for KeyLength in range(2, Limit + 1):

            IOC = NGramVigenereIOC(Msg, KeyLength, NGramLength, 0, NGramLength - 1)
            
            if IOC > IOCLowerBound and IOC < IOCUpperBound:

                #print ("Digraph Vigenere key length:", KeyLength)

                return True

    return False


def VigenereIOC(Msg, KeyLength):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    columnsnum = ceil(len(Msg)/KeyLength)

    TransposedMsg = Transpose(Msg, "Column", columnsnum).split("\n")

    IOC = 0

    LinesNum = 0

    for line in TransposedMsg:

        if line != "" and line != " " and line != "\n":

            IOC += IndexOfCoincidence(line, None)

            LinesNum += 1

    #IOC /= LinesNum
    if LinesNum > 0:

        IOC /= LinesNum

    else:

        IOC = -1

    return IOC


def ProgressiveKeyIOC(Msg, KeyLength, progressionIndex):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    columnsnum = ceil(len(Msg)/KeyLength)

    TransposedMsg = Transpose(Msg, "Column", columnsnum).split("\n")

    IOC = 0

    LinesNum = 0

    for line in TransposedMsg:

        if line != "" and line != " " and line != "\n":

            editedLine = ""

            for i in range(0, len(line)):

                editedLine = editedLine + alphabetlist[(alphabetlist.index(line[i]) - progressionIndex * i) % 26]

            IOC += IndexOfCoincidence(editedLine, None)

            LinesNum += 1

    if LinesNum > 0:

        IOC /= LinesNum

    else:

        IOC = -1

    return IOC


def IsProgressiveKey(Msg,IOCLowerBound, IOCUpperBound = 100000000, Limit = 13):

    if len(Msg) > 0:

        for progressionIndex in range(0, 26):
        
            for KeyLength in range(2, Limit + 1):

                IOC = ProgressiveKeyIOC(Msg, KeyLength, progressionIndex)
                
                if IOC > IOCLowerBound and IOC < IOCUpperBound:

                    Yes = True

                    #for x in range(2, 3):
                        
                        #if ProgressiveKeyIOC(Msg, KeyLength*x, progressionIndex) < IOCLowerBound:

                            #Yes = False

                    if Yes:

                        return True, (KeyLength, progressionIndex)

    return False, None


def IsInterruptedKey(Msg,IOCLowerBound, IOCUpperBound = 100000000):

    msg = Msg.split()

    if len(msg) > 0:

        totalIOC = 0

        lineNum = 0

        #for i in range(0, min(8, max(len(x) for x in msg))):
        for i in range(0, min(5, max(len(x) for x in msg))):

            vigenereString = ""

            for j in msg:

                if i < len(j):
                    
                    vigenereString = vigenereString + j[i]

                    #totalIOC += IndexOfCoincidence(vigenereString, None)

            #print (vigenereString)

            totalIOC += IndexOfCoincidence(vigenereString, None)

            lineNum += 1

        if lineNum != 0:

            ioc = totalIOC / lineNum

        else:

            ioc = -1

        return ioc > IOCLowerBound and ioc < IOCUpperBound

    else:

        return False


"""def RearrangeTranspoVigen(Msg):

    Msg = RemoveSpaces(RemovePunctuation(Msg).lower())

    #for KeyLength in range(0, 15):
    for KeyLength in range(1, 15):

        #KeyLength = 4

        NewMsg = Transpose(Msg, "Column", KeyLength)

        Order = []

        for x in range(1, KeyLength+1):

            Order.append(x)

        print ("Checking", factorial(KeyLength), "Permutations!")
        print()

        #CheckTranspoVigenOrders(NewMsg, Order)
        #CheckTranspoVigenOrders(NewMsg, Order, [])
        PossibleCombinations = CheckTranspoVigenOrders(NewMsg, Order, [])

        print (PossibleCombinations)"""


#def RearrangeTranspoVigen(Msg, KeyLength, TranspoType):
def RearrangeTranspoVigen(Msg, KeyLength):

    #Msg = RemoveSpaces(RemovePunctuation(Msg).lower())
    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    #print (Msg)
    Msg = Transpose(Msg, "Row", KeyLength)

    #NewMsg = Transpose(Msg, "Column", KeyLength)
    #NewMsg = Transpose(Msg, TranspoType, KeyLength)
    NewMsg = Msg

    Order = []

    for x in range(1, KeyLength+1):

        Order.append(x)

    print ("Checking", factorial(KeyLength), "Permutations!")
    print()

    #print (Transpose(Msg, "Switch Columns", "4,5,2,3,6,1"))
    #print (IsVigenere(Transpose(Msg, "Switch Columns", "4,5,2,3,6,1"), 0.06))

    PossibleCombinations = CheckTranspoVigenOrders(NewMsg, Order, [])

    #PossibleCombinations = sorted(PossibleCombinations, key = lambda x: x[1])
    PossibleCombinations = list(reversed(sorted(PossibleCombinations, key = lambda x: x[1])))

    #print (PossibleCombinations)
    Report = ""
    for x in range(0, len(PossibleCombinations)):

        Str = ""

        #for y in PossibleCombinations[x]:
        for y in PossibleCombinations[x][0]:

            Str = Str + str(y) + ","

        Str = Str[:-1]

        Report = Report + Str

        if x < len(PossibleCombinations)-1:

            Report = Report + "\n"

    return Report


#def CheckTranspoVigenOrders(Msg, Order):
def CheckTranspoVigenOrders(Msg, OnesLeft, Order):

    Combinations = []

    #if len(Order) > 1:
    if len(OnesLeft) > 1:

        #for x in range(Order[0], Order[-1]+1):
        for x in range(0, len(OnesLeft)):

            #NewOrder = Order
            NewOrder = copy.deepcopy(Order)

            #NewOrder.append(Order[x])
            NewOrder.append(OnesLeft[x])

            #NewOnesLeft = OnesLeft
            NewOnesLeft = copy.deepcopy(OnesLeft)

            NewOnesLeft.remove(OnesLeft[x])

            #CheckTranspoVigenOrders(Msg, Order[1:])
            #CheckTranspoVigenOrders(Msg, OnesLeft[1:], NewOrder)
            NewCombinations = CheckTranspoVigenOrders(Msg, NewOnesLeft, NewOrder)

            if len(NewCombinations) > 0:

                for y in NewCombinations:

                    Combinations.append(y)

        return Combinations

    else:

        Order.append(OnesLeft[0])

        InOrder = True

        for x in range (2, len(Order)):

            if Order[x] < Order[x-1]:

                InOrder = False

        if InOrder:

            print ((Order[0]-1)*factorial((len(Order)-1)))

        #print (Order)

        #return Order

        """if Order[0] == 1:

            Combinations.append(Order)

        else:

            pass"""

        OrderString = ""

        for y in Order:

            #OrderString = OrderString + y + ","
            OrderString = OrderString + str(y) + ","

        OrderString = OrderString[:-1]

        #print (OrderString)

        NewMsg = Transpose(Msg, "Switch Columns", OrderString)

        if IsVigenere(NewMsg, 0.06):
        #if IsVigenere(NewMsg, 0.062):

            print (Order)

            #Combinations.append(Order)
            Combinations.append((Order, max([VigenereIOC(NewMsg,x) for x in range(2, 12)])))
            """
#Hi
    """

        return Combinations

#def CombineWithOriginalMsg(OriginalMsg, NewMsg):
def CombineWithOriginalMsg(NewMsg, OriginalMsg):
        
    NewNewMsg = ""

    letter = 0
    letter2 = 0

    #while letter < len(NewMsg):
    while letter < len(NewMsg) and letter2 < len(OriginalMsg):

        #if OriginalMsg[letter2] in alphabetset:
        if OriginalMsg[letter2].lower() in alphabetset:

            NewNewMsg = NewNewMsg + NewMsg[letter]

            letter += 1

        else:

            NewNewMsg = NewNewMsg + OriginalMsg[letter2]

        letter2 += 1

    return NewNewMsg


def CrackTransposition(Msg, RowOrColumn, Length):

    #Msg = Transpose(RemoveSpaces(RemovePunctuation(Msg.lower())), RowOrColumn, Length)

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    Msg = Transpose(Msg, "Row", Length)

    Columns = []

    for x in range(1, Length+1):

        Columns.append(x)

    BestString = ""

    for y in Columns:

        BestString = BestString + str(y) + ","

    BestString = BestString[:-1]

    BestScore = None

    BestStr = ""

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    print ("Checking:", factorial(Length))

    #BestScore = QuadgramScore(NewMsg, QuadgramFrequencies)
    BestScore = QuadgramScore(Msg, QuadgramFrequencies)

    #BestStr, BestScore = CheckAnagramOrders(Msg, Columns, [], BestScore, QuadgramFrequencies):
    #BestStr, BestScore = CheckAnagramOrders(Msg, Columns, [], BestScore, QuadgramFrequencies)
    BestStr, BestScore = CheckAnagramOrders(Msg, Columns, [], BestScore, QuadgramFrequencies, BestString)

    print ("Checked All!")

    return BestStr


#def CheckAnagramOrders(Msg, OnesLeft, Order, BestScore, QuadgramFrequencies):
def CheckAnagramOrders(Msg, OnesLeft, Order, BestScore, QuadgramFrequencies, BestStr):

    Combinations = []

    if len(OnesLeft) > 1:

        for x in range(0, len(OnesLeft)):

            NewOrder = copy.deepcopy(Order)

            NewOrder.append(OnesLeft[x])

            NewOnesLeft = copy.deepcopy(OnesLeft)

            NewOnesLeft.remove(OnesLeft[x])

            #BestStr, BestScore = CheckAnagramOrders(Msg, NewOnesLeft, NewOrder, BestScore, QuadgramFrequencies)
            BestStr, BestScore = CheckAnagramOrders(Msg, NewOnesLeft, NewOrder, BestScore, QuadgramFrequencies, BestStr)

            """if len(NewCombinations) > 0:

                for y in NewCombinations:

                    Combinations.append(y)"""

        return BestStr, BestScore

    else:

        Order.append(OnesLeft[0])

        InOrder = True

        for x in range (2, len(Order)):

            if Order[x] < Order[x-1]:

                InOrder = False

        if InOrder:

            print ((Order[0]-1)*factorial((len(Order)-1)))

        OrderString = ""

        for y in Order:

            OrderString = OrderString + str(y) + ","

        OrderString = OrderString[:-1]

        NewMsg = Transpose(Msg, "Switch Columns", OrderString)

        Score = QuadgramScore(NewMsg, QuadgramFrequencies)

        if Score > BestScore:

            BestScore = Score

            BestStr = OrderString

            print (OrderString, Score)

        return BestStr, BestScore


def DecodeTrithemius(Msg, Mode):

    OriginalMsg = Msg

    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))

    NewMsg = ""

    for letter in range(0, len(Msg)):

         #x= Msg[;etter]
        x = Msg[letter]

        if Mode == "Normal":

            #NewMsg = NewMsg + alphabetlist[cycle( (alphabet[x]-1) - letter%26 , 25, 0)]
            NewMsg = NewMsg + alphabetlist[CycleRound( (alphabet[x]-1) - letter%26 , 0, 25)]

        elif Mode == "Reversed":

            #NewMsg = NewMsg + alphabetlist[cycle( letter%26 - (alphabet[x]-1) , 25, 0)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( letter%26 - (alphabet[x]-1) , 0, 25)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( alphabetlist[25-letter%26] - (alphabet[x]-1) , 0, 25)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( 25-letter%26 - (alphabet[x]-1) , 0, 25)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( 25-letter%26 + (alphabet[x]-1) , 0, 25)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( 26-letter%26 + (alphabet[x]-1) , 0, 25)]
            #NewMsg = NewMsg + alphabetlist[CycleRound( 26-letter%26 - (alphabet[x]-1) , 0, 25)]
            NewMsg = NewMsg + alphabetlist[CycleRound( letter%26 + (alphabet[x]-1) , 0, 25)]

    return CombineWithOriginalMsg(NewMsg, OriginalMsg)


def IOCKeyLength(Msg, KeyLength):

    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))
    #Msg = RemoveSpaces(Msg.lower())

    return VigenereIOC(Msg, KeyLength)


def SolveRunningTotal(Msg):

    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))

    BestMsg = Msg

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    BestScore =QuadgramScore(Msg, QuadgramFrequencies)

    for shift in range(0, 26):

        #NewMsg = DecodeRunningTotal(Msg, Shift)
        #NewMsg = DecodeRunningTotal(Msg, shift)
        NewMsg = DecodeRunningTotal(Msg, shift, "A=Zero")

        NewScore = QuadgramScore(NewMsg, QuadgramFrequencies)

        if NewScore > BestScore:

            BestScore = NewScore

            BestMsg = NewMsg

        print (NewMsg)
        print()

    for shift in range(0, 26):

        NewMsg = DecodeRunningTotal(Msg, shift, "A=One")

        NewScore = QuadgramScore(NewMsg, QuadgramFrequencies)

        if NewScore > BestScore:

            BestScore = NewScore

            BestMsg = NewMsg

        #print (NewMsg)
        #print()

    return BestMsg


#def DecodeRunningTotal(Msg, Shift):
def DecodeRunningTotal(Msg, Shift, Mode):

    Msg = RemovePunctuation(RemoveSpaces(Msg.lower()))

    NewMsg = ""

    #RunTotal = 0
    RunTotal = Shift

    for letter in range(0, len(Msg)):

        #print (RunTotal)

        #NewMsg = NewMsg + alphabetlist[ (alphabet[Msg[letter]]-1 + RunTotal)%26 ]
        if Mode == "A=Zero":
            
            NewMsg = NewMsg + alphabetlist[ (alphabet[Msg[letter]]-1 - RunTotal)%26 ]

            RunTotal += alphabet[NewMsg[-1]]-1

        elif Mode == "A=One":

            NewMsg = NewMsg + alphabetlist[ (alphabet[Msg[letter]] - RunTotal)%26 ]
            ##NewMsg = NewMsg + alphabet[ (alphabet[Msg[letter]] - RunTotal)%26 ]

            RunTotal += alphabet[NewMsg[-1]]

        #RunTotal += alphabetlist[NewMsg[-1]]-1
        #RunTotal += alphabet[NewMsg[-1]]-1

    return NewMsg


def NGramIOC(Msg, NGramLength):

    #NgraphFreq = NGramFrequencies(Msg, 2)
    NgraphFreq = NGramFrequencies(Msg, NGramLength)

    NgraphIOC = IndexOfCoincidence(Msg, NgraphFreq)

    return NgraphIOC


#def Solve2Square(Msg, Horizontal, SwapDrigrams):
def Solve2Square(Msg, Horizontal, SwapDigrams):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("2-Square Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["2SquareCreaker.exe", str(Horizontal), str(SwapDigrams)])


def Solve3Square(Msg, Order):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("3-Square Message.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["2SquareCreaker.exe", str(Order)])
    subprocess.Popen(["3SquareCreaker.exe", str(Order)])


def BigramVariance(Msg, Step):

    #Counts = {}
    Count = {}

    for x in range(0, len(Msg)-1-Step):

        #if x % Step

        bigram = Msg[x] + Msg[x+1+Step]

        #print(bigram)

        #if bigram in Counts:
        if bigram in Count:

            Count[bigram] += 1

        else:

            Count[bigram] = 1

    Mean = 0

    for bigram in Count:

        Mean += Count[bigram]

    Mean /= len(Count)

    Variance = 0

    for bigram in Count:

        Variance += (Count[bigram] - Mean)**2

    Variance /= len(Count)

    return Variance


def SolveBifid(Msg, Period):

    MissingLetter = "j"

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("Bifid Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["BifidCreaker.exe", str(Period), str(MissingLetter)])


def SolveDoubleTransposition(Msg, Type1, Length1, Type2, Length2):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    #Msg = RemoveSpaces(Msg.lower())
    Msg = RemoveSpaces(Msg.lower()).replace("\n","")

    File = open("Double Transposition Message.txt", "w")
    #File = open("--DoubleTranspoMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["DoubleTranspoCreaker.exe", str(Type1), str(Length1), str(Type2), str(Length2)])
    #subprocess.call(["SolveDoubleTranspo.exe", str(Length2), str(Length1), str(Type2 == "Row").lower(), str(Type1 == "Row").lower()])


def BruteForceDoubleTransposition(Msg, Type1, Length1, Type2, Length2):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower()).replace("\n","")

    #File = open("Brute Double Transposition Message.txt", "w")
    File = open("C# Programs\\--BruteDoubleTranspoMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["DoubleTranspoBruter.exe", str(Type1), str(Length1), str(Type2), str(Length2)])
    #subprocess.call(["Brute Double Transpo Test.exe", str(Type1), str(Length1), str(Type2), str(Length2)])
    subprocess.Popen(["C# Programs\\BruteDoubleTranspo.exe", str(Length2), str(Length1), str(Type2 == "Row").lower(), str(Type1 == "Row").lower()], cwd = "C# Programs")


#def NGramVigenereIOC(Msg, KeyLength, NGramLength):
def NGramVigenereIOC(Msg, KeyLength, NGramLength, From, To):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    NgraphIOC = 0

    NgraphFreq = {}

    #for step in range(0, KeyLength):
    for step in range(From-1, To):

        NgraphFreq = {}

        #index = KeyLength*step
        index = NGramLength*step

        while index < len(Msg)+1-NGramLength:

            #ngram = Msg[index:index+KeyLength]
            ngram = Msg[index:index+NGramLength]

            if ngram in NgraphFreq:

                NgraphFreq[ngram] += 1

            else:

                NgraphFreq[ngram] = 1

            #index += KeyLength
            index += KeyLength*NGramLength

        #NgraphFreq = list(reversed(sorted(NGramFreq.items(), key = lambda a: a[1])))
        NgraphFreq = list(reversed(sorted(NgraphFreq.items(), key = lambda a: a[1])))

        NgraphIOC += IndexOfCoincidence(Msg, NgraphFreq)

    #return NgraphIOC/KeyLength
    return NgraphIOC/(To-From+1)


def FindPotentialAnagrams(Input, Dictionary):

    Input = Input.lower()
    
    Words = []

    LetterFreqs = {}

    for letter in Input:

        if letter in alphabetset:

            if letter in LetterFreqs:

                LetterFreqs[letter] += 1

            else:

                LetterFreqs[letter] = 1

    for word in Dictionary:

        if len(word) <= len(Input):
            
            NumberSubstitutions = {}

            Match = True

            WordLetterFreqs = {}

            for letter in word:

                if letter in alphabetset:

                    if letter in WordLetterFreqs:

                        WordLetterFreqs[letter] += 1

                    else:

                        WordLetterFreqs[letter] = 1

            for letter in WordLetterFreqs:

                if letter not in LetterFreqs:

                    Match = False

                #elif LetterFreqs[letter] < WordLetterFreqs:
                elif LetterFreqs[letter] < WordLetterFreqs[letter]:

                    Match = False                    
                        

            if Match:

                #Words.append(word)
                Words.append([word, len(word)])

    Words = list(reversed(sorted(Words, key = lambda a: a[1])))

    Anagrams = []

    #for word in Word:
    for word in Words:

        #Anagrams.append(word)
        Anagrams.append(word[0])

    #return Words
    return Anagrams


def FindPotentialContainAnagrams(Input, Dictionary):

    Input = Input.lower()
    
    Words = []

    LetterFreqs = {}

    for letter in Input:

        if letter in alphabetset:

            if letter in LetterFreqs:

                LetterFreqs[letter] += 1

            else:

                LetterFreqs[letter] = 1

    for word in Dictionary:

        #if len(word) <= len(Input):
        if len(word) >= len(Input):
            
            NumberSubstitutions = {}

            Match = True

            WordLetterFreqs = {}

            for letter in word:

                if letter in alphabetset:

                    if letter in WordLetterFreqs:

                        WordLetterFreqs[letter] += 1

                    else:

                        WordLetterFreqs[letter] = 1

            #for letter in WordLetterFreqs:
            for letter in LetterFreqs:

                #if letter not in LetterFreqs:
                if letter not in WordLetterFreqs:

                    Match = False

                elif LetterFreqs[letter] > WordLetterFreqs[letter]:

                    Match = False               
                        

            if Match:

                Words.append([word, len(word)])

    #Words = list(reversed(sorted(Words, key = lambda a: a[1])))
    Words = list(sorted(Words, key = lambda a: a[1]))

    Anagrams = []

    for word in Words:

        Anagrams.append(word[0])

    return Anagrams


#def SolveStraddleCheckerboard(Msg):
def SolveStraddleCheckerboard(Msg, TempThing):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))
    Msg = RemoveSpaces(Msg.lower())

    File = open("Straddle Checkerboard Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["StraddleCheckerboardCreaker.exe"])


#def SolveAutokey(Msg):
def SolveAutokey(Msg, RangeStart, RangeEnd):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #AutoKeyHillClimb(Msg)
    #AutokeyDecrypter.AutoKeyHillClimb(Msg)
    #AutokeyDecrypter.AutoKeyHillClimb(Msg, RangeStart, RangeEnd)
    Key = AutokeyDecrypter.AutoKeyHillClimb(Msg, RangeStart, RangeEnd)
    return Key


def TrigramVariance(Msg, Step):

    Count = {}

    #for x in range(0, len(Msg)-1-Step):
    for x in range(0, len(Msg)-2-2*Step):

        trigram = Msg[x] + Msg[x+1+Step] + Msg[x+2+2*Step]

        if trigram in Count:

            Count[trigram] += 1

        else:

            Count[trigram] = 1

    Mean = 0

    for trigram in Count:

        Mean += Count[trigram]

    Mean /= len(Count)

    Variance = 0

    for trigram in Count:

        Variance += (Count[trigram] - Mean)**2

    Variance /= len(Count)

    return Variance


def SolveSeriatedPlayfair(Msg, Period):

    MissingLetter = "j"

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("Seriated Playfair Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["SeriatedPlayfairCreaker.exe", str(False), str(MissingLetter), str(Period)])


#def NGramVariance(Msg, NGramLength Step):
def NGramVariance(Msg, NGramLength, Step):

    Count = {}

    for x in range(0, len(Msg)-(NGramLength-1)-(NGramLength-1)*Step):

        ngram = ""

        for i in range(0, NGramLength):

            ngram = ngram + Msg[x+i+i*Step]

        if ngram in Count:

            Count[ngram] += 1

        else:

            Count[ngram] = 1

    Mean = 0

    for ngram in Count:

        Mean += Count[ngram]

    Mean /= len(Count)

    Variance = 0

    for ngram in Count:

        Variance += (Count[ngram] - Mean)**2

    Variance /= len(Count)

    return Variance


def SolveTrifid(Msg, Period):

    ExtraLetter = "#"

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("Trifid Message.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["TrifidCreaker.exe", str(Period), str(ExtraLetter)])


def CutIntoChunks(Msg, Period):

    #Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    Chunk = ""

    NewMsg = ""

    for letter in Msg:

        Chunk = Chunk + letter

        if len(Chunk) == Period:

            NewMsg = NewMsg + Chunk + " "

            Chunk = ""

    NewMsg = NewMsg + Chunk

    return NewMsg


def BaseNToLetter(Msg, Base, StringLength, A):

    #alphabetStr = "abcdefghijklmnopqrstuvwxyzz"
    alphabetStr = "abcdefghijklmnopqrstuvwxyz."

    NewMsg = ""

    String = ""

    Num = 0

    letter = 0

    #for letter in range(0, len(Msg)+1-StringLength):
    while letter < len(Msg)+1-StringLength:

        Num = 0

        String = Msg[letter:letter+StringLength]

        for x in range(0, StringLength):

            Num += int(String[x])*(Base**(StringLength-1-x))

        #print (String, Num)

        if A == 0:

            #NewMsg = NewMsg + alphabetlist[Num]
            NewMsg = NewMsg + alphabetStr[Num]

        elif A == 1:

            NewMsg = NewMsg + alphabet[Num]

        letter += StringLength

    return NewMsg


def ReverseWords(Msg):

    OriginalMsg = Msg

    #Msg = RemovePunctuation(Msg)

    Msg = Msg.split(" ")

    NewMsg = ""

    for word in Msg:

        newword = ""

        for letter in word:

            newword = letter + newword

        NewMsg = NewMsg + newword

    #print (NewMsg, OriginalMsg)

    #return NewMsg
    return CombineWithOriginalMsg(NewMsg, OriginalMsg)


def CreateKeyedAlphabet(Key):
    
    KeyWord = RemovePunctuation(RemoveSpaces(Key)).lower()

    if KeyWord != "":
        
        KeyedAlphabet = []

        for letter in KeyWord:

            if letter not in KeyedAlphabet:

                KeyedAlphabet.append(letter)

        #for letter in alphabet:
        for letter in alphabetlist:

            if letter not in KeyedAlphabet:

                KeyedAlphabet.append(letter)

        return KeyedAlphabet

    return None


#def EncodeHutton(Msg, Alphabet, Key):
def EncodeHutton(Msg, Alphabet, Password):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #print (Msg, "\n", Alphabet, "\n", Key, "\n")

    Ciphertext = ""

    for letter in range(0, len(Msg)):

        #print (Alphabet.index(Msg[letter]), alphabet[Key[letter%len(Key)]])
        #Ciphertext = Ciphertext + Alphabet[CycleRound(Alphabet.index(Msg[letter]) + alphabet[Key[letter%len(Key)]], 0, 25)]
        Ciphertext = Ciphertext + Alphabet[CycleRound(Alphabet.index(Msg[letter]) + alphabet[Password[letter%len(Password)]], 0, 25)]

        #Alphabet[Alphabet.index(Msg[letter])] = Ciphertext[-1]
        Alphabet[Alphabet.index(Msg[letter])] = None

        Alphabet[Alphabet.index(Ciphertext[-1])] = Msg[letter]

        Alphabet[Alphabet.index(None)] = Ciphertext[-1]

        #print (Key, "\n")
        #print (Alphabet, "\n")

    return Ciphertext


def DecodeHutton(Msg, Alphabet, Password):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    Plaintext = ""

    for letter in range(0, len(Msg)):

        Plaintext = Plaintext + Alphabet[CycleRound(Alphabet.index(Msg[letter]) - alphabet[Password[letter%len(Password)]], 0, 25)]

        Alphabet[Alphabet.index(Msg[letter])] = None

        Alphabet[Alphabet.index(Plaintext[-1])] = Msg[letter]

        Alphabet[Alphabet.index(None)] = Plaintext[-1]

    return Plaintext


def ReplaceChar(Msg, Char, With):

    return Msg.replace(Char, With)


def ChiSquared(TextDistribution, AgainstDistribution, significanceLevel = 0.05):

    #print (TextDistribution, AgainstDistribution)

    chiSquared = 0

    numItems = 0

    totalFreq = 0

    for x in TextDistribution:

        if x in AgainstDistribution:

            if TextDistribution[x] >= 5 and AgainstDistribution[x] >= 5:
            #if True:

                #chiSquared += (TextDistribution[x]**2) / AgainstDistribution[x]
                chiSquared += ((TextDistribution[x] - AgainstDistribution[x]) ** 2) / AgainstDistribution[x]

                numItems += 1

                totalFreq += TextDistribution[x]

    ### Returns (chiSquared, True) if accept null hyopthesis. Otherwise, return (chiSquared, False) ###
                
    #return chiSquared - len(TextDistribution), chiSquared < scipy.stats.chi2.isf(significanceLevel, numItems)
    #return chiSquared - numItems, chiSquared < scipy.stats.chi2.isf(significanceLevel, numItems)
    #return chiSquared - numItems, chiSquared < stats.chi2.isf(significanceLevel, numItems)
    #return chiSquared - numItems, chiSquared < stats.chi2.isf(significanceLevel, numItems - 1)
    #return chiSquared - totalFreq, chiSquared < stats.chi2.isf(significanceLevel, numItems - 1)

    #chiSquared -= totalFreq
                
    #return chiSquared, chiSquared < stats.chi2.isf(significanceLevel, numItems - 1)
    #return chiSquared, chiSquared < stats.chi2.isf(significanceLevel, numItems)
    return chiSquared, chiSquared < 100


def TextChiSquared(Msg, Distribution):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #AgainstDistribution = self.ChiDistributionOption.get()

    #if AgainstDistribution == "English":
    if Distribution == "English":

        AgainstDistribution = copy.deepcopy(averageletterfrequencies)

    else:

        assert ValueError("No such known distribution: " + Distribution)

    for x in AgainstDistribution:

        AgainstDistribution[x] *= len(Msg) / 100

    TextDistribution = LetterCount(Msg)

    return ChiSquared(TextDistribution, AgainstDistribution)


def LetterCount(Msg, KeyWordLength = 1, KeyWordLetter = 1):

    KeyWordLetter -= 1

    if KeyWordLetter < 0:

        KeyWordLetter = 0

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    LetterFreq = {}

    for letter in alphabetlist:

        LetterFreq[letter] = 0

    Length = 0

    for letter in range(0, len(Msg)):

        if letter%KeyWordLength == KeyWordLetter:

            Length += 1

            if Msg[letter] in LetterFreq:

                LetterFreq[Msg[letter]] += 1

            elif Msg[letter] in alphabetset:

                LetterFreq[Msg[letter]] = 1

    #LetterFreq = list(reversed(sorted(LetterFreq.items(), key = lambda a: a[1])))

    return LetterFreq


#def SolveAutokeyCSharp(Msg, RangeStart, RangeEnd):
#def SolveAutokeyCSharp(Msg, keyLength):
def SolveAutokeyCSharp(Msg, keyLength, alphabet):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #File = open("C# Code/Finished Programs/--AutokeyMessage.txt", "w")
    File = open("C# Programs\\--AutokeyMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.call(["C# Code/Finished Programs/SolveAutokey.exe", str(RangeStart)])
    #subprocess.call(["SolveAutokey.exe", str(RangeStart)])
    #subprocess.Popen(["C# Programs\\SolveAutokey.exe", str(RangeStart)])
    #subprocess.Popen(["C# Programs\\SolveAutokey.exe", str(keyLength)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolveAutokey.exe", str(keyLength), str(alphabet)], cwd = "C# Programs")

    return

def SolveADFGX(Msg, ColumnNum, missingLetter = "j"):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("C# Programs\\--ADFGXMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.Popen(["C# Programs\\SolveADFGX.exe", str(ColumnNum)])
    subprocess.Popen(["C# Programs\\SolveADFGX.exe", str(ColumnNum), str(missingLetter)], cwd = "C# Programs")

    return

def SolveNormalVigenere(Msg, Key, KeyWordLength):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("C# Programs\\--VigenereMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveVigenere.exe", str(KeyWordLength)], cwd = "C# Programs")

    return


MONOSUB_IOC_MAX = 0.08
MONOSUB_IOC = 0.067
#MONOSUB_IOC_TOLERANCE = 0.006
#MONOSUB_IOC_TOLERANCE = 0.007
MONOSUB_IOC_TOLERANCE = 0.0075

#BIGRAMSUB_IOC = 0.007
#BIGRAMSUB_IOC_TOLERANCE = 0.0006
BIGRAMSUB_IOC_MIN = 0.0066
#BIGRAMSUB_IOC_MAX = 0.0082
#BIGRAMSUB_IOC_MAX = 0.0085
BIGRAMSUB_IOC_MAX = 0.0087

#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2"]
#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition"]
#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition", "Running Key", "Homophonic Substitution"]
#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition", "Running Key", "Homophonic Substitution", "Vigenère + Transposition"]
#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition", "Running Key", "Homophonic Substitution", "Vigenère + Transposition", "A weird Polybius", "Pollux"]
#UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition", "Running Key", "Homophonic Substitution", "Vigenère + Transposition",
UNIDENTIFIABLE_CIPHERS = ["Bifid", "Trifid", "Seriated Playfair", "NxN Hill, N > 2", "Autokey", "Running Total", "Hutton?", "Vigenère + Transposition", "Running Key", "Homophonic Substitution",
                          "A weird Polybius", "Pollux", "Nicodemus", "Ragbaby", "Gromark"]

SECTIONATER = "------------------------------"

def IdentifyCipher(Msg):

    Msg = Msg.rstrip("\n")

    #OriginalMsg = Msg
    OriginalMsg = copy.deepcopy(Msg)

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    #OriginalMsg = Msg

    Report = ""

    PotentialCiphers = []


    ## Mono-sub ##
    
    #if abs(IndexOfCoincidence(Msg, LetterFrequencies(Msg)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE:
    #if abs(IndexOfCoincidence(Msg, LetterFrequencies(Msg)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE or abs(CharIOC(OriginalMsg) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE:
    if abs(IndexOfCoincidence(Msg, LetterFrequencies(Msg)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE or abs(CharIOC(RemoveSpaces(OriginalMsg)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE:

        PotentialCiphers.append("Monoalphabetic Substitution")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Transpo-Sub")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Bazeries")

        IsMonoSub = True

    else:

        IsMonoSub = False


    ## Transposition ##
        
    #if ChiSquared(Msg, "English")[1]:
    #if TextChiSquared(Msg, "English")[1]:
    if TextChiSquared(Msg, "English")[1] and TextChiSquared(Msg, "English")[0] > 0:

        PotentialCiphers.append("Transposition")

        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Double Transposition")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Myszkowski Transposition")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Rail Fence")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Route Transposition")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("AMSCO")

        if len(RemoveSpaces(OriginalMsg)) % 25 == 0:
            
            PotentialCiphers.append("<TAB>")
            PotentialCiphers.append("Cadenus")

        if IsMonoSub:
            
            PotentialCiphers.remove("Monoalphabetic Substitution")
            PotentialCiphers.remove("<TAB>")
            PotentialCiphers.remove("Transpo-Sub")
            PotentialCiphers.remove("<TAB>")
            PotentialCiphers.remove("Bazeries")

            IsMonoSub = False

        IsTranspo = True

    else:

        IsTranspo = False


    ## Vigenere ##
        
    if IsVigenere(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX, 20) and not IsMonoSub and not IsTranspo:
    #if IsVigenere(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX, 20):

        #PotentialCiphers.append("Vigenère")
        PotentialCiphers.append("Vigenère (" + str(FindVigenereKeyLength(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE)) + ")")

        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Beaufort")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Keyed Vigenère")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("Transposition + Vigenère")
        #PotentialCiphers.append("<TAB>")
        #PotentialCiphers.append("Nicodemus")

    ## Trithemius ##
        
    if VigenereIOC(Msg, 26) > MONOSUB_IOC - MONOSUB_IOC_TOLERANCE and VigenereIOC(Msg, 26) < MONOSUB_IOC_MAX and not IsMonoSub and not IsTranspo:
    #if VigenereIOC(Msg, 26) > MONOSUB_IOC - MONOSUB_IOC_TOLERANCE and VigenereIOC(Msg, 26) < MONOSUB_IOC_MAX:

        PotentialCiphers.append("Trithemius")

    ## ProgressiveKey ##

    #isProgKey = IsProgressiveKey(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX, 20)
    isProgKey = IsProgressiveKey(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX, 10)
        
    if isProgKey[0] and not IsMonoSub and not IsTranspo:
    #if isProgKey[0]:

        PotentialCiphers.append("Progressive Key " + str(isProgKey[1]))


    ## Interrupted Key ##
        
    #if IsInterruptedKey(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX) and not IsMonoSub and not IsTranspo:
    #if IsInterruptedKey(RemovePunctuation(OriginalMsg), MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX) and not IsMonoSub and not IsTranspo:
    if IsInterruptedKey(RemovePunctuation(OriginalMsg), MONOSUB_IOC - MONOSUB_IOC_TOLERANCE) and not IsMonoSub and not IsTranspo:

        PotentialCiphers.append("Interrupted Key")


    ## Polybius ##

    #if NumOfUniqueChars(OriginalMsg) <= 5:
    #if NumOfUniqueChars(OriginalMsg) <= 5 or abs(CharIOC(MakePolybiusSubstitution(OriginalMsg, 2)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE:
    if abs(CharIOC(MakePolybiusSubstitution(OriginalMsg, 2)) - MONOSUB_IOC) < MONOSUB_IOC_TOLERANCE:

        PotentialCiphers.append("Polybius")


    ## Straddling Checkerboard ##

    if IsAllNum(OriginalMsg):

        PotentialCiphers.append("Straddling Checkerboard")
        #PotentialCiphers.append("Straddle Vigenère (Straddle Nihilist)")
        PotentialCiphers.append("Straddle Nihilist")
        PotentialCiphers.append("Pollux")

    ## Homophonic Substitution ##

    if IsAllNum(OriginalMsg):

        PotentialCiphers.append("Homophonic Substitution")


    ## ADFGX ##

    if NumOfUniqueChars(OriginalMsg) <= 5 and len(OriginalMsg) % 2 == 0:

        PotentialCiphers.append("ADFGX")


    ## ADFGVX ##

    if NumOfUniqueChars(OriginalMsg) <= 6 and len(OriginalMsg) % 2 == 0:

        PotentialCiphers.append("ADFGVX")


    ## ABCDEFGHIK ##

    #if NumOfUniqueChars(OriginalMsg) <= 12 and len(OriginalMsg) % 2 == 0:
    if NumOfUniqueChars(OriginalMsg) <= 12:

        PotentialCiphers.append("ABCDEFGHIK")
        #PotentialCiphers.append("A weird Polybius")


    ## Nihilist ##

    if IsAllNum(OriginalMsg) and IsVigenere(MakeNihilistSubstitution(OriginalMsg), MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX):

        PotentialCiphers.append("Nihilist")

    elif IsAllNum(OriginalMsg) and IsVigenere(MakeNihilistSubstitution(NormaliseNihilist(OriginalMsg)), MONOSUB_IOC - MONOSUB_IOC_TOLERANCE, MONOSUB_IOC_MAX):

        PotentialCiphers.append("Nihilist")


    if IsNGramVigenere(Msg, 2, BIGRAMSUB_IOC_MIN, BIGRAMSUB_IOC_MAX) and not IsMonoSub and not IsTranspo:
    #if IsNGramVigenere(Msg, 2, BIGRAMSUB_IOC_MIN, BIGRAMSUB_IOC_MAX):

        #PotentialCiphers.append("Digraph Vigenère")
        PotentialCiphers.append("Digraph Vigenère (" + str(FindVigenereKeyLength(Msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE) / 2) + ")")


    ## Digraph substitution ##

    #print (IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)), NGramFrequencies(Msg, 2))
        
    #if abs(IndexOfCoincidence(Msg, LetterFrequencies(Msg, 2)) - BIGRAMSUB_IOC) < BIGRAMSUB_IOC_TOLERANCE:
    #if abs(IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) - BIGRAMSUB_IOC) < BIGRAMSUB_IOC_TOLERANCE:
    #if IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) > BIGRAMSUB_IOC:
    #if IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) > BIGRAMSUB_IOC_MIN and not IsMonoSub:
    if IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) > BIGRAMSUB_IOC_MIN and IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) < BIGRAMSUB_IOC_MAX and not IsMonoSub:
    #if IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) > BIGRAMSUB_IOC_MIN and IndexOfCoincidence(Msg, NGramFrequencies(Msg, 2)) < BIGRAMSUB_IOC_MAX:
    #if IndexOfCoincidence(RemoveSpaces(OriginalMsg.strip("\n").lower()), NGramFrequencies(RemoveSpaces(OriginalMsg.strip("\n").lower()), 2)) > BIGRAMSUB_IOC_MIN and IndexOfCoincidence(RemoveSpaces(OriginalMsg.strip("\n").lower()), NGramFrequencies(RemoveSpaces(OriginalMsg.strip("\n").lower()), 2)) < BIGRAMSUB_IOC_MAX and not IsMonoSub:

        PotentialCiphers.append("Digraph Substitution")

        isPlayfair = True

        for letter in range(0, len(Msg)):

            if letter % 2 == 0 and letter != len(Msg)-1:

                if Msg[letter] == Msg[letter+1]:

                    isPlayfair = False

        if isPlayfair:
            
            PotentialCiphers.append("<TAB>")
            PotentialCiphers.append("Playfair")
            
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("2-Square")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("3-Square")
        PotentialCiphers.append("<TAB>")
        PotentialCiphers.append("4-Square")
        PotentialCiphers.append("<TAB>")
        #PotentialCiphers.append("Hill")
        PotentialCiphers.append("2x2 Hill")



    ## Write report

    #Report = Report + "Potential Ciphers:\n\n"
    #Report = Report + "Potential Ciphers:\n-----------------\n\n"
    Report = Report + "Potential Ciphers:\n------------------\n\n"

    if len(PotentialCiphers) == 0:

        #Report = Report + "Sorry, none has been found."
        Report = Report + "Sorry, none has been found.\n\n"

        Report = Report + "Perhaps it is one of the following ciphers I cannot identify yet:\n\n"

        for x in range(0, len(UNIDENTIFIABLE_CIPHERS)):

            cipher = UNIDENTIFIABLE_CIPHERS[x]

            Report = Report + "• " + cipher

            if x < len(UNIDENTIFIABLE_CIPHERS)-1:

                Report = Report + "\n"

    else:

        for x in range(0, len(PotentialCiphers)):

            cipher = PotentialCiphers[x]

            if cipher == "<TAB>":

                #Report = Report + "\t"
                Report = Report + "  "

            else:

                #Report = Report + cipher
                #Report = Report "• " + cipher
                Report = Report + "• " + cipher

                if x < len(PotentialCiphers)-1:

                    Report = Report + "\n"


    ## General / specific info ##

    
    Report = Report + "\n\n" + SECTIONATER + "\n\nEnd of report."

    return Report, PotentialCiphers


NONCHAR_CHARS = [" ", "\n", "\t", "\r", "\s"]

def NumOfUniqueChars(Msg):

    count = 0

    foundChars = set()

    for x in Msg:

       # if x not in foundChars and x != " ":
        if x not in foundChars and x not in NONCHAR_CHARS:

            foundChars.add(x)

            count += 1

    return count

def IsAllNum(Msg):

    for x in Msg:

        #if x != " " and x not in digitset:
        #if x not in digitset and x not in [" ", "\n", "\t", "\r", "\s"]:
        if x not in digitset and x not in NONCHAR_CHARS:

            #print (x)

            return False

    return True

def FindVigenereKeyLength(Msg, IOCLowerBound, IOCUpperBound = 100000000, Limit = 30):

    if len(Msg) > 0:
        
        for KeyLength in range(2, Limit + 1):

            IOC = VigenereIOC(Msg, KeyLength)
            
            if IOC > IOCLowerBound and IOC < IOCUpperBound:

                Yes = True

                #for x in range(2, 3 + 1):
                for x in range(2, 3):
                    
                    if VigenereIOC(Msg, KeyLength * x) < IOCLowerBound:

                        Yes = False

                if Yes:

                    return KeyLength

    return -1


def AutoSolve(msg, ciphers):

    result = ""

    count = 0

    originalMsg = copy.deepcopy(msg)

    for x in range(0, len(ciphers)):

        ciphe = ciphers[x]

        DoneSomething = True

        if ciphe == "Monoalphabetic Substitution":

            SolveSubstitutionAnnealing(msg, None)

        elif ciphe == "Transposition":

            #SolveTransposition(msg)
            SolveTranspositionCSharp(msg)

        elif ciphe == "Vigenère":

            keyLength = FindVigenereKeyLength(msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE)

            SolveNormalVigenere(msg, None, keyLength)
            #SolveVigenere(msg, None, keyLength)

        elif ciphe == "Nihilist":

            newMsg = MakeNihilistSubstitution(msg)

            keyLength = FindVigenereKeyLength(newMsg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE)

            #SolveNormalVigenere(msg, None, keyLength)
            #SolveVigenere(msg, None, keyLength)
            SolveVigenere(newMsg, None, keyLength)

        elif ciphe == "Rail Fence":

            SolveRailFence(msg, 30, "top")
            SolveRailFence(msg, 30, "bottom")

        elif ciphe == "Keyed Vigenère":

            keyLength = FindVigenereKeyLength(msg, MONOSUB_IOC - MONOSUB_IOC_TOLERANCE)

            SolveVigenere(msg, None, keyLength)
            #SolvePolyalphabetic(msg, keyLength, "abcdefghijklmnopqrstuvwxyz")
            #SolvePolyalphabetic(RemoveSpaces(RemovePunctuation(msg)), keyLength, "abcdefghijklmnopqrstuvwxyz")

        else:

            DoneSomething = False

        if DoneSomething:

            count += 1

            #result = result + ciphe
            result = result + "• " + ciphe

            if x < len(ciphers)-1:

                #result = result + ", "
                result = result + "\n"

                #if x == len(ciphers)-1:

                    #result = result + "and "

    if count >= 1:

        #result = "I identified and tried these ciphers:\n-------------------------------------\n\n" + result
        result = "I tried these ciphers:\n----------------------\n\n" + result

    else:

        result = "Sorry, I couldn't try any ciphers!"

    return result


def SolveNxNHill(Msg, N):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("C# Programs\\--NxNHillMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveNxNHill.exe", str(N)], cwd = "C# Programs")

    return


def SolveRailFence(msg, limit = 10, startAt = "top"):

    msg = RemoveSpaces(RemovePunctuation(msg.lower()))

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    bestMsg = msg

    bestScore = QuadgramScore(msg, QuadgramFrequencies)

    for n in range(2, limit + 1):

        newMsg = DecodeRailFence(msg, n, startAt)

        newScore = QuadgramScore(newMsg, QuadgramFrequencies)

        if newScore > bestScore:

            bestMsg = newMsg

            bestScore = newScore

    print (bestMsg)
    return bestMsg


def DecodeRailFence(msg, noOfRows, startAt = "top"):

    newMsg = ""

    grid = ["" for i in range(0, noOfRows)]

    goingUp = False

    if startAt.lower() == "top":
        
        rowNum = 0

    else:

        rowNum = noOfRows - 1

    count = 0

    while count < len(msg):

        for i in range(0, noOfRows):

            if i == rowNum:

                grid[i] = grid[i] + "X"

            else:

                grid[i] = grid[i] + "-"

        if rowNum == noOfRows-1:

            goingUp = True

            rowNum -= 1

        elif rowNum == 0:

            goingUp = False

            rowNum += 1

        elif goingUp:

            rowNum -= 1

        elif not goingUp:

            rowNum += 1

        count += 1

    #for row in grid:
        #print (row)
        #print ()

    #index = 0

    #for rowNum in range(0, noOfRows):

        #row = "-" * rowNum

        #while index

    index = 0

    for rowNum in range(0, len(grid)):

        for charNum in range(0, len(grid[rowNum])):
        
            if grid[rowNum][charNum] == "X":

                #grid[rowNum][charNum] = msg[index]
                #grid[rowNum] = grid[rowNum][charNum:] + msg[index] + grid[rowNum][:charNum+1]
                grid[rowNum] = grid[rowNum][:charNum] + msg[index] + grid[rowNum][charNum+1:]

                index += 1


    #for row in grid:
        #print (row)


    goingUp = False

    if startAt.lower() == "top":

        rowNum = 0

    else:

        rowNum = noOfRows - 1

    count = 0

    for i in range(0, len(grid[0])):

        newMsg = newMsg + grid[rowNum][i] 

        if rowNum == noOfRows-1:

            goingUp = True

            rowNum -= 1

        elif rowNum == 0:

            goingUp = False

            rowNum += 1

        elif goingUp:

            rowNum -= 1

        elif not goingUp:

            rowNum += 1

    #print (newMsg)
    return newMsg


#DecodeRailFence("dhsgfyugdukyudgyu", 3)
#DecodeRailFence("dhsgfyugdukyudgyu", 5)
#DecodeRailFence("dhsgfyugdukyudgyu", 4)


def MakeVICLetterOrderKey(key):

    letterIndex = 1

    for i in range(0, 26):

        letterFound = False

        for x in range(0, len(key)):

            if isinstance(key[x], str) and key[x].lower() == alphabetlist[i]:

                letterFound = True
                
                key[x] = letterIndex

                letterIndex = (letterIndex + 1) % 10

        #if letterFound:

            #letterIndex = (letterIndex + 1) % 10

    return key


def EncryptStraddleCheckerboard(msg, boardKey, columnNumbering):

    #if len(boardKey[:10].replace("#", "")) != 8:
    if len(boardKey[:10].replace(" ", "")) != 8:

        assert ValueError("Invalid board key: " + str(boardKey))

    blanks = []

    for x in range(0, 10):

        #if boardKey[x] == "#":
        if boardKey[x] == " ":

            blanks.append(columnNumbering[x])

    else:

        newMsg = ""

        for i in range(0, len(msg)):

            if msg[i] in boardKey[:10]:

                newMsg = newMsg + str(columnNumbering[boardKey.index(msg[i])])

            else:

                #print (msg[i])
                newMsg = newMsg + str(blanks[boardKey.index(msg[i]) // 10 - 1]) + str(columnNumbering[boardKey.index(msg[i]) % 10])

    return newMsg

        
def EncryptVIC(msg, keyphrase, date, indic, personalNum, checkerboardKey):

    #msg = "W EAREP L EASED TOH EAROF Y OU RSU C C ESSINESTAB L ISH INGY OU RF AL SEID ENTITY Y OU W IL L B ESENTSOM EM ONEY TOC OV EREX P ENSESW ITH INAM ONTH"
    #Msg = "IVES INVALIDATED . REPORT IMMEDIATELY TO SAFE HOUSE . AWAIT EXTRACTION INSTRUCTIONS WITHIN WEEK .. ASSIGNED OBJECT"
    #msg = "IVES INVALIDATED . REPORT IMMEDIATELY TO SAFE HOUSE . AWAIT EXTRACTION INSTRUCTIONS WITHIN WEEK .. ASSIGNED OBJECT"

    #msg = RemoveSpaces(RemovePunctuation(msg.lower()))
    #msg = RemoveSpaces(msg.lower())
    msg = RemoveSpaces(msg.lower()).rstrip("\n")

    print ("Msg:")
    print (msg)
    print()

    keyphrase = keyphrase.lower()

    straddleTopKey, transpoKey1, transpoKey2 = GetVICSettings(keyphrase, date, indic, personalNum)

    #straddleKey = "AT#ONE#SIRBCDFGHJKLMPQUVWXYZ./".lower()
    #straddleKey = "AT ONE SIRBCDFGHJKLMPQUVWXYZ./".lower()

    straddleKey = CreateStraddleKey(checkerboardKey.lower())

    print ("STraddle cherkberboard key:")
    print (straddleKey)

    newMsg = EncryptStraddleCheckerboard(msg, straddleKey, straddleTopKey)

    #print (newMsg)

    print ("Encrypted with straddle")
    print ()
    print (newMsg)

    #for x in range(0, (5 - len(msg) % 5) % 5):
    for x in range(0, (5 - len(newMsg) % 5) % 5):
        
        newMsg = newMsg + digitlist[random.randint(0, 9)]

    print ("Msg with random digits to fill gap")
    print (newMsg)
    print ()

    #newMsg = MyszkowskiTranspose(newMsg, transpoKey1)
    newMsg = VICTranspose(newMsg, transpoKey1)

    #for x in range(0, (5 - len(msg) % 5) % 5):
        
        #newMsg = newMsg + digitlist[random.randint(0, 9)]

    print ("FIrst transposition:")
    #print (newMsg)
    #print (InsertSpacesEveryN(newMsg, 10))
    print (InsertSpacesEveryN(newMsg, 11))
    #print (CutIntoChunks(newMsg, 11))

    print()
    print (transpoKey2)

    disruptTranspoGrid = []

    #print ([alphabetlist[x] for x in transpoKey2])
    #transpoKey2 = MakeVICLetterOrderKey([alphabetlist[x] for x in transpoKey2])

    #print (transpoKey2)
    #print ()

    usedColumns = set()

    startIndex = 0

    while startIndex not in transpoKey2:

        startIndex += 1

    for i in range(0, len(transpoKey2)):

        if transpoKey2[i] == startIndex:

            rowLength = i

            break

    usedColumns.add(rowLength)

    print (rowLength, startIndex)
        
    #rowLength = transpoKey2.index(0)

    row = ""

    #startIndex = 1

    #for i in range(0, len(newMsg)):

    i = 0

    #while i < len(newMsg) and len(disruptTranspoGrid) < len(newMsg) // len(transpoKey2):
    #while i < len(newMsg) and len(disruptTranspoGrid) < ceil(len(newMsg) / len(transpoKey2)):
    #while i < len(newMsg) and len(disruptTranspoGrid) < ceil(len(newMsg) / len(transpoKey2)) and not (len(disruptTranspoGrid) == ceil(len(newMsg) / len(transpoKey2)) - 2 and len(row) == len(newMsg) % len(transpoKey2)):
    while i < len(newMsg) and len(disruptTranspoGrid) < ceil(len(newMsg) / len(transpoKey2)) and not (len(disruptTranspoGrid) == ceil(len(newMsg) / len(transpoKey2)) - 1 and len(row) == len(newMsg) % len(transpoKey2)):

        #row = row + msg[i]
        row = row + newMsg[i]

        if len(row) == rowLength:

            #row = row + " " * (10 - len(row))
            row = row + " " * (len(transpoKey2) - len(row))

            disruptTranspoGrid.append(row)

            row = ""

            rowLength += 1

            #if rowLength == 11:
            #if rowLength == len(transpoKey2) + 1:
            #if rowLength >= len(transpoKey2) + 1:
            #if rowLength >= len(transpoKey2):
            #if rowLength >= len(transpoKey2) or (len(disruptTranspoGrid) == ceil(len(newMsg) / len(transpoKey2)) - 1 and len(row) == len(newMsg) % len(transpoKey2)):
            #if rowLength >= len(transpoKey2) or (len(disruptTranspoGrid) == ceil(len(newMsg) / len(transpoKey2)) - 2 and len(row) == len(newMsg) % len(transpoKey2)):
            if rowLength >= len(transpoKey2):

                #rowLength = transpoKey2.index(startIndex)

                for j in range(0, len(transpoKey2)):

                    #if transpoKey2[i] == startIndex and transpoKey2 not in usedColumns:
                    if transpoKey2[j] == startIndex and j not in usedColumns:

                        rowLength = j

                        break

                #if rowLength in usedColumns:
                if rowLength-1 in usedColumns:

                    startIndex += 1

                    while startIndex not in transpoKey2:

                        startIndex += 1

                    for j in range(0, len(transpoKey2)):

                        #if transpoKey2[i] == startIndex and transpoKey2:
                        if transpoKey2[j] == startIndex:

                            rowLength = j

                            break

                usedColumns.add(rowLength)

                #print (rowLength, startIndex)

                #startIndex += 1

        i += 1

    if row != "":

        disruptTranspoGrid.append(row)

    index = i

    print ("Disrupted transpo grid part 1:")
    for x in disruptTranspoGrid:

        print (x)
        newMsg = newMsg + x

    print()

    #row = row + " " * (10 - len(row))

    #disruptTranspoGrid.append(row)

    for row in range(0, len(disruptTranspoGrid)):

        for i in range(0, len(disruptTranspoGrid[row])):

            if disruptTranspoGrid[row][i] == " " and index < len(newMsg):

                disruptTranspoGrid[row] = disruptTranspoGrid[row][:i] + newMsg[index] + disruptTranspoGrid[row][(i+1):]

                index += 1

    newMsg = ""

    print ("Disrupted transpo grid:")
    for x in disruptTranspoGrid:

        print (x)
        newMsg = newMsg + x


    print ()

    newMsg = VICTranspose(newMsg, transpoKey2)

    #newMsg = InsertSpacesEveryN(newMsg, 5)
    newMsg = InsertSpacesEveryN(newMsg.replace(" ", ""), 5)

    newMsg = newMsg.split(" ")

    #newMsg.insert(1 - date[-1], indic)
    #newMsg.insert(1 - date[-1], "".join(str(x) for x in indic))
    #newMsg.insert(-((date[-1] - 1) % 10), "".join(str(x) for x in indic))

    if date[-1] == 1:

        #newMsg.insert("".join(str(x) for x in indic))
        newMsg.append("".join(str(x) for x in indic))

    else:
        
        newMsg.insert(-((date[-1] - 1) % 10), "".join(str(x) for x in indic))

    newMsg = "".join(newMsg)

    newMsg = InsertSpacesEveryN(newMsg.replace(" ", ""), 5)

    return newMsg


#def MyszkowskiTranspose(msg, key):
def VICTranspose(msg, key):

    newMsg = ""

    #for i in range(0, max(key)):
    for i in range(0, max(key)+1):

        if i in key:

            for j in range(0, len(key)):

                if key[j] == i:

                    index = j

                    #newMsg = newMsg + msg[j]
                    #newMsg = newMsg + msg[index]

                    while index < len(msg):

                        #index += len(key)

                        #newMsg = newMsg + msg[j]
                        newMsg = newMsg + msg[index]

                        index += len(key)

    return newMsg


def InsertSpacesEveryN(text, n):

    newText = ""

    for i in range(0, len(text)):

        newText = newText + text[i]

        #if i % n == 0:
        #if (i+1) % n == 0:
        #if i % n == 1:
        if i % n == n-1:

            newText = newText + " "

    return newText.rstrip(" ")


def GetVICSettings(keyphrase, date, indic, personalNum):

    print ("Keyphrase:")
    print (keyphrase)
    print ("Date:")
    print (date)
    print ("Message Indicator:")
    print (indic)
    print ("Personal Number:")
    print (personalNum)

    #keyphrase = keyphrase[:10]
    keyphrase = keyphrase[:20]

    fibKey = []

    for i in range(0, len(indic)):

        fibKey.append((indic[i] - date[i]) % 10)

    for i in range(0, 5):

        fibKey.append((fibKey[i] + fibKey[i+1]) % 10)

    print ("Fibonacci key")
    print (fibKey)

    phrasehalf1 = [x for x in keyphrase[:ceil(len(keyphrase)/2)]]
    phrasehalf2 = [x for x in keyphrase[ceil(len(keyphrase)/2):]]

    phrasehalf1 = MakeVICLetterOrderKey(phrasehalf1)
    phrasehalf2 = MakeVICLetterOrderKey(phrasehalf2)    

    print()
    print ("1st half of keyphrase")
    print (phrasehalf1)
    print ("2nd half of keyphrase")
    print (phrasehalf2)

    fibKey2 = []

    for i in range(0, len(phrasehalf1)):

        fibKey2.append((phrasehalf1[i] + fibKey[i]) % 10)

    print ()
    print ("Extended fibonacci key")
    print (fibKey2)

    encodedFibKey2 = []

    for i in range(0, len(fibKey2)):

        #encodedFibKey2.append((phrasehalf2.index(fibKey2[i]) + 1) % 10)
        #encodedFibKey2.append((phrasehalf2[i] + 1) % 10)
        #encodedFibKey2.append(phrasehalf2[(fibKey2[i] + 1) % 10])
        #encodedFibKey2.append(phrasehalf2[(fibKey2[i] - 1) % 10 + 1])
        encodedFibKey2.append(phrasehalf2[(fibKey2[i] - 1) % 10])

    print ()
    print ("ENcoded Fibonacci Key:")
    print (encodedFibKey2)

    pseudoRand = [x for x in encodedFibKey2]

    for i in range(0, 50):

        pseudoRand.append((pseudoRand[i] + pseudoRand[i+1]) % 10)

    pseudoRand = pseudoRand[10:]

    print ("Pseudo-random numbers:")
    print (pseudoRand)

    #straddleKey = MakeVICLetterOrderKey([alphabetlist[x] for x in pseudoRand[40:]])
    straddleTopKey = MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in pseudoRand[40:]])

    print ("Checkerboard key")
    print (straddleTopKey)

    columnLength2 = personalNum + pseudoRand[-1]

    columnLength1 = personalNum + [x for x in reversed(pseudoRand) if x != pseudoRand[-1]][0]

    print (columnLength1, columnLength2)

    transpoKeyDigits = []

    #print ([alphabetlist[(x-1) % 10] for x in encodedFibKey2])
    #print (MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in encodedFibKey2]))

    for i in range(0, 10):

        for j in range(0, 5):

            #transpoKeyDigits.append(pseudoRand[MakeVICLetterOrderKey(encodedFibKey2).index(i) + 5 * j])
            #transpoKeyDigits.append(pseudoRand[MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in encodedFibKey2]).index(i) + 5 * j])
            #transpoKeyDigits.append(pseudoRand[MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in encodedFibKey2]).index((i-1) % 10) + 10 * j])
            #print (i, j)
            #transpoKeyDigits.append(pseudoRand[MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in encodedFibKey2]).index(i) + 10 * j])
            transpoKeyDigits.append(pseudoRand[MakeVICLetterOrderKey([alphabetlist[(x-1) % 10] for x in encodedFibKey2]).index((i+1) % 10) + 10 * j])

    transpoKeyDigits = transpoKeyDigits[:(columnLength1 + columnLength2)]

    print ("Transposition key digits")
    print (transpoKeyDigits)

    transpoKey1 = transpoKeyDigits[:columnLength1]
    #transpoKey1 = [(x-1) % 10 for x in transpoKey1]
    transpoKey1 = [(x-1) % columnLength1 for x in transpoKey1]
    
    transpoKey2 = transpoKeyDigits[columnLength1:]
    #transpoKey2 = [(x-1) % 10 for x in transpoKey2]
    transpoKey2 = [(x-1) % columnLength2 for x in transpoKey2]

    print ("1st transpo key")
    print (transpoKey1)
    print ("2nd transpo key")
    print (transpoKey2)

    return straddleTopKey, transpoKey1, transpoKey2


def WordCount(msg):

    return len(msg.split(" "))


def IntoMorseSubstitution(text, dot, dash, delim):

    return text.replace(".", dot).replace("-", dash).replace(" ", delim)

def FromMorseSubstitution(text, dot, dash, delim):

    return text.replace(dot, ".").replace(dash, "-").replace(delim, " ")


def SubstituteSpecialMorseChars(char):

    if char == "<fullstop>":

        return "."

    elif char == "<hyphen>":

        return "-"

    elif char == "<aa>":

        return "\n"

    else:

        return char


def DecodeMorse(msg, dot, dash, delim):

    msg = msg.replace("\n", "")

    #if delim == "<SPACE>":
    if delim == "<s>":

            delim = " "
            
    else:

        for i in range(1, 20):

            if delim == "<" + str(i) + "s>":

                delim = " " * i

    newMsg = ""

    #msg.split(delim)
    msg = msg.split(delim)

    for i in range(0, len(msg)):

        morseChar = FromMorseSubstitution(msg[i], dot, dash, delim)

        #newMsg = newMsg + MORSE[morseChar] + " "
        newMsg = newMsg + SubstituteSpecialMorseChars(MORSE[morseChar]) + " "

    return newMsg[:-1]


def CreateStraddleKey(key):

    key = RemoveSpaces(key)

    newKey = ""

    for x in key:

        if x not in newKey:

            newKey = newKey + x

            finalLetter = x

    for x in alphabetlist:
    #for x in range(0, 26):

        #letter = alphabet[alphabet[finalLetter] + x]
        #letter = alphabet[(alphabet[finalLetter] + x - 1) % 26 + 1]

        if x not in newKey:
        #if letter not in newKey:

            newKey = newKey + x
            #newKey = newKey + letter

    straddleKey = newKey[:8] + "  " + newKey[8:]

    return straddleKey



ENIGMA_ROTORS = {1: ("ekmflgdqvzntowyhxuspaibrcj", "r"),
                 2: ("ajdksiruxblhwtmcqgznpyfvoe", "f"),
                 3: ("bdfhjlcprtxvznyeiwgakmusqo", "w"),
                 4: ("esovpzjayquirhxlnftgkdcmwb", "k"),
                 5: ("vzbrgityupsdnhlxawmjqofeck", "a"),
                 6: ("jpgvoumfyqbenhzrdkasxlictw", ("a", "n")),
                 7: ("nzjhgrcxmyswboufaivlpekqdt", ("a", "n")),
                 8: ("fkqhtlxocbjspdzramewniuygv", ("a", "n")),
                 "beta": ("leyjvcnixwpbqmdrtakzgfuhos", None),
                 "gamma": ("fsokanuerhmbtiycwlqpzxvgjd", None)}

ENIGMA_REFLECTORS = {"a": "ejmzalyxvbwfcrquontspikhgd",
                     "b": "yruhqsldpxngokmiebfzcwvjat",
                     "b-thin": "enkqauywjicopblmdxzvfthrgs",
                     "c": "fvpjiaoyedrzxwgctkuqsbnmhl",
                     "c-thin": "rdobjntkvehmlfcwzaxgyipsuq"}

def EnigmaPlugboard(msg, plugboard = []):

    newMsg = ""

    for i in msg:

        inPlugboard = False

        for j in plugboard:

            if i == j[0]:

                newMsg = newMsg + j[1]

                inPlugboard = True

            elif i == j[1]:

                newMsg = newMsg + j[0]

                inPlugboard = True

        if not inPlugboard:

            newMsg = newMsg + i

    return newMsg
        

def EncryptEngima(msg, rotorOrder = [1,2,3], rotorPositions = ["a","a","a"], ring = ["a","a","a"], plugboard = [], reflector = "b"):

    #return DecryptEngima(msg, rotorOrder, rotorPositions, ring, plugboard, reflector)
    return DecryptEnigma(msg, rotorOrder, rotorPositions, ring, plugboard, reflector)

def DecryptEnigma(msg, rotorOrder = [1,2,3], rotorPositions = ["a","a","a"], ring = ["a","a","a"], plugboard = [], reflector = "b", runSilently = False):

    rotorOrder = list(reversed(rotorOrder))
    rotorPositions = list(reversed(rotorPositions))
    ring = list(reversed(ring))

    #print ("Rotor Order:\t\t", rotorOrder)
    #print ("Rotor Start Positions:\t\t", rotorPositions)
    #print ("Ring:\t\t", ring)
    #print ("Plugboard:\t\t", plugboard)
    if not runSilently:
        print ("Rotor Order: ", rotorOrder)
        print ("Rotor Start Positions: ", rotorPositions)
        print ("Ring: ", ring)
        print ("Plugboard: ", plugboard)

    for i in alphabetlist:

        if len([x for x in plugboard if i in x]) > 1:

            #raise ValueError("Plugboard has swapped " + str(i) + " with more than one letter.")
            raise ValueError("Plugboard has swapped '" + str(i) + "' with more than one letter.")

    #if len(Ring) != 3:
    if len(ring) != 3:

        #raise ValueError("Ring's length is not 3.")
        raise ValueError("Ring's length must be exactly 3.")

    if len(rotorOrder) != 3:

        raise ValueError("There must be exactly 3 rotors (Rotor Order).")

    if len(rotorPositions) != 3:

        raise ValueError("There must be exactly 3 rotors (Rotor Starting Positions).")

    ### My Enigma works on the basis of the left-most rotor being the one the plaintext letter in encryption (or ciphertext letter in decryption) first goes through ###

    rotations = [0,0,0]

    newMsg = ""

    #Rotor2Moved = False

    for i in range(0, len(msg)):

        if msg[i] in alphabetset:

            rotations[0] += 1

            #if Rotor2Moved:

                #rotations[1] += 1

            if (rotations[1] + 1) % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[1]][1]] - alphabet[rotorPositions[1]]) % 26:

                rotations[1] += 1
                rotations[2] += 1

            if rotations[0] % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[0]][1]] - alphabet[rotorPositions[0]]) % 26:

                rotations[1] += 1

                #Rotor2Moved = True

                #if rotations[0] % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[1]][1]] - alphabet[rotorPositions[1]]) % 26:
                #if rotations[1] % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[1]][1]] - alphabet[rotorPositions[1]]) % 26:
                #if Rotor2Moved and (rotations[1] + 1) % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[1]][1]] - alphabet[rotorPositions[1]]) % 26:
                #if (rotations[1] + 1) % 26 == (alphabet[ENIGMA_ROTORS[rotorOrder[1]][1]] - alphabet[rotorPositions[1]]) % 26:

                 #   rotations[1] += 1
                 #   rotations[2] += 1

                    #Rotor2Moved = False

            #print (ENIGMA_ROTORS[rotorOrder[0]][0][rotations[0]%26], ENIGMA_ROTORS[rotorOrder[1]][0][rotations[1]%26], ENIGMA_ROTORS[rotorOrder[2]][0][rotations[2]%26])
            #print (ENIGMA_ROTORS[rotorOrder[0]][0][(rotations[0] + alphabetlist.index(rotorPositions[0]))%26],
            #       ENIGMA_ROTORS[rotorOrder[1]][0][(rotations[1] + alphabetlist.index(rotorPositions[1]))%26],
            #       ENIGMA_ROTORS[rotorOrder[2]][0][(rotations[2] + alphabetlist.index(rotorPositions[2]))%26])

            #print (alphabetlist[(rotations[0] + alphabetlist.index(rotorPositions[0]) + alphabetlist.index(ring[0]))%26],
            #       alphabetlist[(rotations[1] + alphabetlist.index(rotorPositions[1]) + alphabetlist.index(ring[1]))%26],
            #       alphabetlist[(rotations[2] + alphabetlist.index(rotorPositions[2]) + alphabetlist.index(ring[2]))%26])

            char = msg[i]

            char = EnigmaPlugboard(char, plugboard)

            #char = alphabet[(alphabet.index(ENIGMA_ROTORS[rotorOrder[0]][(rotations[0] + alphabetlist[ring[0]] - 1) % 26]) - rotations[0]) % 26]
            #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[0]][(rotations[0] + alphabetlist[ring[0]] - 1) % 26]) - rotations[0]) % 26]
            #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[0]][(rotations[0] + alphabet[ring[0]] - 1) % 26]) - rotations[0]) % 26]
            #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[0]][0][(rotations[0] + alphabet[ring[0]] - 1) % 26]) - rotations[0]) % 26]

            for rotor in range(0, 3):

                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabet[ring[rotor]] - 1) % 26]) - rotations[rotor]) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabet[ring[rotor]] - 1 + alphabet[char]) % 26]) - rotations[rotor]) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabet[ring[rotor]] - 1 + alphabet[char] - 1) % 26]) - rotations[rotor]) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabetlist.index(ring[rotor]) + alphabet.list(char)) % 26]) - rotations[rotor]) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabetlist.index(ring[rotor]) + alphabetlist.index(char)) % 26]) - rotations[rotor]) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabetlist.index(ring[rotor]) + alphabetlist.index(char)) % 26]) - rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] + alphabetlist.index(ring[rotor]) - alphabetlist.index(char)) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] - alphabetlist.index(ring[rotor]) + alphabetlist.index(char)) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] - alphabetlist.index(ring[rotor]) + alphabetlist.index(char) + alphabetlist.index(rotorPositions[rotor])) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor])) % 26]
                char = alphabetlist[(alphabetlist.index(ENIGMA_ROTORS[rotorOrder[rotor]][0][(rotations[rotor] - alphabetlist.index(ring[rotor]) + alphabetlist.index(char) + alphabetlist.index(rotorPositions[rotor])) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor]) - alphabetlist.index(rotorPositions[rotor])) % 26]

                #print ("After Rotor", str(rotor) + ":")
                #print (char)

            char = ENIGMA_REFLECTORS[reflector][alphabetlist.index(char)]

            #print ("After Reflector:")
            #print (char)

            for rotor in reversed(range(0, 3)):

                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor][0]].index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].find(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]

                #if rotor == 0:
                    
                    #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(char) - alphabetlist.index(ring[rotor])) % 26] - rotations[0]

                #else:

                    #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor-1]][0].index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]

                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(char) + rotations[rotor-1] - alphabetlist.index(ring[rotor])) % 26]

                #char = alphabetlist[(((ENIGMA_ROTORS[rotorOrder[rotor]][0].index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26) - rotations[rotor]) % 26]
                #char = alphabetlist[(((ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor]) % 26]) - alphabetlist.index(ring[rotor])) % 26) - rotations[rotor]) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor]) % 26]) - alphabetlist.index(ring[rotor]) - rotations[rotor]) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor]) % 26]) - alphabetlist.index(ring[rotor]) - rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor] + alphabetlist.index(ring[rotor])) % 26]) - rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor])) % 26]
                #char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor] - alphabetlist.index(ring[rotor])) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor]) - alphabetlist.index(rotorPositions[rotor])) % 26]
                char = alphabetlist[(ENIGMA_ROTORS[rotorOrder[rotor]][0].index(alphabetlist[(alphabetlist.index(char) + rotations[rotor] - alphabetlist.index(ring[rotor]) + alphabetlist.index(rotorPositions[rotor])) % 26]) - rotations[rotor] + alphabetlist.index(ring[rotor]) - alphabetlist.index(rotorPositions[rotor])) % 26]

                #print ("After Rotor", str(rotor) + ":")
                #print (char)

            #print()

            char = EnigmaPlugboard(char, plugboard)

            newMsg = newMsg + char

            #print (char)

        else:

            newMsg = newMsg + msg[i]

    return newMsg


def SlideEnigmaCrib(ciphertext, crib):

    ciphertext = RemovePunctuation(RemoveSpaces(ciphertext)).lower().rstrip("\n")
    crib = RemovePunctuation(RemoveSpaces(crib)).lower().rstrip("\n")

    possiblePositions = []

    for i in range(0, len(ciphertext) - len(crib) + 1):

        if IsValidEnigmaCribPosition(ciphertext, crib, i):

            possiblePositions.append(i)

    return possiblePositions

def IsValidEnigmaCribPosition(ciphertext, crib, position):

    valid = True

    for j in range(0, len(crib)):

        #if ciphertext[i + position] == crib[j]:
        if ciphertext[position + j] == crib[j]:

            #valid = False
            return False

    #if valid:

        #possiblePositions.append(i)
    return True


##def BletchleyBombe(ciphertext, crib, position):
##
##    ciphertext = RemovePunctuation(RemoveSpaces(ciphertext.lower().rstrip("\n")))
##
##    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")
##
##    bestRotorOrder = [1,2,3]
##    bestRotorPositions = ["a","a","a"]
##
##    #print (DecryptEnigma(ciphertext, bestRotorOrder, bestRotorPositions, ["a","a","a"], [], "b", runSilently = True))
##
##    bestScore = QuadgramScore(DecryptEnigma(ciphertext, bestRotorOrder, bestRotorPositions, ["a","a","a"], [], "b", runSilently = True), QuadgramFrequencies)
##
##    for a in range(0, 5):
##
##        for b in [x for x in range(0, 5) if x != a]:
##
##            for c in [x for x in range(0, 5) if x != a and x != b]:
##
##                #a,b,c=4,3,1
##                a,b,c=3,2,0
##
##                #print ("Searching rotor order:", alphabetlist[a].upper() + alphabetlist[b].upper() + alphabetlist[c].upper())
##                #print ("Searching rotor order:", str(a) + str(b) + str(c))
##                print ("Searching rotor order:", str(a+1) + str(b+1) + str(c+1))
##
##                newRotorOrder = [a+1,b+1,c+1]
##
##                for i in range(0, 26):
##
##                    for j in range(0, 26):
##
##                        for k in range(0, 26):
##
##                            newRotorPositions = [alphabetlist[i],alphabetlist[j],alphabetlist[k]]
##
##                            #newScore = QuadgramScore(DecryptEnigma(ciphertext, newRotorOrder, newRotorPositions, ["a","a","a"], [], "b", runSilently = True), QuadgramFrequencies)
##                            newScore = QuadgramScore(DecryptEnigma(ciphertext, newRotorOrder, newRotorPositions, ["o","o","p"], [], "b", runSilently = True), QuadgramFrequencies)
##
##                            if newScore > bestScore:
##
##                                #print ("Yes")
##
##                                bestScore = newScore
##
##                                bestRotorOrder = copy.deepcopy(newRotorOrder)
##                                bestRotorPositions = copy.deepcopy(newRotorPositions)
##
##                    print (26**2 * i)
##
##                print ("Best Settings so far:")
##                print ("Rotor Order:", bestRotorOrder)
##                print ("Rotor Positions:", bestRotorPositions)
##                print()
##
##    #return newMsg
##    print ("Finished!")
##    print ("Best Settings:")
##    print ("Rotor Order:", bestRotorOrder)
##    print ("Rotor Positions:", bestRotorPositions)


def EncryptCustomSubstitution(msg, alphabet, key):

    if len(alphabet) != len(key):

        raise ValueError("Alphabet and key are not the same length.")

    newMsg = ""

    for i in range(0, len(msg)):

        if msg[i] in alphabet:

            newMsg = newMsg + key[alphabet.index(msg[i])]

        else: newMsg = newMsg + msg[i]

    return newMsg

def DecryptCustomSubstitution(msg, alphabet, key):

    return EncryptCustomSubstitution(msg, key, alphabet)


def CharFrequencies(msg):
    
    freqs = {}

    for i in range(0, len(msg)):

        if msg[i] in freqs:

            freqs[msg[i]] += 1

        else:

            freqs[msg[i]] = 1

    freqs = list(reversed(sorted(freqs.items(), key = lambda a: a[1])))

    return freqs


#def CreateOrderedFrequencyAlphaNumericKey(msg):
def CreateOrderedFrequencyAlphaNumericKey(msg, alphabet):

    #key = ALPHABETS["alphanumeric"]
    #key = alphabet
    key = copy.deepcopy(alphabet)

    charFreq = CharFrequencies(msg)

    print (charFreq)

    index = 0

    for letter in range(0, len(charFreq)):

        #if charFreq[letter] in ALPHABETS["alphanumeric"]:
        #if charFreq[letter][0] in ALPHABETS["alphanumeric"]:
        if charFreq[letter][0] in alphabet:

            while orderedalphanumericonaveragefrequencies[index] not in alphabet:

                index += 1

            #key = key[:ALPHABETS["alphanumeric"].index(orderedalphanumericonaveragefrequencies[letter])] + charFreq[letter][0] + key[ALPHABETS["alphanumeric"].index(orderedalphanumericonaveragefrequencies[letter])+1:]
            #key = key[:ALPHABETS["alphanumeric"].index(orderedalphanumericonaveragefrequencies[index])] + charFreq[letter][0] + key[ALPHABETS["alphanumeric"].index(orderedalphanumericonaveragefrequencies[index])+1:]
            key = key[:alphabet.index(orderedalphanumericonaveragefrequencies[index])] + charFreq[letter][0] + key[alphabet.index(orderedalphanumericonaveragefrequencies[index])+1:]

            index += 1

    if len(charFreq) < len(alphabet):

        for letter in range(0, len(alphabet)):

            if alphabet[letter] not in [x[0] for x in charFreq]:

                while orderedalphanumericonaveragefrequencies[index] not in alphabet:

                    index += 1

                key = key[:alphabet.index(orderedalphanumericonaveragefrequencies[index])] + alphabet[letter] + key[alphabet.index(orderedalphanumericonaveragefrequencies[index])+1:]

                index += 1

    return key


def SolveCustomSubstitution(msg, alphabet, NumOfTrials = 10000):

    Msg = msg

    QuadgramFrequencies = LoadQuadgramFrequencyTable("Dictionary", "All")

    #if set(x for x in alphabet) == set(x for x in ALPHABETS["alphanumeric"]):
    #if set(x for x in alphabet).issubsetof(x for x in ALPHABETS["alphanumeric"]):
    if set(x for x in alphabet).issubset(x for x in ALPHABETS["alphanumeric"]):

        #Key = CreateOrderedFrequencyAlphaNumericKey(msg, alphabet)
        #Key = CreateOrderedFrequencyAlphaNumericKey(msg)
        Key = CreateOrderedFrequencyAlphaNumericKey(msg, alphabet)
        #Key = copy.deepcopy(alphabet)

        #print (key)
        print (Key)

    else:

        Key = copy.deepcopy(alphabet)

    NewKey = copy.deepcopy(Key)

    BestScore = QuadgramScore(DecryptCustomSubstitution(Msg, alphabet, NewKey), QuadgramFrequencies)

    BestKey = copy.deepcopy(NewKey)

    #NumOfTrials = 10000
    #NumOfTrials = 1000

    for trial in range(0, NumOfTrials):

        if trial % (NumOfTrials//10) == 0:

            print ("Trial:", trial, "/", NumOfTrials)

        SwapIndex1 = random.randint(0, len(alphabet)-1)

        SwapIndex2 = SwapIndex1

        while SwapIndex2 == SwapIndex1:

            SwapIndex2 = random.randint(0, len(alphabet)-1)

        temp = NewKey[SwapIndex1]

        NewKey = NewKey[:SwapIndex1] + NewKey[SwapIndex2] + NewKey[SwapIndex1+1:]

        NewKey = NewKey[:SwapIndex2] + temp + NewKey[SwapIndex2+1:]

        NewScore = QuadgramScore(DecryptCustomSubstitution(Msg, alphabet, NewKey), QuadgramFrequencies)

        if NewScore > BestScore:

            BestScore = NewScore

            BestKey = copy.deepcopy(NewKey)

        else:

            Probability = e**((NewScore-BestScore)/(NumOfTrials-trial))

            NewKey = copy.deepcopy(BestKey)

    #print (Decode(Msg, BestKey))
    print (DecryptCustomSubstitution(Msg, alphabet, BestKey))
    print()
    print (BestKey)

    return BestKey


def CreateRandomKey(alphabet):

    for i in range(0, 100*len(alphabet)):

        index1 = random.randint(0, len(alphabet)-1)

        index2 = random.randint(0, len(alphabet)-1)

        temp = alphabet[index1]

        alphabet = alphabet[:index1] + alphabet[index2] + alphabet[index1+1:]

        alphabet = alphabet[:index2] + temp + alphabet[index2+1:]

    return alphabet


def SolveADFGVX(Msg, ColumnNum, transpoType):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("C# Programs\\--ADFGVXMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveADFGVX.exe", str(ColumnNum), str(transpoType)], cwd = "C# Programs")

    return

def BruteADFGVX(Msg, ColumnNum, transpoType):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower()))

    File = open("C# Programs\\--BruteADFGVXMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\BruteADFGVX.exe", str(ColumnNum), str(transpoType)], cwd = "C# Programs")

    return


def BaseNToBaseM(num, baseN, baseM):

    if baseM > 36:

        raise ValueError("BaseNToBaseM() only works for BaseM up to and including 36")

    #num = int(num, baseN)
    num = int(str(num), baseN)

    digits = []

    #while num >= 0:
    while num > 0:

        digits.append(num % baseM)

        num //= baseM

        #print (num, digits)

    for i in range(0, len(digits)):

        if digits[i] >= 10:

            #digits[i] = alphabetlist[i - 10]
            digits[i] = alphabetlist[digits[i] - 10]

    #num = "".join(reversed(digits))
    num = "".join(str(x) for x in reversed(digits))

    return num.lstrip("0")


def FindPotentialNames(text, dictionary):

    text = RemovePunctuation(text.lower()).split(" ")

    potentialNames = []

    for word in text:

        #if word not in dictionary:
        #if word not in dictionary and word not in PotentialNames:
        if word not in dictionary and word not in potentialNames:

            potentialNames.append(word)

    return potentialNames


def LaunchRunningKeyWorkspace(msg):

    msg = RemoveSpaces(RemovePunctuation(msg.lower().strip("\n")))

    File = open("C# Programs\\--RunningKeyWorkspaceMessage.txt", "w")

    File.write(msg)

    File.close()

    subprocess.Popen(["C# Programs\\WorkspaceRunningKey.exe"], cwd = "C# Programs")

    return


def CreateLetterOrderKey(keyword, sameLettersGetConsecutiveNumbers = True):

    #letterIndex = 0
    letterIndex = 1

    key = [x.lower() for x in keyword]

    for i in range(0, 26):

        Found = False

        for x in range(0, len(key)):

            if isinstance(key[x], str) and key[x].lower() == alphabetlist[i]:
                
                key[x] = letterIndex

                if sameLettersGetConsecutiveNumbers:

                    letterIndex = letterIndex + 1

                else:

                    Found = True

        if Found:

            letterIndex += 1

    return key


def FindLetterOrder(letterOrder, dictionaryType):

    dictionary = GetDictionary(dictionaryType)

    potentialWords = []

    for word in dictionary:

        #if letterOrder == CreateLetterOrderKey(word):
        if letterOrder == CreateLetterOrderKey(word) or letterOrder == CreateLetterOrderKey(word, False):

            potentialWords.append(word)

    return potentialWords


def GetDictionary(DictionaryType):

    if DictionaryType == 1:

        DictionaryType = "Large"

    elif DictionaryType == 2:

        DictionaryType = "Full"

    elif DictionaryType == 3:

        DictionaryType = "SOWPODS"

    Dictionary = open(DictionaryType + " Dictionary.txt", "r").readlines()

    for word in range(0, len(Dictionary)):

        Dictionary[word] = RemovePunctuation(Dictionary[word]).lower()

    Dictionary = set(Dictionary)

    return Dictionary


def ScoreMonoSubFastBigrams(ObservedMatrix, ExpectedMatrix):

    #return sum(ObservedMatrix[i%26][i//26] - ExpectedMatrix[i%26][i//26] for i in range(0, 26 * 26))
    #return sum(abs(ObservedMatrix[i%26][i//26] - ExpectedMatrix[i%26][i//26]) for i in range(0, 26 * 26))
    return sum((ObservedMatrix[i%26][i//26] - ExpectedMatrix[i%26][i//26])**2 for i in range(0, 26 * 26))

#def UpdateFastBigramMatrix(Msg, bigramObservedMatrix):
#
#    for i in range(0, len(Msg)-1):
#
 #       
#
 #   return

def SolveMonoSubFastBigrams(Msg):

    Msg = RemoveSpaces(RemovePunctuation(Msg.lower().strip("\n")))

    bigramFile = open("Bigram Frequencies.txt", "r").readlines()

    bigramExpectedMatrix = [[0 for x in range(0, 26)] for x in range(0, 26)]

    totalBigramCount = 0

    for i in range(0, len(bigramFile)):

        totalBigramCount += int(bigramFile[i].split(" ")[1])

    for i in range(0, len(bigramFile)):

        bigram = bigramFile[i].lower().split(" ")

        #bigramExpectedMatrix[alphabet[bigram[0][0]]-1][alphabet[bigram[0][1]]-1] = int(bigram[1]) * len(Msg) / totalBigramCount
        #bigramExpectedMatrix[alphabet[bigram[0][0]]-1][alphabet[bigram[0][1]]-1] = round(int(bigram[1]) * len(Msg) / totalBigramCount)
        bigramExpectedMatrix[orderedalphanumericonaveragefrequencies.index(bigram[0][0])][orderedalphanumericonaveragefrequencies.index(bigram[0][1])] = int(bigram[1]) * len(Msg) / totalBigramCount

    bigramObservedMatrix = [[0 for x in range(0, 26)] for x in range(0, 26)]

    key = CreateOrderedFrequencyKey(Msg)

    initialPlaintext = Decode(Msg, key)

    for i in range(0, len(Msg)-1):

        #bigramObservedMatrix[alphabet[Msg[i]]-1][alphabet[Msg[i+1]]-1] += 1
        #bigramObservedMatrix[alphabet[initialPlaintext[i]]-1][alphabet[initialPlaintext[i+1]]-1] += 1
        bigramObservedMatrix[orderedalphanumericonaveragefrequencies.index(initialPlaintext[i])][orderedalphanumericonaveragefrequencies.index(initialPlaintext[i+1])] += 1

    #for i in range(0, 26 * 26):

        #print (alphabetlist[i%26] + alphabetlist[i//26] + " " + str(bigramExpectedMatrix[i%26][i//26]) + " " + str(bigramObservedMatrix[i%26][i//26]))

    bestScore = ScoreMonoSubFastBigrams(bigramObservedMatrix, bigramExpectedMatrix)

    #for trial in range(0, 2):
    #for trial in range(0, 100):
    for trial in range(0, 1):

        #for i in range(0, 25):
        for i in range(0, 26):

            for j in range(0, 26 - i):

                newMatrix = bigramObservedMatrix

                rowTemp = newMatrix[j]
                newMatrix[j] = newMatrix[j+i]
                newMatrix[j+i] = rowTemp

                for x in range(0, 26):

                    columnTemp = newMatrix[x][j]
                    newMatrix[x][j] = newMatrix[x][j+i]
                    newMatrix[x][j+i] = columnTemp

                newScore = ScoreMonoSubFastBigrams(newMatrix, bigramExpectedMatrix)

                if newScore < bestScore:

                    bigramObservedMatrix = newMatrix

                    keyTemp = key[alphabetlist[j]]
                    #keyTemp = key[orderedalphanumericonaveragefrequencies[j]]
                    #keyTemp = key[orderedalphanumericonaveragefrequencies.index(alphabetlist[j])]
                    key[alphabetlist[j]] = key[alphabetlist[j+i]]
                    #key[orderedalphanumericonaveragefrequencies[j]] = key[orderedalphanumericonaveragefrequencies[j+i]]
                    #key[orderedalphanumericonaveragefrequencies.index(alphabetlist[j])] = key[orderedalphanumericonaveragefrequencies.index(alphabetlist[j+i])]
                    key[alphabetlist[j+i]] = keyTemp
                    #key[orderedalphanumericonaveragefrequencies[j+i]] = keyTemp
                    #key[orderedalphanumericonaveragefrequencies.index(alphabetlist[j+i])] = keyTemp

                    bestScore = newScore

    print (Decode(Msg, key))

    return Decode(Msg, key), key

def PercentageCorrect(text1, text2):

    total = 0
    correct = 0

    for i in range(0, min(len(text1), len(text2))):

        total += 1

        if text1[i] == text2[i]:

            correct += 1

    return 100 * correct / total


def SolveHomoSub(Msg, Alphabet = "abcdefghijklmnopqrstuvwxyz"):

    #Msg = RemovePunctuation(Msg.lower()).strip("\n")
    ##Msg = Msg.lower().strip("\n")
    Msg = Msg.strip("\n")

    File = open("C# Programs\\--HomophonicBigramMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveHomoSubBigram.exe", str(Alphabet)], cwd = "C# Programs")

    return

def SolveTranspoSub(Msg, ColumnNum, transpoType, alphabet = "abcdefghijklmnopqrstuvwxyz"):

    Msg = Msg.lower().strip("\n")

    File = open("C# Programs\\--TranspoSubMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.Popen(["C# Programs\\SolveTranspoSub.exe", str(Alphabet), str(transpoType), str(columnNum)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolveTranspoSub.exe", str(alphabet), str(transpoType), str(ColumnNum)], cwd = "C# Programs")

    return

def SolveTranspoVigen(Msg, ColumnNum, transpoType, period, alphabet = "abcdefghijklmnopqrstuvwxyz"):

    Msg = Msg.lower().strip("\n")

    #File = open("C# Programs\\--TranspoSubMessage.txt", "w")
    File = open("C# Programs\\--TranspoVigenMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.Popen(["C# Programs\\SolveTranspoSub.exe", str(alphabet), str(transpoType), str(ColumnNum), str(period)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolveTranspoVigen.exe", str(alphabet), str(transpoType), str(ColumnNum), str(period)], cwd = "C# Programs")

    return


def BruteVigenTranspo(Msg, ColumnNum, transpoType, alphabet = "abcdefghijklmnopqrstuvwxyz"):

    Msg = Msg.lower().strip("\n")

    File = open("C# Programs\\--BruteVigenTranspoMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\BruteVigenTranspo.exe", str(alphabet), str(transpoType), str(ColumnNum)], cwd = "C# Programs")

    return

def SolveVigenTranspo(Msg, ColumnNum, transpoType, alphabet = "abcdefghijklmnopqrstuvwxyz"):

    Msg = Msg.lower().strip("\n")

    File = open("C# Programs\\--VigenTranspoMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveVigenTranspo.exe", str(alphabet), str(transpoType), str(ColumnNum)], cwd = "C# Programs")

    return

#def LongSolveHomoSub(Msg, Alphabet = "abcdefghijklmnopqrstuvwxyz"):
def SolveHomoSubQuadgram(Msg, Alphabet = "abcdefghijklmnopqrstuvwxyz"):

    Msg = Msg.strip("\n")

    #File = open("C# Programs\\--HomophonicBigramMessage.txt", "w")
    File = open("C# Programs\\--HomophonicQuadgramMessage.txt", "w")

    File.write(Msg)

    File.close()

    #subprocess.Popen(["C# Programs\\LongSolveHomoSubBigram.exe", str(Alphabet)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolveHomoSubQuadgram.exe", str(Alphabet)], cwd = "C# Programs")

    return


def DecryptHomoSub(ciphertext, key):

    plaintext = ""

    for i in ciphertext.split():

        if i in key:
            
            plaintext = plaintext + key[i]

        else:

            plaintext = plaintext + i


    return plaintext

#def DecryptHomoSubShowSymbols(ciphertext, key, textBoxWidth = 70):
def DecryptHomoSubShowSymbols(ciphertext, key, textBoxWidth = 90):

    plaintext = ""

    ciphertext = ciphertext.split()

    for i in ciphertext:

        if i in key:
            
            plaintext = plaintext + key[i] + " "

        else:

            plaintext = plaintext + i + " "

        
    plaintext = plaintext.split()

    outputPlaintext = ""
    #outputSymbol = ""
    outputCiphertext = ""

    for i in range(0, len(plaintext)):

        #if len(ciphertext[i]) > len(plaintext):
        if len(ciphertext[i]) > len(plaintext[i]):

            #outputPlaintext = outputPlaintext + " " * (len(ciphertext[i]) - len(plaintext)) + plaintext[i]
            outputPlaintext = outputPlaintext + " " * (len(ciphertext[i]) - len(plaintext[i])) + plaintext[i]
            outputCiphertext = outputCiphertext + ciphertext[i]

        else:

            #outputCiphertext = outputCiphertext + " " * (len(plaintext[i]) - len(ciphertext)) + ciphertext[i]
            outputCiphertext = outputCiphertext + " " * (len(plaintext[i]) - len(ciphertext[i])) + ciphertext[i]
            outputPlaintext = outputPlaintext + plaintext[i]

        outputPlaintext = outputPlaintext + " "
        outputCiphertext = outputCiphertext + " "

    outputPlaintext = outputPlaintext.rstrip("")
    outputCiphertext = outputCiphertext.rstrip("")

    output = ""

    while len(outputPlaintext) > 0:
        
        output = output + outputPlaintext[:textBoxWidth]
        outputPlaintext = outputPlaintext[textBoxWidth:]

        output = output + "\n"

        output = output + outputCiphertext[:textBoxWidth]
        outputCiphertext = outputCiphertext[textBoxWidth:]

        output = output + "\n\n"

    output = output.strip("\n")

    #return plaintext
    return output


def SortHomoSubKey(key):

    newKey = key.split("\n")

    outputKey = ""

    for i in range(0, len(newKey)):

        newKey[i] = newKey[i].split()

##    allNumbers = True
##
##    for i in newKey:
##
##        if not i[0].isnumeric():
##
##            allNumbers = False
##
##    if allNumbers:
##
##        for i in range(0, len(newKey)):
##
##            newKey[i][0] = int(newKey[i][0])
##
##        newKey.sort(key = lambda x: x[0])
##
##        for i in newKey:
##
##            outputKey = outputKey + str(i[0]) + " " + i[1] + "\n"
##
##    else:
##
##        return key

    newKey.sort(key = lambda x: x[0])

    for i in newKey:

        outputKey = outputKey + str(i[0]) + " " + i[1] + "\n"

    outputKey = outputKey.strip("\n").strip(" ")

    return outputKey


def SolveMyszTranspo(Msg, ColumnNum):

    Msg = Msg.lower().strip("\n")

    #File = open("C# Programs\\----MyszkowskiMessage.txt", "w")
    File = open("C# Programs\\--MyszkowskiMessage.txt", "w")

    File.write(Msg)

    File.close()

    subprocess.Popen(["C# Programs\\SolveMyszkowski.exe", str(ColumnNum)], cwd = "C# Programs")

    return


def SolveAMSCO(msg, columnNum, chunkSize):

    msg = msg.lower().strip("\n")

    file = open("C# Programs\\--AMSCOMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveAMSCO.exe", str(columnNum), str(chunkSize)], cwd = "C# Programs")

    return


def SolvePlayfairCSharp(msg, alphabet, height, width):

    msg = msg.lower().strip("\n")

    file = open("C# Programs\\--PlayfairMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolvePlayfair.exe", str(alphabet), str(height), str(width)], cwd = "C# Programs")

    return

def SolveStraddleVigenere(msg, period, rowNum, columnNum, alphabet):

    msg = msg.lower().strip("\n")

    file = open("C# Programs\\--StraddleVigenereMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveStraddleVigenere.exe", str(period), str(rowNum), str(columnNum), str(alphabet)], cwd = "C# Programs")

    return


def ExamineText(text):

    report = ""

    report = "Summary:\n--------\n\n"

    ioc = CharIOC(text)

    report = report + "IOC:\t" + str(ioc)

    ioc = CharNGramIOC(text, 2)

    report = report + "\n\nDigraph IOC:\t" + str(ioc)

    #report = report + "\n\n--------------------\n\n"
    report = report + "\n\n------------------------------\n\n"

    report = report + "Character Counts:\n-----------------\n\n"

    #charCounts = CountChars(text)
    charCounts = CountChars(text, "abcdefghijklmnopqrstuvwxyz0123456789")

    total = sum(charCounts.values())

    #for i in sorted(charCounts.keys(), key = lambda x: charCounts[x]):
    for i in reversed(sorted(charCounts.keys(), key = lambda x: charCounts[x])):

        char = i

        if char == "\n":

            char = "\\n"

        elif char == "\t":

            char = "\\t"

        elif char == " " or char == "\s":

            #print (char)

            #char == "\\s"
            #char == r"\s"
            #char == "<s>"
            char = "\\s"

            #print ("Ja")
            #print (char)
            print (char)

        #report = report + i + "\t" + str(charCounts[i]) + "\t" + str("%.1f" % (100 * charCounts[i] / total)) + "\n"
        report = report + char + "\t" + str(charCounts[i]) + "\t" + str("%.1f" % (100 * charCounts[i] / total)) + "\n"

    return report

#def NToTheMArragements(n, m):
def NToTheMArrangements(n, m):

    #return [[j % (n**(m-i)) for i in range(0, m)] for j in range(0, n**m)]
    #return [[j // (n**(m-i)) % n for i in range(0, m)] for j in range(0, n**m)]
    return [[j // (n**(m-i-1)) % n for i in range(0, m)] for j in range(0, n**m)]


def SolveNGraphVigenere(msg, n, period, alphabet):

    msg = msg.lower().strip("\n")

    file = open("C# Programs\\--NGraphVigenereMessage.txt", "w")

    file.write(msg)

    file.close()

    if alphabet == "":

        #for i in NToTheMArrangements(26, n):

            #alphabet = alphabet + "".join(alphabetlist[x] for x in i)

        #alphabet = alphabet + "".join("".join(alphabetlist[x] for x in i) for i in NToTheMArrangements(26, n))
        alphabet = "".join("".join(alphabetlist[x] for x in i) for i in NToTheMArrangements(26, n))

    file = open("C# Programs\\--NGraphVigenereAlphabet.txt", "w")

    file.write(alphabet)

    file.close()

    #subprocess.Popen(["C# Programs\\SolveNGraphVigenere.exe", str(period), str(alphabet), str(n)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolveNGraphVigenere.exe", str(period), str(n)], cwd = "C# Programs")

    return


def SolvePollux(msg, alphabet, numDash, numDot, numSep):

    #msg = msg.lower().strip("\n")
    msg = msg.strip("\n")

    file = open("C# Programs\\--PolluxMessage.txt", "w")

    file.write(msg)

    file.close()

    #subprocess.Popen(["C# Programs\\SolvePollux.exe", str(alphabet)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolvePollux.exe", str(alphabet), str(numDash), str(numDot), str(numSep)], cwd = "C# Programs")

    return


#def BruteABCDEFGHIK(msg, columnNum, transpoType, alphabet):
def BruteABCDEFGHIK(msg, columnNum, transpoType, alphabet, ngramLength):

    #msg = msg.lower().strip("\n")
    msg = msg.strip("\n")

    file = open("C# Programs\\--BruteABCDEFGHIKMessage.txt", "w")

    file.write(msg)

    file.close()

    #subprocess.Popen(["C# Programs\\BruteABCDEFGHIK.exe", str(columnNum), str(transpoType), str(alphabet)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\BruteABCDEFGHIK.exe", str(columnNum), str(transpoType), str(alphabet), str(ngramLength)], cwd = "C# Programs")

    return

def NormaliseNihilist(msg):

    msg = msg.split()

    newMsg = ""

    for i in range(0, len(msg)):

        #pass

        if len(msg[i]) > 2:

            newMsg = newMsg + "0" + msg[i][-1]

        else:

            newMsg = newMsg + msg[i]

        newMsg =  newMsg + " "

    newMsg = newMsg.strip()

    return newMsg


#def SolvePolyalphabetic(msg, period, alphabet):
#def SolvePolyalphabetic(msg, period, alphabet, step):
def SolvePolyalphabetic(msg, period, alphabet, step, alphabetChangePeriod):

    #msg = msg.lower().strip("\n")
    msg = msg.strip("\n")

    file = open("C# Programs\\--PolyalphabeticMessage.txt", "w")

    file.write(msg)

    file.close()

    #subprocess.Popen(["C# Programs\\SolvePolyalphabetic.exe", str(alphabetic), str(period)], cwd = "C# Programs")
    #subprocess.Popen(["C# Programs\\SolvePolyalphabetic.exe", str(alphabet), str(period)], cwd = "C# Programs")
    #subprocess.Popen(["C# Programs\\SolvePolyalphabetic.exe", str(alphabet), str(period), str(step)], cwd = "C# Programs")
    subprocess.Popen(["C# Programs\\SolvePolyalphabetic.exe", str(alphabet), str(period), str(step), str(alphabetChangePeriod)], cwd = "C# Programs")

    return


def SolveSpecificTransposition(msg, columnNum, transpoType):

    msg = msg.lower().strip("\n")

    file = open("C# Programs\\--TranspoMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveTranspo.exe", str(columnNum), str(transpoType)], cwd = "C# Programs")

    return


def SolveBazeries(msg, alphabet):

    msg = msg.strip("\n")

    file = open("C# Programs\\--BazeriesMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveBazeries.exe", str(alphabet)], cwd = "C# Programs")

    return

def SolveNicodemus(msg, alphabet, columnNum, chunkSize):

    msg = msg.strip("\n")

    file = open("C# Programs\\--NicodemusMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveNicodemus.exe", str(alphabet), str(columnNum), str(chunkSize)], cwd = "C# Programs")

    return


def SolveCadenus(msg, vertAlphabet, columnNum):

    msg = msg.strip("\n")

    file = open("C# Programs\\--CadenusMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveCadenus.exe", str(columnNum), str(vertAlphabet)], cwd = "C# Programs")

    return


def SolveProgressiveKey(msg, period, alphabet, progressionIndex):

    msg = msg.strip("\n")

    file = open("C# Programs\\--ProgressiveKeyMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveProgressiveKey.exe", str(period), str(progressionIndex), str(alphabet)], cwd = "C# Programs")

    return

def SolveRagbaby(msg, alphabet, startIndex):

    msg = msg.strip("\n")

    file = open("C# Programs\\--RagbabyMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveRagbaby.exe", str(startIndex), str(alphabet)], cwd = "C# Programs")

    return


def SolveInterruptedKey(msg, alphabet):

    msg = msg.strip("\n")

    file = open("C# Programs\\--InterruptedKeyMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveInterruptedKey.exe", str(alphabet)], cwd = "C# Programs")

    return


def SolveCustomSubstitutionCSharp(msg, alphabet, numOfTrials):

    msg = msg.strip("\n")

    file = open("C# Programs\\--MonoSubMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolveMonoSub.exe", str(alphabet), str(numOfTrials)], cwd = "C# Programs")

    return

def SolvePolybiusVigenere(msg, n, period, alphabet):

    msg = msg.strip("\n")

    file = open("C# Programs\\--PolybiusVigenMessage.txt", "w")

    file.write(msg)

    file.close()

    subprocess.Popen(["C# Programs\\SolvePolybiusVigen.exe", str(period), str(alphabet), str(n)], cwd = "C# Programs")

    return


def ASCIIToLetter(msg, base = 2):
    msg = msg.split(" ")
    newMsg = ""

    for i in msg:
        newMsg = newMsg + chr(int(i, base))

    return newMsg

def LetterToASCII(msg, base = 2):
    #msg = RemoveSpaces(msg)
    newMsg = ""

    for i in msg:
        newMsg = newMsg + "0" * (8 - len(BaseNToBaseM(ord(i), 10, base))) + BaseNToBaseM(ord(i), 10, base) + " "

    return newMsg.rstrip(" ")
