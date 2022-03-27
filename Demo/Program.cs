// See https://aka.ms/new-console-template for more information
using Repository;

ToDoRepository _repo = new ToDoRepository();

//insert todo items
var todo = new ToDo()
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
List<ToDo> todos = _repo.GetAllToDo();
foreach(var t in todos)
{
    Console.WriteLine($"Id:{t.Id}, Title:{t.Title}, Description:{t.Description}, LastModified:{t.LastModified,0:g}, DueDate:{t.DueDate,0:g}.");
}

//delete the last todo item
var todoToDelete = todos.OrderByDescending(t => t.Id).FirstOrDefault();
if (todoToDelete != null)
{
    bool result = _repo.DeleteToDo(todoToDelete.Id);
    Console.WriteLine($"Deleted ToDo record with Id:{todoToDelete.Id}, Result:{result}.");

    if (result)
    {
        //see if you can still find the todo item in the database
        var todo2 =_repo.GetToDo(todoToDelete.Id);
        if(todo2 != null)
        {
            Console.WriteLine($"Found ToDo record with Id:{todoToDelete.Id}.");
        }
        else
        {
            Console.WriteLine($"Found no ToDo record with Id:{todoToDelete.Id}.");
        }
    }
}

//add a week to the due date of the 1st todo item
var todo3 = _repo.GetToDo(1);
if (todo3 != null)
{
    Console.WriteLine($"Found ToDo record with Id:1, Id from object:{todo3.Id}, Title:{todo3.Title}.");
    todo3.DueDate.AddDays(7);
    _repo.UpdateToDo(todo3);
}
else
{
    Console.WriteLine($"Found no ToDo record with Id:1.");
}

//list all todo items again
todos = _repo.GetAllToDo();
foreach (var t in todos)
{
    Console.WriteLine($"Id:{t.Id}, Title:{t.Title}, Description:{t.Description}, LastModified:{t.LastModified,0:g}, DueDate:{t.DueDate,0:g}.");
}

_repo.Dispose();