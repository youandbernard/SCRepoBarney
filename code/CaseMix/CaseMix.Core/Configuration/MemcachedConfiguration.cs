namespace CaseMix.Configuration
{
    public class MemcachedConfiguration
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string MenuCacheKey { get; set; }
        public int CacheLifetimeSeconds { get; set; }
    }
}
