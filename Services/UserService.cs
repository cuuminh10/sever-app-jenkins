using AutoMapper;
using gmc_api.DTO.PP;
using gmc_api.Repositories;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.DTO.User;
using gmc_api.Base.Helpers;

namespace gmc_api.Services
{
    public interface IUserService : IServiceGMCBase<UserResponse, UserCreateRequest, User, User>
    {
        LoginResponse Login(LoginRequest model);
        List<RoleOfUser> roleInSytems(int userId);
    }

    public class UserService : ServiceBaseImpl<UserResponse, UserCreateRequest, User, User>, IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly IADUsersRepository _userRepository;
        //    private readonly IReponsitoriesFireBase<UserFireBase> _fireBaseRepository;
        private readonly IMapper _mapper;

        public UserService(IOptions<AppSettings> appSettings, IADUsersRepository userRepository,
            IMapper mapper) : base(userRepository, mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            //  _fireBaseRepository = fireBaseRepository;
            _mapper = mapper;
        }

        public LoginResponse Login(LoginRequest model)
        {
            // Mapping to Entity object
            User dbObject = _mapper.Map<User>(model);
            var user = _userRepository.GetUserByNameAndPass(dbObject.ADUserName, dbObject.ADPassword);

            if (user == null) return null;
            var token = Utils.generateJwtToken(user, _appSettings.Secret);

            var authenResponse = _mapper.Map<LoginResponse>(user);
            authenResponse.Token = token;
            return authenResponse;
        }

        public List<RoleOfUser> roleInSytems(int userId)
        {
            return _userRepository.roleInSytems(userId);
        }
    }
}
