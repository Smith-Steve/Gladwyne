namespace Gladwyne.Models
{
    public partial class Organization
    {
        public int OrdId { get; set; }
        public string OrgName { get; set; } = "";
        public string OrgDescription { get; set; } = "";
        public string OrgIndustry { get; set; } = "";
        public string OrgWebsite { get; set; } = "";
        public DateTime OrgUpdateDate { get; set; }
        public DateTime OrgCreateDate { get; set; }
        public bool OrgActive { get; set; }
    }
}