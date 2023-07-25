using ChessChallenge.API;

public class MyBot : IChessBot {
    int turnsElapsed;

    public MyBot() {
        turnsElapsed = 0;
    }

    // Default move.
    Move def(Board board) {
        return board.GetLegalMoves()[0];
    }

    // Try to make the given move, make default move otherwise.
    Move withDefault(string moveName, Board board) {
        var move = new Move(moveName, board);
        foreach (Move m in board.GetLegalMoves())
            if (move.StartSquare == m.StartSquare && move.TargetSquare == m.TargetSquare) return move;
        return def(board);
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
