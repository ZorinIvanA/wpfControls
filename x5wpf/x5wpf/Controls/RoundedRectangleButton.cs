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
    /// Кнопка в виде прямоугольника с закруглёнными краями
    /// Для использования необходимо задать высоту и ширину. 
    /// </summary>
    public class RoundedRectangleButton : Button
    {
        private Point TopEdgeCenter { get; set; }
        private Point LeftEdgeCenter { get; set; }
        private Point RightEdgeCEnter { get; set; }
        private Point BottomEdgeCenter { get; set; }

        /// <summary>
        /// Итоговый Path для отрисовки картинки
        /// </summary>
        public static DependencyProperty ButtonPathStringProperty = DependencyProperty.Register("ButtonPathString", typeof(String), typeof(RoundedRectangleButton),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback((d, e) => { StaticPropertyChanged(d, "ButtonPathString"); })));
        public String ButtonPathString
        {
            get { return GetValue(ButtonPathStringProperty) as String; }
            private set { SetValue(ButtonPathStringProperty, value); }
        }

        /// <summary>
        /// Размер дуги
        /// </summary>
        public static DependencyProperty ArcSizeProperty = DependencyProperty.Register("ArcSize", typeof(Double), typeof(RoundedRectangleButton),
            new PropertyMetadata(Double.NaN, new PropertyChangedCallback((d, e) =>
            {
                var sender = d as RoundedRectangleButton;
                if (sender != null)
                {
                    sender.RecalcButtonPaths(sender.Width, sender.Height, (Double)e.NewValue);
                }
                StaticPropertyChanged(d, "ArcSize");
            })));
        public Double ArcSize
        {
            get { return (Double)GetValue(ArcSizeProperty); }
            set { SetValue(ArcSizeProperty, value); }
        }

        static RoundedRectangleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedRectangleButton), new FrameworkPropertyMetadata(typeof(RoundedRectangleButton)));
        }

        private static void StaticPropertyChanged(DependencyObject d, String name)
        {

        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            TopEdgeCenter = new Point(sizeInfo.NewSize.Width / 2, 0);
            BottomEdgeCenter = new Point(sizeInfo.NewSize.Width / 2, sizeInfo.NewSize.Height);
            LeftEdgeCenter = new Point(0, sizeInfo.NewSize.Height / 2);
            RightEdgeCEnter = new Point(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height / 2);

            RecalcButtonPaths(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height, ArcSize);
        }

        private void RecalcButtonPaths(Double width, Double height, double arcSize)
        {
            //Рассчитываем точки концов отрезка, задаваемого ArcSize 
            #region Пояснения по математике расчётов кругляшной кнопки
            /*
             * Кнопка данного вида представляет собой хрень с закруглёнными углами, вписанная в прямоугольник (в абсолюте - квадрат).
             * Строится кубическая линия Безье, в которой в качестве крайних точек выступают центры сторон соответствующего прямоугольника (точки, где стороны становятся касательными для линий хрени),
             * а в качестве средних двух точек - концы отрезка длинной ArcSize, который задаётся из XAML.
             * Данный отрезок представляет собой гипотенузу прямоугольного треугольника, катеты которого образованы отрезками сторон прямоугольника, в который вписан контрол, от угла до пересечения с 
             * гипотенузой. При этом, исходим из того, что катеты тр-ка имеют то же соотношение длин, что и стороны основного пр-ка.
             * Решение системы уравнений для расчёта имеет вид:
             * x = m*y
             * y = Math.Sqrt(Math.Sqr(z)/(m+1)),
             * где: x и y - катеты прямоугольников. z - длина гипотенузы (значение св-ва ArcSize), m - соотношение между длинами катетов
             * 
             * Далее - рассчитываем непосредственно концы отрезка в зависимости от того угла, в котором она находится
             * Исходим из того, что линии рисуются по часовой стрелке
             * ЛВУ - {0,y}, {x,0}
             * ПВУ - {Width-x,0}, {Width, y}
             * ПНУ - {Width, Height-y}, {Width-x, Height}, 
             * ЛНУ - {x, Height}, {0, Height-y}
            */
            #endregion

            var m = width / height;
            var y = Math.Sqrt(Math.Pow(arcSize, 2) / (m + 1));
            var x = m * y;

            var leftTop = $"M {LeftEdgeCenter.X},{LeftEdgeCenter.Y} C 0,{y} {x},0 {TopEdgeCenter.X},{TopEdgeCenter.Y}";
            var rightTop = $"C {width - x},0 {width},{y} {RightEdgeCEnter.X},{RightEdgeCEnter.Y}";
            var rightBottom = $"C {width},{height - y} {width - x},{height} {BottomEdgeCenter.X},{BottomEdgeCenter.Y}";
            var leftBottom = $"C {x},{height} 0,{height - y} {LeftEdgeCenter.X},{LeftEdgeCenter.Y}";
            
            ButtonPathString = $"{leftTop} {rightTop} {rightBottom} {leftBottom}";
        }
    }

}
