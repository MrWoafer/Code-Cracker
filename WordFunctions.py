from Alphabet import *
from googletrans import *

translator = Translator()

def RemovePunctuation(Msg):

    NewMsg = ""

    for letter in Msg:

        if letter == " ":

            NewMsg = NewMsg + " "

        #elif letter in alphabetset:
        elif letter.lower() in alphabetset:

            NewMsg = NewMsg + letter

    return NewMsg


def RemoveSpaces(Msg):

    NewMsg = ""

    for x in Msg:

        if x != " ":

            NewMsg = NewMsg + x

    return NewMsg



def ContainsEnglish(Msg, Dictionary, MinLength):

    Word = ""

    for letter in Msg:

        Word = Word+letter

        if Word not in Dictionary:

            Word = Word[:-1]

            if len(Word) >= MinLength:

                return (True, Word)

    return (False, None)

#def Translate(text, sourceLanguage = "ge", destinationLanguage = "en"):
def Translate(text, sourceLanguage = "de", destinationLanguage = "en"):

    return translator.translate(text, src = sourceLanguage, dest = destinationLanguage).text

def DetectLanguage(text):

    return translator.detect(text).lang
