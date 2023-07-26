using ChessChallenge.API;
using System.Collections.Generic;
using System.Linq;

public class MyBot : IChessBot {
    int turnsElapsed;

    public MyBot() {
        turnsElapsed = 0;
    }

    // Default move strategy.
    Move defaultMove(Board board) {
        var mates = checkmateMoves(board);
        if (mates.Count > 0) return mates[0];
        //var checks = checkMoves(board);
        //if (checks.Count > 0) return mostForcingMoves(board, checks.ToArray())[0];
        var legalMoves = board.GetLegalMoves();
        var promos = legalMoves.Where(m => m.IsPromotion && m.PromotionPieceType == PieceType.Queen).ToArray();
        if (promos.Length > 0) return mostForcingMoves(board, promos)[0];
        //var castles = legalMoves.Where(m => m.IsCastles).ToArray();
        //if (castles.Length > 0) return mostForcingMoves(board, castles)[0];
        //var captures = legalMoves.Where(m => m.IsCapture).ToArray();
        //if (captures.Length > 0) return mostForcingMoves(board, captures)[0];
        return mostForcingMoves(board, legalMoves)[0];
    }

    // Try to make the given move, make default move otherwise.
    Move withDefault(string moveName, Board board) {
        var move = new Move(moveName, board);
        foreach (Move m in board.GetLegalMoves())
            if (move.StartSquare == m.StartSquare && move.TargetSquare == m.TargetSquare) return move;
        return defaultMove(board);
    }

    List<Move> checkmateMoves(Board board) {
        var myMoves = new List<Move>();
        foreach (Move m in board.GetLegalMoves()) {
            board.MakeMove(m);
            if (board.IsInCheckmate()) {
                myMoves.Add(m);
            }
            board.UndoMove(m);
        }
        return myMoves;
    }

    /*
    List<Move> checkMoves(Board board) {
        var myMoves = new List<Move>();
        foreach (Move m in board.GetLegalMoves()) {
            board.MakeMove(m);
            if (board.IsInCheck()) {
                myMoves.Add(m);
            }
            board.UndoMove(m);
        }
        return myMoves;
    }
    */

    // Moves that give the opponent the fewest options.
    List<Move> mostForcingMoves(Board board, Move[] candidateMoves) {
        int maxMoves = 300;
        var myMoves = new List<Move>();
        foreach (Move m in candidateMoves) {
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
            return defaultMove(board);
        } finally {
            turnsElapsed++;
        }
    }
}
