// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;

namespace AvaloniaEdit.Editing
{
    internal sealed class CaretLayer : Layer
    {
        private readonly TextArea _textArea;

        private bool _isVisible;
        private Rect _caretRectangle;
        private bool _blink = false;
        public bool Blink { get => _blink; set { _blink = value; } }

        public CaretLayer(TextArea textArea) : base(textArea.TextView, KnownLayer.Caret)
        {
            _textArea = textArea;
            IsHitTestVisible = false;
        }

        public void Show(Rect caretRectangle)
        {
            _caretRectangle = caretRectangle;
            _isVisible = true;
            InvalidateVisual();
        }

        public void Hide()
        {
            if (_isVisible)
            {
                _isVisible = false;
                InvalidateVisual();
            }
        }

        internal IBrush CaretBrush;

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            if (_isVisible && _blink)
            {
                var caretBrush = CaretBrush ?? TextView.GetValue(TextBlock.ForegroundProperty);

                if (_textArea.OverstrikeMode)
                {
                    if (caretBrush is ISolidColorBrush scBrush)
                    {
                        var brushColor = scBrush.Color;
                        var newColor = Color.FromArgb(100, brushColor.R, brushColor.G, brushColor.B);
                        caretBrush = new SolidColorBrush(newColor);
                    }
                }

                var r = new Rect(_caretRectangle.X - TextView.HorizontalOffset,
                                  _caretRectangle.Y - TextView.VerticalOffset,
                                  _caretRectangle.Width,
                                  _caretRectangle.Height);

                drawingContext.FillRectangle(caretBrush, PixelSnapHelpers.Round(r, PixelSnapHelpers.GetPixelSize(this)));
            }
        }
    }
}
