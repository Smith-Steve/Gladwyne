using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Models.Responses;
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
        public ActionResult<ItemResponse<Note>> GetAllNotesFromOrg(int orgId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetAllNotesFromOrg = $"EXECUTE [GladwyneSchema].[NOTES_GETNOTESFROMORG_Procedure] @OrgId = {orgId}";
            try
            {
                IEnumerable<Note> notesFromOrg = _dapper.LoadData<Note>(sqlGetAllNotesFromOrg);
                if(notesFromOrg != null)
                {
                    response = new ItemsResponse<Note> {Items = notesFromOrg};
                }
                else
                {
                    responseCode = 404;
                    response = new ErrorResponse("Application Resource Not Found.");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpGet("GetSingleNote/Organization/{orgId}/Note/{noteId}")]
        public ActionResult<ItemResponse<Note>> GetSingleNoteFromOrg(int orgId, int noteId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetOneNoteProc = $"EXECUTE [GladwyneSchema].[NOTES_GETONENOTE_FROMORG_Procedure] @NoteId ={noteId}, @OrgId = {orgId}";
            try
            {
                Note note = _dapper.LoadDataSingle<Note>(sqlGetOneNoteProc);
                if(note != null)
                {
                    response = new ItemResponse<Note> {Item = note};
                }
                else
                {
                    throw new Exception("Organization Or Note Does Not Exist");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpGet("GetSingleNote/Note/{noteId}")]
        public ActionResult<ItemResponse<Note>> GetSingleNote(int noteId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetOneNoteProc = $"EXECUTE [GladwyneSchema].[NOTES_GETONENOTE_Procedure] @NoteId ={noteId}";
            try
            {
                Note note = _dapper.LoadDataSingle<Note>(sqlGetOneNoteProc);
                if(note != null)
                {
                    response = new ItemResponse<Note> {Item = note};
                }
                else
                {
                    throw new Exception("Organization Or Note Does Not Exist");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }
        [HttpGet("OrgNotes/Organization/{orgId}/User/{userId}")]
        public ActionResult<ItemResponse<Note>> GetAllNotesByUserInOrg(int orgId, int userId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string GetAllNotesByUserInOrgProc = $"EXECUTE [GladwyneSchema].[NOTES_GETALLNOTESBYUSERINORG_Procedure] @OrgId={orgId}, @UserId={userId}";
            try
            {
                IEnumerable<Note> notesByUserInOrg = _dapper.LoadData<Note>(GetAllNotesByUserInOrgProc);
                if(notesByUserInOrg != null)
                {
                    response = new ItemsResponse<Note> {Items = notesByUserInOrg};
                }
                else
                {
                    responseCode = 404;
                    response = new ErrorResponse("Application Resource Could Not Be Found.");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }



        [HttpPost("NotePost")]
        public ActionResult<ItemResponse<Note>> AddNote(Note note)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string userId = this.User.FindFirst("userId")?.Value;
            Console.WriteLine(userId);
            string sqlAddNote = $"EXECUTE [GladwyneSchema].[NOTES_INSERT_Procedure] @UserId={userId}, @OrgId={note.OrgId}, @NoteTitle='{note.NoteTitle}', @NoteContent='{note.NoteContent}'";
            try
            {
                _dapper.ExecuteSql(sqlAddNote);
                response = new SuccessResponse();
            }
            catch(Exception exception)
            {
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpPut("NoteEdit")]
        public IActionResult EditNote(NoteToEditDTO noteToEdit)
        {
            int statusCode = 200;
            BaseResponse response = null;
            ActionResult<ItemResponse<Note>> responseFromGet = null;

            string userId = this.User.FindFirst("userId")?.Value;
            string sqlUpdateNote = $"EXECUTE [GladwyneSchema].[NOTES_UPDATE_Procedure] @NoteId={noteToEdit.NoteId}, @UserId={userId}, @NoteTitle='{noteToEdit.NoteTitle}', @NoteContent='{noteToEdit.NoteContent}'";
            try 
            {
                responseFromGet = GetSingleNote(noteToEdit.NoteId);
                if(responseFromGet != null)
                {
                    _dapper.ExecuteSql(sqlUpdateNote);
                    response = new SuccessResponse();
                }
                else
                {
                    statusCode = 500;
                    response = new ErrorResponse("Resource Does Not Exist");
                }
            }
            catch (Exception exception)
            {
                statusCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(statusCode,response);
        }

        [HttpDelete("DeleteNote/{noteId}")]
        public IActionResult DeleteNote(int noteId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlDelete = $"EXECUTE [GladwyneSchema].[NOTES_DELETE_Procedure] @NoteId={noteId}";
            try
            {
                _dapper.ExecuteSql(sqlDelete);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse("Application Resource Not Found");
            }
            return StatusCode(responseCode, response);
        }
    }
}