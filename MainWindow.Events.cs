
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GameOfLifeWPF.Extensions;
using GameOfLifeWPF.IO;
using GameOfLifeWPF.Models;
using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF;

public partial class MainWindow
{
    #region Buttons

    private void BtnNew_Click(object sender, RoutedEventArgs e)
    {
        var width = int.TryParse(TxtWidth.Text, out var w) ? w : 200;
        var height = int.TryParse(TxtHeight.Text, out var h) ? h : 200;
        _boardManager.Board.Resize(width, height);
        _boardManager.ResetStats();
        
        Bitmap = _renderer.CreateBitmap(_boardManager.Board);
        BoardImage.Source = Bitmap;
        UpdateImageSize();

        Redraw();
    }

    private void BtnRandom_Click(object sender, RoutedEventArgs e)
    {
        var aliveProb = 0.2;
        _boardManager.Randomize(aliveProb);
        Redraw();
    }

    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
        _boardManager.Clear();
        Redraw();
    }

    private void BtnStep_Click(object sender, RoutedEventArgs e)
    {
        _boardManager.Step();
        Redraw();
    }

    private async void BtnPlay_Checked(object sender, RoutedEventArgs e)
    {
        IsPlaying = true;
        while (IsPlaying)
        {
            _boardManager.Step();
            Redraw();
            await Task.Delay(Math.Max(1, (int)SpeedSlider.Value));
        }
    }

    private void BtnPlay_Unchecked(object sender, RoutedEventArgs e)
    {
        IsPlaying = false;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        FileIoHandlers.SaveBoard(this);
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        FileIoHandlers.LoadBoard(this);
    }

    private void BtnExportImage_Click(object sender, RoutedEventArgs e)
    {
        FileIoHandlers.ExportImage(this);
    }

    private void BtnExportMp4_Click(object sender, RoutedEventArgs e)
    {
        int frames = int.TryParse(TxtExportFrames.Text, out var f) && f > 0 ? f : 30;
        FileIoHandlers.ExportMp4(this, frames);
    }

    private void BtnExportGif_Click(object sender, RoutedEventArgs e)
    {
        int frames = int.TryParse(TxtExportFrames.Text, out var f) ? f : 30;
        FileIoHandlers.ExportGif(this, frames);
    }

    private void BtnApplyRule_Click(object sender, RoutedEventArgs e)
    {
        _boardManager.ApplyRule(TxtRule.Text);
        Redraw();
    }

    private void BtnInsertPattern_Click(object sender, RoutedEventArgs e)
    {
        if (CmbPatterns.SelectedItem is string name && PatternRepository.Patterns.TryGetValue(name, out var pattern))
        {
            SelectedPattern = pattern;
            IsInsertMode = true;
            Mouse.OverrideCursor = Cursors.Cross;
        }
        else
        {
            IsInsertMode = false;
            Mouse.OverrideCursor = null;
        }
    }

    #endregion

    #region Sliders and Palette

    private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        UpdateImageSize();
    }

    #endregion
    

    #region Pattern Helpers

    private void CmbPatterns_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var name = CmbPatterns.SelectedItem as string;
        if (name != null && PatternRepository.Patterns.TryGetValue(name, out var pattern))
            SelectedPattern = pattern;
    }

    #endregion

    #region Wrap Toggle

    private void ToggleWrap_Checked(object sender, RoutedEventArgs e)
    {
        _boardManager.Wrap = true;
        Redraw();
    }

    private void ToggleWrap_Unchecked(object sender, RoutedEventArgs e)
    {
        _boardManager.Wrap = false;
        Redraw();
    }

    #endregion
}
