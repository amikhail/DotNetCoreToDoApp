using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Repository
{
    public class ToDoRepository : IDisposable
    {
        private const string ToDoTable =
            @"CREATE TABLE [ToDo](
                    Id INTEGER PRIMARY KEY,
                    Title VARCHAR(200) NULL,
                    Description VARCHAR(2000) NULL,
                    LastModified DATETIME NULL,
                    DueDate DATETIME NULL
                  );";

        private SqliteConnection Connection { get; set; }

        public ToDoRepository() {
            var conn = new SqliteConnection("Data Source=:memory:");
            Connection = conn;
            conn.Open();
            (new SqliteCommand(ToDoTable, conn)).ExecuteNonQuery();
        }

        public ToDo AddToDo(ToDo todo)
        {
            var command = Connection.CreateCommand();
            command.CommandText = @"INSERT INTO ToDo
                (Title, Description, LastModified, DueDate)
                VALUES(@title, @description, DateTime('now'), @dueDate);
                SELECT last_insert_rowid();";
            AddParameter(command, "@title", todo.Title);
            AddParameter(command, "@description", todo.Description);
            AddParameter(command, "@dueDate", todo.DueDate);
            long toDoId = (long)command.ExecuteScalar();
            todo.Id = (int)toDoId;

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

            var command = Connection.CreateCommand();
            command.CommandText = @"UPDATE ToDo
                SET Title = @title,
                Description = @description,
                LastModified = DateTime('now'),
                DueDate = @dueDate
                WHERE Id = @id";
            AddParameter(command, "@title", todo.Title);
            AddParameter(command, "@description", todo.Description);
            AddParameter(command, "@dueDate", todo.DueDate);
            AddParameter(command, "@id", todo.Id);
            command.ExecuteNonQuery();

            return true;
        }

        public bool DeleteToDo(int? id)
        {
            if (id == null || id <= 0)
            {
                return false;
            }

            var command = Connection.CreateCommand();
            command.CommandText = @"DELETE FROM ToDo
                WHERE Id=@id";
            AddParameter(command, "@id", id);
            command.ExecuteNonQuery();

            return true;
        }

        public List<ToDo> GetAllToDo()
        {
            var todos = new List<ToDo>();

            var command = Connection.CreateCommand();
            command.CommandText = @"SELECT 
                Id, Title, Description, LastModified, DueDate
                FROM ToDo
                ORDER BY Id ASC";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var todo = new ToDo()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    LastModified = reader.GetDateTime(3),
                    DueDate = reader.GetDateTime(4)
                };
                todos.Add(todo);                 
            }

            return todos;
        }

        public ToDo? GetToDo(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            var command = Connection.CreateCommand();
            command.CommandText = @"SELECT 
                Id, Title, Description, LastModified, DueDate
                FROM ToDo
                WHERE Id = @id
                ORDER BY Id DESC
                LIMIT 1";
            AddParameter(command, "@id", id);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var todo = new ToDo()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    LastModified = reader.GetDateTime(3),
                    DueDate = reader.GetDateTime(4)
                };

                return todo;
            }

            return null;
        }

        private void AddParameter(DbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            if (value == null)
                throw new ArgumentNullException("value");
            Type t = value.GetType();
            if (t == typeof(int))
                p.DbType = DbType.Int32;
            else if (t == typeof(string))
                p.DbType = DbType.String;
            else if (t == typeof(DateTime))
                p.DbType = DbType.DateTime;
            else
                throw new ArgumentException(
                $"Unrecognized type: {t.ToString()}", "value");
            p.Direction = ParameterDirection.Input;
            p.ParameterName = name;
            p.Value = value;
            cmd.Parameters.Add(p);
        }

        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();
        }
    }
}