using System.Windows;
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

        var (board, rule, model) = BoardSerializer.LoadText(dlg.FileName);
        mainWindow.BoardManager.LoadBoard(board);
        mainWindow.BoardManager.ApplyRule(rule);
        
        mainWindow.BoardManager.ColoringStrategy = model switch
        {
            "ImmigrationColoring" => new ImmigrationColoring(),
            "QuadLife" => new QuadLifeColoring(),
            _ => new ClassicColoring()
        };
        
        var bitmap = mainWindow.Renderer.CreateBitmap(board);
        mainWindow.BoardImage.Source = bitmap;
        mainWindow.Bitmap = bitmap;
        mainWindow.UpdateImageSize();
        
        mainWindow.Redraw();
    }

    public static void ExportImage(MainWindow mainWindow)
    {
        var dlg = new SaveFileDialog { Filter = "PNG (*.png)|*.png" };
        if (dlg.ShowDialog() == true)
            BoardSerializer.ExportImage(mainWindow.BoardManager.Board, mainWindow.Renderer, mainWindow.BoardManager.ColoringStrategy, dlg.FileName);
    }

    public static async void ExportMp4(MainWindow mainWindow, int frameCount, int framerate = 24)
    {
        var dlg = new SaveFileDialog { Filter = "MP4 (*.mp4)|*.mp4" };
        if (dlg.ShowDialog() != true) return;

        var boardManager = mainWindow.BoardManager;
        var renderer = new BoardRenderer();
        var coloring = boardManager.ColoringStrategy;
        var outputPath = dlg.FileName;
        
        await Task.Run(() =>
        {
            BoardSerializer.ExportMp4(boardManager, renderer, coloring, outputPath, frameCount, framerate);
        });

        MessageBox.Show("MP4 export finished!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public static async void ExportGif(MainWindow mainWindow, int frameCount)
    {
        var dlg = new SaveFileDialog { Filter = "GIF (*.gif)|*.gif" };
        if (dlg.ShowDialog() != true) return;

        var boardManager = mainWindow.BoardManager;
        var renderer = new BoardRenderer();
        var coloring = boardManager.ColoringStrategy;
        var outputPath = dlg.FileName;
        
        await Task.Run(() =>
        {
            BoardSerializer.ExportGif(boardManager, renderer, coloring, outputPath, frameCount);
        });

        MessageBox.Show("GIF export finished!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}