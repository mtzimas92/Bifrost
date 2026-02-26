using System.ServiceModel.Syndication;
using System.Xml;

namespace Bifrost.Core.News
{
    [Flags]
    public enum NewsFeedSourceCategories
    { 
        None    = 0,
        Default = 1 << 0,
        Server  = 1 << 1,
        All     = Default | Server,
    }

    public class NewsFeedSource
    {
        private readonly List<NewsFeedItem> _items = new();

        public NewsFeed Feed { get; }
        public string Url { get; }
        public string Name { get; }
        public NewsFeedSourceCategories Category { get; }

        public bool IsLoaded { get; private set; }

        public IReadOnlyList<NewsFeedItem> Items { get => _items; }

        internal NewsFeedSource(NewsFeed feed, string url, string name, NewsFeedSourceCategories category)
        {
            Feed = feed;
            Url = url;
            Name = name;
            Category = category;
        }

        public override string ToString()
        {
            return $"[{Category}] {Name} ({Url})";
        }

        public void Load(Action<NewsFeedSource> callback = null)
        {
            _items.Clear();

            try
            {
                XmlReader reader = XmlReader.Create(Url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();

                foreach (SyndicationItem feedItem in feed.Items)
                        {
                            string description = feedItem.Summary?.Text?.Trim() ?? string.Empty;

                            // Remove '**NOTICE:**' and all '§MENTION§...§END§' blocks
                            description = description.Replace("**NOTICE:**", string.Empty);
                            while (true)
                            {
                                int start = description.IndexOf("§MENTION§");
                                if (start == -1) break;
                                int end = description.IndexOf("§END§", start);
                                if (end == -1) break;
                                description = description.Remove(start, end - start + 6);
                            }

                            string url = feedItem.Links.Count > 0 ? feedItem.Links[0].Uri.AbsoluteUri : string.Empty;
                            DateTime timestamp = feedItem.PublishDate != default ? feedItem.PublishDate.LocalDateTime : feedItem.LastUpdatedTime.LocalDateTime;

                            // Only show description in the UI (title replaced by cleaned description)
                            NewsFeedItem item = new(this, description, url, timestamp);
                            _items.Add(item);
                        }

                IsLoaded = true;
            }
            catch
            {
                return;
            }
            finally
            {
                callback?.Invoke(this);
            }
        }
    }
}
