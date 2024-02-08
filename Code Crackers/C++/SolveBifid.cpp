#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>
#include <time.h>

using namespace std;


/*/DisplayRow(int* Row[]){*/
//DisplayRow(int* Row[]){
//DisplayRow(int Row[]){
DisplayRow(int Row[], int Length){
	int i;
	//for (i = 0; i < Row.length(); i++){
	for (i = 0; i < Length; i++){
		cout << " " << Row[i];
	}
}


void DisplayKey(char Key[5][5])
{
    int y;
    int x;
    for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            cout << " " << Key[x][y];
        }
        cout << "\n";
    }
    return;
}


void FindLetterInKey(char Letter, char Key[5][5], int * OutputArray)
{
    OutputArray[0], OutputArray[1] = 5, 5;
    int y;
    int x;
    for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            if (Key[x][y] == Letter){
                OutputArray[0] = x;
                OutputArray[1] = y;
            }
        }
    }
    return;
}


int CycleRound(int Num, int Min, int Max)
{
    while (Num > Max){
          Num -= Max-Min+1;
          }
    while (Num < Min){
          Num += Max-Min+1;
          }
    return Num;
}


//string DecodePlayfair(string Msg, char Key[5][5], int Period)
string DecodeBifid(string Msg, char Key[5][5], int Period)
{
	string NewMsg;
	NewMsg = "";
	int Row1[Period];
	int Row2[Period];
	int Pos;
	
	float HalfPeriod = Period/2;
	int FlooredHalfPeriod = floor(Period/2);
   
	string NewKey;
	int x;
	int y;
   
	for (y = 0; y < 5; y++){
		for (x = 0; x < 5; x++){
			NewKey += Key[x][y];
		}
	}
   
	char * KeyString = new char [26];
   
	strcpy(KeyString, NewKey.c_str());
   
	int i;
	int j;
	for (i = 0; i < Msg.length()+1-Period; i += Period){
		
		for (j = 0; j < Period; j++){
			Pos = strchr(KeyString, Msg[i+j])-KeyString;
			if (Period%2 == 0){
				if (j < HalfPeriod){
					Row1[2*j] = floor(Pos/5);
					Row1[2*j+1] = Pos%5;
				}
				
				else {
					//Row2[2*(j-HalfPeriod)] = floor(Pos/5);
					//Row2[2*(j-HalfPeriod)+1] = Pos%5;
					Row2[2*j-Period] = floor(Pos/5);
					Row2[2*j-Period+1] = Pos%5;
				}
			}
			
			else {
				//cout << "YOPE!";
				/*cout << "YOPE!\n";*/
				//if (j == HalfPeriod-0.50){
				if (j == FlooredHalfPeriod){
					Row1[Period-1] = floor(Pos/5);
					Row2[0] = Pos%5;
				}
				else if (j < HalfPeriod){
					Row1[2*j] = floor(Pos/5);
					Row1[2*j+1] = Pos%5;
				}
				
				else {
					//Row2[1+2*(j-HalfPeriod)] = floor(Pos/5);
					//Row2[1+2*(j-HalfPeriod)+1] = Pos%5;
					//Row2[1+2*j-Period] = floor(Pos/5);
					//Row2[1+2*j-Period+1] = Pos%5;
					Row2[2*j-Period] = floor(Pos/5);
					Row2[2*j-Period+1] = Pos%5;
				}
			}
		}
		
		/*//cout << i << "/" << floor(Msg.length()/Period) << "\n";
		cout << i << "/" << Msg.length() << "\n";
		//DisplayRow(Row1);
		DisplayRow(Row1, Period);
		cout << "\n";
		//DisplayRow(Row2);
		DisplayRow(Row2, Period);
		cout << "\n\n";/**/
		
		for (j = 0; j < Period; j++){
			NewMsg += Key[Row2[j]][Row1[j]];
		}
		
	}
	return NewMsg;
}


double Score(string Msg, char Key[5][5], int Period)
{
	
	string DecodedMsg;
	//DecodedMsg = DecodePlayfair(Msg, Key, Period);
	DecodedMsg = DecodeBifid(Msg, Key, Period);
	
	double QuadScore;
	QuadScore = QuadgramScore(DecodedMsg);
	
	return QuadScore;
}


void SwapLetters(char (&Key)[5][5])
{
	int x1;
	int y1;
	int x2;
	int y2;
	
	x1 = rand()%5;
	y1 = rand()%5;
	x2 = x1;
	y2 = y1;
	while ((x1 == x2) && (y1 == y2)){
		x2 = rand()%5;
		y2 = rand()%5;
	}
	
	char Temp;
	Temp = Key[x1][y1];
	Key[x1][y1] = Key[x2][y2];
	Key[x2][y2] = Temp;
}


void SwapRows(char (&Key)[5][5])
{
	int y1;
	int y2;
	
	y1 = rand()%5;
	y2 = y1;
	while (y1 == y2){
		y2 = rand()%5;
	}
	
	char Temp[5];
	int i;
	for (i = 0; i < 5; i++){
		Temp[i] = Key[i][y1];
	}
	
	for (i = 0; i < 5; i++){
		Key[i][y1] = Key[i][y2];
		Key[i][y2] = Temp[i];
	}
}


void SwapColumns(char (&Key)[5][5])
{
	int x1;
	int x2;
	
	x1 = rand()%5;
	x2 = x1;
	while (x1 == x2){
		x2 = rand()%5;
	}
	
	char Temp[5];
	int i;
	for (i = 0; i < 5; i++){
		Temp[i] = Key[x1][i];
	}
	
	for (i = 0; i < 5; i++){
		Key[x1][i] = Key[x2][i];
		Key[x2][i] = Temp[i];
	}
}


void SwapAllColumns(char (&Key)[5][5])
{
	char Temp[5][5];
	int x;
	int y;
	int i;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	char TempItem;
	for (x = 0; x < 2; x++){
		for (i = 0; i < 5; i++){
			TempItem = Key[x][i];
			Key[x][i] = Key[4-x][i];
			Key[4-x][i] = TempItem;
		}
	}
}


void SwapAllRows(char (&Key)[5][5])
{
	char Temp[5][5];
	int x;
	int y;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	char TempItem;
	int i;
	for (y = 0; y < 2; y++){
		for (i = 0; i < 5; i++){
			TempItem = Key[i][y];
			Key[i][y] = Key[i][4-y];
			Key[i][4-y] = TempItem;
		}
	}
}


void ReverseColumn(char (&Key)[5][5])
{
	int x1;
	
	x1 = rand()%5;
	
	char Temp[5];
	int i;
	for (i = 0; i < 5; i++){
		Temp[i] = Key[x1][i];
	}
	
	for (i = 0; i < 5; i++){
		Key[x1][i] = Temp[4-i];
	}
}


void ReverseRow(char (&Key)[5][5])
{
	int y1;
	
	y1 = rand()%5;
	
	char Temp[5];
	int i;
	for (i = 0; i < 5; i++){
		Temp[i] = Key[i][y1];
	}
	
	for (i = 0; i < 5; i++){
		Key[i][y1] = Temp[4-i];
	}
}


void ReverseKey(char (&Key)[5][5])
{
	char Temp[5][5];
	int x;
	int y;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			Key[x][y] = Temp[4-x][4-y];
		}
	}
}


void MessAroundWithTheKey(char (&Key)[5][5])
{
	int Manoeuvre = rand()%50;
	switch (Manoeuvre){
		case 0: SwapRows(Key);
		case 1: SwapColumns(Key);
		case 4: ReverseKey(Key);
		case 5: SwapAllRows(Key);
		case 6: SwapAllColumns(Key);
		default: SwapLetters(Key);
	}
}


void CopyKey(char InputKey[5][5], char (&OutputKey)[5][5])
{
	int x;
	int y;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			OutputKey[x][y] = InputKey[x][y];
		}
	}
	return;
}


double BifidAnnealing(string Msg, char (&OutputKey)[5][5], int Period, float Temperature, float Step, int Count)
{
	char BestKey[5][5];
	CopyKey(OutputKey, BestKey);
	
	double BestScore;
	BestScore = Score(Msg, BestKey, Period);
	
	char CurrentKey[5][5];
	//.//CopyKey(StartingKey, CurrentKey);
	CopyKey(OutputKey, CurrentKey);
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		
		cout << T << "\n";
		
		for (Trial = 0; Trial < Count; Trial++){
			
			MessAroundWithTheKey(CurrentKey);
			CurrentScore = Score(Msg, CurrentKey, Period);
			
			if (CurrentScore > BestScore){
				
				BestScore = CurrentScore;
				CopyKey(CurrentKey, BestKey);
				
			}
			
			else if (CurrentScore < BestScore){
				
				bool ReplaceAnyway;
				
				ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
				
				if (ReplaceAnyway == true){
					
					BestScore = CurrentScore;
					CopyKey(CurrentKey, BestKey);
					
				}
				else {
					
					CopyKey(BestKey, CurrentKey);
					
				}
			}
			else {
				
				CopyKey(BestKey, CurrentKey);
				
			}
		}
	}
	CopyKey(BestKey, OutputKey);
	return BestScore;
}


//void SolvePlayfair(string Msg, int Period, char MissingLetter)
void SolveBifid(string Msg, int Period, string AlphabetForKey)
{
	char BestKey[5][5];
	
	int i;
	for (i = 0; i < 25; i++){
        //Key[i%5][floor(i/5)] = AlphabetForKey.at(i);
		BestKey[i%5][int(floor(i/5))] = AlphabetForKey.at(i);
    }
	
	for (i = 0; i < 100; i++){
		//SwapLetters(Key);
		SwapLetters(BestKey);
	}
	
	double BestScore = Score(Msg, BestKey, Period);
	
	cout.precision(17);
	
	char NewKey[5][5];
	double NewScore;
	
	DisplayKey(BestKey);
	cout << "\nScore: " << BestScore << "\n\n";
	cout << "--------------------------------------\n\n";
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		CopyKey(BestKey, NewKey);
		NewScore = BifidAnnealing(Msg, NewKey, Period, 20, 0.2, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			CopyKey(NewKey, BestKey);
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKey(BestKey);
			cout << "\n";
			string Decipherment = DecodeBifid(Msg, BestKey, Period);
			cout << "Decipherment: " << Decipherment;
			cout << "\n\n";
			
		}
		else {
			cout << "Didn't find a better key...";
			cout << "\n\n";
		}
		cout << "--------------------------------------\n\n";
	}
}


int main(int argc, char *argv[])
{    
	srand(time(0));

    int PERIOD;
	char MISSINGLETTER;
    string MESSAGE;
    
    PERIOD = atof(argv[1]);
	MISSINGLETTER = argv[2][0];
	
	ifstream File("Bifid Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);       
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "";
	
	int i;
	
	for (i = 0; i < 26; i++){
		if (Alphabet[i] != MISSINGLETTER){
			AlphabetForKey += Alphabet[i];
		}
	}
    
    //int i;
	int y;	
    char Key[5][5];
    for (i = 0; i < 25; i++){
        y = int(floor(i/5));
        Key[i%5][y] = AlphabetForKey.at(i);
    }
	
	char BestKey[5][5];
	CopyKey(Key, BestKey);
	
	char TestKey[5][5];
	
	//string TesKey
	string TestKeyString;
	TestKeyString = "ahgfemnbiwporuylskvzcxqdt";
	
	int j;
	for (i = 0; i < 25; i++){
		//TestKey[i%5][floor(i/5)] = TestKeyString[i];
		TestKey[i%5][int(floor(i/5))] = TestKeyString[i];
	}
	
	/*DisplayKey(TestKey);
	
	string TestDecode;
	
	TestDecode = DecodeBifid(MESSAGE, TestKey, PERIOD);
	
	cin.get();
	
	double TestScore;
	//TestScore = Score(MESSAGE, TestKey);
	TestScore = Score(MESSAGE, TestKey, PERIOD);
	
	cout << "Score: " << TestScore << "\nDecipherment: " << TestDecode << "\n\n";*/
	
	cout << "Starting Solve!\n\n";
	
	//SolveBifid(MESSAGE, PERIOD, MISSINGLETTER);
	SolveBifid(MESSAGE, PERIOD, AlphabetForKey);
	
	cin.get();
    
    return 1;
}
