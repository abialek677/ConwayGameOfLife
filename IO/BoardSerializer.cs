using System.IO;
using GameOfLifeWPF.Models;
using GameOfLifeWPF.Rendering;
using System.Windows.Media.Imaging;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using GameOfLifeWPF.Logic;

namespace GameOfLifeWPF.IO
{
    public static class BoardSerializer
    {
        public static void SaveText(Board board, string rule, string model, string path)
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine($"MODEL:{model}");
            writer.WriteLine(rule);
            writer.WriteLine($"{board.Width} {board.Height}");
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                    writer.Write(board.Cells[y, x]);
                writer.WriteLine();
            }
        }

        public static (Board board, string rule) LoadText(string path)
        {
            using var reader = new StreamReader(path);
            var rule = reader.ReadLine()?.Trim() ?? "B3/S23";
            var dims = reader.ReadLine()?.Split(' ', '\t') ?? throw new IOException("no dims");
            var w = int.Parse(dims[0]);
            var h = int.Parse(dims[1]);
            var board = new Board(w, h);
            for (var y = 0; y < h; y++)
            {
                var line = reader.ReadLine();
                
                for (var x = 0; x < w && x < (line?.Length ?? 0); x++)
                {
                    board.Cells[y, x] = byte.TryParse(line[x].ToString(), out byte b) ? b : (byte)0;
                }
                    
            }
            return (board, rule);
        }

        public static void ExportImage(Board board, BoardRenderer renderer, IColoringStrategy coloringStrategy, string path)
        {
            var bitmap = renderer.CreateBitmap(board);
            renderer.Render(board, bitmap, coloringStrategy);
            using var stream = new FileStream(path, FileMode.Create);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }

        public static void ExportMp4(BoardManager boardManager, BoardRenderer renderer, IColoringStrategy coloringStrategy, string outputPath, int frameCount, int framerate = 10)
        {
            var board = boardManager.Board;
            var videoParams = new VideoEncoderSettings(
                width: board.Width, height: board.Height, framerate: framerate, codec: VideoCodec.H264);

            using var file = MediaBuilder.CreateContainer(outputPath).WithVideo(videoParams).Create();
            for (var i = 0; i < frameCount; i++)
            {
                var bitmap = renderer.CreateBitmap(board);
                renderer.Render(board, bitmap, coloringStrategy);

                var w = bitmap.PixelWidth;
                var h = bitmap.PixelHeight;
                var stride = w * 4;
                var pixelBuffer = new byte[h * stride];
                bitmap.CopyPixels(pixelBuffer, stride, 0);

                var imageData = ImageData.FromArray(pixelBuffer, ImagePixelFormat.Bgra32, w, h);
                file.Video.AddFrame(imageData);

                boardManager.Step();
            }
        }

        public static void ExportGif(BoardManager boardManager, BoardRenderer renderer, IColoringStrategy coloringStrategy, string path, int frameCount)
        {
            var gifEncoder = new GifBitmapEncoder();
            for (var i = 0; i < frameCount; i++)
            {
                var bitmap = renderer.CreateBitmap(boardManager.Board);
                renderer.Render(boardManager.Board, bitmap, coloringStrategy);
                gifEncoder.Frames.Add(BitmapFrame.Create(bitmap));
                boardManager.Step();
            }
            using var stream = new FileStream(path, FileMode.Create);
            gifEncoder.Save(stream);
        }
    }
}
