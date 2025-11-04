namespace GameOfLifeWPF.Rendering;

public interface IColoringStrategy
{
    byte SelectBirthColor(byte[] neighborStates);
    
    System.Drawing.Color GetColor(byte state);
}