using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    class UmlConnector : UmlElement
    {
        public UmlConnector(Canvas cvs) : base(cvs, ShapeType.Line)
        {
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

        //EFFECTS

        public override void SetColor(Brush newColor)
        {
            shape.Stroke = newColor;
        }
    }
}
