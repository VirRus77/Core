using System;
using System.Windows;
using System.Windows.Threading;

namespace Core.Wpf.Tools
{
    /// <summary>
    /// Класс хелпер для создания синхронизации межпоточного взаимодействия
    /// </summary>
    public static class DispatcherInvoke
    {
#if false
        private class DepObj
            : DependencyObject
        {
        }

        /// <summary>
        /// Возвращает делегат который выполнится в потоке который вызвал метод
        /// </summary>
        /// <param name="action">Делегат выполнения</param>
        /// <returns>Делегат выполняемый в этом потоке</returns>
        public static Action CreateInvoke(Action action)
        {
            var obj = new DepObj();
            return () =>
            {
                if (obj.CheckAccess())
                    action();
                else
                    obj.Dispatcher.BeginInvoke(action);
            };
        }

        /// <summary>
        /// Возвращает делегат который выполнится в потоке который вызвал метод
        /// </summary>
        /// <param name="action">Делегат выполнения</param>
        /// <returns>Делегат выполняемый в этом потоке</returns>
        public static Action<T> CreateInvoke<T>(Action<T> action)
        {
            var obj = new DepObj();
            return v =>
            {
                if (obj.CheckAccess())
                    action(v);
                else
                    obj.Dispatcher.BeginInvoke(action, v);
            };
        }

        /// <summary>
        /// Возвращает делегат который выполнится в потоке который вызвал метод
        /// </summary>
        /// <param name="action">Делегат выполнения</param>
        /// <returns>Делегат выполняемый в этом потоке</returns>
        public static Action<T, T2> CreateInvoke<T, T2>(Action<T, T2> action)
        {
            var obj = new DepObj();
            return (v, v2) =>
            {
                if (obj.CheckAccess())
                    action(v, v2);
                else
                    obj.Dispatcher.BeginInvoke(action, v);
            };
        }
#endif
        /// <summary>
        /// Возвращает делегат который выполнится в потоке который вызвал метод
        /// </summary>
        /// <param name="action">Делегат выполнения</param>
        /// <returns>Делегат выполняемый в этом потоке</returns>
        public static Action CreateInvoke(Action action)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            return () =>
            {
                if (dispatcher.CheckAccess())
                    action();
                else
                    dispatcher.BeginInvoke(action);
            };
        }

        /// <summary>
        /// Возвращает делегат который выполнится в потоке который вызвал метод
        /// </summary>
        /// <param name="action">Делегат выполнения</param>
        /// <returns>Делегат выполняемый в этом потоке</returns>
        public static Action<T> CreateInvoke<T>(Action<T> action)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            return v =>
            {
                if (dispatcher.CheckAccess())
                    action(v);
                else
                    dispatcher.BeginInvoke(action, v);
            };
        }
    }
}
