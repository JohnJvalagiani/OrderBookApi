using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record ServiceResponse<TResult>
    {
        public bool IsSuccess { get; set; }
        public TResult Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
