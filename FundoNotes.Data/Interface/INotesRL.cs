using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public NotesEntity CreateNotes(NotesModel model, int userId);

        public List<NotesEntity> GetAllNotes(int userId);

        public NotesEntity GetNotes(int userId, int noteId);

        public NotesEntity UpdateNotes(int userId, int noteId, NotesModel notesModel);

        public bool DeleteNote(int userId, int noteId);

        public bool ToggleArchive(int userId, int noteId);

        public bool ToggleTrash(int userId, int noteId);
    }
}
