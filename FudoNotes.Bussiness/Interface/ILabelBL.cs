using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace BussinessLayer.Interface
{
    public interface ILabelBL
    {
        public LabelEntity CreateLabel(int noteId, int userId, string name);

        public bool DeleteLabel(int labelId, int userId);

        public LabelEntity RetrieveLabel(int labelId, int userId);

        public LabelEntity EditLabel(int labelId, string labelName, int userId);
    }
}
