using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Core.Requests.Stripe;
using FinTracker.Core.Responses;
using FinTracker.Core.Responses.Stripe;

namespace FinTracker.Core.Handlers
{
    public interface IStripeHandler
    {
        Task<Response<string?>> CreateSessionAsync(CreateSessionRequest request);
        Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request);
    }
}
