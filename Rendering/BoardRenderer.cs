using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfLifeWPF.Models;
using Color = System.Drawing.Color;

namespace GameOfLifeWPF.Rendering
{
    public class BoardRenderer
    {
        private byte[] pixelBuffer;

        public WriteableBitmap CreateBitmap(Board board)
        {
            pixelBuffer = new byte[board.Width * board.Height * 4];
            return new WriteableBitmap(board.Width, board.Height, 96, 96, PixelFormats.Bgra32, null);
        }
        
        public void Render(Board board, WriteableBitmap bitmap, IColoringStrategy coloringStrategy)
        {
            var w = board.Width;
            var h = board.Height;
            var stride = w * 4;
            var tileSize = 64;
            var tileCountX = (w + tileSize - 1) / tileSize;
            var tileCountY = (h + tileSize - 1) / tileSize;

            Parallel.For(0, tileCountY, ty =>
            {
                var yStart = ty * tileSize;
                var yEnd = Math.Min(yStart + tileSize, h);

                for (var tx = 0; tx < tileCountX; tx++)
                {
                    var xStart = tx * tileSize;
                    var xEnd = Math.Min(xStart + tileSize, w);

                    for (var y = yStart; y < yEnd; y++)
                    {
                        var rowOffset = y * stride;
                        for (var x = xStart; x < xEnd; x++)
                        {
                            var state = board.Cells[y, x];
                            var c = coloringStrategy?.GetColor(state) ?? Color.Magenta;
                            var idx = rowOffset + x * 4;
                            pixelBuffer[idx + 0] = c.B;
                            pixelBuffer[idx + 1] = c.G;
                            pixelBuffer[idx + 2] = c.R;
                            pixelBuffer[idx + 3] = 255;
                        }
                    }
                }
            });

            bitmap.WritePixels(new System.Windows.Int32Rect(0, 0, w, h), pixelBuffer, stride, 0);
        }
    }
}
