using ToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data
{
    //contexto de dados => representação do banco em memória (mapa do banco de dados)
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        //representa a tabela no banco de dados
        public DbSet<Todo> Todos { get; set; }
    }
}
