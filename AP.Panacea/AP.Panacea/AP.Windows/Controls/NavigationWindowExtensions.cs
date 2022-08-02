using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AP.Windows.Controls
{
    public static class NavigationWindowExtensions
    {
        private const string PART_ContentPresenterName = "PART_NavWinCP";
        
        private const string CanInfuseContentPropertyName = "CanInfuseContent";
        public static readonly DependencyProperty CanInfuseContentProperty;
        
        private const string UseJournalEntryNamePropertyName = "UseJournalEntryName";
        public static readonly DependencyProperty UseJournalEntryNameProperty;
        
        static NavigationWindowExtensions()
        {
            Type navigationWindowType = typeof(NavigationWindow);

            CanInfuseContentProperty = DependencyProperty.Register
            (
                CanInfuseContentPropertyName, typeof(bool), navigationWindowType,
                new PropertyMetadata
                (
                    false,
                    new PropertyChangedCallback((window, e) =>
                    {
                        if (e.NewValue != e.OldValue)
                        {
                            if ((bool)e.NewValue)
                                ((NavigationWindow)window).Navigated += Navigated;
                            else
                                ((NavigationWindow)window).Navigated -= Navigated;
                        }
                    })
                )
            );
            
            UseJournalEntryNameProperty = DependencyProperty.Register
            (
                UseJournalEntryNamePropertyName, typeof(bool), navigationWindowType,
                new PropertyMetadata
                (
                    false,
                    new PropertyChangedCallback((frame, e) =>
                    {
                        if (e.NewValue != e.OldValue)
                        {
                            if ((bool)e.NewValue)
                                ((NavigationWindow)frame).Navigating += GetTitle;
                            else
                                ((NavigationWindow)frame).Navigating -= GetTitle;
                        }
                    })
                )
            );
        }

        /// <summary>
        /// Gets the CanInfuseContent dependency property value.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool CanInfuseContent(this NavigationWindow window)
        {
            return (bool)window.GetValue(CanInfuseContentProperty);
        }

        /// <summary>
        /// Sets the CanInfuseContent dependency property value.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="value"></param>
        public static void CanInfuseContent(this NavigationWindow window, bool value)
        {
            window.SetValue(CanInfuseContentProperty, value);
        }
        public static void Infuse(this NavigationWindow window, object content)
        {
            Infuse(window, content, null);
        }
        public static void Infuse(this NavigationWindow window, object content, object extraData)
        {
            InfuseInternal(window, content, null, extraData);
        }
        public static void Infuse(this NavigationWindow window, Uri source)
        {
            Infuse(window, source, null);
        }
        public static void Infuse(this NavigationWindow window, Uri source, object extraData)
        {
            InfuseInternal(window, null, source, extraData);
        }

        private static void InfuseInternal(NavigationWindow window, object content, Uri source, object extraData)
        {
            if (!(bool)window.GetValue(CanInfuseContentProperty))
                throw new InvalidOperationException("Can't infuse content.");

            Frame f = new Frame { JournalOwnership = JournalOwnership.OwnsJournal, IsHitTestVisible = false };

            // add the notification for when loading is complete
            f.LoadCompleted += (s, e) =>
            {
                // update the presenting window - this doesn't change the url or history!
                ContentPresenter cp = (ContentPresenter)window.Template.FindName(PART_ContentPresenterName, window);
                cp.Content = f.Content;
            };

            // navigate using the dummy window
            if (content != null)
                f.Navigate(content, extraData);
            else
                f.Navigate(source, extraData);
        }

        private static void Navigated(object sender, NavigationEventArgs e)
        {
            NavigationWindow w = (NavigationWindow)e.Navigator;
            object last = w.Content;

            // revert the content to what it should be - doesn't matter if it already is what it is supposed to be
            ContentPresenter cp = (ContentPresenter)w.Template.FindName(PART_ContentPresenterName, w);

            if (cp.Content != last)
                cp.Content = last;
        }
        
        private static void GetTitle(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Refresh)
                return;

            NavigationWindow window = (NavigationWindow)e.Navigator;
            object content = window.Content;

            if (content == null || content is DependencyObject)
                return;

            // it's a normal object, retrieve the DataTemplate - uses the PART_FrameCP
            ContentPresenter cp = (ContentPresenter)window.Template.FindName(PART_ContentPresenterName, window);

            if (VisualTreeHelper.GetChildrenCount(cp) == 0)
                return;

            DependencyObject c = VisualTreeHelper.GetChild(cp, 0);

            string title = c.GetValue(JournalEntry.NameProperty) as string;

            if (title != null)
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

                object journalScope = typeof(NavigationService).GetProperty("JournalScope", flags).GetValue(window.NavigationService);
                object journal = journalScope.GetType().GetField("_journal", flags).GetValue(journalScope);

                Type journalType = journal.GetType();

                List<JournalEntry> journalEntryList = (List<JournalEntry>)journalType.GetField("_journalEntryList", flags).GetValue(journal);

                // store the index of the current content
                int currentIndex = (int)journalType.GetField("_currentEntryIndex", flags).GetValue(journal);

                // assign null to the applyTitle lambda expression (allows removing the eventhandler from within)
                NavigatedEventHandler applyTitle = null;

                applyTitle = (sender1, e1) =>
                {
                    // use the index of the old content
                    journalEntryList[currentIndex].Name = title;

                    // remove the eventhandler to avoid memory leaks
                    window.NavigationService.Navigated -= applyTitle;
                };

                // add the eventhandler
                window.NavigationService.Navigated += applyTitle;
            }
        }

        /// <summary>
        /// Gets the UseJournalEntryName property value.
        /// </summary>
        /// <param name="navigationWindow">The NavigationWindow.</param>
        /// <returns>True when the JournalEntry's name should be retrieved from a DataTemplate (JournalEntry.NameProperty)</returns>
        public static bool UseJournalEntryName(this NavigationWindow navigationWindow)
        {
            return (bool)navigationWindow.GetValue(UseJournalEntryNameProperty);
        }

        /// <summary>
        /// Sets the UseJournalEntryName property value.
        /// </summary>
        /// <param name="navigationWindow">The NavigationWindow.</param>
        /// <param name="value">The value.</param>
        public static void UseJournalEntryName(this NavigationWindow navigationWindow, bool value)
        {
            navigationWindow.SetValue(UseJournalEntryNameProperty, value);
        }    
    }
}