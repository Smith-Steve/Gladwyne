using Gladwyne.API.Data;
using Gladwyne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gladwyne.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public NotesController(IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
        }

        [HttpGet("OrgNotesAll/Organization/{orgId}")]
        public IEnumerable<Note> GetAllNotesFromOrg(int orgId)
        {
            string sqlGetAllNotesFromOrg = $"SELECT NoteId, UserId, NoteTitle, NoteContent, NoteCreated, NoteUpdated FROM GladwyneSchema.Notes WHERE OrgId = {orgId}";
            IEnumerable<Note> noteResult = _dapper.LoadData<Note>(sqlGetAllNotesFromOrg);
            return noteResult;
        }
        [HttpGet("GetSingleNote/Note/{noteId}")]
        public Note GetSingleNote(int noteId)
        {
            string sqlGetSingleNote = $"SELECT UserId, OrgId, NoteTitle, NoteContent, NoteCreated, NoteUpdated FROM GladwyneSchema.Notes WHERE NoteId = {noteId}";
            Note returnedNote = _dapper.LoadDataSingle<Note>(sqlGetSingleNote);
            return returnedNote;
        }
        [HttpGet("OrgNotes/Organization/{orgId}/User/{userId}")]
        public IEnumerable<Note> GetAllNotessByUserInOrg(int orgId, int userId)
        {
            string sqlGetSingleNote = $"SELECT UserId, OrgId, NoteTitle, NoteContent, NoteCreated, NoteUpdated FROM GladwyneSchema.Notes WHERE OrgId = {orgId} AND UserId = {userId}";
            IEnumerable<Note> noteResult = _dapper.LoadData<Note>(sqlGetSingleNote);
            return noteResult;
        }

        [HttpPost("AddNote")]
        public IActionResult AddNote(NoteToAddDTO note)
        {
            string userId = this.User.FindFirst("userId")?.Value;
            string sqlAddNote = $"INSERT INTO GladwyneSchema.Notes([UserId],[OrgId],[NoteTitle],[NoteContent], [NoteCreated], [NoteUpdated]) VALUES ({userId}, {note.OrgId}, '{note.NoteTitle}', '{note.NoteContent}', GETDATE(), GETDATE())";
            if(_dapper.ExecuteSql(sqlAddNote))
            {
                return Ok();
            }
            throw new Exception("Failed To Create New Post");
        }
        [HttpPut("EditNote")]
        public IActionResult EditNote(NoteToEditDTO noteToEdit)
        {
            string userId = this.User.FindFirst("userId")?.Value;
            string sqlUpdateNote = $"Update GladwyneSchema.Notes SET NoteTitle = '{noteToEdit.NoteTitle}', NoteContent = '{noteToEdit.NoteContent}', NoteUpdated = GETDATE() WHERE NoteId = {noteToEdit.NoteId} AND UserId = {userId}";
            if(_dapper.ExecuteSql(sqlUpdateNote))
            {
                return Ok();
            }
            throw new Exception("Failed To Edit Post");
        }

        [HttpDelete("DeleteNote/{noteId}")]
        public IActionResult DeleteNote(int noteId)
        {
            string sqlDelete = $"DELETE FROM GladwyneSchema.Notes WHERE NoteId = {noteId}";
            if(_dapper.ExecuteSql(sqlDelete))
            {
                return Ok();
            }
            throw new Exception("Failed To Delete Note");
        }
    }
}