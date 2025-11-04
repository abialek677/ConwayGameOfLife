using Microsoft.Win32;
using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF.IO;

public static class FileIoHandlers
{
    public static void SaveBoard(MainWindow mainWindow)
    {
        var dlg = new SaveFileDialog { Filter = "Text board (*.txt)|*.txt" };
        if (dlg.ShowDialog() != true) return;

        var modelName = mainWindow.BoardManager.ColoringStrategy.GetType().Name;
        BoardSerializer.SaveText(mainWindow.BoardManager.Board, mainWindow.BoardManager.Rule.Description, modelName, dlg.FileName);
    }

    public static void LoadBoard(MainWindow mainWindow)
    {
        var dlg = new OpenFileDialog { Filter = "Text board (*.txt)|*.txt" };
        if (dlg.ShowDialog() != true) return;

        var (board, ruleText) = BoardSerializer.LoadText(dlg.FileName);
        mainWindow.BoardManager.LoadBoard(board);
        mainWindow.BoardManager.ApplyRule(ruleText);
        mainWindow.Redraw();
    }

    public static void ExportImage(MainWindow mainWindow)
    {
        var dlg = new SaveFileDialog { Filter = "PNG (*.png)|*.png" };
        if (dlg.ShowDialog() == true)
            BoardSerializer.ExportImage(mainWindow.BoardManager.Board, mainWindow.Renderer, mainWindow.BoardManager.ColoringStrategy, dlg.FileName);
    }

    public static void ExportMp4(MainWindow mainWindow, int frameCount, int framerate = 24)
    {
        var dlg = new SaveFileDialog { Filter = "MP4 (*.mp4)|*.mp4" };
        if (dlg.ShowDialog() == true)
            BoardSerializer.ExportMp4(mainWindow.BoardManager, new BoardRenderer(), mainWindow.BoardManager.ColoringStrategy, dlg.FileName, frameCount, framerate);
    }

    public static void ExportGif(MainWindow mainWindow, int frameCount)
    {
        var dlg = new SaveFileDialog { Filter = "GIF (*.gif)|*.gif" };
        if (dlg.ShowDialog() == true)
            BoardSerializer.ExportGif(mainWindow.BoardManager, new BoardRenderer(), mainWindow.BoardManager.ColoringStrategy, dlg.FileName, frameCount);
    }
}