namespace Gladwyne.Models
{
    public partial class NoteToAddDTO
    {
        public int OrgId { get; set; }
        public string NoteTitle { get; set; } = "";
        public string NoteContent { get; set; } = "";
    }
}