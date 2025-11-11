using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfLifeWPF.Models;
using Color = System.Drawing.Color;

namespace GameOfLifeWPF.Rendering;

public class BoardRenderer
{
    private byte[]? pixelBuffer;
    private int bufferWidthPx;
    private int bufferHeightPx;

    public WriteableBitmap CreateBitmap(Board board, int zoomLevel)
    {
        var widthPx = board.Width * zoomLevel;
        var heightPx = board.Height * zoomLevel;
        if (pixelBuffer == null || bufferWidthPx != widthPx || bufferHeightPx != heightPx)
        {
            pixelBuffer = new byte[widthPx * heightPx * 4];
            bufferWidthPx = widthPx;
            bufferHeightPx = heightPx;
        }

        return new WriteableBitmap(widthPx, heightPx, GameOfLifeDefaults.DefaultDpiX, GameOfLifeDefaults.DefaultDpiY,
            PixelFormats.Bgra32, null);
    }

    public void Render(Board board, WriteableBitmap bitmap, IColoringStrategy coloringStrategy, int zoomLevel,
        CellShape cellShape)
    {
        var w = board.Width;
        var h = board.Height;
        var widthPx = w * zoomLevel;
        var heightPx = h * zoomLevel;
        var stride = widthPx * 4;
        var tileSize = 64;
        var tileCountX = (w + tileSize - 1) / tileSize;
        var tileCountY = (h + tileSize - 1) / tileSize;

        if (pixelBuffer == null || bufferWidthPx != widthPx || bufferHeightPx != heightPx)
        {
            pixelBuffer = new byte[widthPx * heightPx * 4];
            bufferWidthPx = widthPx;
            bufferHeightPx = heightPx;
        }

        Array.Clear(pixelBuffer, 0, pixelBuffer.Length);

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
                    for (var x = xStart; x < xEnd; x++)
                    {
                        var state = board.Cells[y, x];
                        if (state == 0) continue;
                        var color = coloringStrategy.GetColor(state);

                        var center = zoomLevel / 2.0;
                        var radius = zoomLevel * 0.45;
                        var px0 = center;
                        var py0 = center - radius;
                        var px1 = center - radius;
                        var py1 = center + radius;
                        var px2 = center + radius;
                        var py2 = center + radius;

                        for (var iy = 0; iy < zoomLevel; iy++)
                        for (var ix = 0; ix < zoomLevel; ix++)
                        {
                            bool paint = false;
                            switch (cellShape)
                            {
                                case CellShape.Circle:
                                    var dx = ix + 0.5 - center;
                                    var dy = iy + 0.5 - center;
                                    paint = (dx * dx + dy * dy <= radius * radius);
                                    break;
                                case CellShape.Square:
                                    paint = true;
                                    break;
                                case CellShape.Triangle:
                                    var s = ((py0 - py2) * (ix - px2) + (px2 - px0) * (iy - py2)) /
                                            ((py0 - py2) * (px1 - px2) + (px2 - px0) * (py1 - py2));
                                    var t = ((py1 - py0) * (ix - px0) + (px0 - px1) * (iy - py0)) /
                                            ((py1 - py0) * (px2 - px0) + (px0 - px1) * (py2 - py0));
                                    var u = 1 - s - t;
                                    paint = (s >= 0) && (t >= 0) && (u >= 0);
                                    break;
                            }

                            if (!paint) continue;

                            var px = x * zoomLevel + ix;
                            var py = y * zoomLevel + iy;
                            var idx = (py * widthPx + px) * 4;
                            pixelBuffer[idx + 0] = color.B;
                            pixelBuffer[idx + 1] = color.G;
                            pixelBuffer[idx + 2] = color.R;
                            pixelBuffer[idx + 3] = 255;
                        }
                    }
                }
            }
        });

        bitmap.WritePixels(
            new System.Windows.Int32Rect(0, 0, widthPx, heightPx),
            pixelBuffer, stride, 0);
    }
}