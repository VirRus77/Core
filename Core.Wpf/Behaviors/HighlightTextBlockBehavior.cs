using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Core.Wpf.Tools.Extensions;

namespace Core.Wpf.Behaviors
{
    /// <summary>
    /// Implements a <see cref="TextBlock"/> behavior to highlight text as specified by
    /// a bound property that adheres to the <see cref="ISelectionRange"/> interface.
    /// </summary>
    public static class HighlightTextBlockBehavior
    {
        #region fields

        /// <summary>
        /// <see cref="https://www.codeproject.com/Articles/1217419/A-Highlightable-WPF-MVVM-TextBlock-in-Csharp-VB-Ne"/>
        /// Back Store of Attachable Dependency Property that indicates the range
        /// of text that should be highlighting (if any).
        /// </summary>
        private static readonly DependencyProperty RangeProperty =
            DependencyProperty.RegisterAttached(
                "Range",
                typeof(IList<ISelectionRange>),
                typeof(HighlightTextBlockBehavior),
                new PropertyMetadata(null, RangePropertyChanged)
            );
        #endregion fields

        #region methods
        /// <summary>
        /// Gets the current values of the Range dependency property.
        /// </summary>
        public static IList<ISelectionRange> GetRange(DependencyObject obj)
        {
            return (IList<ISelectionRange>)obj.GetValue(RangeProperty);
        }

        /// <summary>
        /// Gets the current values of the Range dependency property.
        /// </summary>
        public static void SetRange(DependencyObject obj, IList<ISelectionRange> value)
        {
            obj.SetValue(RangeProperty, value);
        }

        /// <summary>
        /// Method executes whenever the Range dependency property valua has changed
        /// (in the bound viewmodel).
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void RangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock textBlock))
                return;

            var textRange = new TextRange(textBlock.ContentStart, textBlock.ContentEnd);
            textRange.ClearAllProperties();

            var ranges = GetRange(d);
            if (ranges?.Any() != true)
            {
                return;
            }

            foreach (var range in ranges)
            {
                var startRange = textBlock.GetPointerFromCharOffset(range.Start);
                var endRange = textBlock.GetPointerFromCharOffset(range.End);
                if (startRange == null || endRange == null)
                    continue;

                var textRangeSelection = new TextRange(startRange, endRange);
                Debug.WriteLine($"{startRange} - {endRange} = {textRangeSelection.Text}");
                if (range.Background != default(Color))
                {
                    var solidColorBrush = new SolidColorBrush(range.Background);
                    textRangeSelection.ApplyPropertyValue(TextElement.BackgroundProperty, solidColorBrush);
                }
                if (range.Foreground != default(Color))
                {
                    var solidColorBrush = new SolidColorBrush(range.Foreground);
                    textRangeSelection.ApplyPropertyValue(TextElement.ForegroundProperty, solidColorBrush);
                }
            }
        }
        #endregion methods
    }
    /// <summary>
    /// Defines a range that can be used to indicate
    /// the start and end of a text selection or any other kind of range.
    /// </summary>
    public interface ISelectionRange
    {
        /// <summary>
        /// Gets the start of the indicated range.
        /// </summary>
        int Start { get; }

        /// <summary>
        /// Gets the end of the indicated range.
        /// </summary>
        int End { get; }

        /// <summary>
        /// Gets the background color that is applied to the background brush,
        /// which should be applied when no match is indicated
        /// (this can be default(Color) in which case standard selection Brush
        /// is applied).
        /// </summary>
        Color Background { get; }
        Color Foreground { get; }
    }

    public class SelectionRange : ISelectionRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
    }
}
