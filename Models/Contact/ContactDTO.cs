
namespace Gladwyne.Models
{
    public partial class ContactDTO
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public int OrgId { get; set; }
    }
}