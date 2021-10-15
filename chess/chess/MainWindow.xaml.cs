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
            board = new Board()
            {
                Name = "Board",
                Height = 400,
                Width = 400,
                Background = Brushes.Black
            };
            mainGrid.Children.Add(board);
            board.RegenerateBoard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            board.RegenerateBoard();
        }
    }
}
