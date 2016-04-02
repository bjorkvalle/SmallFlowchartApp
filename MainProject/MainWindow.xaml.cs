using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MainProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShapeType _activeType;
        private UmlElement _activeElement;
        private Brush _activeBrush;
        private int _currentZIndex;
        private Button
            _activeShapeButton,
            _activeColorButton;

        public MainWindow()
        {
            InitializeComponent();
            InitializeValues();
            InitializeEventHandlers();
        }

        private void InitializeValues()
        {
            _currentZIndex = 0;
        }

        private void InitializeEventHandlers()
        {
            cvsUml.MouseLeftButtonDown += UmlCanvas_MouseDown;
        }

        //this only registers CLICKS (not hold)
        private void UmlCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UmlElement elm = (sender as UmlElement);

            if (elm != null)
            {
                ClickElement(elm);
                e.Handled = true; //to prevent the event from firing again if hovering over new element
            }
            else if((sender as Canvas) != null)
                ClickCanvas(e.GetPosition(cvsUml));
        }

        private void ClickCanvas(Point point)
        {
            if (_activeElement != null)
                DeselectUmlElement();
            else 
                CreateUmlElement(point); //create new shape - not lines
        }

        private void ClickElement(UmlElement elm)
        {
            DeselectUmlElement();

            //set new element state
            if (_activeType != ShapeType.Line)
            {
                _activeElement = elm;
                _activeElement.SetState(UmlElementState.Selected, ++_currentZIndex);
            }
            else 
            {
                if (elm.GetType() == typeof(UmlShape))
                {
                    //line drag possible
                    elm.SetState(UmlElementState.LineDrag, ++_currentZIndex);
                }
                else if (elm.GetType() == typeof(UmlConnector))
                {
                    _activeElement = elm;
                    elm.SetState(UmlElementState.Selected, _currentZIndex);
                }
            }
        }

        private void DeselectUmlElement()
        {
            if (_activeElement != null)
                _activeElement.SetState(UmlElementState.NotSelected, _currentZIndex);

            _activeElement = null;
        }

        private void CreateUmlElement(Point point)
        {
            if (_activeType != ShapeType.None && _activeType != ShapeType.Line)
            {
                _activeElement = new UmlShape(cvsUml, _activeType);
                SetElementStartUpState();
                SetElementPosition(point);
                SetElementEventHandlers();
            }
        }

        private void SetElementStartUpState()
        {
            _activeElement.SetState(UmlElementState.Selected, ++_currentZIndex);
            SetElementColor(_activeBrush);
        }

        private void SetElementPosition(Point point)
        {
            Canvas.SetLeft(_activeElement, point.X - (_activeElement.Width / 2));
            Canvas.SetTop(_activeElement, point.Y - (_activeElement.Height / 2));
        }

        private void SetElementEventHandlers()
        {
            _activeElement.MouseLeftButtonDown += UmlCanvas_MouseDown;
            _activeElement.Drop += UmlElement_MouseUp;
        }

        private void UmlElement_MouseUp(object sender, DragEventArgs e)
        {
            UmlShape elm = sender as UmlShape;

            if(elm != null)
            {
                UmlConnector con = elm.AddedNewConnector();

                if (con != null)
                    con.MouseLeftButtonDown += UmlCanvas_MouseDown;
            }
        }

        //**MENU**//

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
            Button btn = (sender as Button);

            SelectButton(ref _activeShapeButton, btn);
            SelectShapeType(btn.Name);
            DeselectUmlElement();
        }

        private void SelectButton(ref Button previous, Button next)
        {
            if (previous != null)
                previous.Background = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");

            if (next != null)
                next.Background = (Brush)new BrushConverter().ConvertFromString("#FFBFBFBF");

            previous = next;
        }

        private void DeselectButtons()
        {
            SelectButton(ref _activeColorButton, null);
            SelectButton(ref _activeShapeButton, null);
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);

            SelectButton(ref _activeColorButton, btn);
            SetElementColor(_activeBrush = btn.Foreground);
        }

        private void SetElementColor(Brush brush)
        {
            if (_activeElement != null && brush != null && _activeElement.GetType() != typeof(UmlConnector))
                _activeElement.SetColor(brush);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DeselectUmlElement();
                DeselectShapeType();
                SelectButton(ref _activeShapeButton, null);
            }
            else if(e.Key == Key.Delete)
            {
                cvsUml.Children.Remove(_activeElement);
                _activeElement = null;
            }
        }

        private void DeselectShapeType()
        {
            _activeType = ShapeType.None;
        }

        private void SelectShapeType(string type)
        {
            switch (type)
            {
                case ("btnRectangle"):
                    {
                        _activeType = ShapeType.Rectangle;
                        break;
                    }
                case ("btnDiamond"):
                    {
                        _activeType = ShapeType.Diamond;
                        break;
                    }
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

        
    }
}
