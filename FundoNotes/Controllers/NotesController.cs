using System;
using System.Collections.Generic;
using System.Linq;
using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace FundoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL _notesManager;
        private readonly FundoContext dBContext;
        private readonly ILogger _logger;


        public NotesController(INotesBL notesManager, FundoContext dBContext)
        {
            _notesManager = notesManager;
            _logger = LogManager.GetCurrentClassLogger();
            this.dBContext = dBContext;
        }

        [Authorize]
        [HttpPost("Create")]
        public IActionResult CreateNotes(NotesModel notesModel)
        {
            try
            {
                _logger.Info("Attempting to create new note");
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                var notesResult = _notesManager.CreateNotes(notesModel, userId.Value);
                if (notesResult != null)
                {
                    _logger.Info($"Note created successfully. NoteId: {notesResult.NotesId}");
                    return Ok(new { success = true, message = "Notes Creation Successful ", data = notesResult });
                }
                else
                {
                    _logger.Warn("Failed to create note for user {UserId}", userId);
                    return BadRequest(new { success = false, message = "Notes Creation Unsuccessful" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating note");
                return StatusCode(500, new { success = false, message = $"error occured:{ex.Message} " });
            }
        }

        [Authorize]
        [HttpGet("RetrieveAllNotes")]
        public IActionResult GetAllNotes()
        {
            try
            {
                _logger.Info("Attempting to retrieve all notes");
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }
                var listNotesResult = _notesManager.GetAllNotes(userId.Value);

                if (!listNotesResult.Any())
                {
                    _logger.Info("No notes found for user {UserId}", userId);
                    return NotFound(new { success = false, message = "No notes found for this user." });
                }

                _logger.Info("Retrieved {Count} notes for user {UserId}", listNotesResult.Count, userId);
                return Ok(new ResponseModel<List<NotesEntity>>
                {
                    Success = true,
                    Message = "All notes retrieved successfully",
                    Data = listNotesResult
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving notes");
                return StatusCode(500, new { success = false, message = $"Exception occurred: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("RetrieveNote")]
        public IActionResult RetrieveNotes(int noteId)
        {
            try
            {
                _logger.Info("Attempting to retrieve note {NoteId}", noteId);
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }
                var notesResult = _notesManager.GetNotes(userId.Value, noteId);

                if (notesResult == null)
                {
                    _logger.Warn("Note {NoteId} not found for user {UserId}", noteId, userId);
                    return NotFound(new { success = false, message = $"No note found with ID {noteId} for user {userId}." });
                }

                _logger.Info("Successfully retrieved note {NoteId}", noteId);
                return Ok(new ResponseModel<NotesEntity>
                {
                    Success = true,
                    Message = "Note retrieved successfully.",
                    Data = notesResult
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving note {NoteId}", noteId);
                return StatusCode(500, new { success = false, message = $"An error occurred while retrieving the note:{ex.Message}." });
            }
        }


        [Authorize]
        [HttpPut("UpdateNotes")]
        public IActionResult UpdateNotes(int noteId, NotesModel model)
        {
            try
            {
                _logger.Info("Attempting to update note {NoteId}", noteId);
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }
                var UpdatedNote = _notesManager.UpdateNotes(userId.Value, noteId, model);
                if (UpdatedNote == null)
                {
                    _logger.Warn("Note {NoteId} not found for user {UserId}", noteId, userId);
                    return NotFound(new
                    {
                        success = false,
                        message = $"No note found with ID {noteId} for user {userId}."
                    });
                }
                _logger.Info("Successfully updated note {NoteId}", noteId);
                return Ok(new ResponseModel<NotesEntity> { Success = true, Data = UpdatedNote, Message = $"Notes:{noteId} updated successfully for user {userId} " });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating note {NoteId}", noteId);
                return StatusCode(500, new { success = false, message = $"An error occurred while retrieving the note:{ex.Message}." });
            }
        }


        [Authorize]
        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(int noteId)
        {
            try
            {
                _logger.Info("Attempting to delete note {NoteId}", noteId);
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                var IsDeleted = _notesManager.DeleteNote(userId.Value, noteId);

                if (!IsDeleted)
                {
                    _logger.Warn("Note {NoteId} not found or not deleted for user {UserId}", noteId, userId);
                    return NotFound(new { success = true, message = "Notes not found or not deleted" });
                }
                _logger.Info("Successfully deleted note {NoteId}", noteId);
                return Ok(new { success = true, message = $"Deleted the Notes:{noteId} Successfully " });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while deleting note {NoteId}", noteId);
                return StatusCode(500, new { success = false, message = $"An error occurred while retrieving the note:{ex.Message}." });
            }
        }

        private int? GetUserId()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            return int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [Authorize]
        [HttpPatch("Archive")]
        public IActionResult ArchiveNotes(int noteId)
        {
            try
            {
                _logger.Info("Attempting to toggle archive status for note {NoteId}", noteId);
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                bool result = _notesManager.ToggleArchive(userId.Value, noteId);

                if (result)
                {
                    _logger.Info("Successfully updated archive status for note {NoteId}", noteId);
                    return Ok(new { success = true, message = "Archive status updated successfully." });
                }

                _logger.Warn("Note {NoteId} not found for user {UserId}", noteId, userId.Value);
                return NotFound(new { success = false, message = $"Note {noteId} not found for user {userId.Value}." });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating archive status for note {NoteId}", noteId);
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

        [Authorize]
        [HttpPatch("Trash")]
        public IActionResult TrashNotes(int noteId)
        {
            try
            {
                _logger.Info("Attempting to toggle trash status for note {NoteId}", noteId);
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                bool result = _notesManager.ToggleTrash(userId.Value, noteId);

                if (result)
                {
                    _logger.Info("Successfully updated trash status for note {NoteId}", noteId);
                    return Ok(new { success = true, message = "Trash status updated successfully." });
                }

                _logger.Warn("Note {NoteId} not found for user {UserId}", noteId, userId.Value);
                return NotFound(new { success = false, message = $"Note {noteId} not found for user {userId.Value}." });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating trash status for note {NoteId}", noteId);
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

    }
}
