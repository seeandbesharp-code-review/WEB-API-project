using AutoMapper;
using DTOs;
using Entities;
using Repositories;

namespace Services
{
    public class UserServices : IUserService
    {
        private const int MinimumPasswordScore = 2;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;


        public UserServices(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
            _jwtService=jwtService;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _userRepository.GetUsers());
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            return _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
        }

        public async Task<AuthResultDTO> AddUser(PostUserDTO user)
        {
            var userEntity = _mapper.Map<PostUserDTO, User>(user);
            userEntity.Password = _passwordService.HashPassword(user.Password);

            var createdUser = await _userRepository.AddUser(userEntity);

            var userDto = _mapper.Map<User, UserDTO>(createdUser);

            var token = _jwtService.GenerateToken(createdUser);

            return new AuthResultDTO(userDto, token);
        }

        public async Task UpdateUser(int id, PostUserDTO user)
        {
            var userEntity = _mapper.Map<PostUserDTO, User>(user);
            userEntity.Password = _passwordService.HashPassword(user.Password);
            await _userRepository.UpdateUser(id, userEntity);
        }

        public async Task<AuthResultDTO> Login(LoginUserDTO loginUser)
        {
            var userEntity = await _userRepository.GetUserByEmail(loginUser.Email);

            if (userEntity == null || !_passwordService.VerifyPassword(loginUser.Password, userEntity.Password))
                return null;

            var userDto = _mapper.Map<User, UserDTO>(userEntity);

            var token = _jwtService.GenerateToken(userEntity);

            return new AuthResultDTO(userDto, token);
        }

        public async Task<bool> UserWithSameEmail(string email,int id=-1)
        {
            return await _userRepository.UserWithSameEmail(email,id);
        }

        public bool IsPasswordStrong(string password)
        {
            int passScore = _passwordService.GetPasswordScore(password);
            if (passScore < MinimumPasswordScore)
                return false;
            return true;
        }
    }
}
