import urllib
from bs4 import BeautifulSoup

def DownloadChallenge(textType, year, num, part):

    ChallengeText = "Unable to download that challenge yet."

    #if year == 2019:
    if year == 2020:

        extension = ""

        if num == 0:

            extension = "introduction"
            print ("Downloading introduction...")

        else:

            #print ("Downloading challenge " + str(num) + part + "...")
            #print ("Downloading challenge " + str(year) + " " + str(num) + part + "...")
            #print ("Need to add it to work for challenges other than introduction!")
            #extension = "challenge-" + str(num)

            if num <= 3:

                extension = "practice-challenge-" + str(num)

            else:

                extension = "competition-challenge-" + str(num)

        url = "https://www.cipherchallenge.org/challenge/" + extension

        print ("URL:", url)

        user_agent = 'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7'
        headers = {'User-Agent': user_agent}

        html = urllib.request.urlopen(urllib.request.Request(url, None, headers)).read()

        soup = BeautifulSoup(html, features = "html.parser")

        try:

            if num != 0:

                #if part.lower() == "a":
                #
                 #   ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content " + part.lower()}).text.strip("\n")

                #else:

                 #   ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content"}).text.strip("\n")

                if textType == "ciphertext":

                    ChallengeText = soup.body.find_all("div", attrs = {"class": "challenge__content"})[["a", "b"].index(part.lower())].text.strip("\n")

                else:

                    #ChallengeText = "Cannot retrieve 2019 plaintexts yet."
                    ChallengeText = soup.body.find_all("div", attrs = {"class": "challenge__answer"})[["a", "b"].index(part.lower())].text.strip("\n").lstrip("Correct Answer").lstrip("\n")

            else:

                #ChallengeText = soup.body.find("div", attrs = {"class": "challenge__contenta"}).text.strip("\n")
                #ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content a"}).text.strip("\n").lstrip("Correct Answer").lstrip("\n")
                ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content a"}).text.strip("\n")
                #print (ChallengeText)

        except Exception as exc:

            #ChallengeText = "Unable to download."
            ChallengeText = "Unable to download.\n\nException: " + str(exc)

    #elif year in [2018, 2017, 2016]:
    elif False:

        extension = ""

        if year == 2016 and num == 0:

            ChallengeText = "2016 has no introduction / challenge 0."

        else:

            if year != 2018 or num != 10:
                
                extension = "challenge-" + str(num)

            else:

                #extensions = "competition-challenge-"
                extension = "competition-challenge-10"
                
            #print ("Downloading challenge " str(year) + " " + str(num) + part + "...")
            print ("Downloading challenge " + str(year) + " " + str(num) + part + "...")

            #url = "https://www.cipherchallenge.org/challenges/" + extension

            
            url = "https://" + str(year) + ".cipherchallenge.org/challenges/" + extension

            print ("URL:", url)

            user_agent = 'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7'
            headers = {'User-Agent': user_agent}

            html = urllib.request.urlopen(urllib.request.Request(url, None, headers)).read()

            soup = BeautifulSoup(html, features = "html.parser")

            try:

                if num != 0:

                    #if part.lower() == "a":
                    #
                    #    ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content " + part.lower()}).text.strip("\n")
                    #
                    #else:

                        #ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content"}).text.strip("\n")
                        #ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content", "id": "#challenge-b__target"}).text.strip("\n")
                        #ChallengeText = soup.body.find_all("div", attrs = {"class": "challenge__content", "id": "#challenge-b__target"})[1].text.strip("\n")
                    #    ChallengeText = soup.body.find_all("div", attrs = {"class": "challenge__content"})[1].text.strip("\n")

                    ChallengeText = soup.body.find_all("div", attrs = {"class": "challenge__content"})[["a", "b"].index(part.lower())].text.strip("\n")

                else:

                    #ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content a"}).text.strip("\n")
                    ChallengeText = soup.body.find("div", attrs = {"class": "challenge__content"}).text.strip("\n")

            except Exception as exc:

                #ChallengeText = "Unable to download."
                ChallengeText = "Unable to download.\n\nException: " + str(exc)

    else:
        
        #ChallengeText = "Cannot yet retrieve that year's challenges."
        #ChallengeText = open("..\\Archive\\Updated 7-Oct-2019\\" + year + "\\" + str(num) + part.upper() + "\\ciphertext.txt").read()

        if textType == "ciphertext":
            
            ChallengeText = open("..\\Archive\\Updated 7-Oct-2019\\" + str(year) + "\\" + str(num) + part.upper() + "\\ciphertext.txt").read()

        else:

            ChallengeText = open("..\\Archive\\Updated 7-Oct-2019\\" + str(year) + "\\" + str(num) + part.upper() + "\\plaintext.txt").read()

    print ("Download complete.")

    return ChallengeText


def UploadChallenge(text, year, num, part):

    if year == 2019:

        if num <= 3:

            extension = "practice-challenge-" + str(num)

        else:

            extension = "competition-challenge-" + str(num)

    url = "https://www.cipherchallenge.org/challenge/" + extension

    #print ("Uploading to " + str(year) + " Challenge " + str(num) + str(part))
    print ("Uploading to " + str(year) + " Challenge " + str(num) + str(part) + "...")
    print ("URL:", url)

    values = {"answer": text}

    # pretend to be a chrome 47 browser on a windows 10 machine
    headers = {"User-Agent": "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7"}

    # encode values for the url
    params = urllib.parse.urlencode(values).encode("utf-8")

    # create the url
    targetUrl = urllib.request.Request(url=url, data=params, headers=headers)

    # open the url
    x = urllib.request.urlopen(targetUrl)

    # read the response
    respone = x.read()
    #print(respone)

    print ("Uploaded. (I hope.)")
