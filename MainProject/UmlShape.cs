using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    class UmlShape : UmlElement
    {
        private TextBox _tBox;
        private Point _mouseSelectPosition;
        private float _moveOffset;
        private bool _canMove;
        
        public UmlShape(Canvas cvs, ShapeType type) : base(cvs, type)
        {
            //pixels before start moving shape
            _moveOffset = 50;

            //set up container
            SetSize(100, 100);
            AddChildren();
            SetColor(shapeBrush);
            SetStroke(idleStroke);

            //attach events
            this.MouseMove += OverElement_MouseMove;
            this.MouseLeftButtonDown += GetOriginSelectPos_MouseDown;
            this.MouseLeftButtonUp += Release_MouseUp;
        }

        //EVENTS

        private void Release_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _canMove = false;
        }

        protected override void LeaveElement(object sender, MouseEventArgs e)
        {
            base.LeaveElement(sender, e);
        }

        private void GetOriginSelectPos_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseSelectPosition = e.GetPosition(umlCanvas);
        }

        private void OverElement_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (Math.Abs(_mouseSelectPosition.X - e.GetPosition(umlCanvas).X) > _moveOffset)
                    _canMove = true;

                if (state == UmlElementState.Selected)
                {
                    if (!_canMove)
                        return;

                    MoveElement(e.GetPosition(umlCanvas));
                }
                else
                {
                    //drag line area
                }
            }
        }

        //ACTIONS

        private void MoveElement(Point mousePos)
        {
            Canvas.SetLeft(this, mousePos.X - (this.Width / 2));
            Canvas.SetTop(this, mousePos.Y - (this.Height / 2));
        }

        //SETUP

        private void AddChildren()
        {
            this.Children.Add(shape = AddShape());
            this.Children.Add(_tBox = AddTextBox());
        }

        private Shape AddShape()
        {
            Shape sp;

            if (type == ShapeType.Rectangle)
                sp = new Rectangle();
            else if (type == ShapeType.Ellipse)
                sp = new Ellipse();
            else if (type == ShapeType.Diamond)
                sp = new Rectangle()
                {
                    LayoutTransform = new RotateTransform(45)
                };
            else
            {
                MessageBox.Show("An error occured when creating a new shape");
                return null;
            }
            return sp;
        }

        private void SetSize(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        private TextBox AddTextBox()
        {
            return new TextBox()
            {
                Text = "TestText",
                MinWidth = this.Width * 0.5f,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        
        //STATES

        public override void SetState(UmlElementState state, int currentZIndex)
        {
            base.SetState(state, currentZIndex);
            SetStroke(state == UmlElementState.NotSelected ? idleStroke : selectStroke);
        }

        //EFFECTS

        public override void SetColor(Brush newColor)
        {
            shape.Fill = newColor;
        }
    }
}
