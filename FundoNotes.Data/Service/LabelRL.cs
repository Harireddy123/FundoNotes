using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class LabelRL:ILabelRL
    {
        private readonly FundoContext _dbContext;

        public LabelRL(FundoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LabelEntity CreateLabel(int noteId, int userId, string name)
        {
            try
            {
                // Check if the note exists and belongs to the user
                var note = _dbContext.Notestable.FirstOrDefault(n => n.NotesId == noteId && n.Id == userId);
                if (note == null)
                {
                    return null;
                }
                var existingLabel = _dbContext.Labeltable
                    .FirstOrDefault(l => l.NoteId == noteId && l.UserId == userId && l.LabelName == name);

                if (existingLabel != null)
                {
                    return existingLabel;
                }
                LabelEntity label = new LabelEntity()
                {
                    NoteId = noteId,
                    UserId = userId,
                    LabelName = name
                };
                _dbContext.Labeltable.Add(label);
                _dbContext.SaveChanges();
                return label;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelEntity RetrieveLabel(int labelId, int userId)
        {
            try
            {
                var result = _dbContext.Labeltable.FirstOrDefault(x => x.LabelId == labelId && x.UserId == userId);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeleteLabel(int labelId, int userId)
        {
            try
            {
                var result = _dbContext.Labeltable.FirstOrDefault(x => x.LabelId == labelId && x.UserId == userId);

                _dbContext.Labeltable.Remove(result);

                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public LabelEntity EditLabel(int labelId, string labelName, int userId)
        {
            try
            {
                var labelEntity = _dbContext.Labeltable.FirstOrDefault(e => e.LabelId == labelId && e.UserId == userId);

                if (labelEntity != null)
                {
                    labelEntity.LabelName = labelName;

                    _dbContext.SaveChanges();
                    return labelEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
