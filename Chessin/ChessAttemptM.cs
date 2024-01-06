//WIP making chess, I will be working on in this in the near future
//So, I made it as a repo, and may convert to kotlin
using System.Data;
using Microsoft.VisualBasic;

string[,] board = new string[8, 8]; string[,] boardA = new string[8, 8]; // arrays
string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };

const string boardDoc = "board.csv";           //variables for reading "board.csv" values; accessing and reading
StreamReader sr = new(boardDoc);
string? scroll = "";

bool player = true; //player true; player 1's turn, player false; player 2's turn
int Phase = 1;  //Whilst phase == 1; character select to move, phase == 0; select to place

int temp = 0;         //group of variables used to determine piece moves
int temp2 = 0;
string newTemp = "";

int recursion = 1; // for switch cases

bool[,] valid = new bool[8, 8];
bool[,] kingValid = new bool[8, 8];
char opposer = '0';


bool checkM = false;
string user = "0";
int mA = 0;
int mB = 0;
bool contension = false;
bool queen = false;
bool king = false;
int temp3 = 0; int temp4 = 0;

//en passent ----------------------------------------------------------------------------
int turnCount = 1;
int mod1 = 0, mod2 = 0;
int enPassent2 = 1;
string newPawn = "";
int newMod = 0;
bool pawnSelect = false;
int[,] enPassent = new int[8, 8];
for (int i = 0; i < 8; i++)
{
    for (int j = 0; j < 8; j++)
    {
        enPassent[i, j] = -1;
    }
}

// TO DO 
// check mate, stalemate,
// Checkmate (MATE) ---------------------------------------------------------------------------------------
/*
1. Since finding if you have any legal moves seems strenuous (code is itterating alot before your turn) it may be easier to implement a forfeit system:
If your king is under attack, otherwise known as being in check, you have three attempts to end your turn with the king freed from check.
If this is not fulfilled, the game will end with whomever checked the king.

2. Done simply you may or may not be warned about being in check, rather if your opponent captures your king, you lose.

3. The classic mate is having no legal moves, while your king is in check. 
So in order to do this every instance of movement for your pieces must occur. Checking if your king is freed from check.
If this is never broken out of then you lose. 

Pseudo code in steps (3)
At the beginning of your turn the opponents availale spots to attack are analyzed. If one of the attacking spots is your king square, you are in check.
So for a checkmate, every possible move you can do must be checked if it is legal. (freeing your king from the check)
Of course this happens before your turn to move. 
First analyze a pawn, figure out its valid tiles and try them all, do this in an array for loop, testing if king is now false. 
No, try that pawns next true etc. 

*/
bool[,] validMate = new bool[8, 8];
bool mateMove = false;
string thisOne = "";
int safe = 0;





bool rookMove = false;
int rookTemp = -1;

bool[] castleThrough = { false, false, false, false, false, false, false, false };
bool startCastle = false;
int endCastle = 0;

// castling check bugs? en passent check bugs?
bool[] castle2 = { true, false, false, false, true, false, false, true };
bool[] castle1 = { true, false, false, false, true, false, false, true };
bool activeCastleL = false;
bool activeCastleR = false;

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
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            kingValid[i, j] = false;
        }
    }
    for (int i = 0; i < 8; i++)
    {
        castleThrough[i] = false;
    }
    Phase = 1;
    recursion = 1;
    pawnSelect = false;
    enPassent2 = 1;
    rookMove = false; rookTemp = -1; endCastle = 0;
    startCastle = false;

    if (player)
    {
        opposer = '1';
        user = "2";
        checkMate();
        opposer = '2';
        user = "1";
        return "1";

    }
    else
    {
        opposer = '2';
        user = "1";
        checkMate();
        opposer = '1';
        user = "2";
        return "2";
    }
}

void checkMate()
{
    checkM = true;
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            if (board[i, j].Contains("p" + user))
            {
                pawn(i, j);
            }
            if (board[i, j].Contains("r" + user))
            {
                Rook(i, j);
            }

            if (board[i, j].Contains("k" + user))
            {
                knight(i, j);
                recursion = 1;
            }

            if (board[i, j].Contains("q" + user))
            {
                Rook(i, j);
                Bishop(i, j);
            }

            if (board[i, j].Contains("b" + user))
            {
                Bishop(i, j);
            }

            if (board[i, j].Contains("m" + user))
            {
                master(i, j);
                recursion = 1;
            }

            if (board[i, j].Contains("m" + opposer))
            {
                mA = i; mB = j;
            }
        }
    }
    if (user == "1")
    {
        for (int i = 0; i < 8; i++)
        {
            castleThrough[i] = valid[0, i];
        }
    }
    else
    {
        for (int i = 0; i < 8; i++)
        {
            castleThrough[i] = valid[7, i];
        }
    }
    checkM = false;

    if (valid[mA, mB] == true) // why enter at end of turn 2
    // because for castle valid is getting altered again adding more valid spots mmm reset before chackmate then.
    {
        if (Phase == 0) // dont enter on next player's turn
        {
            System.Console.WriteLine("\nYou are still under attack if you go there...");
            contension = false;
            if (startCastle) // should put back to start of turn board if ending up with a checked king
            { // is this even needed? the king cannot even castle if one of the squares are checked anyways
                if (user == "1") // player 2. ******************** might not be needed since you can't castle if one of the tiles are contested ***********
                {
                    if (endCastle == 1)
                    {
                        board[0, 3] = Replace(0, 3);
                        board[0, 2] = Replace(0, 2);
                        board[0, 0] = "r2 ";
                        board[0, 4] = "m2 ";
                    }
                    else if (endCastle == 2)
                    {
                        board[0, 5] = Replace(0, 5);
                        board[0, 6] = Replace(0, 6);
                        board[0, 7] = "r2 ";
                        board[0, 4] = "m2 ";
                    }
                }
                else
                {
                    if (endCastle == 1)
                    {
                        board[7, 3] = Replace(7, 3);
                        board[7, 2] = Replace(7, 2);
                        board[7, 0] = "r1 ";
                        board[7, 4] = "m1 ";
                    }
                    else if (endCastle == 2)
                    {
                        board[7, 5] = Replace(7, 5);
                        board[7, 6] = Replace(7, 6);
                        board[7, 7] = "r1 ";
                        board[7, 4] = "m1 ";
                    }
                }
                // put pieces back from castle move. *************************************************************************************
            }
            else
            {
                board[temp, temp2] = board[temp3, temp4];
                board[temp3, temp4] = Replace(temp3, temp4);
                if (enPassent2 == 0)
                {
                    board[temp3 + newMod, temp4] = newPawn;
                }
            }
            move(turn());
        }
        contension = true; // run sub paths in methods
        Console.WriteLine("\nPROTECT");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                kingValid[i, j] = valid[i, j];
                valid[i, j] = false;
            }
        }
        mate();
        mateMove = false;
        // CHECKMATE ----------------------------------------------------------------------------------------------------------------
        // remember user = 2 while player 1 currently
        // find all of my pieces.

    }

    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            valid[i, j] = false;
        }
    }

}









// could return a bool for active moves
void mate()
{
    mateMove = true;
    if (user == "2")
    {
        user = "1"; opposer = '2';
    }
    else
    {
        user = "2"; opposer = '1';
    }
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            if (board[i, j].Contains("p" + user))
            {
                pawn(i, j);
            }
            else if (board[i, j].Contains("r" + user))
            {
                Rook(i, j);
            }
            else if (board[i, j].Contains("k" + user))
            {
                knight(i, j);
                recursion = 1;
            }
            else if (board[i, j].Contains("b" + user))
            {
                Bishop(i, j);
            }
            else if (board[i, j].Contains("q" + user))
            {
                Rook(i, j);
                Bishop(i, j);
            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    validMate[x, y] = valid[x, y];
                    valid[x, y] = false;
                }
            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (validMate[x, y] == true)
                    {
                        thisOne = board[x, y];
                        board[x, y] = board[i, j];
                        board[i, j] = Replace(i, j);
                        for (int a = 0; a < 8; a++)
                        {
                            for (int b = 0; b < 8; b++)
                            {
                                if (user == "2")
                                {
                                    user = "1"; opposer = '2';
                                }
                                else
                                {
                                    user = "2"; opposer = '1';
                                }
                                if (board[a, b].Contains("p" + user))
                                {
                                    pawn(a, b);
                                }
                                if (board[a, b].Contains("r" + user))
                                {
                                    Rook(a, b);
                                }

                                if (board[a, b].Contains("k" + user))
                                {
                                    knight(a, b);
                                    recursion = 1;
                                }

                                if (board[a, b].Contains("q" + user))
                                {
                                    Rook(a, b);
                                    Bishop(a, b);
                                }

                                if (board[a, b].Contains("b" + user))
                                {
                                    Bishop(a, b);
                                }

                                if (board[a, b].Contains("m" + user))
                                {
                                    master(a, b);
                                    recursion = 1;
                                }

                                if (board[a, b].Contains("m" + opposer))
                                {
                                    mA = a; mB = b;
                                }
                                if (user == "2")
                                {
                                    user = "1"; opposer = '2';
                                }
                                else
                                {
                                    user = "2"; opposer = '1';
                                }
                            }
                        }
                        if (valid[mA, mB] == true)
                        {
                            System.Console.WriteLine("WILL NOT PROTECT"); // en passent?? weird replace.
                            // must replace with original piece there!!!
                            board[i, j] = board[x, y];
                            board[x, y] = Replace(x, y);
                            board[x, y] = thisOne;

                            for (int c = 0; c < 8; c++)
                            {
                                for (int d = 0; d < 8; d++)
                                {
                                    valid[c, d] = false;
                                }
                            }

                        }
                        else
                        {
                            System.Console.WriteLine("WILL PROTECT");
                            board[i, j] = board[x, y];
                            board[x, y] = Replace(x, y);
                            board[x, y] = thisOne;
                            for (int c = 0; c < 8; c++)
                            {
                                for (int d = 0; d < 8; d++)
                                {
                                    valid[c, d] = false;
                                }
                            }
                            safe++;
                        }
                    }
                }

            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    valid[x, y] = false;
                }
            }

        }
    }
    if (safe == 0){
        System.Console.WriteLine("Player " + opposer + " wins!");
        Environment.Exit(0);
    }
    else{
        safe = 0;
    }
}


void move(string who)
{
    Console.WriteLine();
    Console.WriteLine("\nPlayer " + who + " select who to move. 'a1' for example. To castle type 'C'");
    string? sel = Console.ReadLine();
    if (sel.Contains("C"))
    {
        if (contension == false)
        {
            Castle();
        }
        else
        {
            System.Console.WriteLine("Castle cannot be made while under attack...");
        }
    }
    else
    {
        spot(sel);
    }
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
    move(turn()); // next turn
}

void Piece(int one, int two)
{
    if (board[one, two].Contains(user) && Phase != 0)
    {
        if (board[one, two].Contains('p'))
        {
            pawnSelect = true; // implement into the king check system true tiles???
            pawn(one, two);
            intermission(one, two);
        }
        else if (board[one, two].Contains('r'))
        {
            rookMove = true;
            if (user == "1") // do one for king
            {
                if (one == 7 && two == 0)
                {
                    rookTemp = 0;
                }
                else if (one == 7 && two == 7)
                {
                    rookTemp = 7;
                }
            }
            else
            {
                if (one == 0 && two == 0)
                {
                    rookTemp = 0;
                }
                else if (one == 0 && two == 7)
                {
                    rookTemp = 7;
                }
            }
            Rook(one, two);
            intermission(one, two);
        }
        else if (board[one, two].Contains('k'))
        {
            knight(one, two);
            possible();
            intermission(one, two);
        }
        else if (board[one, two].Contains('b'))
        {
            Bishop(one, two);
            intermission(one, two);
        }
        else if (board[one, two].Contains('q'))
        {
            queen = true;
            Rook(one, two);
            Bishop(one, two);
            possible();
            intermission(one, two);
        }
        else if (board[one, two].Contains('m'))
        {
            king = true;
            master(one, two);
            possible();
            intermission(one, two);
        }
    }
    if (contension)
    {
        if (king == false)
        {
            endTurn(one, two);
        }
        else if (valid[one, two] == true && kingValid[one, two] == true)
        {
            System.Console.WriteLine("You are still under attack if you go there...");
            contension = false;
            king = false;
            move(turn());
        }
    }
    if (Phase == 0 && checkM == false)
    {
        endTurn(one, two);
    }
}


void endTurn(int one, int two)
{
    if (valid[one, two] == true)
    {
        if (pawnSelect == true)
        { //might have to move this down in end turn like after checkmate() // might inherently do it: if previous turn occured was a double pawn activate kingValid check with it
            if ((temp + temp2 + mod2) == (one + two))
            {
                enPassent[one, two] = turnCount;
            }

            if (user == "1")
            {
                newMod = 1;
            }
            else
            {
                newMod = -1;
            }
            if (enPassent[one + newMod, two] == (turnCount - 1))
            {
                enPassent2 = 0;
                newPawn = board[one + newMod, two];
                board[one + newMod, two] = Replace(one + newMod, two);
            }
        }


        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                valid[i, j] = false;
            }
        }
        temp3 = one; temp4 = two;
        newTemp = board[one, two];
        board[one, two] = board[temp, temp2];
        board[temp, temp2] = Replace(temp, temp2);
        if (user == "1")
        {
            user = "2";
            opposer = '1';
        }
        else
        {
            user = "1"; opposer = '2';
        }
        checkMate();
        if (rookMove == true) // users got flipped
        {
            if (user == "2")
            {
                castle1[rookTemp] = false;
            }
            else
            {
                castle2[rookTemp] = false;
            }
        }
        if (king == true)
        {
            if (user == "2")
            {
                castle1[0] = false; castle1[4] = false; castle1[7] = false;
            }
            else
            {
                castle2[0] = false; castle2[4] = false; castle2[7] = false;
            }
        }
        print();
        turnCount++;
        System.Console.WriteLine("\nTurn " + turnCount);
        player = !player;
        king = false;

    }
    else
    {
        move(turn());
    }
}

void intermission(int one, int two)
{
    temp = one; temp2 = two; Phase = 0; recursion = 1; queen = false;
    System.Console.WriteLine("Move to where? Use coordinate system.");
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
            return "■  ";
        }
        else
        {
            return "O  ";
        }
    }
    else
    {
        if (spot2 % 2 == 1 && spot1 % 2 == 0)
        {
            return "■  ";
        }
        else
        {
            return "O  ";
        }
    }
}

// PIECES -------------------------------------------------------------------------------------------------------

void pawn(int tile1, int tile2)
{

    if (user == "1") // user 1
    {
        mod1 = -1;
        mod2 = -2;
        if (tile1 != 6)
        {
            mod2 = 0;
        }
    }
    else // user 2
    {
        mod1 = 1;
        mod2 = 2;
        if (tile1 != 1)
        {
            mod2 = 0;
        }
    }

    // check for pieces in front.
    if (board[tile1 + mod1, tile2].Contains('1') || board[tile1 + mod1, tile2].Contains('2'))
    {
        valid[tile1 + mod1, tile2] = false;
    }
    else
    {
        valid[tile1 + mod1, tile2] = true;
    }

    if (mod2 == 0)
    {
        valid[tile1 + mod2, tile2] = false;
    }
    else if (board[tile1 + mod2, tile2].Contains('1') || board[tile1 + mod2, tile2].Contains('2'))
    {
        valid[tile1 + mod2, tile2] = false;
    }
    else
    {
        valid[tile1 + mod2, tile2] = true;
    }

    if (tile2 < 7)
    {
        if (board[tile1 + mod1, tile2 + 1].Contains(opposer))
        {
            valid[tile1 + mod1, tile2 + 1] = true;
        }
        if (enPassent[tile1, tile2 + 1] == (turnCount - 1))
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
        if (enPassent[tile1, tile2 - 1] == (turnCount - 1))
        {
            valid[tile1 + mod1, tile2 - 1] = true;
        }
    }
    if (checkM == false)
    {
        possible();
    }
}

void knight(int one, int two)
{
    while (recursion < 9)
    {
        try
        {
            switch (recursion)
            {
                case 1:
                    valid[one + 2, two - 1] = true; check(one + 2, two - 1);
                    break;
                case 2:
                    valid[one + 2, two + 1] = true; check(one + 2, two + 1);
                    break;
                case 3:
                    valid[one - 2, two - 1] = true; check(one - 2, two - 1);
                    break;
                case 4:
                    valid[one - 2, two + 1] = true; check(one - 2, two + 1);
                    break;
                case 5:
                    valid[one + 1, two - 2] = true; check(one + 1, two - 2);
                    break;
                case 6:
                    valid[one + 1, two + 2] = true; check(one + 1, two + 2);
                    break;
                case 7:
                    valid[one - 1, two - 2] = true; check(one - 1, two - 2);
                    break;
                case 8:
                    valid[one - 1, two + 2] = true; check(one - 1, two + 2);
                    break;
            }
        }
        catch (IndexOutOfRangeException)
        {
            recursion++;
            knight(one, two);
        }
    }

}

void check(int one, int two)
{
    if (board[one, two].Contains(user))
    {
        valid[one, two] = false;
    }
    recursion++;
}

void UniversalMove(int one, int two, int move1, int move2, int rest) //moving method for rook and bishop
{
    for (int i = 1; i <= rest; i++)
    {
        if (board[one + (i * move1), two + (i * move2)].Contains(user))
        {
            break;
        }
        else if (board[one + (i * move1), two + (i * move2)].Contains(opposer))
        {
            valid[one + (i * move1), two + (i * move2)] = true;
            break;
        }
        else
        {
            valid[one + (i * move1), two + (i * move2)] = true;
        }
    }
}

void Rook(int one, int two)
{
    switch (recursion)
    {
        case 1:
            UniversalMove(one, two, -1, 0, one);
            break;
        case 2:
            UniversalMove(one, two, 0, 1, 7 - two);
            break;
        case 3:
            UniversalMove(one, two, 1, 0, 7 - one);
            break;
        case 4:
            UniversalMove(one, two, 0, -1, two);
            break;
    }
    if (recursion != 4)
    {
        recursion++;
        Rook(one, two);
    }
    recursion = 1;
    valid[one, two] = false;
    if (checkM == false && !queen)
    {
        possible();
    }
}

void Bishop(int one, int two)
{
    switch (recursion)
    {
        case 1:
            UniversalMove(one, two, 1, 1, Math.Min(7 - one, 7 - two));
            break;
        case 2:
            UniversalMove(one, two, 1, -1, Math.Min(7 - one, two));
            break;
        case 3:
            UniversalMove(one, two, -1, -1, Math.Min(one, two));
            break;
        case 4:
            UniversalMove(one, two, -1, 1, Math.Min(one, 7 - two));
            break;
    }
    if (recursion != 4)
    {
        recursion++;
        Bishop(one, two);
    }
    recursion = 1;
    if (checkM == false && !queen)
    {
        possible();
    }
}

void master(int one, int two)
{
    while (recursion < 9)
    {
        try
        {
            switch (recursion)
            {
                case 1:
                    valid[one + 1, two] = true; check(one + 1, two);
                    break;
                case 2:
                    valid[one - 1, two] = true; check(one - 1, two);
                    break;
                case 3:
                    valid[one, two - 1] = true; check(one, two - 1);
                    break;
                case 4:
                    valid[one, two + 1] = true; check(one, two + 1);
                    break;
                case 5:
                    valid[one + 1, two + 1] = true; check(one + 1, two + 1);
                    break;
                case 6:
                    valid[one + 1, two - 1] = true; check(one + 1, two - 1);
                    break;
                case 7:
                    valid[one - 1, two - 1] = true; check(one - 1, two - 1);
                    break;
                case 8:
                    valid[one - 1, two + 1] = true; check(one - 1, two + 1);
                    break;
            }
        }
        catch (IndexOutOfRangeException)
        {
            recursion++;
            master(one, two);
        }
    }
}

void possible()
{
    int reset = 0;
    foreach (var item in valid)
    {
        if (item == false)
        {
            reset++;
        }
    }
    if (reset == 64 && mateMove == false)
    {
        Console.WriteLine("No valid moves...");
        queen = false;
        move(turn());
    }

}

void Castle()
{
    startCastle = true;
    System.Console.WriteLine("So player " + user + " you want to castle eh? ");
    System.Console.WriteLine("Let's see what you can do...");
    // check to see if user's castle(1/2) is possible for castles, they would be false when moved

    // castle true tile...
    if (user == "1") // player one
    {
        if (castle1[4] == true && castle1[0] == true && board[7, 3].Contains(opposer) == false)
        { // check if have moved
            // check if visible. So can the (your) rook contact the king.
            // pass through universal move and check if the two tiles are true; // save valid to a temp array.
            // ask specifically for castle to run a castle method... either long or short. *************************************************
            UniversalMove(7, 0, 0, 1, 7);
            if (valid[7, 1] && valid[7, 2] && valid[7, 3] && castleThrough[1] == false && castleThrough[2] == false && castleThrough[3] == false)
            {
                activeCastleL = true;
            }
        }
        if (castle1[4] == true && castle1[7] == true && board[7, 5].Contains(opposer) == false)
        { // check if have moved
            UniversalMove(7, 7, 0, -1, 7);
            if (valid[7, 5] && valid[7, 6] && castleThrough[5] == false && castleThrough[6] == false)
            {
                activeCastleR = true;
            }
        }
        if (activeCastleL || activeCastleR)
        {
            System.Console.WriteLine("Long castle (left) or short castle (right)?");
            bool newCastleLoop1 = true;
            while (newCastleLoop1)
            { // catch invalid inputs
                string castleDirection = Console.ReadLine().ToLower();
                if (castleDirection == "left" && activeCastleL == true)
                { //WORKS
                    newCastleLoop1 = false;
                    board[7, 0] = Replace(7, 0);
                    board[7, 4] = Replace(7, 4);
                    board[7, 3] = "r1 ";
                    board[7, 2] = "m1 ";
                    endCastle = 1;
                }
                else if (castleDirection == "right" && activeCastleR == true)
                {
                    newCastleLoop1 = false;
                    board[7, 7] = Replace(7, 7);
                    board[7, 4] = Replace(7, 4);
                    board[7, 5] = "r1 ";
                    board[7, 6] = "m1 ";
                    endCastle = 2;
                }
                else
                {
                    System.Console.WriteLine("Which direction did you say? Long castle (left) or short castle (right).");
                }
            }
        }
        else
        {
            System.Console.WriteLine("Sorry, it seems a castle cannot be fortified.");
            move(user);
        }
    }
    else
    {
        if (castle2[4] == true && castle1[0] == true && board[7, 3].Contains(opposer) == false)
        {
            UniversalMove(0, 0, 0, 1, 7);
            if (valid[0, 1] && valid[0, 2] && valid[0, 3] && castleThrough[1] == false && castleThrough[2] == false && castleThrough[3] == false)
            {
                activeCastleL = true;
            }
        }
        if (castle1[4] == true && castle1[7] == true && board[7, 5].Contains(opposer) == false)
        {
            UniversalMove(0, 7, 0, -1, 7);
            if (valid[0, 5] && valid[0, 6] && castleThrough[5] == false && castleThrough[6] == false)
            {
                activeCastleR = true;
            }
        }
        if (activeCastleL || activeCastleR)
        {
            System.Console.WriteLine("Long castle (left) or short castle (right)?");
            bool newCastleLoop2 = true;
            while (newCastleLoop2)
            { // catch invalid inputs
                string castleDirection = Console.ReadLine().ToLower();
                if (castleDirection == "left" && activeCastleL == true)
                { //WORKS
                    newCastleLoop2 = false;
                    board[0, 0] = Replace(0, 0);
                    board[0, 4] = Replace(0, 4);
                    board[0, 3] = "r2 ";
                    board[0, 2] = "m2 ";
                    endCastle = 1;
                }
                else if (castleDirection == "right" && activeCastleR == true)
                {
                    newCastleLoop2 = false;
                    board[0, 7] = Replace(0, 7);
                    board[0, 4] = Replace(0, 4);
                    board[0, 5] = "r2 ";
                    board[0, 6] = "m2 ";
                    endCastle = 2;
                }
                else
                {
                    System.Console.WriteLine("Which direction did you say? Long castle (left) or short castle (right).");
                }
            }
        }
        else
        {
            System.Console.WriteLine("Sorry, it seems a castle cannot be fortified.");
            move(user);
        }
    }

    if (user == "1")
    {
        user = "2";
        opposer = '1';
    }
    else
    {
        user = "1"; opposer = '2';
    }
    for (int i = 0; i < 8; i++) // new
    {
        for (int j = 0; j < 8; j++)
        {
            valid[i, j] = false;
        }
    }
    checkMate();
    if (user == "1")
    {
        castle2[0] = false; castle2[4] = false; castle2[7] = false;
    }
    else
    {
        castle1[0] = false; castle1[4] = false; castle1[7] = false;
    }

    print();
    turnCount++;
    System.Console.WriteLine("\nTurn " + turnCount);
    player = !player;
    move(turn());
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

//Start ( begin the recursion )
print();
move(turn());

// complete starting board
/*
r2 ,k2 ,b2 ,q2 ,m2 ,b2 ,k2 ,r2 
p2 ,p2 ,p2 ,p2 ,p2 ,p2 ,p2 ,p2 
O  ,■  ,O  ,■  ,O  ,■  ,O  ,■  
■  ,O  ,■  ,O  ,■  ,O  ,■  ,O  
O  ,■  ,O  ,■  ,O  ,■  ,O  ,■  
■  ,O  ,■  ,O  ,■  ,O  ,■  ,O  
p1 ,p1 ,p1 ,p1 ,p1 ,p1 ,p1 ,p1 
r1 ,k1 ,b1 ,q1 ,m1 ,b1 ,k1 ,r1 
*/