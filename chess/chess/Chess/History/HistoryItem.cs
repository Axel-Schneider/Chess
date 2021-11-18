using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace chess
{
    class HistoryItem : Border
    {
        public Label label;
        public SvgViewbox Image { get; private set; }
        public string Content { get; private set; }
        public HistoryItem(SvgViewbox image, string content)
        {
            Image = image;
            Content = content;
            initUI();
        }
        public HistoryItem(MoveLog move) : base()
        {
            Image = new SvgViewbox()
            {
                Source = move.Piece.UIPiece.Image.Source
            };
            Content = move.ToString();
            initUI();
        }

        private void initUI()
        {
            BorderThickness = new Thickness(0.5);
            BorderBrush = Brushes.LightGray;
            Margin = new Thickness(20, 10, 20, 0);
            Height = 50;
            Background = Brushes.White;
            CornerRadius = new CornerRadius(10);
            DropShadowEffect effect = new DropShadowEffect()
            {
                BlurRadius = 9,
                Direction = 315,
                Opacity = 0.35,
                ShadowDepth = 9
            };
            Effect = effect;


            Grid grd = new Grid();

            Image.Margin = new Thickness(5);
            Image.HorizontalAlignment = HorizontalAlignment.Left;

            label = new Label()
            {
                Content = Content,
                Margin = new Thickness(Height, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Nirmala UI"),
                FontSize = 16
            };

            grd.Children.Add(Image);
            grd.Children.Add(label);

            Child = grd;

        }
    }
}
