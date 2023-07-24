using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    int turnCounter;

    public MyBot() {
        turnCounter = 1;
    }

    Move def(Board board) {
        Move[] moves = board.GetLegalMoves();
        return moves[0];
    }

    Move withDefault(Move move, Board board) {
        foreach (Move m in board.GetLegalMoves()) {
            if (move.StartSquare == m.StartSquare && move.TargetSquare == m.TargetSquare) {
                return move;
            }
        }
        return def(board);
    }

    public Move Think(Board board, Timer timer) {
        try {
            if (turnCounter == 1) {
                if (board.IsWhiteToMove) {
                    return withDefault(new Move("g1f3", board), board);
                }
            }
            return def(board);
        } finally {
            //Console.WriteLine(turnCounter);
            turnCounter += 1;
        }
    }
}
