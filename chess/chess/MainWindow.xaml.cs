using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board board;
        public MainWindow()
        {
            InitializeComponent();
            Height = BoardGrid.Height + 60;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Show.Background);
            bitmap.EndInit();

            BoardGrid.Background = new ImageBrush(bitmap);

            board = new Board()
            {
                Name = "Board",
                Height = BoardGrid.Height - 20,
                Width = BoardGrid.Width - 20,
            };
            BoardGrid.Children.Add(board);
            board.RegenerateBoard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            board.RegenerateBoard();
        }
    }
}
