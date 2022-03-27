// See https://aka.ms/new-console-template for more information
using Repository;

//IToDoRepository _repo = new AdoNetSqliteToDoRepository();

bool result = false;
IToDoRepository _repo;
ToDo todo;
ToDo? todoToDelete;
List<ToDo> todos;

using (var dbContext = new EfCoreDbContext())
{
    using (_repo = new EfCoreSqliteToDoRepository(dbContext))
    {
        //insert todo items
        todo = new ToDo()
        {
            Title = "Go Grocery Shopping",
            Description = "milk,\r\ncoffee,\r\ncreamer,\r\neggs,\r\nbread,\r\n",
            DueDate = new DateTime(2022, 03, 27, 16, 0, 0)
        };
        _repo.AddToDo(todo);

        todo = new ToDo()
        {
            Title = "Doctor Appointment",
            Description = "Doctor Appointment",
            DueDate = new DateTime(2022, 03, 30, 11, 30, 0)
        };
        _repo.AddToDo(todo);

        todo = new ToDo()
        {
            Title = "Christmas Party",
            Description = "Open presents and have brunch.",
            DueDate = new DateTime(2022, 12, 25, 10, 0, 0)
        };
        _repo.AddToDo(todo);

        //list all todo items
        todos = _repo.GetAllToDo();
        foreach (var t in todos)
        {
            Console.WriteLine($"Id:{t.Id}, Title:{t.Title}, Description:{t.Description}, LastModified:{t.LastModified,0:g}, DueDate:{t.DueDate,0:g}.");
        }

        //delete the last todo item
        todoToDelete = todos.OrderByDescending(t => t.Id).FirstOrDefault();
        if (todoToDelete != null)
        {
            result = _repo.DeleteToDo(todoToDelete.Id);
            Console.WriteLine($"Deleted ToDo record with Id:{todoToDelete.Id}, Result:{result}.");
        }
    }
}

using (var dbContext = new EfCoreDbContext())
{
    using (_repo = new EfCoreSqliteToDoRepository(dbContext))
    {
        if (result && todoToDelete != null)
        {
            //see if you can still find the todo item in the database
            var todo2 = _repo.GetToDo(todoToDelete.Id);
            if (todo2 != null)
            {
                Console.WriteLine($"Found ToDo record with Id:{todoToDelete.Id}.");
            }
            else
            {
                Console.WriteLine($"Found no ToDo record with Id:{todoToDelete.Id}.");
            }
        }
        else
        {
            bool isNull = false;
            if (todoToDelete == null)
            {
                isNull = true;
            }
            Console.Write($"Result:{result}, todoToDelete is null:{isNull}");
        }

        //add a week to the due date of the 1st todo item
        var todo3 = _repo.GetToDo(1);
        if (todo3 != null)
        {
            Console.WriteLine($"Found ToDo record with Id:1, Id from object:{todo3.Id}, Title:{todo3.Title}.");
            todo3.DueDate = todo3.DueDate.AddDays(7);
            result = _repo.UpdateToDo(todo3);
            Console.WriteLine($"Result of updating ToDo record with Id:{todo3.Id}, DueDate:{todo3.DueDate}, Result:{result}.");
            
        }
        else
        {
            Console.WriteLine($"Found no ToDo record with Id:1.");
        }
    }
}

using (var dbContext = new EfCoreDbContext())
{
    using (_repo = new EfCoreSqliteToDoRepository(dbContext))
    {

        //list all todo items again
        todos = _repo.GetAllToDo();
        foreach (var t in todos)
        {
            Console.WriteLine($"Id:{t.Id}, Title:{t.Title}, Description:{t.Description}, LastModified:{t.LastModified,0:g}, DueDate:{t.DueDate,0:g}.");
        }

    }
}