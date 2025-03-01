using System;
using System.Collections.Generic;
using System.Text;
using FundoNotes.Data.Models;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public User RegisterUser(RegisterModel model);
        public string Login(LoginModel model);
        public bool EmailExists(string email);
    }
}
