using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FundoNotes.Data.Models;
using RepositoryLayer.Entity;

namespace FudoNotes.Bussiness.Interface
{
    public interface IUserBL
    {
        public User Registeration(RegisterModel model);

        public string Login(LoginModel model);

        public bool EmailExists(string email);

    }
}
