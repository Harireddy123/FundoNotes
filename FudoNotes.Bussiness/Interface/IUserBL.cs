
using FundoNotes.Data.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace FudoNotes.Bussiness.Interface
{
    public interface IUserBL
    {
        public User Registeration(RegisterModel model);

        public string Login(LoginModel model);

        public bool EmailExists(string email);
        ForgetPasswordModel ForgetPassword(string email);
        public bool ResetPassword(string email, ResetPasswordModel model);

    }
}
