using System.ComponentModel.DataAnnotations;

namespace FinTracker.Core.Requests.Account
{
    public class RegisterRequest : Request
    {
        [Required(ErrorMessage = "E-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Senha é obrigatória!")]
        public string Password { get; set; } = string.Empty;
    }
}
