
using FudoNotes.Bussiness.Interface;
using FundoNotes.Data.Models;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;


namespace FudoNotes.Bussiness.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;
        public UserBL(IUserRL userRepository)
        {
            _userRepository = userRepository;
        }

        public User Registeration(RegisterModel model) => _userRepository.RegisterUser(model);

        public string Login(LoginModel model) => _userRepository.Login(model);

        public bool EmailExists(string email) => _userRepository.EmailExists(email);
        public ForgetPasswordModel ForgetPassword(string email) => _userRepository.ForgetPassword(email);

        public bool ResetPassword(string email, ResetPasswordModel model) => _userRepository.ResetPassword(email, model);


    }
}