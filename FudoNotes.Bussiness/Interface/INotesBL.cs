using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace BussinessLayer.Interface
{
    public interface INotesBL
    {
        public NotesEntity CreateNotes(NotesModel model, int userId);

        public List<NotesEntity> GetAllNotes(int userId);

        public NotesEntity GetNotes(int userId, int noteId);
        public NotesEntity UpdateNotes(int userId, int noteId, NotesModel notesModel);

        public bool DeleteNote(int userId, int noteId);

        bool ToggleTrash(int userId, int noteId);
        bool ToggleArchive(int userId, int noteId);
    }
}
