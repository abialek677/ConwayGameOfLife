using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GameOfLifeWPF.Logic;
using GameOfLifeWPF.Models;
using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF;

public partial class MainWindow : Window
{
    private BoardManager _boardManager;
    private BoardRenderer _renderer;
    
    private DispatcherTimer _timer;

    private int _zoomLevel = 4;
    public bool IsPlaying;
    public bool IsDrawing;
    public Pattern? SelectedPattern;
    public bool IsInsertMode;
    public WriteableBitmap Bitmap;
    
    public int ZoomLevel => _zoomLevel;
    public BoardManager BoardManager => _boardManager;
    public BoardRenderer Renderer => _renderer;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGame();
        InitializeUi();
    }
    
    private void InitializeGame()
    {
        _renderer = new BoardRenderer();
        FFMediaToolkit.FFmpegLoader.FFmpegPath = ConfigurationManager.AppSettings["ffmpegPath"];

        _boardManager = new BoardManager(
            w: 200,
            h: 200,
            rule: new BinaryRule("B3/S23"),
            coloring: new ClassicColoring(),
            wrap: false
        );

        Bitmap = _renderer.CreateBitmap(_boardManager.Board);
        BoardImage.Source = Bitmap;

        _timer = new DispatcherTimer();
        _timer.Tick += (_, _) =>
        {
            _boardManager.Step();
            Redraw();
        };
        _timer.Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value);
        SpeedSlider.ValueChanged += (_, _) =>
            _timer.Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value);
    }
    
    private void InitializeUi()
    {
        CmbPalette.SelectionChanged += UiHandlers.OnPaletteChanged;
        CmbPatterns.ItemsSource = PatternRepository.Patterns.Keys;

        BoardImage.MouseLeftButtonDown += UiHandlers.OnBoardMouseDown;
        BoardImage.MouseLeftButtonUp += UiHandlers.OnBoardMouseUp;
        BoardImage.MouseMove += UiHandlers.OnBoardMouseMove;

        ZoomSlider.ValueChanged += UiHandlers.OnZoomChanged;

        Redraw();
    }
    
    public void Redraw()
    {
        if (Bitmap.PixelWidth != _boardManager.Board.Width || Bitmap.PixelHeight != _boardManager.Board.Height)
            Bitmap = _renderer.CreateBitmap(_boardManager.Board);

        _renderer.Render(_boardManager.Board, Bitmap, _boardManager.ColoringStrategy);
        UpdateImageSize();
        UiHandlers.RefreshStatistics(_boardManager, TxtGenerations, TxtBorn, TxtDied, TxtCells);
    }

    public void UpdateImageSize()
    {
        if (BoardImage == null || _boardManager?.Board == null || ZoomSlider == null) 
            return;
        _zoomLevel = (int)ZoomSlider.Value;
        BoardImage.Width = _boardManager.Board.Width * _zoomLevel;
        BoardImage.Height = _boardManager.Board.Height * _zoomLevel;
    }
}