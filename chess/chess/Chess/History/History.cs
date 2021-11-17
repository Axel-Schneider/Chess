using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace chess
{
    class HistoryGrid : Border
    {
        private HistoryList list;
        public HistoryGrid() : base()
        {
            Margin = new System.Windows.Thickness(0,0,0,0);
            Width = double.NaN;
            CornerRadius = new System.Windows.CornerRadius(10);
            Background = new SolidColorBrush(Color.FromRgb(0xf7, 0xf7, 0xf7));
            Effect = new DropShadowEffect()
            {
                Direction = -45,
                Opacity = 0.35,
                BlurRadius = 5.3
            };
            initUI();
            
        }

        private void initUI()
        {
            SvgViewbox svg = new SvgViewbox()
            {
                Source = new Uri(GraphicPath.Show.History),

            };

            HistoryItem title = new HistoryItem(svg, "History")
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Margin = new System.Windows.Thickness(10, 10, 10, 0),
            };
            title.label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            title.label.FontSize = 20;
                    

            list = new HistoryList()
            {
            Margin = new System.Windows.Thickness(20, 100, 20, 20)

            };

            Grid grid = new Grid();
            Child = grid;
            grid.Children.Add(title);
            grid.Children.Add(list);


        }
        public void AddChildren(HistoryItem item) => list.AddChildren(item);
        public void RemoveChildren(HistoryItem item) => list.RemoveChildren(item);
    }
}
