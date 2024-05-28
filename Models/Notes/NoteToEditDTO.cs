namespace Gladwyne.Models
{
    public partial class NoteToEditDTO
    {
        public int NoteId { get; set; }
        public int OrgId { get; set; }
        public string NoteTitle { get; set; } = "";
        public string NoteContent { get; set; } = "";

    }
}