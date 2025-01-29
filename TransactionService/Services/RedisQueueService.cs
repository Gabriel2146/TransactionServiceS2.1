using StackExchange.Redis;
using System.Threading.Tasks;

public class RedisQueueService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    // Constructor: Inyectamos la conexión a Redis
    public RedisQueueService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    // Método para encolar una transacción
    public async Task EnqueueTransactionAsync(string queueName, string transactionData)
    {
        // Encolar la transacción en Redis
        await _database.ListRightPushAsync(queueName, transactionData);
    }

    // Método para desencolar una transacción
    public async Task<string> DequeueTransactionAsync(string queueName)
    {
        // Obtener la transacción más antigua de la cola (y eliminarla de la misma)
        return await _database.ListLeftPopAsync(queueName);
    }

    // Método para obtener el tamaño de la cola (cuántos elementos tiene)
    public async Task<long> GetQueueLengthAsync(string queueName)
    {
        // Retorna el tamaño de la cola
        return await _database.ListLengthAsync(queueName);
    }
}
