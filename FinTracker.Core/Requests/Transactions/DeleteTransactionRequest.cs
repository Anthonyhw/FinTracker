using FinTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Core.Requests.Transactions
{
    public class DeleteTransactionRequest : Request
    {
        [Required(ErrorMessage = "Id é obrigatório.")]
        public long Id { get; set; }
    }
}
