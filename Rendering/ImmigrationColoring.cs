using System.Drawing;

namespace GameOfLifeWPF.Rendering;

public class ImmigrationColoring : IColoringStrategy
{
    public byte SelectBirthColor(byte[] neighborStates)
    {
        var counts = new int[3];
        foreach (var state in neighborStates)
            if (state >= 1) counts[state]++;
        if (counts[1] > counts[2]) return 1;
        if (counts[2] > counts[1]) return 2;
        return (byte)(1 + (Random.Shared.Next(2)));
    }

    public Color GetColor(byte state)
    {
        return state switch
        {
            1 => Color.Red,
            2 => Color.Blue,
            _ => Color.Black
        };
    }
}