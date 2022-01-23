using System.Windows.Controls;

namespace MineSweeper
{
    public class ButtonCell : Button
    {
        public bool IsMined { get; set; }
        public bool IsFlagged { get; set; }
        public ButtonCell(bool isMined)
        {
            IsMined = isMined;
        }
    }
}
