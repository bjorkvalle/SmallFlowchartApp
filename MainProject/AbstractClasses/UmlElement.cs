﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    abstract class UmlElement : Grid, IUmlElement
    {
        protected Shape shape;
        protected UmlElementState state;
        protected ShapeType type;
        protected Canvas umlCanvas;
        protected Brush 
            shapeBrush,
            idleStroke,
            selectStroke;
        protected int
            strokeThicknessEnter = 8,
            strokeThicknessLeave = 4;

        public UmlElement(Canvas cvs, ShapeType type)
        {
            InitializeBrushes();
            
            //setup shape
            state = UmlElementState.Selected;
            this.type = type;
            umlCanvas = cvs;
            umlCanvas.Children.Add(this);

            //attach events
            this.MouseEnter += EnterElement;
            this.MouseLeave += LeaveElement;
        }

        //SETUP

        private void InitializeBrushes()
        {
            shapeBrush = Brushes.LightGray;
            idleStroke = Brushes.DarkGray;
            selectStroke = Brushes.Black;
        }

        protected virtual void SetDepth(int currentZIndex)
        {
            Canvas.SetZIndex(this, currentZIndex);
        }

        //STATES

        public virtual void SetState(UmlElementState state, int currentZIndex)
        {
            this.state = state;
            SetDepth(currentZIndex);
        }

        //EVENTS

        protected void EnterElement(object sender, MouseEventArgs e)
        {
            //applies to all shapes - improves drag n drop understanding
            HoverEffect(true);
        }

        protected virtual void LeaveElement(object sender, MouseEventArgs e)
        {
            HoverEffect(false);

            if (state == UmlElementState.LineDrag)
                state = UmlElementState.NotSelected;
        }

        //ACTIONS

        protected virtual void HoverEffect(bool entered)
        {
            shape.StrokeThickness = entered ? strokeThicknessEnter : strokeThicknessLeave;
        }

        public virtual void SetColor(Brush color) { }

        protected virtual void SetFill(Brush newColor)
        {
            shape.Fill = newColor;
        }
        protected virtual void SetStroke(Brush newColor)
        {
            shape.Stroke = newColor;
        }

        public virtual void OnRemove() { }
    }
}
