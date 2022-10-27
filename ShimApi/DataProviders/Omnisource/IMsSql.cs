namespace ShimApi.DataProviders
{
    public interface IMsSql
    {
        public (bool connected, string response) Version();
    }
}