using GameOfLifeWPF.Models;

namespace GameOfLifeWPF.Extensions;

public static class BoardExtensions
{
    public static bool IsCellInBounds(this Board board, int x, int y) 
        => x >= 0 && x < board.Width && y >= 0 && y < board.Height;
}