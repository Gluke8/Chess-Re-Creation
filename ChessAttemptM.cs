﻿//WIP making chess, I will be working on in this in the near future
//So, I made it as a repo, and may convert to kotlin
string[,] board = new string[8, 8]; string[,] boardA = new string[8, 8]; // arrays
string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };

const string boardDoc = "board.csv";           //variables for reading "board.csv" values; accessing and reading
StreamReader sr = new StreamReader(boardDoc);
string scroll = "";

bool player = true; //player true; player 1's turn, player false; player 2's turn
int Phase = 1;  //Whilst phase == 1; character select to move, phase == 0; select to place

int temp = 0;         //group of variables used to determine piece moves
int temp2 = 0;
string newTemp = "";

bool[,] valid = new bool[8, 8];
char opposer = '0';



////////////////////////////// METHODS //////////////////////////////

void print() //interface
{
    Console.WriteLine("  ___________________________\n |                           |");
    for (int i = 0; i < 8; i++)
    {
        Console.Write(i + 1 + "|  ");
        for (int j = 0; j < 8; j++)
        {
            Console.Write(board[i, j]);
        }
        Console.Write(" |\n");
    }
    Console.WriteLine(" |___________________________|");
    Console.Write("    ");
    for (int i = 0; i < 8; i++)
    {
        Console.Write(letters[i] + "  ");
    }
}

string turn()
{
    if (player)
    {
        opposer = '2';
        return "1";

    }
    else
    {
        opposer = '1';
        return "2";
    }
}

void move(string who)
{
    Console.WriteLine();
    Console.WriteLine("\nPlayer " + who + " select who to move. 'a1' for example.");
    string sel = Console.ReadLine();
    spot(sel);
}

void spot(string here)
{
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            if (boardA[i, j] == here)
            {
                Piece(i, j);
            }
        }
    }
    move(turn());
}

void Piece(int one, int two)
{

    if (board[one, two].Contains(turn()) && Phase != 0)
    {
        if (board[one, two].Contains('p'))
        {
            pawn(one, two);
            intermission(one,two);
        }
        else if (board[one, two].Contains('r'))
        {
            intermission(one,two);
        }
        else if (board[one, two].Contains('k'))
        {
            intermission(one,two);
        }
        else if (board[one, two].Contains('b'))
        {
            intermission(one,two);
        }
        else if (board[one, two].Contains('q'))
        {
            intermission(one,two);
        }
        else if (board[one, two].Contains('m'))
        {
            intermission(one,two);
        }


    }
    else if (Phase == 0)
    {
        if (valid[one, two] == true)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    valid[i, j] = false;
                }
            }
            newTemp = board[one, two];
            board[one, two] = board[temp, temp2];
            board[temp, temp2] = Replace(temp, temp2);
            print();
            Phase = 1;
            player = !player;
        }
        else
        {
            Phase = 1;
            move(turn());
        }
    }
}

void intermission(int one, int two){
    temp = one; temp2 = two; Phase = 0;
            Console.WriteLine("Move to where? Use coordinate system.");
            newTemp = Console.ReadLine();
            spot(newTemp);
}

string Replace(int spot1, int spot2)  //tile replacer. 
//Could use another array with original tile, but this is my legacy board creator system
//Therefore, I'm keeping it in
{
    if (spot1 % 2 == 1)
    {
        if (spot2 % 2 == 0 && spot1 % 2 == 1)
        {
            return "o  ";
        }
        else
        {
            return "-  ";
        }
    }
    else
    {
        if (spot2 % 2 == 1 && spot1 % 2 == 0)
        {
            return "o  ";
        }
        else
        {
            return "-  ";
        }
    }
}

void pawn(int tile1, int tile2)
{
    int mod1 = -1;
    int mod2 = -2;
    if (player == false)
    {
        mod1 = 1;
        mod2 = 2;
    }
    valid[tile1 + mod1, tile2] = true;
    valid[tile1 + mod2, tile2] = true;
    if (tile2 < 7)
    {
        if (board[tile1 + mod1, tile2 + 1].Contains(opposer))
        {
            valid[tile1 + mod1, tile2 + 1] = true;
        }
    }
    if (tile2 > 0)
    {
        if (board[tile1 + mod1, tile2 - 1].Contains(opposer))
        {
            valid[tile1 + mod1, tile2 - 1] = true;
        }
    }
}

////////////////////////////// GAME //////////////////////////////

//Initialize
for (int i = 0; i < 16; i++)
{
    scroll = sr.ReadLine();
    string[] boardB = scroll.Split(",");
    for (int j = 0; j < 8; j++)
    {
        if (i < 8)
        {
            boardA[i, j] = boardB[j];
        }
        else
        {
            board[i - 8, j] = boardB[j];
        }
    }
}

//Start
print();
move(turn());

