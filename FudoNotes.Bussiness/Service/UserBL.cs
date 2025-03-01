
using FudoNotes.Bussiness.Interface;
using FundoNotes.Data.Models;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;


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


    }
}