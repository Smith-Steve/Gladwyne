
namespace Gladwyne.Models
{
    public partial class OrganizationAddress
    {
        public int OrgId { get; set; }
        public string OrgCountry { get; set; } = "";
        public string OrgStreetAddress { get; set; } = "";
        public string OrgStreetAddress2 { get; set; } = "";
        public string OrgCity { get; set; } = "";
        public string OrgState { get; set; } = "";
        public string OrgZip { get; set; } = "";
    }
}