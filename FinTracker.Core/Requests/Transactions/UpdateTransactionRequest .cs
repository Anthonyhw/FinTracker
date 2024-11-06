using FinTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Core.Requests.Transactions
{
    public class UpdateTransactionRequest : Request
    {
        [Required(ErrorMessage = "Id é obrigatório.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório.")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Tipo é obrigatório.")]
        public EtransactionType Type { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória.")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Data é obrigatória.")]
        public DateTime? PaidOrReceivedAt { get; set; }
    }
}
