using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace x5wpf.Controls
{
    /// <summary>
    /// </summary>
    public class ButtonEx : Button, INotifyPropertyChanged
    {
        #region Описание "носика" кнопки
        /// <summary>
        /// Ширина прямоугольника кнопки
        /// </summary>
        public static readonly DependencyProperty RectangleWidthProperty = DependencyProperty.Register("RectangleWidth", typeof(Double), typeof(ButtonEx),
            new PropertyMetadata(Double.NaN, new PropertyChangedCallback((d, e) =>
            {
                var x = d as ButtonEx;
                if (x != null)
                {
                    x.RectangleWidth = (Double)e.NewValue;
                }
                StaticPropertyChanged(d, "RectangleWidth");
            })));
        public Double RectangleWidth
        {
            get { return (Double)GetValue(RectangleWidthProperty); }
            private set { SetValue(RectangleWidthProperty, value); }
        }

        /// <summary>
        /// Ширина носика кнопки
        /// </summary>
        public static readonly DependencyProperty NoseWidthProperty = DependencyProperty.Register("NoseWidth", typeof(Double), typeof(ButtonEx),
            new PropertyMetadata(Double.NaN, new PropertyChangedCallback((d, e) =>
            {
                var x = d as ButtonEx;
                if (x != null)
                {
                    x.NoseWidth = (Double)e.NewValue;
                    x.RectangleWidth = x.Width - x.NoseWidth;

                }
                StaticPropertyChanged(d, "NoseWidth");
            })));
        public Double NoseWidth
        {
            get { return (Double)GetValue(NoseWidthProperty); }
            set { SetValue(NoseWidthProperty, value); }
        }

        public Point StartPoint { get; private set; }

        public Point EndPoint { get; private set; }

        public Point Point1 { get; private set; }

        public Point Point2 { get; private set; }

        /// <summary>
        /// Направление кнопки - влево или вправо
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(ButtonDirection), typeof(ButtonEx),
            new PropertyMetadata(ButtonDirection.Left, new PropertyChangedCallback((d, e) =>
            {
                var x = d as ButtonEx;
                if (x != null)
                {
                    x.Direction = (ButtonDirection)e.NewValue;
                }
            })));
        public ButtonDirection Direction
        {
            get { return (ButtonDirection)GetValue(DirectionProperty); }
            set
            {
                SetValue(DirectionProperty, value);
                switch (value)
                {
                    case ButtonDirection.Right:
                        Corners = new CornerRadius(5, 0, 0, 5);
                        break;
                    case ButtonDirection.Left:
                        Corners = new CornerRadius(0, 5, 5, 0);
                        break;
                }
                RenderButton(ArcSize, Width, value);
            }
        }

        /// <summary>
        /// Размер дуги. 
        /// </summary>
        public static readonly DependencyProperty ArcSizeProperty = DependencyProperty.Register("ArcSize", typeof(Double), typeof(ButtonEx),
                        new PropertyMetadata(Double.NaN, new PropertyChangedCallback((d, e) =>
                        {
                            var x = d as ButtonEx;
                            if (x != null)
                            {
                                x.ArcSize = (Double)e.NewValue;
                            }

                            StaticPropertyChanged(d, "ArcSize");
                            StaticPropertyChanged(d, "Point1");
                            StaticPropertyChanged(d, "Point2");
                        })));
        public Double ArcSize
        {
            get { return (Double)GetValue(ArcSizeProperty); }
            set
            {
                RenderButton(value, Width, Direction);
                SetValue(ArcSizeProperty, value);
            }
        }
        #endregion

        #region Описание прямоугольника
        /// <summary>
        /// Уголки
        /// </summary>
        public static readonly DependencyProperty CornersProperty = DependencyProperty.Register("Corners", typeof(CornerRadius), typeof(ButtonEx),
            new PropertyMetadata(new CornerRadius(0), new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "Corners"); })));
        public CornerRadius Corners
        {
            get { return (CornerRadius)GetValue(CornersProperty); }
            private set { SetValue(CornersProperty, value); }
        }
        #endregion

        /// <summary>
        /// Рисунок носика
        /// </summary>
        public static readonly DependencyProperty PathStringProperty = DependencyProperty.Register("PathString", typeof(String), typeof(ButtonEx),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "PathString"); })));
        public String PathString
        {
            get { return (String)GetValue(PathStringProperty); }
            set { SetValue(PathStringProperty, value); }
        }

        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.HeightChanged)
            {
                ArcSize = (sizeInfo.PreviousSize.Height != 0) ? ArcSize * sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height : ArcSize * sizeInfo.NewSize.Height / 100;
                EndPoint = new Point(EndPoint.X, sizeInfo.NewSize.Height);

                NotifyPropertyChanged("StartPoint");
                NotifyPropertyChanged("EndPoint");
            }

            if (sizeInfo.WidthChanged)
            {
                RectangleWidth = sizeInfo.NewSize.Width - NoseWidth;

                NotifyPropertyChanged("RectangleWidth");
            }

            base.OnRenderSizeChanged(sizeInfo);

            RenderButton(ArcSize, Width, Direction);
        }

        /// <summary>
        /// Отрисовка носика кнопки
        /// </summary>
        /// <param name="arcSize">Размер дуги. Отрезок вертикальной прямой, на концах которого должны находиться 2я и 3я точки кубической дуги Безье</param>
        /// <param name="width">Ширина кнопки</param>
        /// <param name="direction">Направление носика кнопки</param>
        private void RenderButton(Double arcSize, Double width, ButtonDirection direction)
        {
            if (Direction == ButtonDirection.Left)
            {
                StartPoint = new Point(NoseWidth, 0);
                EndPoint = new Point(NoseWidth, Height);
                Point1 = new Point(-NoseWidth * 0.1, (Height - arcSize) / 2);
                Point2 = new Point(-NoseWidth * 0.1, arcSize + (Height - arcSize) / 2);
                Corners = new CornerRadius(0, 5, 5, 0);
            }
            else
            {
                StartPoint = new Point(0, 0);
                EndPoint = new Point(0, Height);
                Point1 = new Point(NoseWidth * 1.1, (Height - arcSize) / 2);
                Point2 = new Point(NoseWidth * 1.1, arcSize + (Height - arcSize) / 2);
                Corners = new CornerRadius(5, 0, 0, 5);
            }
            PathString = $"M {StartPoint.X},{StartPoint.Y} C {Point1.X},{Point1.Y} {Point2.X},{Point2.Y} {EndPoint.X},{EndPoint.Y}";

            NotifyPropertyChanged("StartPoint");
            NotifyPropertyChanged("EndPoint");
            NotifyPropertyChanged("Point1");
            NotifyPropertyChanged("Point2");
            NotifyPropertyChanged("PathString");
            NotifyPropertyChanged("Corners");
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private static void StaticPropertyChanged(Object sender, String name)
        {
            if (sender is ButtonEx s)
            {
                s.NotifyPropertyChanged(name);
            }
        }

        private void NotifyPropertyChanged(String name)
        {
            var pc = PropertyChanged;
            pc?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

}

