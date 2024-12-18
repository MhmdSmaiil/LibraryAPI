using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Resources.Models
{
    public class RequestResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = String.Empty;
        public object? Result { get; set; }
    }
}
