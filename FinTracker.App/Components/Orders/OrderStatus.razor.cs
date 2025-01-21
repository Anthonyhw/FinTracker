using FinTracker.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace FinTracker.App.Components.Orders
{
    public partial class OrderStatusComponent :ComponentBase
    {
        #region Parameters
        [Parameter, EditorRequired]
        public EOrderStatus Status { get; set; }
        #endregion
    }
}
