namespace Gladwyne.Models
{
    public class SuccessResponse : BaseResponse
    {
        //This class sets "Is Successful" to true by default.
        //All Success Responses Will By Default Return True
        //For this Value.
        public SuccessResponse()
        {
            this.IsSuccessful = true;
        }
    }
}