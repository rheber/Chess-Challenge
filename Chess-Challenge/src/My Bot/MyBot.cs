using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    private int turnCounter;

    public MyBot() {
        turnCounter = 1;
    }

    public Move Think(Board board, Timer timer) {
        try {
            Move[] moves = board.GetLegalMoves();
            return moves[0];
        } finally {
            //Console.WriteLine(turnCounter);
            turnCounter += 1;
        }
    }
}
