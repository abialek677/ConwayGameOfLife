using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF.Models;

public class Board
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    private byte[,] _cellsBuffer;
    private byte[,] _nextBuffer;
    public byte[,] Cells => _cellsBuffer;

    public Board(int width, int height)
    {
        Resize(width, height);
    }

    public void Resize(int width, int height)
    {
        Width = Math.Max(1, width);
        Height = Math.Max(1, height);
        _cellsBuffer = new byte[Height, Width];
        _nextBuffer = new byte[Height, Width];
    }

    public void Clear() => Array.Clear(_cellsBuffer, 0, _cellsBuffer.Length);

    public void Randomize(Random rnd, double aliveProbability = 0.2, int colors = 2)
    {
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            _cellsBuffer[y, x] = (byte)(rnd.NextDouble() < aliveProbability ? rnd.Next(1, colors + 1) : 0);
    }

    public StepStats Step(bool wrap, IAutomatonRule rule, IColoringStrategy coloringStrategy)
    {
        var born = 0;
        var died = 0;
        var tileSize = 64;
        var tileCountX = (Width + tileSize - 1) / tileSize;
        var tileCountY = (Height + tileSize - 1) / tileSize;

        Parallel.For(0, tileCountY, ty =>
        {
            var yStart = ty * tileSize;
            var yEnd = Math.Min(yStart + tileSize, Height);
            for (var tx = 0; tx < tileCountX; tx++)
            {
                var xStart = tx * tileSize;
                var xEnd = Math.Min(xStart + tileSize, Width);
                for (var y = yStart; y < yEnd; y++)
                for (var x = xStart; x < xEnd; x++)
                {
                    var current = _cellsBuffer[y, x];
                    var neighborStates = GetNeighborStates(x, y, wrap);
                    var aliveNeighbors = neighborStates.Count(s => s > 0);
                    var ruleResult = rule.NextState(current, aliveNeighbors);
                    var next =
                        (current == 0 && ruleResult > 0 && coloringStrategy != null)
                            ? coloringStrategy.SelectBirthColor(neighborStates)
                            : ruleResult > 0
                                ? (current > 0 ? current : ruleResult)
                                : (byte)0;
                    _nextBuffer[y, x] = next;

                    if (current == 0 && next != 0) Interlocked.Increment(ref born);
                    if (current != 0 && next == 0) Interlocked.Increment(ref died);
                }
            }
        });

        (_cellsBuffer, _nextBuffer) = (_nextBuffer, _cellsBuffer);
        return new StepStats(born, died);
    }

    private byte[] GetNeighborStates(int x, int y, bool wrap)
    {
        var neighbors = new byte[8];
        var count = 0;

        for (var dy = -1; dy <= 1; dy++)
        for (var dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0) continue;

            var nx = x + dx;
            var ny = y + dy;

            if (wrap)
            {
                nx = (nx + Width) % Width;
                ny = (ny + Height) % Height;
            }

            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
                neighbors[count++] = _cellsBuffer[ny, nx];
        }

        if (count >= 8) return neighbors;
       
        var result = new byte[count];
        Array.Copy(neighbors, result, count);
        return result;
    }
}