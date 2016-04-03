using System.Windows.Controls;
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
            _poly = new Polygon() { Fill = System.Windows.Media.Brushes.LightGray };
            _poly.Points.Add(new System.Windows.Point(0, 0));
            _poly.Points.Add(new System.Windows.Point((_size / 2), _size));
            _poly.Points.Add(new System.Windows.Point(_size, 0));

            Children.Add(_poly);
        }
    }
}
