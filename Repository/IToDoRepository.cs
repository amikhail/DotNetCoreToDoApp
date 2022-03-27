using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IToDoRepository: IDisposable
    {
        public ToDo AddToDo(ToDo todo);
        public bool UpdateToDo(ToDo todo);
        public bool DeleteToDo(int? id);
        public List<ToDo> GetAllToDo();
        public ToDo? GetToDo(int? id);
    }
}
