namespace Gladwyne.Models.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public List<String> Errors { get; set; }
        public ErrorResponse(string errorMessage)
        {
            Errors = new List<string>();
            Errors.Add(errorMessage);
            this.IsSuccessful = false;
        }

        public ErrorResponse(IEnumerable<String> errorMessage)
        {
            Errors = new List<string>();
            Errors.AddRange(errorMessage);
            this.IsSuccessful = false;
        }
    }
}