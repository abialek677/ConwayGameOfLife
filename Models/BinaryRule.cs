namespace GameOfLifeWPF.Models;

public class BinaryRule : IAutomatonRule
{
    private readonly bool[] _birth = new bool[9];
    private readonly bool[] _survive = new bool[9];
    public string Description { get; }
    public BinaryRule(string text)
    {
        Description = text;
        Parse(text);
    }
    public void Parse(string text)
    {
        Array.Clear(_birth, 0, 9);
        Array.Clear(_survive, 0, 9);

        var parts = text.ToUpper().Split('/', StringSplitOptions.RemoveEmptyEntries);
        foreach (var p in parts)
        {
            if (p.StartsWith('B'))
            {
                for (var i = 1; i < p.Length; i++)
                    if (char.IsDigit(p[i]))
                        _birth[p[i] - '0'] = true;
            }
            else if (p.StartsWith('S'))
            {
                for (var i = 1; i < p.Length; i++)
                    if (char.IsDigit(p[i]))
                        _survive[p[i] - '0'] = true;
            }
        }
    }
    public byte NextState(byte currentState, int aliveNeighbors)
    {
        var alive = currentState != 0;
        var next = alive ? _survive[aliveNeighbors] : _birth[aliveNeighbors];
        return next ? alive ? currentState : (byte)1 : (byte)0;
    }
}