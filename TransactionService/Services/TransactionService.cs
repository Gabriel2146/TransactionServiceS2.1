using TransactionService.Models;
using TransactionService.Repositories;

namespace TransactionService.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // Crear una nueva transacción
        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.CreateTransactionAsync(transaction);
        }

        // Obtener todas las transacciones
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        // Obtener una transacción por su ID
        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }
    }
}
