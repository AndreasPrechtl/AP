using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

using AP;
using AP.Collections.Specialized;
using AP.Linq;
using AP.Collections;

namespace AP.Web.UI
{
    public static class PageMerger
    {
        public const string DefaultTitleSeparator = ", ";
        public const string DefaultTagSeparator = ",";
        public const string DefaultDescriptionSeparator = ". ";

        /// <summary>
        /// Merge method that should be used from Page level
        /// </summary>
        /// <param name="page"></param>
        public static void Merge(System.Web.UI.Page page, string titleSeparator = DefaultTitleSeparator, string descriptionSeparator = DefaultDescriptionSeparator, string tagSeparator = DefaultTagSeparator)
        {
            StringBuilder title = new StringBuilder();
            StringBuilder description = new StringBuilder();
            StringSet tags = new StringSet();

            IPage mergeablePage = page as IPage;

            bool isMergeablePage = mergeablePage != null;

            bool mergeTitle = isMergeablePage ? (mergeablePage.MergeSettings & AP.UI.MergeSettings.Title) == AP.UI.MergeSettings.Title : page.Title.IsNullOrWhiteSpace();
            bool mergeDescription = isMergeablePage ? (mergeablePage.MergeSettings & AP.UI.MergeSettings.Description) == AP.UI.MergeSettings.Description : page.MetaDescription.IsNullOrWhiteSpace();
            bool mergeTags = isMergeablePage ? (mergeablePage.MergeSettings & AP.UI.MergeSettings.Tags) == AP.UI.MergeSettings.Tags : page.MetaKeywords.IsNullOrWhiteSpace();

            System.Web.UI.MasterPage m = page.Master;

            RobotSettings rs = RobotSettings.Index | RobotSettings.Follow | RobotSettings.NoArchive;
            bool robotSettingsFound = false;

            while (m != null)
            {
                IPage mp = m as IPage;
                if (mp != null)
                {
                    if ((mp.MergeSettings & AP.UI.MergeSettings.Title) == AP.UI.MergeSettings.Title && mergeTitle && !mp.MetaData.Title.IsNullOrWhiteSpace())
                    {
                        title.Insert(0, titleSeparator);
                        title.Insert(0, mp.MetaData.Title);
                    }
                    if ((mp.MergeSettings & AP.UI.MergeSettings.Description) == AP.UI.MergeSettings.Description && mergeDescription && !mp.MetaData.Description.IsNullOrWhiteSpace())
                    {
                        description.Insert(0, descriptionSeparator);
                        description.Insert(0, mp.MetaData.Description);
                    }
                    if ((mp.MergeSettings & AP.UI.MergeSettings.Tags) == AP.UI.MergeSettings.Tags && mergeTags && !mp.MetaData.Tags.IsEmpty())
                    {
                        tags.UnionWith(mp.MetaData.Tags);
                    }

                    if (!robotSettingsFound)
                    {
                        // only need the robots tag from the next masterpage

                        RobotSettings rsMaster = mp.Robots;

                        if (robotSettingsFound = (rsMaster != 0))
                            rs = mp.Robots;                        
                    }
                }
                m = m.Master;
            }

            // remove the last separators
            if (title.EndsWith(titleSeparator))
                title.Remove(title.Length - titleSeparator.Length, titleSeparator.Length);

            if (description.EndsWith(descriptionSeparator))
                description.Remove(description.Length - descriptionSeparator.Length, descriptionSeparator.Length);
            
            if (isMergeablePage)
            {
                if (mergeTitle)
                    mergeablePage.MetaData.Title = title.Append(mergeablePage.MetaData.Title).ToString();

                if (mergeDescription)
                    mergeablePage.MetaData.Description = (description.Length > 0 ? description.Append(descriptionSeparator) : description).Append(mergeablePage.MetaData.Description).ToString();

                if (mergeTags)
                    mergeablePage.MetaData.Tags.UnionWith(tags);     

                RobotSettings rsPage = mergeablePage.Robots;

                // use a default fallback
                if (rsPage != 0)
                    rs = rsPage;
            }
            
            // should be unreachable anyway.
            //else
            //{
            //    if (mergeTitle)
            //        page.Title = title.ToString();

            //    if (mergeDescription)
            //        page.MetaDescription = description.ToString();

            //    if (mergeTags)
            //        page.MetaKeywords = new StringSet(tags.ToString()).ToString();
            //}

            if ((rs & RobotSettings.Follow) == RobotSettings.Follow && (rs & RobotSettings.NoFollow) == RobotSettings.NoFollow)
                throw new InvalidOperationException("RobotSettings");

            if ((rs & RobotSettings.Index) == RobotSettings.Index && (rs & RobotSettings.NoIndex) == RobotSettings.NoIndex)
                throw new InvalidOperationException("RobotSettings");
                                    
            HtmlHead header = page.Header;
          
            header.Controls.Add(new HtmlMeta { Name = "description", Content = description.ToString() });
            header.Controls.Add(new HtmlMeta { Name = "keywords", Content = tags.ToString(tagSeparator) });
            header.Controls.Add(new HtmlMeta { Name = "robots", Content = rs.ToString().Replace("|", ",") });

            page.Title = title.ToString();
        }

        /// <summary>
        /// Merge method that should be used from MasterPage level
        /// (tests if the master qualifies for the merge operation)
        /// </summary>
        /// <param name="master"></param>
        public static void Merge(System.Web.UI.MasterPage master, string titleSeparator = DefaultTitleSeparator, string summarySeparator = DefaultDescriptionSeparator, string tagSeparator = DefaultTagSeparator)            
        {
            if (!(master is IPage))
                return;

            // let the page handle the merging if possible
            if (master.Page is IPage)
                return;

            System.Web.UI.MasterPage m = master.Master;
            IPage mp = (IPage)master;

            while (m != null)
            {
                if (m is IPage)
                    mp = (IPage)m;

                m = m.Master;
            }

            // is it the top mergeable master?
            if (mp != master)
                return;

            Merge(master.Page);
        }
    }
}
