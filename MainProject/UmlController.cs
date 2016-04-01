using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainProject
{
    class UmlController
    {
        private static UmlController _instance;

        private UmlElement _activeUmlElement;
        private Brush _activeColor;
        private ShapeType _activeType;
        private bool draggingConnector;

        private UmlController()
        {

        }

        public UmlController GetInstance()
        {
            if (_instance == null)
                _instance = new UmlController();

            return _instance;
        }
    }
}
