using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using BussinessLayer.Interface;
using NLog;

namespace FundoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly FundoContext dBContext;
        private readonly ILabelBL _label;
        private readonly ILogger _logger;


        public LabelController(FundoContext dBContext, ILabelBL label)
        {
            this.dBContext = dBContext;
            _label = label;
            _logger = LogManager.GetCurrentClassLogger();

        }

        private int? GetUserId()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            return int.TryParse(idClaim, out int userId) ? userId : (int?)null;
        }

        [Authorize]
        [HttpPost("CreateLabel")]
        public IActionResult AddLabel(int noteId, string labelName)
        {
            try
            {
                _logger.Info($"Creating new label. NoteId: {noteId}, LabelName: {labelName}");  // Changed from LogInformation
                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");  // Changed from LogWarning
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }
                var result = _label.CreateLabel(noteId, userId.Value, labelName);
                if (result == null)
                {
                    _logger.Warn($"Failed to create label for NoteId: {noteId}, UserId: {userId}");  // Changed from LogWarning
                    return BadRequest(new { success = false, message = "Label not added ", });
                }
                _logger.Info($"Label created successfully. LabelId: {result.LabelId}, NoteId: {noteId}");  // Changed from LogInformation
                return Ok(new ResponseModel<LabelEntity> { Success = true, Message = "Label added successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error creating label. NoteId: {noteId}, Error: {ex.Message}");  // Changed from LogError
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

        [Authorize]
        [HttpPut("editLabel")]
        public IActionResult EditLabel(int labelId, string labelName)
        {
            try
            {
                _logger.Info("Editing label. LabelId: {LabelId}, NewName: {LabelName}", labelId, labelName);

                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                var result = _label.EditLabel(labelId, labelName, userId.Value);
                if (result == null)
                {
                    _logger.Warn("Failed to edit label. LabelId: {LabelId}, UserId: {UserId}", labelId, userId);
                    return BadRequest(new { success = false, message = "Label not added ", });
                }

                _logger.Info("Label updated successfully. LabelId: {LabelId}", labelId);
                return Ok(new ResponseModel<LabelEntity> { Success = true, Message = "Label updated successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error editing label. LabelId: {LabelId}, Error: {Error}", labelId, ex.Message);
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

        [Authorize]
        [HttpDelete("DeleteLabel")]
        public IActionResult DeleteLabel(int labelId)
        {
            try
            {
                _logger.Info("Deleting label for NoteId: {NoteId}", labelId);

                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                var result = _label.DeleteLabel(labelId, userId.Value);
                if (!result)
                {
                    _logger.Warn("Failed to delete label. NoteId: {NoteId}, UserId: {UserId}", labelId, userId);
                    return BadRequest(new { success = false, message = "Label not added s", });
                }

                _logger.Info("Label deleted successfully. NoteId: {NoteId}", labelId);
                return Ok(new ResponseModel<bool> { Success = true, Message = "Label Deleted successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting label. NoteId: {NoteId}, Error: {Error}", labelId, ex.Message);
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

        [Authorize]
        [HttpGet("RetrieveLabel")]
        public IActionResult RetrieveLabel(int labelId)
        {
            try
            {
                _logger.Info("Retrieving label. LabelId: {LabelId}", labelId);

                int? userId = GetUserId();
                if (userId == null)
                {
                    _logger.Warn("Unauthorized access attempt - Invalid or missing user ID");
                    return Unauthorized(new { success = false, message = "Invalid or missing user ID." });
                }

                var result = _label.RetrieveLabel(labelId, userId.Value);
                if (result == null)
                {
                    _logger.Warn("Label not found. LabelId: {LabelId}, UserId: {UserId}", labelId, userId);
                    return BadRequest(new { success = false, message = "Label not added" });
                }

                _logger.Info("Label retrieved successfully. LabelId: {LabelId}", labelId);
                return Ok(new ResponseModel<LabelEntity> { Success = true, Message = "Label Retrieved successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving label. LabelId: {LabelId}, Error: {Error}", labelId, ex.Message);
                return StatusCode(500, new { success = false, message = $"An error occurred :{ex.Message}." });
            }
        }

    }
}
