using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    class UmlConnector : UmlElement
    {
        private UmlShape _originConnection;
        private UmlShape _targetConnection;

        public UmlConnector(Canvas cvs) : base(cvs, ShapeType.Line)
        {
            //set up container
            shape = new Polyline();
            InitializePolyline();
            shape.StrokeThickness = strokeThicknessLeave;
            SetColor(shapeBrush);
            SetDepth(0);
            this.Children.Add(shape);
        }

        //SETUP

        private void InitializePolyline()
        {
            Polyline p = shape as Polyline;

            if(p != null)
            {
                p.Points.Add(new Point(0, 0));
                p.Points.Add(new Point(0, 0));
                p.Points.Add(new Point(0, 0));
                p.Points.Add(new Point(0, 0));
            }
        }

        //STATES

        public override void SetState(UmlElementState state, int currentZIndex)
        {
            this.state = state == UmlElementState.NotSelected ? 
                                    UmlElementState.NotSelected : UmlElementState.Selected;

            SetStroke(state == UmlElementState.Selected ? selectStroke : shapeBrush);
        }

        //ACTIONS

        public void SetPosition()
        {
            Polyline line = shape as Polyline;

            if (line != null)
            {
                SetPoints(line, _originConnection, true);
                SetPoints(line, _targetConnection, false);
            }
        }
        
        public void SetPosition(UmlShape originObj, UmlShape targetObj)
        {
            if (_originConnection == null)
                _originConnection = originObj;

            if (_targetConnection == null)
                _targetConnection = targetObj;

            SetPosition();
        }

        private void SetPoints(Polyline line, UmlShape posObj, bool firstPoint)
        {
            if (posObj != null)
            {
                Point pos = new Point(Canvas.GetLeft(posObj) + (posObj.Width / 2),
                                            Canvas.GetTop(posObj) + (posObj.Height / 2));

                if (firstPoint)
                {
                    if (pos != line.Points[0])
                    {
                        line.Points[0] = pos;
                    }
                }
                else
                {
                    if (pos != line.Points[line.Points.Count - 1])
                    {
                        line.Points[line.Points.Count - 1] = pos;
                    }
                }
                SetBreakPoint(line);
            }
        }

        private void SetBreakPoint(Polyline line)
        {
            double ypos = (line.Points[0].Y) / 2 + (line.Points[line.Points.Count - 1].Y) / 2;
            double xpos = (line.Points[0].X) / 2 + (line.Points[line.Points.Count - 1].X) / 2;
            double minDistance = 100;

            _originConnection._tBox.Text = line.Points[0].Y.ToString();
            _targetConnection._tBox.Text = line.Points[line.Points.Count - 1].Y.ToString();

            if (minDistance > Math.Abs(line.Points[0].Y - line.Points[line.Points.Count - 1].Y))
            {
                line.Points[1] = new Point(xpos, line.Points[0].Y);
                line.Points[2] = new Point(xpos, line.Points[line.Points.Count - 1].Y);
            }
            else
            {
                line.Points[1] = new Point(line.Points[0].X, ypos);
                line.Points[2] = new Point(line.Points[line.Points.Count - 1].X, ypos);
            }
        }

        public override void SetColor(Brush newColor)
        {
            shape.Stroke = newColor;
        }
    }
}
