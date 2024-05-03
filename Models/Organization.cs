namespace Gladwyne.Models
{
    public class Organization
    {
        public int OrdId { get; set; }
        public string OrgName { get; set; }
        public string OrgDescription { get; set; }
        public string OrgCountry { get; set; } = "United States of America";
        public string OrgIndustry { get; set; }
        public string OrgWebsite { get; set; }
        public string OrgStreetAddress { get; set; }
        public string OrgStreetAddress2 { get; set; } = "";
        public string OrgCity { get; set; }
        public string OrgState { get; set; }
        public string OrgZip { get; set; }
        public DateTime OrgUpdateDate { get; set; }
        public DateTime OrgCreateDate { get; set; }
        public bool OrgActive { get; set; }
    }
}