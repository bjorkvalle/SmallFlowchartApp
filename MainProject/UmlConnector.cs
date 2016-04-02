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

        private void InitializePolyline()
        {
            Polyline p = shape as Polyline;

            if(p != null)
            {
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

                //-->DEBUG - REMOVE LATER
                if (line.Points.Count > 3)
                {
                    MessageBox.Show("WARNING");
                    return;
                }//<--

                if (firstPoint)
                {
                    if (pos != line.Points[0])
                    {
                        line.Points[0] = pos;
                    }
                }
                else
                {
                    if (pos != line.Points[2])
                    {
                        line.Points[2] = pos;
                    }
                }

                SetBreakPoint(line);
            }
        }

        private void SetBreakPoint(Polyline line)
        {
            line.Points[1] = new Point(line.Points[0].X, line.Points[2].Y);
        }

        public override void SetColor(Brush newColor)
        {
            shape.Stroke = newColor;
        }
    }
}
