namespace Web.Startup
{
    public static class Env
    {
        public static string? Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
