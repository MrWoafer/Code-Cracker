#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
//#include "QuadgramScores.h"
//#include "QuadgramScores.hpp"
//#include "Annealing.hpp"
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>

using namespace std;

//int main()
//{
//    cout << "Woo, this works! 'I mean, Hello World!'";
//    cin.get();
//    
//    return 1;
//}


//int DisplayKey(Key)
//int DisplayKey(char Key[])
//int DisplayKey(char Key[][])
//int DisplayKey(char Key[5][5])
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
    //return 1;
    return;
}

//#int *FindLetterInKey(string Letter, char Key[5][5])
//int * FindLetterInKey(char Letter, char Key[5][5])
//int * FindLetterInKey(char Letter, char Key[5][5], int *OutputArray[2])
//int * FindLetterInKey(char Letter, char Key[5][5], int OutputArray[2])
//int * FindLetterInKey(char Letter, char Key[5][5], int *OutputArray)
//#int * FindLetterInKey(char Letter, char Key[5][5], int& OutputArray)
//int * FindLetterInKey(char Letter, char Key[5][5], int& OutputArray[2])
//void FindLetterInKey(char Letter, char Key[5][5], int& OutputArray[2])
//void FindLetterInKey(char Letter, char Key[5][5], int& OutputArray)
void FindLetterInKey(char Letter, char Key[5][5], int * OutputArray)
{
    OutputArray[0], OutputArray[1] = 5, 5;
    //static int Coords[2];
    int y;
    int x;
    for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            if (Key[x][y] == Letter){
                          //cout << Key[x][y] << Letter << " " << x << ", " << y << " \n";
                          //Coords[0], Coords[1] = x, y;
                          //OutputArray[0], OutputArray[1] = x, y;
                          OutputArray[0] = x;
                          OutputArray[1] = y;
                          //cout << OutputArray[0] << "," << OutputArray[1] << " \n";
                          //return Coords;
                          }
            }
        }
    //Coords[0], Coords[1] = 5, 5;
    //OutputArray[0], OutputArray[1] = 5, 5;
    //return Coords;
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


string DecodePlayfair(string Msg, char Key[5][5], bool DecodeAsDoubleLetter)
{
       string NewMsg;
       //string Letter1;
       //string Letter2;
       char Letter1;
       char Letter2;
       NewMsg = "";
       
       //int *Coords1[2];
       //int *Coords2[2];
       //int *Coords1;
       //int *Coords2;
       int Coords1[2];
       int Coords2[2];
	   
	   string NewKey;
	   //char NewKey[];
	   int x;
	   int y;
	   
	   for (y = 0; y < 5; y++){
		   for (x = 0; x < 5; x++){
			   NewKey += Key[x][y];
		   }
	   }
	   
	   //char [] KeyString = NewKey.c_str();
	   //char KeyString[] = NewKey.c_str();
	   //char KeyString[25] = NewKey.c_str();
	   
	   char * KeyString = new char [26];
	   
	   strcpy(KeyString, NewKey.c_str());
	   
	   /////cout << NewKey << " " << KeyString << "\n";
       
	   char DoubleLetter;
       DoubleLetter = 'x';
	   
       //int it;
       int i;
       //i = -2;
       i = 0;
       //while (i < Msg.length()-1){
		for (i = 0; i < Msg.length()-1; i += 2){
             Letter1 = Msg[i];
             Letter2 = Msg[i+1];
             
             //#string DoubleLetter;
             char DoubleLetter;
             //DoubleLetter = "x";
             //DoubleLetter = 'x';
             
             //if ((DecodeAsDoubleLetter == true) && (Letter1 == Letter2)){
             //if ((DecodeAsDoubleLetter == true) && (Letter2 == "x")){
             if ((DecodeAsDoubleLetter == true) && (Letter2 == DoubleLetter)){
                                       NewMsg += Letter1;
                                       NewMsg += Letter1;
                                       }
             //else if ((DecodeAsDoubleLetter == false) && (Letter2 == "x")){
             else if ((DecodeAsDoubleLetter == false) && (Letter2 == DoubleLetter)){
                  NewMsg += Letter1;
                  NewMsg += Letter2;
                  }
             else{
                 //Coords1 = FindLetterInKey(Letter1, Key);
                 //Coords2 = FindLetterInKey(Letter2, Key);
                 ////FindLetterInKey(Letter1, Key, Coords1);
                 ////FindLetterInKey(Letter2, Key, Coords2);
				 //Coords1[0] = 1; 
				 //Coords1[1] = 1; 
				 //Coords2[0] = 1; 
				 //Coords2[1] = 1; 
				 
				 //int Pos1 = strchr(NewKey, Letter1);
				 //int Pos1 = strchr(KeyString, Letter1);
				 //char * Pos1 = strchr(KeyString, Letter1);
				 ///////cout << "'Bout To Search!\n";
				 //int Pos1 = strchr(KeyString, Letter1)-KeyString+1;
				 int Pos1 = strchr(KeyString, Letter1)-KeyString;
				 //int Pos1 = 13;
				 Coords1[0] = Pos1%5;
				 Coords1[1] = floor(Pos1/5);
				 //int Pos2 = strchr(NewKey, Letter2);
				//int Pos2 = strchr(KeyString, Letter2);
				//char * Pos2 = strchr(KeyString, Letter2);
				//int Pos2 = strchr(KeyString, Letter2)-KeyString+1;
				int Pos2 = strchr(KeyString, Letter2)-KeyString;
				//int Pos2 = 24;
				 Coords2[0] = Pos2%5;
				 Coords2[1] = floor(Pos2/5);
				 ////cout << "I'm Done Searchin'!\n";
				 ///cout << Letter1 << Coords1[0] << "," << Coords1[1] << " " << Letter2 << Coords2[0] << "," << Coords2[1]<<"\n\n";

                 //NewMsg.append(Coords1);
                 //cout << Coords1 << Coords2 << " ";
                 //cout << Coords1[0] << Coords1[1] << "," << Coords2[0] << Coords2[1] << " ";
                 //#cout << Letter1 << Letter2 << " " << Coords1[0] << "," << Coords1[1]<< " " << Coords2[0] << "," << Coords2[1] << "\n";
                 //cout << Letter1 << Letter2 << " " << *(Coords1+0) << "," << *(Coords1+1)<< " " << *(Coords2+0) << "," << *(Coords2+1) << "\n";
                 
                 
                 
                 //else if (Coords1[0] == Coords2[0]){
                 if (Coords1[0] == Coords2[0]){
                                //NewMsg.append(Key[Coords1[0]][CycleRound(Coords1[1]-1, 0, 4)]);
                                //NewMsg.append(Key[Coords2[0]][CycleRound(Coords2[1]-1, 0, 4)]);
                                
                                Coords1[1] = CycleRound(Coords1[1]-1, 0, 4);
                                //Coords2[1] = CycleRound(Coords12[1]-1, 0, 4);
                                Coords2[1] = CycleRound(Coords2[1]-1, 0, 4);
                                
                                //#NewMsg.append(Key[Coords1[0]][Coords1[1]]);
                                //NewMsg.append(Key[Coords1[0]][Coords1[1]].c_str());
                                NewMsg += Key[Coords1[0]][Coords1[1]];
                                //NewMsg.append(Key[Coords2[0]][Coords2[1]]);
                                //NewMsg.append(Key[Coords2[0]][Coords2[1]].c_str());
                                NewMsg+= Key[Coords2[0]][Coords2[1]];
                                }
                                
                 else if (Coords1[1] == Coords2[1]){
                                
                                Coords1[0] = CycleRound(Coords1[0]-1, 0, 4);
                                Coords2[0] = CycleRound(Coords2[0]-1, 0, 4);
                                
                                NewMsg += Key[Coords1[0]][Coords1[1]];
                                NewMsg+= Key[Coords2[0]][Coords2[1]];
                                }
                                
                 else{
                      NewMsg += Key[Coords2[0]][Coords1[1]];
                      NewMsg+= Key[Coords1[0]][Coords2[1]];
                      }
                 }
             
             //i += 2;
             }
		//cout << "Decoded Msg: " << DecodedMsg << "\n\n\n";
		//cout << "Decoded Msg: " << NewMsg << "\n\n\n";
       return NewMsg;
}


//float Score(string Msg, char Key[5][5], bool DecodeAsDoubleLetter)
double Score(string Msg, char Key[5][5], bool DecodeAsDoubleLetter)
{
	//return -100;
	
	string DecodedMsg;
	//DecodedMsg = DecodePlayfair(Msg, Key[5][5], DecodeAsDoubleLetter);
	DecodedMsg = DecodePlayfair(Msg, Key, DecodeAsDoubleLetter);
	////
	//cout << "Decode not!\n";
	//DecodedMsg = Msg;
	
	//cout << "\n\n" << Msg << "\n" << DecodedMsg << "\n\n";
	
	//cout << "Decoded!\n";
	
	//float QuadScore;
	double QuadScore;
	QuadScore = QuadgramScore(DecodedMsg);
	//QuadScore = -100;
	//QuadScore = QuadgramScore(Msg);
	
	//cout << "Scored!\n";
	//cout << "Score: " << QuadScore << "\n";
	
	//cout << "Scored, BOY!\n";
	
	return QuadScore;
}


void SwapLetters(char (&Key)[5][5])
{
	//cout << "Swapping Letters!\n";
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


//void SwapRow(char (&Key)[5][5])
void SwapRows(char (&Key)[5][5])
{
	//cout << "Swapping Rows!\n";
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
	//cout << "Swapping Columns!\n";
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
	//cout << "Swapping All Columns!\n";
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
	//for (x = 0, x < 2; x++){
	for (x = 0; x < 2; x++){
		for (i = 0; i < 5; i++){
			TempItem = Key[x][i];
			Key[x][i] = Key[4-x][i];
			//Key[x2][i] = Temp[i];
			Key[4-x][i] = TempItem;
		}
	}
}


void SwapAllRows(char (&Key)[5][5])
{
	//cout << "Swapping All Rows!\n";
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
	//for (y = 0, y < 2; y++){
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
	//cout << "Reversing Columns!\n";
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


//void ReverseColumn(char (&Key)[5][5])
void ReverseRow(char (&Key)[5][5])
{
	//cout << "Reversing Row!\n";
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
	//cout << "Reversing Key!\n";
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
	//int Manoeuvre = rand()%52;
	//int Manoeuvre = rand()%60;
	switch (Manoeuvre){
		case 0: SwapRows(Key);
		case 1: SwapColumns(Key);
		////case 2: ReverseRow(Key);
		////
		///case 3: ReverseColumn(Key);
		///
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
			//OutputKey[x][y] = CopyKey[x][y];
			OutputKey[x][y] = InputKey[x][y];
		}
	}
	return;
}


//float PlayfairAnnealing(string Msg, char (&StartingKey)[5][5], float StartingScore; float Temperature, float Step, int Count, bool DecodeAsDoubleLetter)
//float PlayfairAnnealing(string Msg, char (&StartingKey)[5][5], float StartingScore, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter)
//float PlayfairAnnealing(string Msg, char (&StartingKey)[5][5], float StartingScore, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter)
double PlayfairAnnealing(string Msg, char (&StartingKey)[5][5], double StartingScore, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter)
{
	//float BestScore;
	double BestScore;
	BestScore = StartingScore;
	
	char BestKey[5][5];
	CopyKey(StartingKey, BestKey);
	
	char CurrentKey[5][5];
	CopyKey(StartingKey, CurrentKey);
	
	//float CurrentScore;
	//float CurrentScore;
	double CurrentScore;
	
	float T;
	int Trial;
	//for (T = Temperature; T > 0; T -= Step){
	for (T = Temperature; T >= 0; T -= Step){
		//cout << "Temp: " << T << "\n";
		//for (Trial = Count; Trial > 0; Trial--){
		for (Trial = 0; Trial < Count; Trial++){
			//cout << "Trial: " << Trial << "\n";
			/////////
			//if (false){
			if (true){
				MessAroundWithTheKey(CurrentKey);
				CurrentScore = Score(Msg, CurrentKey, DecodeAsDoubleLetter);
				//cout "Scored: " << CurrentScore << "!\n";
				//cout << "Scored: " << CurrentScore << "!\n";
				//CurrentScore = 0;
				//CurrentScore = BestScore+1;
				if (CurrentScore > BestScore){
					BestScore = CurrentScore;
					CopyKey(CurrentKey, BestKey);
				}
				else if (CurrentScore < BestScore){
					//float ProbabilityNum = rand()/RAND_MAX;
					bool ReplaceAnyway;
					ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
					//if (ReplaceAnyway == True){
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
	}
	/////////////////DisplayKey(BestKey);
	CopyKey(BestKey, StartingKey);
	return BestScore;
}


//void PlayfairAnnealing(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char * OutputKey)
//#//void SolvePlayfair(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char * OutputKey)
//void SolvePlayfair(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char * OutputKey[5])
//void SolvePlayfair(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char& OutputKey[5][5])
//void SolvePlayfair(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char **OutputKey)
void SolvePlayfair(string Msg, float Temperature, float Step, int Count, bool DecodeAsDoubleLetter, char (&OutputKey)[5][5])
{
	//char BestKey[5][5] = OutputKey;
	//char BestKey;
	char BestKey[5][5];
	//BestKey = OutputKey;
	//strcpy(BestKey, OutputKey);
	//strcpy(BestKey[][], OutputKey);
	CopyKey(OutputKey, BestKey);
	//CopyKey(OutputKey, *BestKey);
	//CopyKey(OutputKey, &BestKey);
	//float BestScore = Score(Msg, BestKey, DecodeAsDoubleLetter);
	double BestScore = Score(Msg, BestKey, DecodeAsDoubleLetter);
	
	cout.precision(17);
	
	//cout << "\n Starting Score: " << BestScore << "!\n";
	/*cout << "\n Starting Score: " << fixed << BestScore << "!\n";
	DisplayKey(BestKey);
	cout << "\n\n";*/
	
	//string StartDecode = DecodePlayfair(Msg, Key, DecodeAsDoubleLetter);
	/*string StartDecode = DecodePlayfair(Msg, BestKey, DecodeAsDoubleLetter);
	cout << "STart Decipherment: " << StartDecode << "\n\n\n\n";*/
	
	char NewKey[5][5];
	//float NewScore;
	double NewScore;
	//float
	
	int anneal;
	anneal = 0;
	//for (anneal = 0; anneal < 20; anneal++){
	while (true){
		anneal += 1;
		//cout << anneal << "\n";
		//cout << "Anneal: " << anneal << "\n\n";
		cout << "Trial: " << anneal << "\n\n";
		CopyKey(BestKey, NewKey);
		//NewScore = PlayfairAnnealing(Msg, BestKey, BestScore; Temperature, Step, Count, DecodeAsDoubleLetter);
		//NewScore = PlayfairAnnealing(Msg, BestKey, BestScore, Temperature, Step, Count, DecodeAsDoubleLetter);
		NewScore = PlayfairAnnealing(Msg, NewKey, BestScore, Temperature, Step, Count, DecodeAsDoubleLetter);
		
		////cout << "Current Key!\n";
		////cout << "Score: " << NewScore << "\n";
		////DisplayKey(NewKey);
		////cout << "\n\n";
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			CopyKey(NewKey, BestKey);
			//cout << "New Best Key!\n";
			cout << "New Best Key!\n\n";
			//cout << "Score: " << BestScore;
			//cout << "Score: " << BestScore << "\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKey(BestKey);
			cout << "\n";
			string Decipherment = DecodePlayfair(Msg, BestKey, DecodeAsDoubleLetter);
			//cout << Decipherment;
			cout << "Decipherment: " << Decipherment;
			cout << "\n\n";
			
		//cout << "\n\n\n";
		//cout << "--------------------------------------\n\n";
		}
		else {
			cout << "Didn't find a better key...";
			cout << "\n\n";
		}
		cout << "--------------------------------------\n\n";
	}
}


//int main(int argc, string *argv[])
int main(int argc, char *argv[])
{
    //int i;
    //cout << argv;
    //for (i = 0; i<5; i++)
    //{
    //    cout << argv[i] << " ";
    //}
    ofstream PythonInputFile;
    PythonInputFile.open("Playfair Msg To Crack.txt");
    
    float TEMPERATURE;
    float STEP;
    int COUNT;
    bool DECODEASDOUBLELETTER;
    string MESSAGE;
    
    //PythonInputFile >> TEMPERATURE >> STEP >> COUNT >> DECODEASDOUBLELETTER >> MESSAGE;
    
    //cout << TEMPERATURE << STEP << COUNT << DECODEASDOUBLELETTER << MESSAGE;
    
    //TEMPERATURE = float(argv[1]);
    //STEP = float(argv[2]);
    //COUNT = int(argv[3]);
    //DECODEASDOUBLELETTER = bool(argv[4]);
    //MESSAGE = string(argv[5]);
    TEMPERATURE = atof(argv[1]);
    STEP = atof(argv[2]);
    COUNT = atoi(argv[3]);
    //DECODEASDOUBLELETTER = atob(argv[4]);
    //MESSAGE = string(argv[5]);
    //MESSAGE = argv[5];
	
	//ifstream File;
	
	//File.open("Playfair Message.txt");
	
	ifstream File("Playfair Message.txt");
	
	//MESSAGE((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	File.seekg(0, ios::end);
	//MESSAGE.reserver(File.tellg());
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	//MESSAGE.lower();
	//trasnform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), tolower);
	//transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), tolower);
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);
	
	/////cout << MESSAGE;
    
    if (argv[4] == "True"){
                DECODEASDOUBLELETTER = true;
                } 
    if (argv[4] == "False"){
                DECODEASDOUBLELETTER = false;
                }          
    
    /////cout << TEMPERATURE << STEP << COUNT << DECODEASDOUBLELETTER << MESSAGE;
    
    //cout << MESSAGE;
    
    //string Key;
    //Key = "abcdefghijklmnopqrstuvwxyz";
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    int i;    
    char Key[5][5];
    //#for (i = 0; i < 26; i++){
    for (i = 0; i < 25; i++){
        //Key[i%5][floor(i/5)] = Alphabet[i];
        //Key[i%5][floor(i/5)] = Alphabet.at(i);
        //Key[i%5][atoi(floor(i/5))] = Alphabet.at(i);
        //Key[i%5][int(floor(i/5)] = Alphabet.at(i);
        int y;
        y = int(floor(i/5));
        //Key[i%5][y] = Alphabet.at(i);
        Key[i%5][y] = AlphabetForKey.at(i);
        }
    //cout << Key;
    //cout << Key[3][2];
    //cout << Key[0][4];
    //cout << Key[4][0];
    /////cout << "\n";
    /////cin.get();
    /////DisplayKey(Key);
    
    //////string DecodedMsgTest;
    ///DecodedMsgTest = DecodePlayfair(Msg, Key);
    //DecodedMsgTest = DecodePlayfair(MESSAGE, Key);
    //DecodedMsgTest = DecodePlayfair(MESSAGE, Key, DECODEASDOUBLELETTER);
    //cout << "\n" << DecodedMsgTest;
    
    /////cin.get();
	
	char BestKey[5][5];
	//BestKey = Key;
	//strcpy(BestKey, Key);
	CopyKey(Key, BestKey);
	
	////*/
	/*
	DisplayKey(Key);
	cout << "\n";
	SwapRows(Key);
	DisplayKey(Key);
	cout << "\n";
	SwapColumns(Key);
	DisplayKey(Key);
	cout << "\n";
	SwapLetters(Key);
	DisplayKey(Key);
	cout << "\n";
	ReverseRow(Key);
	DisplayKey(Key);
	cout << "\n";
	ReverseColumn(Key);
	DisplayKey(Key);
	cout << "\n";
	ReverseKey(Key);
	DisplayKey(Key);
	cout << "\n";
	SwapAllRows(Key);
	DisplayKey(Key);
	cout << "\n";
	SwapAllColumns(Key);
	DisplayKey(Key);
	cout << "\n";
	*/
	
	////*/cin.get();*/
	
	char TestKey[5][5];
	
	TestKey[0][0] = 'l';
	TestKey[1][0] = 'q';
	TestKey[2][0] = 'd';
	TestKey[3][0] = 'a';
	TestKey[4][0] = 'r';
	TestKey[0][1] = 's';
	TestKey[1][1] = 'u';
	TestKey[2][1] = 'm';
	TestKey[3][1] = 'b';
	TestKey[4][1] = 'n';
	TestKey[0][2] = 'y';
	TestKey[1][2] = 'i';
	TestKey[2][2] = 'o';
	TestKey[3][2] = 'w';
	TestKey[4][2] = 'e';
	TestKey[0][3] = 'v';
	TestKey[1][3] = 'k';
	TestKey[2][3] = 'p';
	TestKey[3][3] = 'f';
	TestKey[4][3] = 'g';
	TestKey[0][4] = 't';
	TestKey[1][4] = 'x';
	TestKey[2][4] = 'h';
	TestKey[3][4] = 'z';
	TestKey[4][4] = 'c';
	//TestKey[5][0] = '';
	//TestKey[5][1] = '';
	//TestKey[5][2] = '';
	//TestKey[5][3] = '';
	//TestKey[5][4] = '';
	
	string TestDecode;
	
	TestDecode = DecodePlayfair(MESSAGE, TestKey, DECODEASDOUBLELETTER);
	
	double TestScore;
	TestScore = Score(MESSAGE, TestKey, DECODEASDOUBLELETTER);
	
	/////cout << "\n\n" << "Actual Decipherment Score: " << TestScore << "\n";
	/////cout << "Actual Decipherment: " << TestDecode << "\n\n";
	
	/////DisplayKey(TestKey);
	
	
	/////cin.get();
	
	//cout << "Starting Solve!\n";
	cout << "Starting Solve!\n\n";
	
	//PlayfairAnnealing(MESSAGE, TEMPERATURE, STEP, COUNT, DECODEASDOUBLELETTER, BestKey);
	SolvePlayfair(MESSAGE, TEMPERATURE, STEP, COUNT, DECODEASDOUBLELETTER, BestKey);
	
	cin.get();
    
    return 1;
}
