using System.Collections.ObjectModel;

namespace GameOfLifeWPF.Models;
public static class PatternRepository
{
    private static readonly Pattern GosperGliderGun = new()
    {
        Name = "Gosper Glider Gun",
        Shape = new byte[,]
        {
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1
            },
            {
                1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            },
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0
            }
        }
    };

    private static readonly Pattern Blinker = new()
    {
        Name = "Blinker",
        Shape = new byte[,]
        {
            { 1, 1, 1 }
        }
    };

    private static readonly Pattern Glider = new()
    {
        Name = "Glider",
        Shape = new byte[,]
        {
            { 0, 1, 0 },
            { 0, 0, 1 },
            { 1, 1, 1 }
        }
    };

    private static readonly Pattern LightweightSpaceship = new()
    {
        Name = "Lightweight Spaceship",
        Shape = new byte[,]
        {
            { 0, 1, 0, 0, 1 },
            { 1, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 0 }
        }
    };

    private static readonly Pattern Pulsar = new()
    {
        Name = "Pulsar",
        Shape = new byte[,]
        {
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 }
        }
    };

    private static readonly Pattern Pentadecathlon = new()
    {
        Name = "Pentadecathlon",
        Shape = new byte[,]
        {
            { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
            { 1, 1, 0, 1, 1, 1, 1, 0, 1, 1 },
            { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 }
        }
    };

    private static readonly Pattern RPentomino = new()
    {
        Name = "R-pentomino",
        Shape = new byte[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        }
    };

    private static readonly Pattern Diehard = new()
    {
        Name = "Diehard",
        Shape = new byte[,]
        {
            { 0, 0, 0, 0, 0, 0, 1, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 1, 1, 1 }
        }
    };

    private static readonly Pattern Acorn = new()
    {
        Name = "Acorn",
        Shape = new byte[,]
        {
            { 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 1, 1, 0, 0, 1, 1, 1 }
        }
    };

    
    public static readonly ReadOnlyDictionary<string, Pattern> Patterns =
        new(new Dictionary<string, Pattern>
        {
            { GosperGliderGun.Name, GosperGliderGun },
            { Blinker.Name, Blinker },
            { Glider.Name, Glider },
            { LightweightSpaceship.Name, LightweightSpaceship },
            { Pulsar.Name, Pulsar },
            { Pentadecathlon.Name, Pentadecathlon },
            { RPentomino.Name, RPentomino },
            { Diehard.Name, Diehard },
            { Acorn.Name, Acorn }
        });

}