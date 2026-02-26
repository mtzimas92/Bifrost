using Bifrost.Core.News;

namespace Bifrost.Core.Models
{
    public class GuiConfig
    {
        public string ThemeOverride { get; set; } = string.Empty;
        public NewsFeedSourceCategories NewsCategoryFilter { get; set; } = NewsFeedSourceCategories.All;
        public string DefaultNewsFeedUrl { get; set; } = "https://mhtahiti.com/announcements/rss";

        public GuiConfig Clone()
        {
            return (GuiConfig)MemberwiseClone();
        }
    }
}
