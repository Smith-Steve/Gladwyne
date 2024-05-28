namespace Gladwyne.Models
{
    public partial class Note
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public int OrgId { get; set; }
        public string NoteTitle { get; set; } = "";
        public string NoteContent { get; set; } = "";
        public DateTime NoteCreated { get; set; }
        public DateTime NoteUpdated { get; set; }
    }
}