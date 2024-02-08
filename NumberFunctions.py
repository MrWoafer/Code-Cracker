#NumberFunctions Module (Was originally called "Numbers", but that is already a module)

from math import *

import random


def positivify(num):

    if isinstance(num, tuple) or isinstance(num, list):

        numlist = []

        for x in range(0, len(num)):

            if num[x] < 0:

                numlist.append(num[x] * -1)

            else:

                numlist.append(num[x])

        return numlist
    

    elif isinstance(num, int) or isinstance(num, float):

        if num < 0:

            return num * -1

        else:

            return num


def negativify(num):

    if isinstance(num, tuple) or isinstance(num, list):

        numlist = []

        for x in range(0, len(num)):

            if num[x] > 0:

                numlist.append(num[x] * -1)

            else:

                numlist.append(num[x])

        return numlist
    

    elif isinstance(num, int) or isinstance(num, float):

        if num > 0:

            return num * -1

        else:

            return num



def switchsign(num):

    if isinstance(num, tuple) or isinstance(num, list):

        numlist = []

        for x in range(0, len(num)):

            numlist.append(num[x] * -1)

        return numlist


    elif isinstance(num, int) or isinstance(num, float):

        return num * -1


def roundnum(num):

    if isinstance(num, float) or isinstance(num, int):

        tempnum = num

        if num > 0:

            while tempnum > 1:

                tempnum -= 1

            if tempnum < 0.5:

                num = floor(num)

            elif tempnum >= 0.5:

                num = ceil(num)


        elif num < 0:

            while tempnum < -1:

                tempnum += 1

            if tempnum > -0.5:

                num = floor(num)

            elif tempnum <= -0.5:

                num = ceil(num)

        return num


def roundnums(nums):

    if isinstance(nums, tuple) or isinstance(nums, list):

        nums = list(nums)

        for x in range(0, len(nums)):

            tempnum = nums[x]

            if nums[x] > 0:

                while tempnum > 1:

                    tempnum -= 1

                if tempnum < 0.5:

                    nums[x] = floor(nums[x])

                elif tempnum >= 0.5:

                    nums[x] = ceil(nums[x])


            elif nums[x] < 0:

                while tempnum < -1:

                    tempnum += 1

                if tempnum > -0.5:

                    nums[x] = floor(nums[x])

                elif tempnum <= -0.5:

                    num[x] = ceil(nums[x])

        return nums



def weightedrand(weights):

    if isinstance(weights, tuple) or isinstance(weights, list):

        weights = list(weights)

        total = 0

        for x in range(0, len(weights)):

            if weights[x] < 0:

                weights[x] = 0

            total += weights[x]

        #print (total)

        rand = random.randint(1, total)

        #print (rand)

        runningtotal = 0

        for x in range(0, len(weights)):

            runningtotal += weights[x]

            if rand <= runningtotal:

                return x



def lowestcommonmultiple(nums):

    if isinstance(nums, tuple) or isinstance(nums, list):

        if len(nums) > 1:

            Found = False

            curnum = 0

            while not Found:

                curnum += nums[0]

                Found = True

                for x in range(1, len(nums)):

                    if curnum % nums[x] != 0:

                        Found = False

                if Found:

                    return curnum


def highestcommonfactor(nums):

    if isinstance(nums, tuple) or isinstance(nums, list):

        if len(nums) > 1:

            curfactor = 1

            for x in range(1, nums[0]):

                Found = True

                for y in range(0, len(nums)):

                    if nums[y] % x != 0:

                        Found = False

                if Found:

                    curfactor = x

            return curfactor



def ismultiple(multiple, multipleof):

    return multiple % multipleof == 0

def isfactor(factor, factorof):

    return factorof % factor == 0


def bubblesort(List, Reverse = False):

    if isinstance(List, tuple) or isinstance(List, list):

        Found = True

        while Found:

            Found = False

            for x in range(0, len(List)-1):

                if List[x] > List[x+1]:

                    List.insert(x+2, List[x])

                    del List[x]

                    Found = True

        if Reverse:

            List.reverse()

    return List


def selectionsort(List, Reverse = False):

    if isinstance(List, tuple) or isinstance(List, list):

        for x in range(0, len(List)):

            lowest = min(List[x:])

            for y in range(x, len(List)):

                if List[y] == lowest:

                    del List[y]

                    break

            List.insert(x, lowest)

        if Reverse:

            List.reverse()

    return List


def square(x):

    if isinstance(x, int) or isinstance(x, float):

        return x**2

def squareroot(x, precision, decimalpoints = "all"):

    if isinstance(x, int) or isinstance(x, float):

        n = x//2

        for y in range(0, precision):

            n = (n+(x/n))/2

        if decimalpoints == "all":

            return n

        else:

            return float(str("%." + str(decimalpoints) + "f") % n)

def triangle(num):

    if isinstance(num, int) or isinstance(num, float):

        return (num**2+num)/2


def rounduptozero(num):

    if isinstance(num, int) or isinstance(num, float):

        if num < 0:

            return 0

        else:

            return num

def rounddowntozero(num):

    if isinstance(num, int) or isinstance(num, float):

        if num > 0:

            return 0

        else:

            return num


def roundupto(num, roundupto):

    if isinstance(num, int) or isinstance(num, float):

        if num < roundupto:

            return roundupto

        else:

            return num

def rounddownto(num, rounddownto):

    if isinstance(num, int) or isinstance(num, float):

        if num > rounddownto:

            return rounddownto

        else:

            return num


def cycle(num, top, bottom):

    if isinstance(num, int) or isinstance(num, float):

        if num > top:

            return bottom

        elif num < bottom:

            return top

        else:

            return num
            

            

    
