﻿// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
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

using System.Collections.Generic;
using Avalonia.Media.TextFormatting;

namespace AvaloniaEdit.Rendering
{
    internal sealed class TextViewCachedElements /*: IDisposable*/
    {
        private TextFormatter _formatter;
        private Dictionary<string, TextLine> _nonPrintableCharacterTexts;

        public TextLine GetTextForNonPrintableCharacter(string text, ITextRunConstructionContext context)
        {
            if (_nonPrintableCharacterTexts == null)
                _nonPrintableCharacterTexts = new Dictionary<string, TextLine>();
            TextLine textLine;
            if (!_nonPrintableCharacterTexts.TryGetValue(text, out textLine)) {
                var p = new VisualLineElementTextRunProperties(context.GlobalTextRunProperties);
                p.SetForegroundBrush(context.TextView.NonPrintableCharacterBrush);
                if (_formatter == null)
                    _formatter = TextFormatter.Current;//TextFormatterFactory.Create(context.TextView);
                textLine = FormattedTextElement.PrepareText(_formatter, text, p);
                _nonPrintableCharacterTexts[text] = textLine;
            }
            return textLine;
        }

        /*public void Dispose()
        {
            if (nonPrintableCharacterTexts != null) {
                foreach (TextLine line in nonPrintableCharacterTexts.Values)
                    line.Dispose();
            }
            if (formatter != null)
                formatter.Dispose();
        }*/
    }
}
