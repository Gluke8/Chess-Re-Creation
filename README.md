# Chess-ReCreate
 Simple self re-creation of chess.

Notes from En Passent:
//en passent ----------------------------------------------------------------------------
int turnCount = 1;
int mod1 = 0, mod2 = 0;
int enPassent2 = 1;
string newPawn = "";
int newMod = 0;

//turnCount ++; // after a succesful move
bool pawnSelect = false;
int[,] enPassent = new int[8, 8]; // new int array set to 0 at every spot. if a pawn is moved by 2 spaces that spot in the array is equal to the turn.
// since player's, 1 and 2, play on even/ odd turn counts respectfully, that is how the pawn's are distinguished in the array
// player 1 play's on odd nums while player 2 is on even
// so before every player's next turn all of there even or odd values will be rest in the array. odd is 3 % 2 = 0, etc.

// 1. capturing pawn must have advanced at least 3 ranks. so player 1 must (if statement) added on to pawn movement be 3 tile's ahead from starting position. 
// player 1 has pawns at 7s so 7 - 3 is at 4.
// player 2 would have opposite 3 as a plus. 2 + 3 at 5's, because 2 pawn advance leaves them beside. 

// 2. pawn moved beside capturing pawn must have done a 2 move advance. 

// 3. en passent must be done immediatly after the opposer's two tile advance. 






//castling -----------------------------  !THIS WILL BE THE HARDEST SYSTEM TO IMPLEMENT!  -----------------------------------------------------
bool rookMove = false;
int rookTemp = -1;

bool[,] validTemp = new bool[8, 8]; // used in castling
// it is a line
bool[] castle2 = { true, false, false, false, true, false, false, true }; // player 2 row

bool[] castle1 = { true, false, false, false, true, false, false, true }; // bools to castle // player 1 row
// -> on player's turn if the rook ar king moves from its original spot set player's castle form original spot to false
// work on one rook at a time
bool activeCastleL = false;
bool activeCastleR = false;
/* 
if (sel.Contains("C"))
    { 
        activeCastle = true;
    }
}
// run in Piece
// rook



*/
// rook can not have moved -> DISTINGUISH BETWEEN ROOKS: 
//  -> selected tile must contain rook that see's king depending on which side for player tiles adjusted. depending on initial spot.
//  -> and bool for method caveat
// king cannot have moved -> bool method caveat
// king cannot be in check -> method caveat: if your king is in check
// can't castle through a check -> if tiles _ _ _ have one true, no castle 
// no pieces in between -> rook must see king.
// Castling 
// few more tweaks: 
// cannot castle through a square that is able to be attacked by opponent
// cannot castle into a check. Leaving castling king in check at the start of the opponents turn
bool[] castleThrough = { false, false, false, false, false, false, false, false }; 
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