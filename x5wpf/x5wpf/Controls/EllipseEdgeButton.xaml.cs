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

namespace x5wpf.Controls
{
    /// <summary>
    /// Interaction logic for EllipseEdgeButton.xaml
    /// </summary>
    public partial class EllipseEdgeButton : UserControl
    {
        #region Описание "носика" кнопки
        public DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(EllipseEdgeButton));
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            private set { SetValue(StartPointProperty, value); }
        }

        public DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(EllipseEdgeButton));
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            private set { SetValue(EndPointProperty, value); }
        }

        public DependencyProperty Point1Property = DependencyProperty.Register("Point1", typeof(Point), typeof(EllipseEdgeButton));

        public Point Point1
        {
            get { return (Point)GetValue(Point1Property); }
            private set { SetValue(Point1Property, value); }
        }
        public DependencyProperty Point2Property = DependencyProperty.Register("Point2", typeof(Point), typeof(EllipseEdgeButton));

        public Point Point2
        {
            get { return (Point)GetValue(Point2Property); }
            private set { SetValue(Point2Property, value); }
        }

        public DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(ButtonDirection), typeof(EllipseEdgeButton));
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


        public DependencyProperty ArcSizeProperty = DependencyProperty.Register("ArcSize", typeof(Double), typeof(EllipseEdgeButton));
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
        public DependencyProperty NosePositionProperty = DependencyProperty.Register("NosePosition", typeof(Double), typeof(EllipseEdgeButton));
        public Double NosePosition
        {
            get { return (Double)GetValue(NosePositionProperty); }
            set
            {
                //StartPoint = new Point(value, StartPoint.Y);
                //EndPoint = new Point(value, EndPoint.Y);
                RectangleWidth = value;
                NoseWidth = Width - value;
                Point1 = new Point(NoseWidth * 1.2, Point1.Y);
                Point2 = new Point(NoseWidth * 1.2, Point2.Y);

                SetValue(NosePositionProperty, value);
            }
        }

        /// <summary>
        /// Ширина носика кнопки
        /// </summary>
        public DependencyProperty NoseWidthProperty = DependencyProperty.Register("NoseWidth", typeof(Double), typeof(EllipseEdgeButton));
        public Double NoseWidth
        {
            get { return (Double)GetValue(NoseWidthProperty); }
            private set { SetValue(NoseWidthProperty, value); }
        }
        #endregion

        #region Описание прямоугольника
        /// <summary>
        /// Ширина прямоугольника кнопки
        /// </summary>
        public DependencyProperty RectangleWidthProperty = DependencyProperty.Register("RectangleWidth", typeof(Double), typeof(EllipseEdgeButton));
        public Double RectangleWidth
        {
            get { return (Double)GetValue(RectangleWidthProperty); }
            private set { SetValue(RectangleWidthProperty, value); }
        }

        /// <summary>
        /// Ширина прямоугольника кнопки
        /// </summary>
        public DependencyProperty CornersProperty = DependencyProperty.Register("Corners", typeof(String), typeof(EllipseEdgeButton));
        public String Corners
        {
            get { return (String)GetValue(CornersProperty); }
            private set { SetValue(CornersProperty, value); }
        }
        #endregion


        public EllipseEdgeButton()
        {
            InitializeComponent();
            Direction = ButtonDirection.Right;
            StartPoint = new Point(0, 0);
            EndPoint = new Point(0, Height);
            NosePosition = Width * 0.8;
        }

        

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.HeightChanged)
            {
                var prevousArcSize = ArcSize;

                var newArcSize = (sizeInfo.PreviousSize.Height != 0) ? ArcSize * sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height : ArcSize * sizeInfo.NewSize.Height / 100;
                ArcSize = newArcSize;
                EndPoint = new Point(EndPoint.X, sizeInfo.NewSize.Height);
            }

            if (sizeInfo.WidthChanged)
            {
                Point1 = new Point(NoseWidth * 1.1, Point1.Y);
                Point2 = new Point(NoseWidth * 1.1, Point2.Y);                
            }

            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
