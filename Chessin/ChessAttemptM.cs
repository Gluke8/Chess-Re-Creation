//WIP making chess, I will be working on in this in the near future
//So, I made it as a repo, and may convert to kotlin
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

//en passent ------------------------ !THIS WILL BE THE HARDEST SYSTEM TO IMPLEMENT! ----------------------------------------------------

// TO DO 
// en passent, castling, check mate, stalemate,

//castling ---------------------------------------------------------------------------------------------------------------------------------
// work on one rook at a time
// rook can not have moved -> DISTINGUISH BETWEEN ROOKS: 
//  -> selected tile must contain rook that see's king depending on which side for player tiles adjusted. depending on initial spot.
//  -> and bool for method caveat
// king cannot have moved -> bool method caveat
// king cannot be in check -> method caveat: if your king is in check
// can't castle through a check -> if tiles _ _ _ have one true, no castle 
// no pieces in between -> rook must see king.

// this will start after some organization.

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
    Phase = 1;

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
    checkM = false;

    if (valid[mA, mB] == true)
    {
        if (Phase == 0) // dont enter on next player's turn
        {
            System.Console.WriteLine("\nYou are still under attack if you go there...");
            contension = false;
            board[temp, temp2] = board[temp3, temp4];
            board[temp3, temp4] = Replace(temp3, temp4);
            move(turn());
        }
        contension = true; // run sub paths in methods
        Console.WriteLine("\nPROTECT");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                kingValid[i, j] = valid[i, j];
            }
        }
    }

    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            valid[i, j] = false;
        }
    }

}


void move(string who)
{
    Console.WriteLine();
    Console.WriteLine("\nPlayer " + who + " select who to move. 'a1' for example.");
    string? sel = Console.ReadLine();
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
    move(turn()); // next turn
}

void Piece(int one, int two)
{
    if (board[one, two].Contains(user) && Phase != 0)
    {
        if (board[one, two].Contains('p'))
        {
            pawn(one, two);
            intermission(one, two);
        }
        else if (board[one, two].Contains('r'))
        {
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
        print();
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
    int mod1, mod2;
    if (user == "1"){
        mod1 = -1;
        mod2 = -2;
        if (tile1 != 6)
        {
            mod2 = 0;
        }
    }
    else {
        mod1 = 1;
        mod2 = 2;
        if (tile1 != 1)
        {
            mod2 = 0;
        }
    }

    if (board[tile1 + mod1, tile2].Contains('1') || board[tile1 + mod1, tile2].Contains('2'))
    {
        valid[tile1 + mod1, tile2] = false;
    }
    else
    {
        valid[tile1 + mod1, tile2] = true;
        if (mod2 == 0)
        {
            valid[tile1 + mod2, tile2] = false;
        }
        else
        {
            valid[tile1 + mod2, tile2] = true;
        }
    }
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
    if (reset == 64)
    {
        Console.WriteLine("No valid moves...");
        queen = false;
        move(turn());
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

//Start ( begin the recursion )
print();
move(turn());