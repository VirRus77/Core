using System.Windows;

namespace Core.Triggers
{
    /// <summary>
    /// Тригер перехватывающий дальнейшую обработку события
    /// </summary>
    public class HandlingEventTrigger : System.Windows.Interactivity.EventTrigger
    {
        public static readonly DependencyProperty IsHandledProperty = DependencyProperty.Register(
            "IsHandled", typeof(bool), typeof(HandlingEventTrigger), new PropertyMetadata(true));

        public bool IsHandled
        {
            get { return (bool)GetValue(IsHandledProperty); }
            set { SetValue(IsHandledProperty, value); }
        }

        protected override void OnEvent(System.EventArgs eventArgs)
        {
            if (eventArgs is RoutedEventArgs routedEventArgs)
            {
                routedEventArgs.Handled = IsHandled;
            }

            base.OnEvent(eventArgs);
        }
    }
}
