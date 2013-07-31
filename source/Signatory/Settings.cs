using System.Configuration;

namespace Signatory
{
    public static class Settings
    {
        public static string Authority = ConfigurationManager.AppSettings["Authority"];
        public static string GitHubKey = ConfigurationManager.AppSettings["GitHubKey"];
        public static string GitHubSecret = ConfigurationManager.AppSettings["GitHubSecret"];
    }
}