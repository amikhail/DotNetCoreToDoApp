using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ToDo
    {
        public ToDo()
        {
            Title = "";
            Description = "";
        }

        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime DueDate { get; set; }
    }
}
