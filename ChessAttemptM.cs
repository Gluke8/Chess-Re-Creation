//WIP making chess, previous code I will be working on in the near future
//So I made it as a repo
string[,] board = new string[8, 8];
bool player = true;
string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
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
    Console.WriteLine();
    //System.Console.Write(i + 1 + " ");
    for (int j = 0; j < 8; j++)
    {
        if (i % 2 == 1)
        {
            if (j % 2 == 0 && i % 2 == 1)
            {
                board[i, j] = "O  ";
                Console.Write(board[i, j]);
            }
            else
            {
                board[i, j] = "-  ";
                Console.Write(board[i, j]);
            }
        }
        else
        {
            if (j % 2 == 1 && i % 2 == 0)
            {
                board[i, j] = "O  ";
                Console.Write(board[i, j]);
            }
            else
            {
                board[i, j] = "-  ";
                Console.Write(board[i, j]);
            }
        }
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

void move(string who)
{
    System.Console.WriteLine();
    System.Console.WriteLine("Player " + who + " select who to move.");
    sel = Console.ReadLine();
    
    
    
    if (sel.Contains(who))
    {

    }
    else
    {
        move(who);
    }
}
void turn()
{
    if (player)
    {
        move("1");
        player = false;

    }
    else
    {
        move("2");
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




Start();
print();
turn();