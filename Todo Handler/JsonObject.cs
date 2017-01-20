using System.Collections.Generic;

namespace Todo_Handler
{
    public class JsonObject
    {
        public List<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}
