using System;
using System.Collections.Generic;

namespace Application.ViewModels.Common
{
    public interface IViewModel
    {
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
        public int ErrorCode { get; set; }
    }
}