using System;
using System.Collections.Generic;
using System.Text;
using BussinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Service
{
    public class LabelBL:ILabelBL
    {
        private readonly ILabelRL _repository;

        public LabelBL(ILabelRL repository)
        {
            this._repository = repository;
        }
        public LabelEntity CreateLabel(int noteId, int userId, string name) =>
            _repository.CreateLabel(noteId, userId, name);

        public bool DeleteLabel(int labelId, int userId) =>
            _repository.DeleteLabel(labelId, userId);

        public LabelEntity EditLabel(int labelId, string labelName, int userId) =>
            _repository.EditLabel(labelId, labelName, userId);

        public LabelEntity RetrieveLabel(int labelId, int userId) =>
            _repository.RetrieveLabel(labelId, userId);
    }
}
