using Microsoft.AspNetCore.Mvc;
using TransactionService.Models;
using TransactionService.Services;
using System.Threading.Tasks;

namespace TransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly RedisQueueService _redisQueueService;

        // Constructor: Inyectar tanto el servicio de transacciones como el servicio de Redis
        public TransactionController(TransactionService transactionService, RedisQueueService redisQueueService)
        {
            _transactionService = transactionService;
            _redisQueueService = redisQueueService;
        }

        // Endpoint para crear una nueva transacción
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            // Crear la transacción en la base de datos
            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);

            // Encolar la transacción para procesamiento posterior (por ejemplo, en un sistema de colas)
            await _redisQueueService.EnqueueTransactionAsync(createdTransaction);

            // Retornar el resultado de la creación de la transacción con una ubicación específica
            return CreatedAtAction(nameof(GetTransactionById), new { id = createdTransaction.Id }, createdTransaction);
        }

        // Endpoint para obtener todas las transacciones
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        // Endpoint para obtener una transacción por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
    }
}
