using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml;
using System.Xml.Linq;
using SidePlayer.Annotations;

namespace SidePlayer.MediasPlayer.Video
{
    public class SubtitleExpression
    {
        public static readonly DependencyProperty InlineExpressionProperty = DependencyProperty.RegisterAttached(
            "SubtitleExpression", typeof (string), typeof (TextBlock),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetInlineExpression(TextBlock textBlock, string value)
        {
            textBlock.SetValue(InlineExpressionProperty, value);

            textBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(value))
                return;

            var descriptions = GetInlineDescriptions(value);
            if (descriptions.Length == 0)
                return;

            var inlines = GetInlines(textBlock, descriptions);
            if (inlines.Length == 0)
                return;

            textBlock.Inlines.AddRange(inlines);
        }

        public static string GetInlineExpression(TextBlock textBlock)
        {
            return (string) textBlock.GetValue(InlineExpressionProperty);
        }

        private enum InlineType
        {
            Run = 0,
            LineBreak = 1,
            Span = 2,
            Bold = 3,
            Italic = 4,
            Hyperlink = 5,
            Underline = 6
        }

        private class SubtitleDescription
        {
            public InlineType Type { get; set; }
            public string Text { get; set; }
            public SubtitleDescription[] Subtitles { get; set; }
            public string FontFace { get; set; }
            public string FontSize { get; set; }
        }

        private static Inline[] GetInlines(FrameworkElement element, IEnumerable<SubtitleDescription> inlineDescriptions)
        {
            var inlines = new List<Inline>();
            foreach (var description in inlineDescriptions)
            {
                var inline = GetInline(element, description);
                if (inline != null)
                    inlines.Add(inline);
            }

            return inlines.ToArray();
        }

        private static Inline GetInline(FrameworkElement element, SubtitleDescription description)
        {
            FontFamily fontFamily = null;
            if (description.FontFace != null)
                fontFamily = new FontFamily(description.FontFace);

            Style style = null;

            Inline inline = null;
            switch (description.Type)
            {
                case InlineType.Run:
                    var run = new Run(description.Text);
                    if (description.FontSize != null) run.FontSize = double.Parse(description.FontSize);
                    if (fontFamily != null) run.FontFamily = fontFamily;
                    inline = run;
                    break;
                case InlineType.LineBreak:
                    var lineBreak = new LineBreak();
                    inline = lineBreak;
                    break;
                case InlineType.Span:
                    var span = new Span();
                    inline = span;
                    break;
                case InlineType.Bold:
                    var bold = new Bold();
                    inline = bold;
                    break;
                case InlineType.Italic:
                    var italic = new Italic();
                    inline = italic;
                    break;
                case InlineType.Hyperlink:
                    var hyperlink = new Hyperlink();
                    inline = hyperlink;
                    break;
                case InlineType.Underline:
                    var underline = new Underline();
                    inline = underline;
                    break;
            }

            if (inline != null)
            {
                var span = inline as Span;
                if (span != null)
                {
                    var childInlines = new List<Inline>();
                    foreach (var inlineDescription in description.Subtitles)
                    {
                        var childInline = GetInline(element, inlineDescription);
                        if (childInline == null)
                            continue;

                        childInlines.Add(childInline);
                    }

                    span.Inlines.AddRange(childInlines);
                }
            }

            return inline;
        }

        private static SubtitleDescription[] GetInlineDescriptions(string inlineExpression)
        {
            if (inlineExpression == null)
                return new SubtitleDescription[0];

            inlineExpression = inlineExpression.Trim();
            if (inlineExpression.Length == 0)
                return new SubtitleDescription[0];

            inlineExpression = inlineExpression.Insert(0, @"<root>");
            inlineExpression = inlineExpression.Insert(inlineExpression.Length, @"</root>");

            var xmlTextReader = new XmlTextReader(new StringReader(inlineExpression));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlTextReader);

            var rootElement = xmlDocument.DocumentElement;
            if (rootElement == null)
                return new SubtitleDescription[0];

            var inlineDescriptions = new List<SubtitleDescription>();

            foreach (XmlNode childNode in rootElement.ChildNodes)
            {
                var description = GetInlineDescription(childNode);
                if (description == null)
                    continue;

                inlineDescriptions.Add(description);
            }

            return inlineDescriptions.ToArray();
        }

        private static SubtitleDescription GetInlineDescription(XmlNode node)
        {
            var element = node as XmlElement;
            if (element != null)
                return GetInlineDescription(element);
            var text = node as XmlText;
            if (text != null)
                return GetInlineDescription(text);
            return null;
        }

        private static SubtitleDescription GetInlineDescription(XmlElement element)
        {
            InlineType type;
            var elementName = element.Name.ToLower();
            switch (elementName)
            {
                case "font":
                    type = InlineType.Run;
                    break;
                case "br":
                    type = InlineType.LineBreak;
                    break;
                case "b":
                    type = InlineType.Bold;
                    break;
                case "i":
                    type = InlineType.Italic;
                    break;
                case "u":
                    type = InlineType.Underline;
                    break;
                default:
                    return null;
            }

            string fontFaceName = null;
            {
                var attribute = element.GetAttributeNode("face");
                if (attribute != null)
                    fontFaceName = attribute.Value;
            }

            string fontSizeName = null;
            {
                var attribute = element.GetAttributeNode("size");
                if (attribute != null)
                    fontSizeName = attribute.Value;
            }

            string text = null;
            var childDescriptions = new List<SubtitleDescription>();

            if (type == InlineType.Run || type == InlineType.LineBreak)
            {
                text = element.InnerText;
            }
            else
            {
                foreach (XmlNode childNode in element.ChildNodes)
                {
                    var childDescription = GetInlineDescription(childNode);
                    if (childDescription == null)
                        continue;

                    childDescriptions.Add(childDescription);
                }
            }

            var inlineDescription = new SubtitleDescription
            {
                Type = type,
                FontFace = fontFaceName,
                FontSize = fontSizeName,
                Text = text,
                Subtitles = childDescriptions.ToArray()
            };

            return inlineDescription;
        }

        private static SubtitleDescription GetInlineDescription(XmlText text)
        {
            var value = text.Value;
            if (string.IsNullOrEmpty(value))
                return null;

            var inlineDescription = new SubtitleDescription
            {
                Type = InlineType.Run,
                Text = value
            };
            return inlineDescription;
        }
    }

    public class Subtitles : INotifyPropertyChanged
    {
        private static Regex _srtRegex = new Regex(
            @"(?<Order>\d+)\n(?<start>(\d\d:\d\d:\d\d,\d\d\d))\s-->\s(?<end>(\d\d:\d\d:\d\d,\d\d\d))\n(?<text>(.+?(\n\n|$)))",
            RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

        private List<Subtitle> _subtitles = new List<Subtitle>();

        private TextBlock _subtitle = null;
        public TextBlock SubtitleTextBlock
        {
            get { return _subtitle; }
            set
            {
                _subtitle = value;
                OnPropertyChanged(nameof(SubtitleTextBlock));
            }
        }

        private class Subtitle
        {
            private readonly string _text;

            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }

            public TextBlock SubtitleBlock { get; } = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White),
                Effect = new DropShadowEffect { BlurRadius = 1, Color = Colors.Black, ShadowDepth = 2 }
            };

            public Subtitle(TimeSpan start, TimeSpan end, string text)
            {
                SubtitleExpression.SetInlineExpression(SubtitleBlock, text);
                SubtitleBlock.TextAlignment = TextAlignment.Center;
                Start = start;
                End = end;
            }
        }

        private bool _in;
        private int _position = -1;

        public void Clear()
        {
            SubtitleTextBlock = null;
            _subtitles.Clear();
        }

        public void UpdateSubtitles(Uri srtFile)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");

            using (var input = new StreamReader(srtFile.ToString()))
            {
                var file = input.ReadToEnd();
                MatchCollection matches = _srtRegex.Matches(file);

                foreach (Match match in matches)
                {
                    _subtitles.Add(new Subtitle(TimeSpan.Parse(match.Groups["start"].Value),
                        TimeSpan.Parse(match.Groups["end"].Value),
                        match.Groups["text"].Value))
                        ;
                }
            }
            Refresh(TimeSpan.Zero);
        }

        public void Refresh(TimeSpan position)
        {
            if (_subtitles.Count == 0)
                return;
            Subtitle sub = null;
            foreach (var subtitle in _subtitles)
                if (subtitle.Start < position && subtitle.End > position)
                    sub = subtitle;
            SubtitleTextBlock = sub?.SubtitleBlock;
        }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}