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
    private CellShape _cellShape = CellShape.Square;
    
    public CellShape CellShape => _cellShape;
    
    public int ZoomLevel => _zoomLevel;
    public BoardManager BoardManager => _boardManager;
    public BoardRenderer Renderer => _renderer;
    public ColorPalette SelectedColorPalette => Palettes.All[CmbColorPalette.SelectedIndex];
    
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
            GameOfLifeDefaults.DefaultBoardWidth,
            GameOfLifeDefaults.DefaultBoardHeight,
            rule: new BinaryRule(GameOfLifeDefaults.DefaultRule),
            coloring: new ClassicColoring(),
            wrap: false
        );

        Bitmap = _renderer.CreateBitmap(_boardManager.Board, _zoomLevel);
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
        
        CmbCellShape.SelectionChanged += (s, e) =>
        {
            _cellShape = (CellShape)CmbCellShape.SelectedIndex;
            Redraw();
        };
        
        CmbColorPalette.ItemsSource = Palettes.All.Select(p => p.Name).ToList();
        CmbColorPalette.SelectedIndex = 0;
        CmbColorPalette.SelectionChanged += (s, e) => Redraw();

        Redraw();
    }
    
    public void Redraw()
    {
        if (Bitmap.PixelWidth != _boardManager.Board.Width * _zoomLevel 
            || Bitmap.PixelHeight != _boardManager.Board.Height * _zoomLevel)
        {
            Bitmap = _renderer.CreateBitmap(_boardManager.Board, _zoomLevel);
            BoardImage.Source = Bitmap;
        }

        _renderer.Render(_boardManager.Board, Bitmap, _boardManager.ColoringStrategy, _zoomLevel, _cellShape, SelectedColorPalette);
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