using System.Drawing;

namespace GameOfLifeWPF.Rendering;

public static class Palettes
{
    public static readonly ColorPalette Classic = new ColorPalette("Classic", new Dictionary<Color, Color>
    {
        { Color.LimeGreen,     Color.LimeGreen },
        { Color.Black,         Color.Black },
        { Color.Red,           Color.Red },
        { Color.Blue,          Color.Blue },
        { Color.Yellow,        Color.Yellow },
        { Color.CornflowerBlue, Color.CornflowerBlue }
    });

    public static readonly ColorPalette Pastel = new ColorPalette("Pastel", new Dictionary<Color, Color>
    {
        { Color.LimeGreen,     Color.FromArgb(180, 255, 234) },        
        { Color.Black,         Color.FromArgb(230, 230, 230) },        
        { Color.Red,           Color.FromArgb(255, 192, 203) },        
        { Color.Blue,          Color.FromArgb(173, 216, 230) },        
        { Color.Yellow,        Color.FromArgb(255, 255, 204) },        
        { Color.CornflowerBlue, Color.FromArgb(159, 200, 255) }
    });

    public static readonly ColorPalette DarkMode = new ColorPalette("Dark Mode", new Dictionary<Color, Color>
    {
        { Color.LimeGreen,     Color.FromArgb(120, 220, 120) },        
        { Color.Black,         Color.FromArgb(32, 32, 32) },           
        { Color.Red,           Color.FromArgb(220, 60, 60) },          
        { Color.Blue,          Color.FromArgb(60, 120, 220) },        
        { Color.Yellow,        Color.FromArgb(235, 180, 60) },      
        { Color.CornflowerBlue, Color.FromArgb(100, 160, 255) }      
    });

    public static readonly List<ColorPalette> All = new List<ColorPalette>
    {
        Classic,
        Pastel,
        DarkMode
    };
}