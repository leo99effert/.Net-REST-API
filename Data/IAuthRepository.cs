namespace Data
{
  public interface IAuthRepository
    {
        Task<ServiceResponse<GetUserDto>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> Delete(int userId);
        Task<bool> UserExists(string username);
    }
}