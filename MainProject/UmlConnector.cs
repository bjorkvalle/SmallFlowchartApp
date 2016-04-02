using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
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
            //set up connection
            //OriginConnection = originConnection;
            //SetPosition();

            //set up container
            shape = new Line();
            shape.StrokeThickness = strokeThicknessEnter;
            SetColor(shapeBrush);
            SetDepth(0);
            this.Children.Add(shape);
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
            Line line = shape as Line;

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

        private void SetPoints(Line line, UmlShape posObj, bool firstPoint)
        {
            if (posObj != null)
            {
                Point pos = new Point(Canvas.GetLeft(posObj) + (posObj.Width / 2),
                                            Canvas.GetTop(posObj) + (posObj.Height / 2));
                
                if (firstPoint)
                {
                    if (pos != new Point(line.X1, line.Y1))
                    {
                        line.X1 = pos.X;
                        line.Y1 = pos.Y;
                    }
                }
                else
                {
                    if (pos != new Point(line.X2, line.Y2))
                    {
                        line.X2 = pos.X;
                        line.Y2 = pos.Y;
                    }
                }
            }
        }

        public override void SetColor(Brush newColor)
        {
            shape.Stroke = newColor;
        }
    }
}
