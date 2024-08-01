﻿namespace Web.Startup
{
    public static class ConfigRegistration
    {
        public static ConfigurationManager SetupAppConfig(this ConfigurationManager manager)
        {
            manager.AddEnvironmentVariables();
            IEnumerable<KeyValuePair<string, string?>> enumerable = manager.GetSection("Web").AsEnumerable();
            if (!enumerable.Any())
            {
                throw new Exception("No \"Web\" section detected in appsettings.json.");
            }

            foreach (KeyValuePair<string, string> item in enumerable)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value.Contains("ENV_"))
                {
                    string name = item.Value.Replace("ENV_", "");
                    manager[item.Key] = Env.Get(name);
                }
            }

            return manager;
        }
    }
}
