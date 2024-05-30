using Gladwyne.Models.Interfaces;
namespace Gladwyne.Models
{
    public class ItemsResponse<T> : SuccessResponse, IItemResponse
    {
        public IEnumerable<T> Items { get; set; }
        object IItemResponse.Item { get { return this.Items; } }
    }
}