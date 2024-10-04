namespace Bifrost.Launcher
{
    public class Server
    {
        public string Name { get; set; } = "Project TAHITI";
        public string SiteConfigUrl { get; set; } = "mhtahiti.com/SiteConfig.xml";

        public Server(string name, string siteConfigUrl)
        {
            Name = name;
            SiteConfigUrl = siteConfigUrl;
        }

        public Server() { }
    }
}
