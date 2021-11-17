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
    //<Border HorizontalAlignment = "Center" Height="811" Margin="0,100,0,0"
    //VerticalAlignment="Top" Width="350"
    //CornerRadius="10,10,10,10" Background="#FFFDFDFD">

    //<Border.Effect>
    //    <DropShadowEffect BlurRadius = "7" Direction="-45" Opacity="0.35"/>
    //</Border.Effect>
    class HistoryList : Border
    {
        private StackPanel StackPanel;
        private ScrollViewer scroll;
        public HistoryList() : base()
        {
            CornerRadius = new System.Windows.CornerRadius(10);
            Background = new SolidColorBrush(Color.FromRgb(0xFD, 0xFD, 0xFD));
            Effect = new DropShadowEffect()
            {
                BlurRadius = 7,
                Direction = -45,
                Opacity = 0.35
            };

            StackPanel = new StackPanel();

            scroll = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
            };
            scroll.Content = StackPanel;
            Child = scroll;
        }

        public void AddChildren(HistoryItem item)
        {
            StackPanel.Children.Insert(0, item);
            scroll.PageUp();
            
        }

        public void RemoveChildren(HistoryItem item)
        {
            if (StackPanel.Children.Contains(item))
                StackPanel.Children.Remove(item);
        }
    }
}
