using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EfCoreSqliteToDoRepository: IToDoRepository
    {
        private readonly EfCoreDbContext db;   
        public EfCoreSqliteToDoRepository(EfCoreDbContext dbContext)
        {
            this.db = dbContext;
        }

        public ToDo AddToDo(ToDo todo)
        {
            db.ToDo.Add(todo);
            db.SaveChanges();

            return todo;
        }

        public bool UpdateToDo(ToDo todo)
        {
            if (todo == null)
            {
                return false;
            }

            if (todo.Id == null || todo.Id == 0)
            {
                return false;
            }

            var entry = db.Entry(todo);
            entry.State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public bool DeleteToDo(int? id)
        {
            if (id == null || id <= 0)
            {
                return false;
            }

            var todo = db.ToDo.Find(id);
            if(todo != null)
            {
                var entry = db.Entry(todo);
                entry.State = EntityState.Deleted;
                db.SaveChanges();
            }

            return true;
        }

        public List<ToDo> GetAllToDo()
        {
            var qTodo = from t in db.ToDo
                   orderby t.Id
                   select t;
            var todos = qTodo.ToList<ToDo>();
            return todos;
        }

        public ToDo? GetToDo(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }   

            return db.ToDo.FirstOrDefault(t => t.Id == id);
        }

        public void Dispose()
        {
        }
    }
}
