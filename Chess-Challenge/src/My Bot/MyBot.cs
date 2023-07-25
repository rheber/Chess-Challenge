using ChessChallenge.API;
using System.Collections.Generic;

public class MyBot : IChessBot {
    int turnsElapsed;

    public MyBot() {
        turnsElapsed = 0;
    }

    // Default move.
    Move def(Board board) {
        return mostForcingMoves(board)[0];
    }

    // Try to make the given move, make default move otherwise.
    Move withDefault(string moveName, Board board) {
        var move = new Move(moveName, board);
        foreach (Move m in board.GetLegalMoves())
            if (move.StartSquare == m.StartSquare && move.TargetSquare == m.TargetSquare) return move;
        return def(board);
    }

    // Moves that give the opponent the fewest options.
    List<Move> mostForcingMoves(Board board) {
        int maxMoves = 300;
        var myMoves = new List<Move>();
        foreach (Move m in board.GetLegalMoves()) {
            board.MakeMove(m);
            var opponentMovesAmount = board.GetLegalMoves().Length;
            if (opponentMovesAmount <= maxMoves) {
                if (opponentMovesAmount < maxMoves) myMoves.Clear();
                maxMoves = opponentMovesAmount;
                myMoves.Add(m);
            }
            board.UndoMove(m);
        }
        return myMoves;
    }

    public Move Think(Board board, Timer timer) {
        try {
            if (turnsElapsed == 0) {
                // For first move, make an uncommon but not terrible move.
                if (board.IsWhiteToMove) return withDefault("g1f3", board); // Zukertort
                if (board.GameMoveHistory[0].TargetSquare.Name == "e4") return withDefault("b8c6", board); // Nimzowitsch
                return withDefault("e7e6", board); // Horwitz or similar
            }
            return def(board);
        } finally {
            turnsElapsed++;
        }
    }
}
