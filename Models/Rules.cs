namespace GameOfLifeWPF.Models;

public interface IAutomatonRule
{
    byte NextState(byte currentState, int aliveNeighbors);
    string Description { get; }

    void Parse(string text);
}