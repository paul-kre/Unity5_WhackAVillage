using UnityEngine;
using System.Collections;

namespace Plotter
{
    public class ValuePlotter : MonoBehaviour
    {
        private Texture2D _tex1;
        private Texture2D _tex2;
        private Color32[] _clearColors;
        private Color _color;

        private int _posTex1;
        private int _posTex2;

        private float lastValue;

        private Rect _rect;
        private int _frames;
        private int _height;

        private string _minLabel = "";
        private string _maxLabel = "";
        private string _currentLabel = "";


        public void Init(int frames, int height, int posX, int posY, Color plotColor, Color background)
        {
            _frames = frames;
            _height = height;
            _rect = new Rect(posX, posY, _frames, _height);

            _tex1 = new Texture2D(frames, height);
            _tex2 = new Texture2D(frames, height);

            _tex1.filterMode = FilterMode.Point;
            _tex2.filterMode = FilterMode.Point;

            _color = plotColor;

            _clearColors = new Color32[height * frames];
            for (int i = 0; i < height * frames; i++)
            {
                _clearColors[i] = background;
            }

            _posTex1 = 0;
            _posTex2 = _frames;

            _tex1.SetPixels32(_clearColors);
            _tex2.SetPixels32(_clearColors);
            _tex1.Apply();
            _tex2.Apply();
        }

        void OnGUI()
        {
            GUI.BeginGroup(_rect);
            GUI.DrawTexture(new Rect(_posTex1, 0, _frames, _height), _tex1);
            GUI.DrawTexture(new Rect(_posTex2, 0, _frames, _height), _tex2);
            GUI.EndGroup();

            GUI.Label(new Rect(_frames + 10 + _rect.x, _rect.y, 100, 30), _maxLabel);
            GUI.Label(new Rect(_frames + 10 + _rect.x, _rect.y + _rect.height - 24, 100, 30), _minLabel);
            GUI.Label(new Rect(_frames + 10 + _rect.x, _rect.y + _rect.height / 2.0f - 12, 100, 30), _currentLabel);

        }

        public void SetLabels(float min, float max, float value)
        {
            _minLabel = "min: " + min.ToString("F2");
            _maxLabel = "max: " + max.ToString("F2");
            _currentLabel = "current:" + value.ToString("F2");
        }

        public void AddFrame(float value)
        {
            //draw
            Texture2D drawTex;
            int drawPos;
            if (_posTex1 < 0)
            {
                drawTex = _tex2;
                drawPos = -_posTex2;
            }
            else
            {
                drawTex = _tex1;
                drawPos = -_posTex1;
            }

            for (int i = 0; i < 2; i++)
            {
                for (int h = 0; h < _height; h++)
                {
                    drawTex.SetPixel(drawPos + i, h, _clearColors[0]);
                }
            }
            DrawLine(drawTex, drawPos - 1, lastValue, drawPos, (int)(value * _height), _color);
            drawTex.Apply();

            //move
            if (_posTex1 <= -_frames)
                _posTex1 = _frames - 1;
            else
                _posTex1--;
            if (_posTex2 <= -_frames)
                _posTex2 = _frames - 1;
            else
                _posTex2--;

            lastValue = (int)(value * _height);
        }

        private void DrawLine(Texture2D tex, float x0, float y0, float x1, float y1, Color col)
        {
            DrawLine(tex, Mathf.RoundToInt(x0), Mathf.RoundToInt(y0), Mathf.RoundToInt(x1), Mathf.RoundToInt(y1), col);
        }

        private void DrawLine(Texture2D tex, int x0, int y0, int x1, int y1, Color col) //Bresenham
        {
            int dy = y1 - y0;
            int dx = x1 - x0;
            int stepy, stepx, fraction;

            if (dy < 0) { dy = -dy; stepy = -1; }
            else { stepy = 1; }
            if (dx < 0) { dx = -dx; stepx = -1; }
            else { stepx = 1; }
            dy <<= 1;
            dx <<= 1;

            tex.SetPixel(x0, y0, col);
            if (dx > dy)
            {
                fraction = dy - (dx >> 1);
                while (x0 != x1)
                {
                    if (fraction >= 0)
                    {
                        y0 += stepy;
                        fraction -= dx;
                    }
                    x0 += stepx;
                    fraction += dy;
                    tex.SetPixel(x0, y0, col);
                }
            }
            else
            {
                fraction = dx - (dy >> 1);
                while (y0 != y1)
                {
                    if (fraction >= 0)
                    {
                        x0 += stepx;
                        fraction -= dy;
                    }
                    y0 += stepy;
                    fraction += dx;
                    tex.SetPixel(x0, y0, col);
                }
            }
        }
    }
}