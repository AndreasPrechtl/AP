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
    public static class FrameExtensions
    {
        private const string PART_ContentPresenterName = "PART_FrameCP";

        private const string CanInfuseContentPropertyName = "CanInfuseContent";
        public static readonly DependencyProperty CanInfuseContentProperty;
        
        private const string UseJournalEntryNamePropertyName = "UseJournalEntryName";
        public static readonly DependencyProperty UseJournalEntryNameProperty;
        
        static FrameExtensions()
        {
            Type frameType = typeof(Frame);

            CanInfuseContentProperty = DependencyProperty.Register
            (
                CanInfuseContentPropertyName, typeof(bool), frameType,
                new PropertyMetadata
                (
                    false,
                    new PropertyChangedCallback((frame, e) =>
                    {
                        if (e.NewValue != e.OldValue)
                        {
                            if ((bool)e.NewValue)
                                ((Frame)frame).Navigated += Navigated;
                            else
                                ((Frame)frame).Navigated -= Navigated;
                        }
                    })
                )
            );
            
            UseJournalEntryNameProperty = DependencyProperty.Register
            (
                UseJournalEntryNamePropertyName, typeof(bool), frameType,
                new PropertyMetadata
                (
                    false,
                    new PropertyChangedCallback((frame, e) =>
                    {
                        if (e.NewValue != e.OldValue)
                        {
                            if ((bool)e.NewValue)
                                ((Frame)frame).Navigating += GetTitle;
                            else
                                ((Frame)frame).Navigating -= GetTitle;
                        }
                    })
                )
            );
        }

        /// <summary>
        /// Gets the CanInfuseContent dependency property value.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool CanInfuseContent(this Frame frame)
        {
            return (bool)frame.GetValue(CanInfuseContentProperty);
        }

        /// <summary>
        /// Sets the CanInfuseContent dependency property value.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="value"></param>
        public static void CanInfuseContent(this Frame frame, bool value)
        {
            frame.SetValue(CanInfuseContentProperty, value);
        }
        public static void Infuse(this Frame frame, object content)
        {
            Infuse(frame, content, null);
        }
        public static void Infuse(this Frame frame, object content, object extraData)
        {
            InfuseInternal(frame, content, null, extraData);
        }
        public static void Infuse(this Frame frame, Uri source)
        {
            Infuse(frame, source, null);
        }
        public static void Infuse(this Frame frame, Uri source, object extraData)
        {
            InfuseInternal(frame, null, source, extraData);
        }

        private static void InfuseInternal(Frame frame, object content, Uri source, object extraData)
        {
            if (!(bool)frame.GetValue(CanInfuseContentProperty))
                throw new InvalidOperationException("Can't infuse content.");

            Frame f = new Frame { JournalOwnership = JournalOwnership.OwnsJournal, IsHitTestVisible = false };

            // add the notification for when loading is complete
            f.LoadCompleted += (s, e) =>
            {
                // update the presenting frame - this doesn't change the url or history!
                ContentPresenter cp = (ContentPresenter)frame.Template.FindName(PART_ContentPresenterName, frame);
                cp.Content = f.Content;
            };

            // navigate using the dummy frame
            if (content != null)
                f.Navigate(content, extraData);
            else
                f.Navigate(source, extraData);
        }

        private static void Navigated(object sender, NavigationEventArgs e)
        {
            Frame f = (Frame)e.Navigator;
            object last = f.Content;

            // revert the content to what it should be - doesn't matter if it already is what it is supposed to be
            ContentPresenter cp = (ContentPresenter)f.Template.FindName(PART_ContentPresenterName, f);

            if (cp.Content != last)
                cp.Content = last;
        }
        
        private static void GetTitle(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Refresh)
                return;

            Frame frame = (Frame)e.Navigator;
            object content = frame.Content;

            if (content == null || content is DependencyObject)
                return;

            // it's a normal object, retrieve the DataTemplate - uses the PART_FrameCP
            ContentPresenter cp = (ContentPresenter)frame.Template.FindName(PART_ContentPresenterName, frame);

            if (VisualTreeHelper.GetChildrenCount(cp) == 0)
                return;

            DependencyObject c = VisualTreeHelper.GetChild(cp, 0);

            string title = c.GetValue(JournalEntry.NameProperty) as string;

            if (title != null)
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

                object journalScope = typeof(NavigationService).GetProperty("JournalScope", flags).GetValue(frame.NavigationService);
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
                    frame.NavigationService.Navigated -= applyTitle;
                };

                // add the eventhandler
                frame.NavigationService.Navigated += applyTitle;
            }
        }

        /// <summary>
        /// Gets the UseJournalEntryName property value.
        /// </summary>
        /// <param name="frame">The Frame.</param>
        /// <returns>True when the JournalEntry's name should be retrieved from a DataTemplate (JournalEntry.NameProperty)</returns>
        public static bool UseJournalEntryName(this Frame frame)
        {
            return (bool)frame.GetValue(UseJournalEntryNameProperty);
        }

        /// <summary>
        /// Sets the UseJournalEntryName property value.
        /// </summary>
        /// <param name="frame">The Frame.</param>
        /// <param name="value">The value.</param>
        public static void UseJournalEntryName(this Frame frame, bool value)
        {
            frame.SetValue(UseJournalEntryNameProperty, value);
        }    
    }
}