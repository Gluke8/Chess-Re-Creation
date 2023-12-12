//WIP making chess, previous code I will be working on in the near future
//So I made it as a repo, may convert to kotlin
using System.Text;

string[,] board = new string[8, 8];
bool player = true;
string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
int newInt = 1;

string[,] boardA = new string[8, 8];

string scroll = "";
const string boardDoc = "board.csv";
StreamReader sr = new StreamReader(boardDoc);

for (int i = 0; i < 8; i++)
{
    scroll = sr.ReadLine();
    string[] boardB = scroll.Split(",");

    for (int j = 0; j < 8; j++)
    {
        boardA[i, j] = boardB[j];
    }
}

void Letter()
{
    Console.WriteLine();
    System.Console.Write("  ");
    for (int l = 0; l < 8; l++)
    {
        Console.Write(letters[l] + "  ");
    }
}

for (int i = 0; i < 8; i++)
{
    for (int j = 0; j < 8; j++)
    {
        board[i,j] = Replace(i,j);
    }
}

System.Console.WriteLine();
void print()
{
    for (int i = 0; i < 8; i++)
    {
        System.Console.WriteLine();
        System.Console.Write(i + 1 + " ");
        for (int j = 0; j < 8; j++)
        {
            Console.Write(board[i, j]);
        }
    }
    Letter();
}
string sel = "";
string spot(string here)
{
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            if (boardA[i, j] == here)
            {
                Piece(i,j);
            }
        }
    }
    move(turn());
    return "";
}
void move(string who)
{
    Console.WriteLine();
    Console.WriteLine("Player " + who + " select who to move. 'a1' for example.");
    sel = Console.ReadLine();
    string sel2 = spot(sel);

    

}
string turn()
{
    if (player)
    {
        return "1";

    }
    else
    {
        return "2";
    }
}
int temp = 0;
int temp2 = 0;
string newTemp2 = "";

void Piece(int one, int two){

    if (board[one,two].Contains(turn()) && newInt != 0){
        temp = one; temp2 = two;
        move2(one, two);

    }
    else if ( newInt == 0 && player == true){
        newTemp2 = board[one,two];
        board[one,two] = board[temp,temp2];
        board[temp,temp2] = Replace(temp, temp2);
        print();
        newInt = 1;
        player = false;
    }
    else if ( newInt == 0 && player == false){
        newTemp2 = board[one,two];
        board[one,two] = board[temp,temp2];
        board[temp,temp2] = Replace(temp, temp2);
        print();
        newInt = 1;
        player = true;
    }
}

void Start()
{
    for (int i = 0; i < 8; i++)
    {
        board[1, i] = "p2 ";
    }
    for (int i = 0; i < 8; i++)
    {
        board[6, i] = "p1 ";
    }
    board[0, 0] = "r2 ";
    board[0, 1] = "k2 ";
    board[0, 2] = "b2 ";
    board[0, 3] = "q2 ";
    board[0, 4] = "m2 ";
    board[0, 5] = "b2 ";
    board[0, 6] = "k2 ";
    board[0, 7] = "r2 ";

    board[7, 0] = "r1 ";
    board[7, 1] = "k1 ";
    board[7, 2] = "b1 ";
    board[7, 3] = "q1 ";
    board[7, 4] = "m1 ";
    board[7, 5] = "b1 ";
    board[7, 6] = "k1 ";
    board[7, 7] = "r1 ";

}

string newTemp = "";

void move2(int one, int two) {
    newInt = 0;
    System.Console.WriteLine("Move to where? Use coordinate system.");
    newTemp = Console.ReadLine();
    spot(newTemp);

}
string Replace(int spot1, int spot2){
    if (spot1 % 2 == 1)
        {
            if (spot2 % 2 == 0 && spot1 % 2 == 1)
            {
                return "O  ";
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
                return "O  ";
            }
            else
            {
                return "-  ";
            }
        }
}

Start();
print();
move(turn());




