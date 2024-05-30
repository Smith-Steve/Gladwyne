using Gladwyne.Models.Interfaces;

namespace Gladwyne.Models
{
     public class ItemResponse<T> : SuccessResponse
    {
        public T Item { get; set; }
    }
}