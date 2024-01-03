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
