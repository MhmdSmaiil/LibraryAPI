using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public Book()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
        }
    }
}
