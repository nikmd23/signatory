using System.Configuration;

namespace Signatory
{
    public static class Settings
    {
        public static readonly string Authority = ConfigurationManager.AppSettings["Authority"];
        public static readonly string GitHubKey = ConfigurationManager.AppSettings["GitHubKey"];
        public static readonly string GitHubSecret = ConfigurationManager.AppSettings["GitHubSecret"];
    }
}