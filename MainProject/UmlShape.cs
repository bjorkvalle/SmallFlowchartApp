using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    class UmlShape : UmlElement
    {
        private List<UmlConnector> _attachedConnectors;

        private TextBox _tBox;
        bool _addedNewConnector;
        //private float _holdTime;
        //private float _timeBeforeMove;
        
        public UmlShape(Canvas cvs, ShapeType type) : base(cvs, type)
        {
            AllowDrop = true;

            _attachedConnectors = new List<UmlConnector>();
            _addedNewConnector = false;
            //_holdTime = 0;
            //_timeBeforeMove = 1.5f;

            //set up container
            SetSize(100, 100);
            AddChildren();
            SetColor(shapeBrush);
            SetStroke(idleStroke);

            //attach events
            this.MouseMove += OnMouseMove;
            this.MouseLeftButtonUp += OnMouseUp;
            this.Drop += Connector_Drop;
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
                MinWidth = this.Width * 0.2f,
                MaxWidth = this.Width*0.95f,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        //EVENTS

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (state == UmlElementState.LineDrag)
                state = UmlElementState.NotSelected;
        }

        protected override void LeaveElement(object sender, MouseEventArgs e)
        {
            base.LeaveElement(sender, e); //hover effect
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                ////if time
                //SetMovePermission();

                if (state == UmlElementState.Selected)
                {
                    ////if time
                    //if (_holdTime < _timeBeforeMove)
                    //    return;

                    MoveElement(e.GetPosition(umlCanvas));
                    MoveConnectors();
                }
                else if (state == UmlElementState.LineDrag)
                {
                    InitiateDragLineEvent();
                }
            }
        }

        private void InitiateDragLineEvent()
        {
            //Package Data
            DataObject data = new DataObject();
            data.SetData("origin", this);

            //initiate drag event
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        }

        private void Connector_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("origin"))
            {
                //get affected objects
                UmlConnector connector = new UmlConnector(umlCanvas);
                UmlShape originObj = (UmlShape)e.Data.GetData("origin");
                UmlShape targetObj = (sender as UmlShape);

                //add connector references to affected shapes
                originObj.AddConnector(connector);
                targetObj.AddConnector(connector);

                //Place connector
                connector.SetPosition(originObj, targetObj);

                _addedNewConnector = true;
            }
        }

        public UmlConnector AddedNewConnector()
        {
            if (_addedNewConnector)
            {
                _addedNewConnector = false;
                return _attachedConnectors[_attachedConnectors.Count-1] as UmlConnector;
            }
            else
                return null;
        }

        //ACTIONS

        public void AddConnector(UmlConnector connector)
        {
            if (connector != null)
                _attachedConnectors.Add(connector);
        }

        ////if time, have a look at a timer
        private void SetMovePermission()
        {
            ////if time, have a look at a timer
            //while (_holdTime < _timeBeforeMove)
            //{
            //    yield return new WaitForSeconds(1);
            //}
        }

        private void MoveElement(Point mousePos)
        {
            Canvas.SetLeft(this, mousePos.X - (this.Width / 2));
            Canvas.SetTop(this, mousePos.Y - (this.Height / 2));
        }

        private void MoveConnectors()
        {
            foreach(UmlConnector item in _attachedConnectors)
            {
                item.SetPosition();
            }
        }

        public override void SetState(UmlElementState state, int currentZIndex)
        {
            base.SetState(state, currentZIndex);

            SetStroke(state == UmlElementState.Selected ? selectStroke : idleStroke);
        }

        public override void SetColor(Brush newColor)
        {
            shape.Fill = newColor;
        }
    }
}
