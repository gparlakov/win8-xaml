using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Paint
{
    public sealed partial class MainPage : Page
    {
        private const int MaximumSize = 130;
        private const int MinimumSize = 10;
        private const int ColorsCount = 4;

        private Figures chosenFigure;
        private int chosenSize;
        private Brush fillStyle;
        private List<Color> colors;
        private int chosenColor;

        public MainPage()
        {
            this.InitializeComponent();
            this.chosenSize = MaximumSize / 2;
            this.chosenFigure = Figures.Line;
            this.InitializeColors();
            this.FillDemoCanvas();
        }
        
        private void InitializeColors()
        {
            this.colors = new List<Color>();
            this.chosenColor = 0;
            this.colors.Add(Colors.Aquamarine);
            this.colors.Add(Colors.PaleVioletRed);
            this.colors.Add(Colors.Plum);
            this.colors.Add(Colors.Black);

            this.fillStyle = new SolidColorBrush(this.colors[this.chosenColor]);
        }
                
        #region Demo canvas methods

        private void Figure_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var obj = sender as UIElement;
            var name = obj.GetType().Name;

            if (name == "Rectangle")
            {
                this.chosenFigure = Figures.Rectangle;
            }
            else if (name == "Ellipse")
            {
                this.chosenFigure = Figures.Elipse;
            }
            else if (name == "Line")
            {
                this.chosenFigure = Figures.Line;
            }

            this.FillDemoCanvas();
        }

        private void FillDemoCanvas()
        {
            if (this.DemoFigure != null)
            {


                switch (this.chosenFigure)
                {
                    case Figures.Elipse:
                        this.ShowElipse();
                        break;
                    case Figures.Rectangle:
                        this.ShowRectanle();
                        break;
                    case Figures.Line:
                        this.ShowLine();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ShowLine()
        {
            this.ClearDemoCanvas();
            var line = new Line();
            line.X1 = 10 ;//+ (MaximumSize - this.chosenSize) / 2;
            line.Y1 = 40 ;//+ (MaximumSize - this.chosenSize) / 2;
            line.X2 = 140;// - (MaximumSize - this.chosenSize) / 2;
            line.Y2 = 140;// -(MaximumSize - this.chosenSize) / 2;
            line.Fill = new SolidColorBrush(this.colors[this.chosenColor]);
            line.StrokeThickness = this.chosenSize / 20;
            this.DemoFigure.Children.Add(line);
        }

        private void ShowRectanle()
        {
            this.ClearDemoCanvas();

            var rect = new Rectangle();
            rect.Fill = new SolidColorBrush(this.colors[this.chosenColor]);
            rect.StrokeThickness = 1.5;
            rect.Width = this.chosenSize;
            rect.Height = this.chosenSize;
            this.DemoFigure.Children.Add(rect);
            Canvas.SetLeft(rect, 10 + (MaximumSize - this.chosenSize) / 2);
            Canvas.SetTop(rect, 25 + (MaximumSize - this.chosenSize) / 2);
        }

        private void ShowElipse()
        {
            this.ClearDemoCanvas();

            var elipse = new Ellipse();
            elipse.Fill = new SolidColorBrush(this.colors[this.chosenColor]);
            elipse.StrokeThickness = 1.5;
            elipse.Width = this.chosenSize;
            elipse.Height = this.chosenSize;
            this.DemoFigure.Children.Add(elipse);
            Canvas.SetLeft(elipse, 10 + (MaximumSize - this.chosenSize) / 2);
            Canvas.SetTop(elipse, 25 + (MaximumSize - this.chosenSize) / 2);
        }

        private void ClearDemoCanvas()
        {
            if (this.DemoFigure != null)
            {
                var text = this.DemoFigure.Children.First();
                this.DemoFigure.Children.Clear();
                this.DemoFigure.Children.Add(text);
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.chosenSize = (int)e.NewValue;
            this.FillDemoCanvas();
        }
        #endregion

        private void WhiteCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null || e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                e.Handled = false;
                return;
            }
            var positionInCanvas = e.GetCurrentPoint(canvas);
            var figure = this.CreateFigure();
            if (this.chosenFigure != Figures.Line )
            {
                figure.Fill = this.fillStyle;
                figure.Width = this.chosenSize;
                figure.Height = this.chosenSize;
                canvas.Children.Add(figure);
                Canvas.SetLeft(figure, positionInCanvas.Position.X - this.chosenSize / 2);
                Canvas.SetTop(figure, positionInCanvas.Position.Y - this.chosenSize / 2);
            }
        }

        private Shape CreateFigure()
        {
            switch (this.chosenFigure)
            {
                case Figures.Elipse: return new Ellipse();
                case Figures.Rectangle: return new Rectangle();
                case Figures.Line: return new Line();                   
                default:
                    break;
            }
            return null;
        }

        

        private void WhiteCanvas_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            this.chosenColor++;
            if (this.chosenColor >= ColorsCount)
            {
                this.chosenColor = 0;
            }
            this.fillStyle = new SolidColorBrush(this.colors[this.chosenColor]);
            this.FillDemoCanvas();
        }
    }
}
