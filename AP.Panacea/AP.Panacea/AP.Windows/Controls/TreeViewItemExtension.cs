using System;
using System.Windows.Controls;
using AP.Linq;
using AP.Collections;

namespace AP.Windows.Controls
{
    public static class TreeViewItemExtension
    {
        /// <summary>
        /// Expands the whole sub-tree of the given node.
        /// </summary>
        /// <param name="item">The TreeViewItem.</param>
        public static void ExpandAll(this TreeViewItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (!item.HasItems)
                return;

            item.IsExpanded = true;
            var items = item.Items;
            
            for (int i = 0, c = items.Count; i < c; ++i)
            {
                TreeViewItem current = items[i] as TreeViewItem;

                if (current != null)
                    current.ExpandAll();                
            }
        }

        /// <summary>
        /// Expands all parent nodes until the node is visible.
        /// </summary>
        /// <param name="item">The TreeViewItem.</param>
        public static void ExpandUntilVisible(this TreeViewItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            // if it's already visible, there's no need to continue.
            if (item.IsVisible)
                return;

            TreeViewItem parent = item.Parent as TreeViewItem;

            if (parent != null)
            {
                parent.Items.MoveCurrentTo(item);
                parent.ExpandUntilVisible();
            }

            item.IsSelected = true;
            item.IsExpanded = true;                        
        }
    }
}
