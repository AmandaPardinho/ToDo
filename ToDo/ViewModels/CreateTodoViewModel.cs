using System.ComponentModel.DataAnnotations;

namespace ToDo.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
