using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    public class ConnectorArrows : Grid
    {
        private Polygon _poly;
        private double _size;

        public ConnectorArrows()
        {
            //Polygons do not affect grid size
            _size = 30;
            Width = _size;
            Height = _size;

            //arrow pointing down as default
            _poly = new Polygon() { Fill = Brushes.Blue };
            _poly.Points.Add(new Point(0, 0));
            _poly.Points.Add(new Point((_size / 2), _size));
            _poly.Points.Add(new Point(_size, 0));

            Children.Add(_poly);
        }
    }
}
