using Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ClienteViewModel: IViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public int Idade { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string>();
        public int ErrorCode { get; set; }
    }
}