namespace DataAccess.Interfaces
{
    public interface IGithubRepository
    {
        T Get<T>(string url);
    }
}
