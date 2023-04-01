namespace Services.User
{
  public interface IUserService
  {
    Task<ServiceResponse<GetUserDto>> Register(Models.User user, string password);
    Task<ServiceResponse<string>> Login(string username, string password);
    Task<ServiceResponse<List<GetUserDto>>> GetUsers();
    Task<ServiceResponse<GetUserDto>> Delete(int userId);
  }
}