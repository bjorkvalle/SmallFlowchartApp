using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    class UmlConnector : UmlElement
    {
        private ConnectorArrows _targetArrow;
        private UmlShape
            _originConnection,
            _targetConnection;
        private Point
            _originPos,
            _targetPos;

        public UmlConnector(Canvas cvs) : base(cvs, ShapeType.Line)
        {
            //set up container
            _targetArrow = new ConnectorArrows();
            shape = new Polyline();
            shape.StrokeThickness = strokeThicknessLeave;
            InitializePolyline();
            SetColor(shapeBrush);
            SetDepth(0);
            cvs.Children.Add(_targetArrow);
            this.Children.Add(shape);
        }

        //SETUP

        private void InitializePolyline()
        {
            Polyline p = shape as Polyline;

            if (p != null)
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

        private Point SetAnchors(bool firstPoint)
        {
            _originPos = new Point(Canvas.GetLeft(_originConnection) + (_originConnection.Width / 2),
                                        Canvas.GetTop(_originConnection) + (_originConnection.Height / 2));
            _targetPos = new Point(Canvas.GetLeft(_targetConnection) + (_targetConnection.Width / 2),
                                        Canvas.GetTop(_targetConnection) + (_targetConnection.Height / 2));
            Point anchorPos = new Point();

            anchorPos = firstPoint ? SetFirstAnchor(_originPos, _targetPos) :
                                        SetSecondAnchor(_originPos, _targetPos);

            return anchorPos;
        }

        private Point SetFirstAnchor(Point originPos, Point targetPos)
        {
            Point anchorPos = new Point();

            if (100 > Math.Abs(originPos.Y - targetPos.Y))
            {
                anchorPos.X = originPos.X + (originPos.X > targetPos.X ?
                                -(_originConnection.Width / 2) : _originConnection.Width / 2);
                anchorPos.Y = originPos.Y;
            }
            else
            {
                anchorPos.X = originPos.X;
                anchorPos.Y = originPos.Y + (originPos.Y > targetPos.Y ?
                                -(_originConnection.Height / 2) : _originConnection.Height / 2);
            }
            return anchorPos;
        }

        private Point SetSecondAnchor(Point originPos, Point targetPos)
        {
            Point anchorPos = new Point();

            if (100 > Math.Abs(originPos.X - targetPos.X))
            {
                anchorPos.X = targetPos.X;
                anchorPos.Y = targetPos.Y + (targetPos.Y > originPos.Y ?
                                -(_targetConnection.Height / 2) : _targetConnection.Height / 2);
            }
            else
            {
                anchorPos.X = targetPos.X + (targetPos.X > originPos.X ?
                                -(_targetConnection.Width / 2) : _targetConnection.Width / 2);
                anchorPos.Y = targetPos.Y;
            }
            return anchorPos;
        }

        private void SetPoints(Polyline line, UmlShape posObj, bool firstPoint)
        {
            if (posObj != null)
            {
                Point pos = SetAnchors(firstPoint);

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
                        SetArrowPosition(line);
                    }
                }
                SetBreakPoint(line);
                SetArrowRotation(line.Points[line.Points.Count - 1]);
            }
        }

        private void SetArrowRotation(Point connectPos)
        {
            double rot = 0;

            if (connectPos.X == _targetPos.X)
            {
                if (connectPos.Y > _targetPos.Y)
                    rot = 180;
                else
                    rot = 0;
            }
            else
            {
                if (connectPos.Y == _targetPos.Y)
                {
                    if (connectPos.X > _targetPos.X)
                        rot = 90;
                    else
                        rot = 270;
                }
            }
            _targetArrow.LayoutTransform = new RotateTransform(rot);
        }

        private void SetArrowPosition(Polyline line)
        {
            Canvas.SetLeft(_targetArrow, line.Points[line.Points.Count - 1].X - (_targetArrow.Width / 2));
            Canvas.SetTop(_targetArrow, line.Points[line.Points.Count - 1].Y - (_targetArrow.Height / 2));
        }

        private void SetBreakPoint(Polyline line)
        {
            double minDistance = 100;

            //add break if to close horizontally
            if (minDistance > Math.Abs(_originPos.X - _targetPos.X))
            {
                double ypos = (line.Points[0].Y / 2) + (line.Points[line.Points.Count - 1].Y / 2);

                line.Points[1] = new Point(line.Points[0].X, ypos);
                line.Points[2] = new Point(line.Points[line.Points.Count - 1].X, ypos);
            }//add break if to close vertically
            else if (minDistance > Math.Abs(_originPos.Y - _targetPos.Y))
            {
                double xpos = (line.Points[0].X / 2) + (line.Points[line.Points.Count - 1].X / 2);

                line.Points[1] = new Point(xpos, line.Points[0].Y);
                line.Points[2] = new Point(xpos, line.Points[line.Points.Count - 1].Y);
            }
            else //"hide" breakers
            {
                line.Points[1] = new Point(line.Points[0].X, line.Points[line.Points.Count - 1].Y);
                line.Points[2] = new Point(line.Points[0].X, line.Points[line.Points.Count - 1].Y);
            }
        }

        public override void SetColor(Brush newColor)
        {
            shape.Stroke = newColor;
        }

        public override void OnRemove()
        {
            umlCanvas.Children.Remove(_targetArrow);
            this.Children.Clear();
        }
    }
}
