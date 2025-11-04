namespace GameOfLifeWPF.Models;

public class Pattern
{
    public string Name { get; init; } = string.Empty;
    public byte[,] Shape { get; init; } = null!;
}