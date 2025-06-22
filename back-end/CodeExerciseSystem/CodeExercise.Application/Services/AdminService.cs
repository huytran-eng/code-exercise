using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace CodeExercise.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;


        public AdminService(IUserRepository userRepository, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ResponseResult<AdminLoginResponseDTO>> AdminLoginAsync(AdminLoginInDTO loginRequest)
        {
            try
            {
                var result = new ResponseResult<AdminLoginResponseDTO>();

                var user = await _userRepository.GetAdminByUsernameAsync(loginRequest.Username);

                if (user == null)
                {
                    result.Success = false;
                    result.Message = "User not found";
                    result.StatusCode = 404;
                    return result;
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                    {
                        result.Success = false;
                        result.Message = "Invalid password";
                        result.StatusCode = 401;
                        return result;
                    }
                }


                var loginResponse = new AdminLoginResponseDTO
                {
                    Token = _tokenService.CreateToken(user)
                };

                result.Success = true;
                result.Message = "Login successful";
                result.StatusCode = 200;
                result.Data = loginResponse;

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during login for user: {Username}", loginRequest.Username);
                return new ResponseResult<AdminLoginResponseDTO>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = $"Lỗi đăng nhập {e}"
                };
            }
        }
    }
}
