using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace chess
{
    class PopUp : Border
    {
        private Grid content = new Grid();
        private Border btnFirst;
        private Border btnSecond;
        private string Text;
        private string btn1Content;
        private string btn2Content;
        public PopUp(string text, string btn1 = "Yes", string btn2 = "No") : base()
        {
            CornerRadius = new CornerRadius(15);
            Child = content;
            Text = text;
            btn1Content = btn1;
            btn2Content = btn2;
            initUI();
        }
        private void initUI()
        {
            Background = Brushes.White;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            Label Texte = new Label()
            {
                Content = Text,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 50),
                FontSize = 20,
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };

            Label label = new Label()
            {
                Content = btn1Content,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                MinWidth = 100,
                FontSize = 16,
                Margin = new Thickness(0),
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };
            btnFirst = new Border()
            {
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(20),
                Child = label,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 5,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };
            btnFirst.Margin = new Thickness(250, 150, 20, 20);
            btnFirst.MouseEnter += BtnDraw_MouseEnter;
            btnFirst.MouseLeave += BtnDraw_MouseLeave;
            btnFirst.MouseUp += BtnDraw_MouseUp;


            Label label2 = new Label()
            {
                Content = btn2Content,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                MinWidth = 100,
                Margin = new Thickness(0),
                FontSize = 16,
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };
            btnSecond = new Border()
            {
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(20),
                Child = label2,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 5,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };
            btnSecond.Margin = new Thickness(20, 150, 250, 20);
            btnSecond.MouseEnter += BtnDraw_MouseEnter;
            btnSecond.MouseLeave += BtnDraw_MouseLeave;
            btnSecond.MouseUp += BtnDraw_MouseUp;

            content.Children.Add(Texte);
            content.Children.Add(btnFirst);
            content.Children.Add(btnSecond);
        }

        private void BtnDraw_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool res = sender == btnFirst;
            onClick?.Invoke(this, new PopUpArgs(res));
        }

        private void BtnDraw_MouseLeave(object sender, MouseEventArgs e)
        {
            ((DropShadowEffect)((Border)sender).Effect).Color = Color.FromRgb(128, 128, 128);
        }

        private void BtnDraw_MouseEnter(object sender, MouseEventArgs e)
        {
            ((DropShadowEffect)((Border)sender).Effect).Color = Color.FromRgb(200, 0, 0);
        }

        public event EventHandler onClick;


    }
    public class PopUpArgs : EventArgs
    {
        public bool Choose { get; private set; }

        public PopUpArgs(bool choose)
        {
            Choose = choose;
        }
    }
}
