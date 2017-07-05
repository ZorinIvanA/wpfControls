using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:x5wpf.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:x5wpf.Controls;assembly=x5wpf.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ButtonEx/>
    ///
    /// </summary>
    public class ButtonEx : Button, INotifyPropertyChanged
    {
        #region Описание "носика" кнопки
        /// <summary>
        /// Ширина прямоугольника кнопки
        /// </summary>
        public static readonly DependencyProperty RectangleWidthProperty = DependencyProperty.Register("RectangleWidth", typeof(GridLength), typeof(ButtonEx),
            new PropertyMetadata(GridLength.Auto, new PropertyChangedCallback((d, e) =>
            {
                var x = d as ButtonEx;
                if (x != null)
                {
                    x.RectangleWidth = (GridLength)e.NewValue;
                }
                StaticPropertyChanged(d, "RectangleWidth");
            })));
        public GridLength RectangleWidth
        {
            get { return (GridLength)GetValue(RectangleWidthProperty); }
            private set { SetValue(RectangleWidthProperty, value); }
        }

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(ButtonEx),
            new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "StartPoint"); })));
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            private set { SetValue(StartPointProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(ButtonEx),
            new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "EndPoint"); })));
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            private set { SetValue(EndPointProperty, value); }
        }

        public static readonly DependencyProperty Point1Property = DependencyProperty.Register("Point1", typeof(Point), typeof(ButtonEx),
            new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "Point1"); })));
        public Point Point1
        {
            get { return (Point)GetValue(Point1Property); }
            private set { SetValue(Point1Property, value); }
        }

        public static readonly DependencyProperty Point2Property = DependencyProperty.Register("Point2", typeof(Point), typeof(ButtonEx),
            new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "Point2"); })));
        public Point Point2
        {
            get { return (Point)GetValue(Point2Property); }
            private set { SetValue(Point2Property, value); }
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(ButtonDirection), typeof(ButtonEx),
            new PropertyMetadata(ButtonDirection.Left, new PropertyChangedCallback((d, e) =>
            {
                StaticPropertyChanged(d, "Direction");
                StaticPropertyChanged(d, "Corners");
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
                        Corners = "5,0,0,5";
                        break;
                    case ButtonDirection.Left:
                        Corners = "0,5,5,0";
                        break;
                }
            }
        }


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
                Point1 = new Point(Point1.X, (Height - ArcSize) / 2);
                Point2 = new Point(Point2.X, (Height - ArcSize) / 2 + ArcSize);
                SetValue(ArcSizeProperty, value);
            }
        }

        /// <summary>
        /// Позиция "носика" кнопки по горизонтали
        /// </summary>
        public static readonly DependencyProperty NosePositionProperty = DependencyProperty.Register("NosePosition", typeof(Int32), typeof(ButtonEx),
                                    new PropertyMetadata(Int32.MinValue, new PropertyChangedCallback((d, e) =>
                                    {
                                        var x = d as ButtonEx;
                                        if (x != null)
                                        {
                                            x.NosePosition = (Int32)e.NewValue;
                                        }

                                        //StaticPropertyChanged(d, "NosePosition");
                                        //StaticPropertyChanged(d, "RectangleWidth");
                                        //StaticPropertyChanged(d, "NoseWidth");
                                        //StaticPropertyChanged(d, "Point1");
                                        //StaticPropertyChanged(d, "Point2");
                                    })));
        public Int32 NosePosition
        {
            get { return (Int32)GetValue(NosePositionProperty); }
            set
            {
                RectangleWidth = new GridLength(value, GridUnitType.Star);
                NoseWidth = new GridLength(Width - value, GridUnitType.Star);
                Point1 = new Point(Width - value * 1.2, Point1.Y);
                Point2 = new Point(Width - value * 1.2, Point2.Y);
                SetValue(NosePositionProperty, value);

                NotifyPropertyChanged("NosePosition");
                NotifyPropertyChanged("RectangleWidth");
                NotifyPropertyChanged("NoseWidth");
                NotifyPropertyChanged("Point1");
                NotifyPropertyChanged("Point2");
            }
        }

        /// <summary>
        /// Ширина носика кнопки
        /// </summary>
        public static readonly DependencyProperty NoseWidthProperty = DependencyProperty.Register("NoseWidth", typeof(GridLength), typeof(ButtonEx),
            new PropertyMetadata(GridLength.Auto, new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "NoseWidth"); })));
        public GridLength NoseWidth
        {
            get { return (GridLength)GetValue(NoseWidthProperty); }
            private set { SetValue(NoseWidthProperty, value); }
        }
        #endregion

        #region Описание прямоугольника


        /// <summary>
        /// Ширина прямоугольника кнопки
        /// </summary>
        public static readonly DependencyProperty CornersProperty = DependencyProperty.Register("Corners", typeof(String), typeof(ButtonEx),
            new PropertyMetadata("5,0,0,5", new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "Corners"); })));
        public String Corners
        {
            get { return (String)GetValue(CornersProperty); }
            private set { SetValue(CornersProperty, value); }
        }


        #endregion

        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }

        public ButtonEx()
        {

        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.HeightChanged)
            {
                var newArcSize = (sizeInfo.PreviousSize.Height != 0) ? ArcSize * sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height : ArcSize * sizeInfo.NewSize.Height / 100;
                ArcSize = newArcSize;
                EndPoint = new Point(EndPoint.X, sizeInfo.NewSize.Height);
            }

            if (sizeInfo.WidthChanged)
            {
                Point1 = new Point((Width-NosePosition) * 1.1, Point1.Y);
                Point2 = new Point((Width - NosePosition) * 1.1, Point2.Y);
            }

            base.OnRenderSizeChanged(sizeInfo);
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

