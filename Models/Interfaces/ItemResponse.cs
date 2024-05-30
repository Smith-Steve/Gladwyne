namespace Gladwyne.Models.Interfaces
{
    public interface IItemResponse
    {
        bool IsSuccessful { get; set;}
        object Item { get;}
    }
}