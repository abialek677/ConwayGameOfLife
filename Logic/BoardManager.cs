using GameOfLifeWPF.Models;
using GameOfLifeWPF.Rendering;

namespace GameOfLifeWPF.Logic
{
    public class BoardManager
    {
        public Board Board { get; private set; }
        public IAutomatonRule Rule { get; set; }
        public IColoringStrategy ColoringStrategy { get; set; }
        public bool Wrap { get; set; }

        public long Generations { get; private set; }
        public long BornTotal { get; private set; }
        public long DiedTotal { get; private set; }

        public BoardManager(int w, int h, IAutomatonRule rule, IColoringStrategy coloring, bool wrap)
        {
            Board = new Board(w, h);
            Rule = rule;
            ColoringStrategy = coloring;
            Wrap = wrap;
            ResetStats();
        }

        public void ApplyRule(string text) => Rule = new BinaryRule(text);

        public void Step()
        {
            var stats = Board.Step(Wrap, Rule, ColoringStrategy);
            Generations++;
            BornTotal += stats.Born;
            DiedTotal += stats.Died;
        }

        public void Randomize(double aliveProb = 0.2)
        {
            int colors = ColoringStrategy is ImmigrationColoring ? 2 :
                         ColoringStrategy is QuadLifeColoring ? 4 : 1;
            Board.Randomize(new Random(), aliveProb, colors);
            ResetStats();
        }

        public void Clear()
        {
            Board.Clear();
            ResetStats();
        }

        public void ResetStats()
        {
            Generations = 0;
            BornTotal = 0;
            DiedTotal = 0;
        }

        public void InsertPattern(byte[,] pattern, int offsetX = -1, int offsetY = -1)
        {
            var pw = pattern.GetLength(1);
            var ph = pattern.GetLength(0);
            var ox = offsetX >= 0 ? offsetX : Math.Max(0, Board.Width / 2 - pw / 2);
            var oy = offsetY >= 0 ? offsetY : Math.Max(0, Board.Height / 2 - ph / 2);

            for (var y = 0; y < ph; y++)
            for (var x = 0; x < pw; x++)
                Board.Cells[oy + y, ox + x] = pattern[y, x];
        }

        public void LoadBoard(Board newBoard)
        {
            Board = newBoard;
            ResetStats();
        }
        
        public void CycleCellColor(int x, int y)
        {
            int colorCount = ColoringStrategy switch
            {
                QuadLifeColoring => 4,
                ImmigrationColoring => 2,
                _ => 1
            };
            var cell = Board.Cells;
            byte curr = cell[y, x];
            cell[y, x] = (byte)((curr + 1) % (colorCount + 1));
        }
    }
}
