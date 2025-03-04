using System;
using System.Collections.Generic;
using System.Text;
using BussinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Models;

namespace BussinessLayer.Service
{
    public class NotesBL:INotesBL
    {
        private readonly INotesRL _notesRepository;

        public NotesBL(INotesRL notesRepository)
        {
            this._notesRepository = notesRepository;
        }

        public NotesEntity CreateNotes(NotesModel model, int userId) =>

            _notesRepository.CreateNotes(model, userId);

        public List<NotesEntity> GetAllNotes(int userId) =>

            _notesRepository.GetAllNotes(userId);

        public NotesEntity GetNotes(int userId, int noteId) =>

            _notesRepository.GetNotes(userId, noteId);

        public NotesEntity UpdateNotes(int userId, int noteId, NotesModel notesModel) =>

            _notesRepository.UpdateNotes(userId, noteId, notesModel);

        public bool DeleteNote(int userId, int noteId) =>

            _notesRepository.DeleteNote(userId, noteId);


        public bool ToggleTrash(int userId, int noteId) =>

            _notesRepository.ToggleTrash(userId, noteId);


        public bool ToggleArchive(int userId, int noteId) =>
            _notesRepository.ToggleArchive(userId, noteId);
    }
}
