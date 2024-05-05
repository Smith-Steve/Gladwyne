namespace Gladwyne.Models
{
    public partial class OrganizationDTO
    {
        public string OrgName { get; set; } = "";
        public string OrgDescription { get; set; } = "";
        public string OrgIndustry { get; set; } = "";
        public string OrgWebsite { get; set; } = "";
        public bool OrgActive { get; set; } = true;
    }
}