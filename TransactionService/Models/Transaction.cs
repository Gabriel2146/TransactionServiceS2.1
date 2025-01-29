namespace TransactionService.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }  // Ejemplo: Pending, Completed, Failed
    }
}
