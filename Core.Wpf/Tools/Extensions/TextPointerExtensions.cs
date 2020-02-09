using System.Windows.Controls;
using System.Windows.Documents;

namespace Core.Tools.Extensions
{
    /// <summary>
    /// <see cref="https://social.msdn.microsoft.com/Forums/vstudio/en-US/bc67d8c5-41f0-48bd-8d3d-79159e86b355/textpointer-into-a-flowdocument-based-on-character-index?forum=wpf"/>
    /// </summary>
    public static class TextPointerExtensions
    {
        public static TextPointer GetPointerFromCharOffset(this TextBlock textBlock, int charOffset)
        {
            var navigator = textBlock.ContentStart;
            var counter = 0;
            while (navigator != null)
            {
                if (counter >= charOffset)
                    return navigator;

                var context = navigator.GetPointerContext(LogicalDirection.Forward);
                if (context == TextPointerContext.Text)
                {
                    navigator = navigator.GetNextInsertionPosition(LogicalDirection.Forward);
                    counter++;
                }
                else
                {
                    navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                }
            }

            return null;
        }
    }
}
