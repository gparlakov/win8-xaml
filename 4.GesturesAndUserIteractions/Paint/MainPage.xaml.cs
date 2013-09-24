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
        private List<Color> colors;
        private int chosenColor;
        private RotateTransform colorsRotation;

        public MainPage()
        {
            this.InitializeComponent();
            this.ChosenSize = MaximumSize / 2;
            this.chosenFigure = Figures.Rectangle;
            this.InitializeColors();
            this.FillDemoCanvas();
            this.colorsRotation = new RotateTransform();
            this.RotatingColors.RenderTransform = this.colorsRotation;
        }
        
        public Brush FillStyle
        {
            get
            {
                return new SolidColorBrush(this.colors[this.chosenColor]);
            }
        }

        public int ChosenSize
        {
            get
            {
                return this.chosenSize;
            }
            set
            {
                if (value > MaximumSize)
                {
                    value = MaximumSize;
                }
                if (value < MinimumSize)
                {
                    value = MinimumSize;
                }
                this.chosenSize = value;                
            }
        }

        private void InitializeColors()
        {
            this.colors = new List<Color>();
            this.chosenColor = 0;
            this.colors.Add(Colors.Red);
            this.colors.Add(Colors.Black);
            this.colors.Add(Colors.Blue);
            this.colors.Add(Colors.Green);
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
            line.Fill = this.FillStyle;
            line.Stroke = this.FillStyle;
            line.StrokeThickness = this.ChosenSize / 20;
            this.DemoFigure.Children.Add(line);
        }

        private void ShowRectanle()
        {
            this.ClearDemoCanvas();

            var rect = new Rectangle();
            rect.Fill = this.FillStyle;
            rect.StrokeThickness = 1.5;
            rect.Width = this.ChosenSize;
            rect.Height = this.ChosenSize;
            this.DemoFigure.Children.Add(rect);
            Canvas.SetLeft(rect, 10 + (MaximumSize - this.ChosenSize) / 2);
            Canvas.SetTop(rect, 25 + (MaximumSize - this.ChosenSize) / 2);
        }

        private void ShowElipse()
        {
            this.ClearDemoCanvas();

            var elipse = new Ellipse();
            elipse.Fill = this.FillStyle;
            elipse.StrokeThickness = 1.5;
            elipse.Width = this.ChosenSize;
            elipse.Height = this.ChosenSize;
            this.DemoFigure.Children.Add(elipse);
            Canvas.SetLeft(elipse, 10 + (MaximumSize - this.ChosenSize) / 2);
            Canvas.SetTop(elipse, 25 + (MaximumSize - this.ChosenSize) / 2);
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
            this.ChosenSize = (int)e.NewValue;
            this.FillDemoCanvas();
        }
        
        private void Canvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            this.colorsRotation.CenterX = canvas.Width / 2;
            this.colorsRotation.CenterY = canvas.Height / 2;
            this.colorsRotation.Angle += e.Delta.Rotation;
        }

        private void RotatingColors_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var angle = this.colorsRotation.Angle % 360;
            if (angle >= 315 || angle < 45)
            {
                this.chosenColor = 0;
            }
            else if (angle >= 45 && angle < 135)
            {
                this.chosenColor = 1;
            }
            else if (angle >= 135 && angle < 225)
            {
                this.chosenColor = 2;
            }
            else
            {
                this.chosenColor = 3;
            }

            this.FillDemoCanvas();
        }

        private void DemoFigure_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var expansion = e.Delta.Expansion;
            var scale = e.Delta.Scale;
            this.ChosenSize += (int)e.Delta.Expansion;
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
                figure.Fill = this.FillStyle;
                figure.Width = this.ChosenSize;
                figure.Height = this.ChosenSize;
                canvas.Children.Add(figure);
                Canvas.SetLeft(figure, positionInCanvas.Position.X - this.ChosenSize / 2);
                Canvas.SetTop(figure, positionInCanvas.Position.Y - this.ChosenSize / 2);
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
       
    }
}
