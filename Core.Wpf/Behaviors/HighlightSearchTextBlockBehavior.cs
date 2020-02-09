using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Core.Tools.Extensions;

namespace Core.Behaviors
{
    public class HighlightSearchTextBlockBehavior:DependencyObject
    {
        #region SearchValuesProperty

        public static readonly DependencyProperty SearchValuesProperty =
            DependencyProperty.RegisterAttached(
                "SearchValues",
                typeof(IList<string>),
                typeof(HighlightSearchTextBlockBehavior),
                new PropertyMetadata(null, SearchValuesPropertyChanged)
            );

        private static void SearchValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock textBlock))
                return;

            UpdateHighlight(textBlock);
        }

        public static void SetSearchValues(DependencyObject element, IList<string> value)
        {
            element.SetValue(SearchValuesProperty, value);
        }

        public static IList<string> GetSearchValues(DependencyObject element)
        {
            return (IList<string>)element.GetValue(SearchValuesProperty);
        }

        #endregion SearchValuesProperty

        #region ForegroundProperty

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
                    "Foreground", typeof(Color), typeof(HighlightSearchTextBlockBehavior), new PropertyMetadata(default(Color)));

        public static void SetForeground(DependencyObject element, Color value)
        {
            element.SetValue(ForegroundProperty, value);
        }

        public static Color GetForeground(DependencyObject element)
        {
            return (Color)element.GetValue(ForegroundProperty);

        }

        #endregion

        #region BackgroundProperty

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background", typeof(Color), typeof(HighlightSearchTextBlockBehavior), new PropertyMetadata(default(Color)));

        public static void SetBackground(DependencyObject element, Color value)
        {
            element.SetValue(BackgroundProperty, value);
        }

        public static Color GetBackground(DependencyObject element)
        {
            return (Color) element.GetValue(BackgroundProperty);
        }

        #endregion

        #region TextBindingProperty

        public static readonly DependencyProperty TextBindingProperty = 
            DependencyProperty.RegisterAttached(
            "TextBinding", 
            typeof(string), 
            typeof(HighlightSearchTextBlockBehavior), 
            new PropertyMetadata(default(Binding), TextBindingPropertyChanged)
            );

        private static void TextBindingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(!(d is TextBlock textBlock))
                return;

            textBlock.Text = e.NewValue as string;
            UpdateHighlight(textBlock);
        }

        public static void SetTextBinding(DependencyObject element, string value)
        {
            element.SetValue(TextBindingProperty, value);
        }

        public static string GetTextBinding(DependencyObject element)
        {
            return (string)element.GetValue(TextBindingProperty);
        }

        #endregion TextBindingProperty

        private static void UpdateHighlight(TextBlock textBlock)
        {
            // Сохраняем биндинг при его присутствии
            if (textBlock.GetBinding(TextBindingProperty) == null)
            {
                var binding = textBlock.GetBinding(TextBlock.TextProperty);
                if (binding != null)
                {
                    textBlock.SetBinding(TextBindingProperty, binding);
                }
            }

            var searchValues = GetSearchValues(textBlock);
            
            if (searchValues?.Any() != true)
            {
                HighlightTextBlockBehavior.SetRange(textBlock, null);
                return;
            }

            var textRange = new TextRange(textBlock.ContentStart, textBlock.ContentEnd);
            var containsText = textRange.Text;
            if (string.IsNullOrEmpty(containsText))
            {
                HighlightTextBlockBehavior.SetRange(textBlock, null);
                return;
            }

            var background = GetBackground(textBlock);
            var foreground = GetForeground(textBlock);

            var indexis = containsText
                .AllIndexesOf(searchValues.First(), StringComparison.InvariantCultureIgnoreCase)
                .ToList();

            var ranges = searchValues
                .Where(v => !string.IsNullOrEmpty(v))
                .Where(v => containsText.Contains(v, StringComparison.CurrentCultureIgnoreCase))
                .SelectMany(v =>
                    containsText.AllIndexesOf(v, StringComparison.InvariantCultureIgnoreCase)
                        .Select(index => new SelectionRange
                        {
                            Start = index,
                            End = index + v.Length,
                            Background = background,
                            Foreground = foreground,
                        }))
                .Cast<ISelectionRange>()
                .ToList();

            HighlightTextBlockBehavior.SetRange(textBlock, !ranges.Any() ? null : ranges);
        }
    }
}
