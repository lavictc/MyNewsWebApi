namespace MyNewsWebApi.Handlers;

public interface IHttpClientHandler<T1, T2>
{
    Task<T1> GetEntityById(int id);
    Task<IEnumerable<T2>?> GetIds();
}