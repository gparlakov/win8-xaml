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
using Newtonsoft.Json;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.Storage.Provider;

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

        void figure_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            FigureChangeColro(sender);
        }

        private void FigureChangeColro(object sender)
        {
            var figure = sender as Shape;
            if (figure != null)
            {
                var brush = figure.Fill as SolidColorBrush;
                var color = brush.Color;
                Color nextColor = GetNextColor(color);
                figure.Fill = new SolidColorBrush(nextColor);
            }
        }

        void figure_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FigureChangeColro(sender);
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
                this.chosenFigure = Figures.Ellipse;
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
                    case Figures.Ellipse:
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
                figure.DoubleTapped += figure_DoubleTapped;
                figure.RightTapped += figure_RightTapped;
                canvas.Children.Add(figure);
                Canvas.SetLeft(figure, positionInCanvas.Position.X - this.ChosenSize / 2);
                Canvas.SetTop(figure, positionInCanvas.Position.Y - this.ChosenSize / 2);
            }
        }

        private Shape CreateFigure()
        {
            switch (this.chosenFigure)
            {
                case Figures.Ellipse: return new Ellipse();
                case Figures.Rectangle: return new Rectangle();
                case Figures.Line: return new Line();                   
                default:
                    break;
            }
            return null;
        }

        private Shape CreateFigure(Figures figure)
        {
            switch (figure)
            {
                case Figures.Ellipse: return new Ellipse();
                case Figures.Rectangle: return new Rectangle();
                case Figures.Line: return new Line();
                default:
                    break;
            }
            return null;
        }

        #region Save methods
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("jsongpng file", new List<string>() { ".jsonpng" });
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file,GetFigureInfo());
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    new MessageDialog("File couldn be saved").ShowAsync();
                }   
            }
        }
  
        private string GetFigureInfo()
        {
            var figures = this.WhiteCanvas.Children.Select(c => new FigureInfo
            {
                Width = ((Shape)c).Width,
                CanvasLeft = Canvas.GetLeft(c),
                CanvasTop = Canvas.GetTop(c),
                CanvasZ = Canvas.GetZIndex(c),
                FigureName = ((Shape)c).GetType().Name,
                FigureBrush = ((SolidColorBrush)((Shape)c).Fill).Color
            });

            return JsonConvert.SerializeObject(figures);
        }

        #endregion

        #region Load methods
        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmed = await GetConfirmationForShapesDelete(this.WhiteCanvas.Children.Count);
            if (confirmed)
            {
                var openFilePicker = new FileOpenPicker();
                openFilePicker.FileTypeFilter.Add(".jsonpng");
                openFilePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                var file = await openFilePicker.PickSingleFileAsync();
                if (file != null)
                {
                    var stream = await file.OpenReadAsync();
                    using (var inputStream = stream.GetInputStreamAt(0))
                    {
                        using (var reader = new DataReader(inputStream))
                        {
                            var numbBytesLoaded = await reader.LoadAsync((uint)stream.Size);
                            var serialzed = reader.ReadString(numbBytesLoaded);

                            this.RestoreObjects(JsonConvert.DeserializeObject<List<FigureInfo>>(serialzed));
                            reader.DetachStream();
                        }
                    }
                }
            }
        }

        private async Task<bool> GetConfirmationForShapesDelete(int childrenCount)
        {
            var confirmed = true;

            if (childrenCount > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Load will clear all your work");
                dialog.Options = Windows.UI.Popups.MessageDialogOptions.AcceptUserInputAfterDelay;
                dialog.Commands.Add(new UICommand("Load"));
                dialog.Commands.Add(new UICommand("Cancel"));
                var answer = await dialog.ShowAsync();
                if (answer.Label != "Load")
                {
                    confirmed = false;
                }
            }

            return confirmed;
        }

        private void RestoreObjects(List<FigureInfo> list)
        {
            if (list != null)
            {
                this.WhiteCanvas.Children.Clear();
                foreach (var fig in list)
                {
                    var figure = this.CreateFigure(FiguresConvert(fig.FigureName));
                    figure.Width = fig.Width;
                    figure.Height = fig.Width;
                    figure.Fill = new SolidColorBrush(fig.FigureBrush);
                    figure.DoubleTapped += figure_DoubleTapped;
                    figure.RightTapped += figure_RightTapped;
                    this.WhiteCanvas.Children.Add(figure);
                    Canvas.SetLeft(figure, fig.CanvasLeft);
                    Canvas.SetTop(figure, fig.CanvasTop);
                    Canvas.SetZIndex(figure, fig.CanvasZ);
                }
            }
        } 
        #endregion

        private Figures FiguresConvert(string name)
        {
            if (name == "Rectangle")
            {
                return Figures.Rectangle;
            }
            else if (name == "Ellipse")
            {
                return Figures.Ellipse;
            }
            return Figures.Line;
        }
        
        private Color GetNextColor(Color color)
        {
            var nextColor = Colors.Black;
            if (color == Colors.Black)
            {
                nextColor = Colors.Blue;
            }
            else if (color == Colors.Blue)
            {
                nextColor = Colors.Green;
            }
            else if (color == Colors.Green)
            {
                nextColor = Colors.Red;
            }
            //else
            //{
            //    nextColor = Colors.RosyBrown;
            //}

            return nextColor;
        }
    }
}
