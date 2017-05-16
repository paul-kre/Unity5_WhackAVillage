using UnityEngine;
using System.Collections;

namespace Plotter
{
    //usage:
    //Plotter.ValuePlotterController vpc = new Plotter.ValuePlotterController(this.gameObject, new Rect(...), Color.black, Color.white, 0, 2);
    //vpc.AddValue(x);
    public class ValuePlotterController
    {
        private ValuePlotter _valuePlotter;

        private float _min;
        private float _max;

        public ValuePlotterController(GameObject parent, Rect rect, Color background, Color lineColor, float min, float max)
        {
            _valuePlotter = parent.AddComponent<ValuePlotter>();
            _valuePlotter.Init((int)rect.width, (int)rect.height, (int)rect.x, (int)rect.y, lineColor, background);
            _min = min;
            _max = max;
        }

        public void AddValue(float value)
        {
            if (_valuePlotter.enabled)
            {
                float v = Mathf.Clamp((value - _min) / (_max - _min), 0f, 1f);
                _valuePlotter.AddFrame(v);
            }

            _valuePlotter.SetLabels(_min, _max, value);
        }

        public void Show()
        {
            _valuePlotter.enabled = true;
        }

        public void Hide()
        {
            _valuePlotter.enabled = false;
        }

        public void DestroyPlotter()
        {
            GameObject.Destroy(_valuePlotter);
        }
    }
}