using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace AP.Windows
{
    public static class WindowExtensions
    {
        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="window">The System.Windows.Window.</param>
        /// <param name="maximized">Indicates if the window should be maximized.</param>
        public static void Show(this System.Windows.Window window, bool maximized)
        {
            if (window.Visibility == Visibility.Hidden)
            {
                window.Visibility = Visibility.Visible;
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(delegate()
                    {
                        window.WindowState = maximized ? WindowState.Maximized : WindowState.Normal;
                        window.Show();
                        window.Activate();
                    })
                );
            }
        }
    }
}