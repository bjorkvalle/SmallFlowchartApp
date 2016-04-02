﻿using System;
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
        private List<UmlConnector> _attachedConnectors;

        private TextBox _tBox;
        //private float _holdTime;
        //private float _timeBeforeMove;
        
        public UmlShape(Canvas cvs, ShapeType type) : base(cvs, type)
        {
            AllowDrop = true;
            _attachedConnectors = new List<UmlConnector>();
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
            //_holdTime = 0;
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
                }
                else if (state == UmlElementState.LineDrag)
                {
                    //drag line area

                    _tBox.Text = "dragginh";
                    

                    //Package Data
                    DataObject data = new DataObject();
                    data.SetData("connector", new UmlConnector(umlCanvas)); 
                    data.SetData("origin", this);

                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
        }

        private void Connector_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("connector") && e.Data.GetDataPresent("origin"))
            {
                //get affected objects
                UmlConnector connector = (UmlConnector)e.Data.GetData("connector");
                UmlShape originObj = (UmlShape)e.Data.GetData("origin");
                UmlShape targetObj = (sender as UmlShape);

                //add connector references to affected shapes
                originObj.AddConnector(connector);
                targetObj.AddConnector(connector);

                //get connector points
                //Point originPos = new Point(Canvas.GetLeft(originObj) + (originObj.Width / 2),
                //                            Canvas.GetTop(originObj) + (originObj.Height / 2));
                //Point targetPos = new Point(Canvas.GetLeft(targetObj) + (targetObj.Width / 2),
                //                            Canvas.GetTop(targetObj) + (targetObj.Height / 2));

                //Place connector
                connector.SetPosition(originObj, targetObj);

                //Line line = (Line)e.Data.GetData("Line");
                //line.X1 = originPos.X;
                //line.Y1 = originPos.Y;
                //line.X2 = targetPos.X;
                //line.Y2 = targetPos.Y;
            }
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
