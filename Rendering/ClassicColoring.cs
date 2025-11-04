using System.Drawing;

namespace GameOfLifeWPF.Rendering;

public class ClassicColoring : IColoringStrategy
{
    public byte SelectBirthColor(byte[] _) => 1;
    public Color GetColor(byte state)
        => state == 1 ? Color.LimeGreen : Color.Black;
}