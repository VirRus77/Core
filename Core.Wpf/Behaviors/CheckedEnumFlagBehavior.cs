using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using Core.Tools.Extensions;

namespace Core.Behaviors
{
    public class CheckedEnumFlagBehavior : Behavior<ToggleButton>
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(object), typeof(CheckedEnumFlagBehavior), new PropertyMetadata(default(object)));

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty FlagsProperty = DependencyProperty.Register(
            nameof(Flags), typeof(object), typeof(CheckedEnumFlagBehavior), new PropertyMetadata(default(object)));

        public object Flags
        {
            get { return (object)GetValue(FlagsProperty); }
            set { SetValue(FlagsProperty, value); }
        }

        private static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            nameof(IsChecked), typeof(bool?), typeof(CheckedEnumFlagBehavior), new PropertyMetadata(default(bool?), IsCheckedPropertyChanged));

        private static void IsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CheckedEnumFlagBehavior behavior))
            {
                return;
            }

            behavior.SetChecked(e.NewValue as bool?);
        }

        private bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            var binding = new Binding(nameof(IsChecked))
            {
                Source = this,
                Mode = BindingMode.OneWayToSource,
            };
            AssociatedObject.SetBinding(ToggleButton.IsCheckedProperty, binding);
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.IsChecked = Source.GetFlag(Flags);
        }

        private void SetChecked(bool? checkedValue)
        {
            if (Source == null || Flags == null)
                return;
            Source = Source.SetFlag(Flags, checkedValue == true);
        }
    }
}
