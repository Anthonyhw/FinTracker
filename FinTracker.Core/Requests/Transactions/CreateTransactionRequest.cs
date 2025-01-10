using FinTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Core.Requests.Transactions
{
    public class CreateTransactionRequest : Request
    {
        [Required(ErrorMessage = "Título é obrigatório.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório.")]
        public EtransactionType Type { get; set; } = EtransactionType.Withdraw;

        [Required(ErrorMessage = "Valor é obrigatório.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória.")]
        public long CategoryId { get; set; }

        public DateTime? PaidOrReceivedAt { get; set; }
    }
}
