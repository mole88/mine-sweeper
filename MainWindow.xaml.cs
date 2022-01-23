using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MineSweeper
{
    public partial class MainWindow : Window
    {
        private ButtonCell[,] Cells;
        private bool isGameOver;
        private int Level;
        private string lastFieldSizeText = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            StartButton.Click += (s, e) => StartButtonClick();
            CheckMinesButton.Click += (s, e) => CheckMinesButtonClick();
        }
        private void CheckMinesButtonClick()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (Cells[x, y].IsMined == Cells[x, y].IsFlagged)
                        continue;
                    else
                    {
                        GameOver();
                        MessageBox.Show("You lose!");
                        return;
                    }
                }
            }
            GameOver();
            MessageBox.Show("You win!");
        }
        private void StartButtonClick()
        {
            int x, y;

            try
            {
                string[] strFieldSize = FieldSizeTextBox.Text.Split('х', 'x');
                x = int.Parse(strFieldSize[0]);
                y = int.Parse(strFieldSize[1]);

                Level = int.Parse(LevelTextBox.Text);

                if (Level >= 9)
                {
                    Level = 1;
                    LevelTextBox.Text = "1";
                }
            }
            catch (Exception)
            {
                x = 9;
                y = 9;
                Level = 1;

                LevelTextBox.Text = "1";
                FieldSizeTextBox.Text = "9x9";

            }

            if (lastFieldSizeText != FieldSizeTextBox.Text)
            {
                GridField.RowDefinitions.Clear();
                GridField.ColumnDefinitions.Clear();
                
                CreateField(x, y);
            }
            else
            {
                UpdateField(x, y);
            }

            lastFieldSizeText = FieldSizeTextBox.Text;
            isGameOver = false;
        }
        private void UpdateField(int x, int y)
        {
            ButtonCell cell;
            for (int Y = 0; Y < y; Y++)
            {
                for (int X = 0; X < x; X++)
                {
                    cell = Cells[X, Y];
                    cell.Content = string.Empty;
                    cell.IsEnabled = true;
                    cell.IsFlagged = false;
                    cell.IsMined = !Convert.ToBoolean(new Random().Next(10 - Level));
                }
            }
        }
        private void CreateField(int x, int y)
        {
            Cells = new ButtonCell[x, y];
            for (int Y = 0; Y < y; Y++)
            {
                GridField.RowDefinitions.Add(new RowDefinition());
            }
            for (int X = 0; X < x; X++)
            {
                GridField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int Y = 0; Y < y; Y++)
            {
                for (int X = 0; X < x; X++)
                {
                    CreateItemField(X, Y, Brushes.AliceBlue);
                }
            }
        }
        private void CreateItemField(int x, int y, Brush color)
        {
            ButtonCell cell = new(!Convert.ToBoolean(new Random().Next(10 - Level)))
            {
                FontSize = 55 * 9 / Cells.GetLength(0),
                FontStretch = FontStretches.Condensed,
                Margin = new Thickness(0.5d)
            };
            cell.Click += (s, e) => LeftMouseButtonClick(x, y);
            cell.MouseRightButtonDown +=  (s, e) => RightMouseButtonClick(x, y);

            GridField.Children.Add(cell);
            Grid.SetRow(cell, y);
            Grid.SetColumn(cell, x);
            Cells[x, y] = cell;
        }
        private void RightMouseButtonClick(int x, int y)
        {
            ButtonCell cell = Cells[x, y];
            if (cell.IsFlagged == false)
            {
                cell.IsFlagged = true;
                cell.Foreground = Brushes.Red;
                cell.Content = "◉";
            }
            else
            {
                cell.IsFlagged = false;
                cell.Foreground = Brushes.Black;
                cell.Content = "";
            }
        }
        private void LeftMouseButtonClick(int x, int y)
        {
            if (!Cells[x, y].IsFlagged) ClickFieldItem(x, y);
        }
        private void ClickFieldItem(int X, int Y)
        {
            int mines = 0;
            ButtonCell cell = Cells[X, Y];
            
            if (cell.IsMined)
            {
                cell.Foreground = Brushes.Black;
                cell.Content = "⚙";
                cell.IsEnabled = false;
                if (!isGameOver)
                    GameOver();
            }
            else
            {
                cell.IsEnabled = false;
                for (int y = Y - 1; y <= Y + 1; y++)
                {
                    for (int x = X - 1; x <= X + 1; x++)
                    {
                        if (y >= 0 && y < Cells.GetLength(0) && x >= 0 && x < Cells.GetLength(1))
                            if (Cells[x, y].IsMined) mines++;
                    }
                }
                if (mines == 0)
                {
                    for (int y = Y - 1; y <= Y + 1; y++)
                    {
                        for (int x = X - 1; x <= X + 1; x++)
                        {
                            if (y >= 0 && y < Cells.GetLength(0) && x >= 0 && x < Cells.GetLength(1))
                                if (Cells[x, y].IsEnabled) ClickFieldItem(x, y);
                        }
                    }
                }

                PrintMinesCount(cell, mines);
            }
        }
        private void PrintMinesCount(Button button, int mines)
        {
            switch (mines)
            {
                case 0: return;
                case 1: button.Foreground = Brushes.Blue; break;
                case 2: button.Foreground = Brushes.Green; break;
                case 3: button.Foreground = Brushes.Red; break;
                case 4: button.Foreground = Brushes.Violet; break;
                case 5: button.Foreground = Brushes.Brown; break;
                case 6: button.Foreground = Brushes.Orange; break;
                case 7: button.Foreground = Brushes.Yellow; break;
                case 8: button.Foreground = Brushes.LightCoral; break;
            }
            button.Content = mines;
        }
        private void GameOver()
        {
            isGameOver = true;
            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                for (int x = 0; x < Cells.GetLength(1); x++)
                    if (Cells[x, y].IsEnabled)
                        ClickFieldItem(x, y);
            }
        }
    }
}
