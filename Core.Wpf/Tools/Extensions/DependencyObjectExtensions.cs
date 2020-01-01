using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Core.Wpf.Tools.Extensions
{
    public static class DependencyObjectExtensions
    {

        public static void ClearBinding(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            BindingOperations.ClearBinding(dependencyObject, dependencyProperty);
        }

        public static BindingExpressionBase SetBinding(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, BindingBase binding)
        {
            return BindingOperations.SetBinding(dependencyObject, dependencyProperty, binding);
        }

        public static Binding GetBinding(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            return BindingOperations.GetBinding(dependencyObject, dependencyProperty);
        }

        public static BindingBase GetBindingBase(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            return BindingOperations.GetBindingBase(dependencyObject, dependencyProperty);
        }

        public static BindingExpression GetBindingExpression(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            return BindingOperations.GetBindingExpression(dependencyObject, dependencyProperty);
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            var parentObject = child?.GetParent();

            //we've reached the end of the tree
            if (parentObject == null)
                return null;

            //check if the parent matches the type we're looking for
            return parentObject is T parent
                ? parent
                : TryFindParent<T>(parentObject);
        }

        public static DependencyObject TryFindParent(this DependencyObject child, Type parentType)
        {
            //get parent item
            var parentObject = child?.GetParent();

            //we've reached the end of the tree
            if (parentObject == null)
                return null;

            //check if the parent matches the type we're looking for
            return parentType.IsInstanceOfType(parentObject)
                ? parentObject
                : parentObject.TryFindParent(parentType);
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Do note, that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParent(this DependencyObject child)
        {
            if (child == null)
                return null;

            if (child is ContentElement contentElement)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                    return parent;

                var frameworkContentElement = contentElement as FrameworkContentElement;
                return frameworkContentElement?.Parent;
            }

            // if it's not a ContentElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }
}
