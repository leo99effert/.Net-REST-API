namespace Data
{
  public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<int>> Delete(int userId);
        Task<bool> UserExists(string username);
    }
}