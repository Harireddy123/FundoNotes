using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity CreateLabel(int noteId, int userId, string name);

        public LabelEntity EditLabel(int labelId, string labelName, int userid);

        public bool DeleteLabel(int labelId, int userId);

        public LabelEntity RetrieveLabel(int labelId, int userId);
    }
}
