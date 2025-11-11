using System.Drawing;

namespace GameOfLifeWPF.Rendering;

public class ColorPalette
{
    public string Name { get; }
    public Dictionary<Color, Color> Mapping { get; }
    
    public ColorPalette(string name, Dictionary<Color, Color> mapping)
    {
        Name = name;
        Mapping = mapping;
    }

    public Color GetDisplayColor(Color logicalColor)
    {
        return Mapping.TryGetValue(logicalColor, out var displayColor) ? displayColor : logicalColor;
    }
}