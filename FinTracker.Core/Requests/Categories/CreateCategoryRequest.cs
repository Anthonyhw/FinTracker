﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Core.Requests.Categories
{
    public class CreateCategoryRequest : Request
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(80, ErrorMessage = "O título deve conter no máximo 80 caracteres!")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = null!;
    }
}
