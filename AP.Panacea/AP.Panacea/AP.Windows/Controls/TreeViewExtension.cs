using System;
using System.Windows.Controls;
using AP.Linq;
using AP.Collections;

namespace AP.Windows.Controls
{
    public static class TreeViewExtension
    {
        /// <summary>
        /// Expands all nodes.
        /// </summary>
        /// <param name="treeView">The TreeView.</param>
        public static void ExpandAll(this TreeView treeView)
        {
            if (treeView == null)
                throw new ArgumentNullException("treeView");

            if (!treeView.HasItems)
                return;

            var items = treeView.Items;

            for (int i = 0, c = items.Count; i < c; ++i)
            {
                TreeViewItem current = items[i] as TreeViewItem;

                if (current != null)
                    current.ExpandAll();
            }
        }
    } 
}
