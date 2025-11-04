using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GameOfLifeWPF.Extensions;
using GameOfLifeWPF.Logic;
using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF;

public static class UiHandlers
{
    public static void OnPaletteChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedIndex >= 0)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null) return;

            var manager = mainWindow.BoardManager;
            switch (comboBox.SelectedIndex)
            {
                case 1:
                    manager.ColoringStrategy = new ImmigrationColoring();
                    break;
                case 2:
                    manager.ColoringStrategy = new QuadLifeColoring();
                    break;
                default:
                    manager.ColoringStrategy = new ClassicColoring();
                    break;
            }
            mainWindow.Redraw();
        }
    }

    public static void RefreshStatistics(BoardManager manager, TextBlock gen, TextBlock born, TextBlock died, TextBlock alive)
    {
        gen.Text = $"Generations: {manager.Generations}";
        born.Text = $"Born: {manager.BornTotal}";
        died.Text = $"Died: {manager.DiedTotal}";

        var count = 0;
        for (var y = 0; y < manager.Board.Height; y++)
            for (var x = 0; x < manager.Board.Width; x++)
                if (manager.Board.Cells[y, x] != 0) count++;

        alive.Text = $"Alive cells: {count}";
    }

    public static void OnBoardMouseDown(object sender, MouseButtonEventArgs e)
    {
        var mainWindow = Application.Current.MainWindow as MainWindow;
        if (mainWindow == null) return;
        if (mainWindow.IsPlaying) return;

        var pos = e.GetPosition(mainWindow.BoardImage);

        if (mainWindow.IsInsertMode && mainWindow.SelectedPattern != null)
        {
            mainWindow.BoardManager.InsertPattern(
                mainWindow.SelectedPattern.Shape,
                (int)(pos.X / mainWindow.ZoomLevel),
                (int)(pos.Y / mainWindow.ZoomLevel)
            );

            mainWindow.Redraw();
            mainWindow.IsInsertMode = false;
            Mouse.OverrideCursor = null;
            return;
        }

        mainWindow.IsDrawing = true;
        SetCell(mainWindow, pos);
    }

    public static void OnBoardMouseUp(object sender, MouseButtonEventArgs e)
    {
        var main = Application.Current.MainWindow as MainWindow;
        if (main != null)
            main.IsDrawing = false;
    }

    public static void OnBoardMouseMove(object sender, MouseEventArgs e)
    {
        var mainWindow = Application.Current.MainWindow as MainWindow;
        if (mainWindow == null || !mainWindow.IsDrawing || mainWindow.IsPlaying) return;
        SetCell(mainWindow, e.GetPosition(mainWindow.BoardImage));
    }

    private static void SetCell(MainWindow window, Point pos)
    {
        var x = (int)(pos.X / window.ZoomLevel);
        var y = (int)(pos.Y / window.ZoomLevel);
        if (!window.BoardManager.Board.IsCellInBounds(x, y)) return;
        window.BoardManager.CycleCellColor(x, y);
        window.Redraw();
    }

    public static void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var mainWindow = Application.Current.MainWindow as MainWindow;
        mainWindow?.Redraw();
    }
}
