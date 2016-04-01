using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UmlElement _activeElement;
        ShapeType _activeType;
        Brush _activeBrush;
        int _currentZIndex;

        public MainWindow()
        {
            InitializeComponent();
            //InitializeValues();

            _currentZIndex = 0;
            cvsUml.MouseLeftButtonDown += UmlCanvas_MouseDown;
            CreateUmlElement(new Point(100, 100));
        }
        
        //this only registers CLICKS (not hold)
        private void UmlCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UmlElement elm = (sender as UmlElement);

            if (elm != null)
            {
                ClickElement(elm);

                //to prevent the event from firing again if hovering over new element
                e.Handled = true;
            }
            else if((sender as Canvas) != null)
            {
                ClickCanvas(e.GetPosition(cvsUml));
            }

            //-->DEBUG - REMOVE LATER
            if (_activeElement != null)
                _activeElement.SetColor(Brushes.BlanchedAlmond);
            else
            {
                foreach (var item in cvsUml.Children)
                {
                    UmlElement i = item as UmlElement;

                    if (i != null)
                        i.SetColor(Brushes.Crimson);
                }
            }
            //<--
        }

        private void ClickCanvas(Point point)
        {
            //deselect previous active
            if (_activeElement != null)
            {
                _activeElement.SetState(UmlElementState.NotSelected, _currentZIndex);
                _activeElement = null;
            }
            else //create new shape - not lines
                CreateUmlElement(point);
        }

        private void ClickElement(UmlElement elm)
        {
            //deselect previous active
            if (_activeElement != null)
                _activeElement.SetState(UmlElementState.NotSelected, _currentZIndex);
            
            //set new active
            _activeElement = elm;

            //set new element state
            if (_activeType != ShapeType.Line)
            {
                //move possible
                _activeElement.SetState(UmlElementState.Selected, ++_currentZIndex);
            }
            else 
            {
                //line drag possible
                _activeElement.SetState(UmlElementState.LineDrag, ++_currentZIndex);
            }
        }

        private void CreateUmlElement(Point point)
        {
            if (_activeType != ShapeType.None && _activeType != ShapeType.Line)
            {
                _activeElement = new UmlShape(cvsUml, _activeType);
                _activeElement.MouseLeftButtonDown += UmlCanvas_MouseDown;
                _activeElement.SetState(UmlElementState.Selected, ++_currentZIndex);

                Canvas.SetLeft(_activeElement, point.X - (_activeElement.Width / 2));
                Canvas.SetTop(_activeElement, point.Y - (_activeElement.Height / 2));
            }
        }




        ////registers constantly when fired
        //private void MovingOnCanvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    _activeElement = (sender as UmlElement);

        //    if (_activeElement != null)
        //    {
        //        _activeElement.SetPosition(cvsUml, e);
        //    }

        //    //this registers HOLDING
        //    if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
        //    {
        //        _activeElement = (sender as UmlElement);
        //        //_mouseLeftReleased = false;

        //        //must have been hovering umlElement to select it,
        //        //not "slide" in from canvas
        //        if (_activeElement != null)
        //        {
        //            if (_mouseLeftReleased)
        //            {
        //                //marks object as isSelected
        //                //_activeElement.SetSelected(true);
        //                tBlock_Debug1.Text = "selected";
        //            }

        //            //to prevent the event to fire again if hovering over new element
        //            e.Handled = true;
        //        }
        //        else if((sender as Canvas) != null) //deselects active UmlElement
        //        {
        //            MessageBox.Show("asdasd");
        //            //to prevent selection of umlElement by "sliding" into it
        //            _mouseLeftReleased = false;

        //            //Mark active UmlElement as !isSelected
        //            if (_activeElement != null)
        //            {
        //                //_activeElement.SetSelected(false);
        //                tBlock_Debug1.Text = "not selected";
        //                _activeElement = null;
        //            }
        //        }
        //    }

        //    if (e.MouseDevice.LeftButton == MouseButtonState.Released)
        //    {
        //        _mouseLeftReleased = true;

        //        //if (sender as UmlElement != null)
        //        //{

        //        //}
        //        //else if ((sender as Canvas) != null)
        //        //{

        //        //}
        //    }
        //}

        //private void MovingOnCanvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    _hoverElement = (sender as UmlElement);

        //    if (_hoverElement != null)
        //    {
        //        _hoverElement.Hover(true, _selectedType);
        //        //to prevent the event to continue firing if hovering over shape
        //        e.Handled = true;
        //        return;
        //    }
        //    else if ((sender as Canvas) != null)
        //    {
        //        if (_hoverElement != null)
        //        {
        //            _hoverElement.Hover(false, _selectedType);
        //            _hoverElement = null;
        //        }
        //    }
        //}





        //private void UmlCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    //if hovering over an uml when clicking, set activeElement to this and cancel the rest
        //    if((sender as UmlElement) != null)
        //    {
        //        if(_activeElement != null)
        //            _activeElement.ReleaseElement();

        //        _activeElement = (sender as UmlElement);
        //        _activeElement.SelectElement();
        //        e.Handled = true;
        //    }

        //    //else deselect the active element, if one is active
        //    if (((sender as Canvas) != null) && _activeElement != null)
        //    {
        //        _activeElement.ReleaseElement();
        //        _activeElement = null;
        //    }
        //}

        //private void UmlElement_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    UmlElement elm = (sender as UmlElement);

        //    if ((sender as UmlElement) != null)
        //        _hoverElement = elm;
        //}

        //private void UmlElement_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    UmlElement elm = (sender as UmlElement);

        //    if ((sender as UmlElement) != null)
        //        _hoverElement = null;
        //}

        private void InitializeValues()
        {
            //_connectorGuide = null;
            _activeType = ShapeType.Line;
            //_activeShape = null;
            _activeBrush = new BrushConverter().ConvertFromString("Blue") as Brush;

            //Elp1.MouseMove += Shape_MouseMove;
            //Elp2.MouseMove += Shape_MouseMove;
            Elp1.Drop += Shape_Drop;
            Elp2.Drop += Shape_Drop;
        }

        private void Shape_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("Line") && e.Data.GetDataPresent("OriginObject"))
            {
                //get connector points
                Shape originObj = (Shape)e.Data.GetData("OriginObject");
                Shape targetObj = (sender as Shape);
                Point originPos = new Point(Canvas.GetLeft(originObj) + (originObj.Width / 2),
                                            Canvas.GetTop(originObj) + (originObj.Height / 2));
                Point targetPos = new Point(Canvas.GetLeft(targetObj) + (targetObj.Width / 2),
                                            Canvas.GetTop(targetObj) + (targetObj.Height / 2));

                //Place connector
                Line line = (Line)e.Data.GetData("Line");
                line.X1 = originPos.X;
                line.Y1 = originPos.Y;
                line.X2 = targetPos.X;
                line.Y2 = targetPos.Y;
            }
        }

        //**SHAPE EVENTS**//

        //private void Shape_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Shape shape = (sender as Shape);
        //    //Point shapePosition = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));

        //    if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && !draggingConnector)
        //    {
        //        //Create Line and Line Data
        //        if (_activeType == ShapeType.Line)
        //        {
        //            draggingConnector = true;

        //            //Set up line
        //            _activeShape = new Line()
        //            {
        //                StrokeThickness = 5,
        //                Stroke = _activeBrush
        //            };
        //            cvsUml.Children.Add(_activeShape);
                    
        //            //Package Data
        //            DataObject data = new DataObject();
        //            data.SetData("OriginObject", shape);
        //            data.SetData("Line", _activeShape);

        //            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        //        }
        //        else //move shape
        //        {
        //            Canvas.SetLeft(shape, e.GetPosition(cvsUml).X - (shape.Width / 2));
        //            Canvas.SetTop(shape, e.GetPosition(cvsUml).Y - (shape.Height / 2));
        //        }
        //    }
        //    else if (e.MouseDevice.LeftButton == MouseButtonState.Released)
        //        draggingConnector = false;
        //}

        //**MENU EVENTS**//

        private void btnSavePng_Click(object sender, RoutedEventArgs e)
        {
        }
        
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _activeElement = null;
            cvsUml.Children.Clear();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnShapeSelect_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case ("btnEllipse"):
                    {
                        _activeType = ShapeType.Ellipse;
                        break;
                    }
                case ("btnLine"):
                    {
                        _activeType = ShapeType.Line;
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            _activeBrush = (sender as Button).Foreground;

            if (_activeElement != null && _activeBrush != null)
                _activeElement.SetColor(_activeBrush);
        }
    }
}
