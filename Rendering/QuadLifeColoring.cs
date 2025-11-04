using System.Drawing;

namespace GameOfLifeWPF.Rendering;

public class QuadLifeColoring : IColoringStrategy
{
    public byte SelectBirthColor(byte[] neighborStates)
    {
        var counts = new int[5];
        foreach (var state in neighborStates)
            if (state >= 1 && state <= 4) counts[state]++;
        var best = 1;
        for (var i = 2; i <= 4; i++) if (counts[i] > counts[best]) best = i;
        return (byte)best;
    }

    public Color GetColor(byte state)
    {
        return state switch
        {
            1 => Color.Red,
            2 => Color.Yellow,
            3 => Color.LimeGreen,
            4 => Color.CornflowerBlue,
            _ => Color.Black
        };
    }
}