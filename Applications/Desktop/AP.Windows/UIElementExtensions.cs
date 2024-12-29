using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AP.Windows
{
    public static class UIElementExtensions
    {
        /// <summary>
        /// Gets a RoutedEvent.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The RoutedEvent.</returns>
        public static RoutedEvent GetEvent(this UIElement source, string eventName)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentException("eventName");

            // check if the type is directly in there, otherwise you need a second pass and a more general approach
            RoutedEvent routedEvent = null;
            Type sourceType = source.GetType();

            // EventManager might return null.
            RoutedEvent[] events = EventManager.GetRoutedEventsForOwner(sourceType);

            // if it's null, use the name to find it in the events array.
            if (events != null)
                routedEvent = FindEvent(events, eventName);

            // still not found, use the sourceType to narrow it down.
            if (routedEvent == null)
                routedEvent = FindEvent(EventManager.GetRoutedEvents(), eventName, sourceType);

            return routedEvent;
        }

        /// <summary>
        /// Finds an event in an array using the name.
        /// </summary>
        /// <param name="events">An array of RoutedEvents.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The RoutedEvent or null.</returns>
        private static RoutedEvent FindEvent(RoutedEvent[] events, string eventName)
        {
            foreach (RoutedEvent e in events)
                if (e.Name == eventName)
                    return e;

            return null;
        }

        /// <summary>
        /// Finds an event using the name using the source type.
        /// </summary>
        /// <param name="events">An array of RoutedEvents.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="source">The source type.</param>
        /// <returns>The RoutedEvent or null.</returns>
        private static RoutedEvent FindEvent(RoutedEvent[] events, string eventName, Type sourceType)
        {
            foreach (RoutedEvent e in events)
                if (e.OwnerType.IsAssignableFrom(sourceType) && e.Name == eventName)
                    return e;

            return null;
        }

        /// <summary>
        /// Raises a RoutedEvent.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventName">The name of the event to raise.</param>
        public static void RaiseEvent(this UIElement source, string eventName)
        {
            RoutedEvent routedEvent = GetEvent(source, eventName);

            if (routedEvent != null)
                source.RaiseEvent(new RoutedEventArgs(routedEvent, source));
        }
    }
}
