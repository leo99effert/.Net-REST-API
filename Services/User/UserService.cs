namespace Services.User
{
  public class UserService : IUserService
  {
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public UserService(DataContext context, IConfiguration configuration, IMapper mapper)
    {
      _mapper = mapper;
      _configuration = configuration;
      _context = context;
    }
    public async Task<ServiceResponse<GetUserDto>> Register(Models.User user, string password)
    {
      var response = new ServiceResponse<GetUserDto>();
      try
      {
        // Check username
        if (await UserExists(user.Username)) throw new Exception($"User with username {user.Username} already exist.");
        // Create password
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        // Save
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = $"User with username {user.Username} was created.";
        response.Data = _mapper.Map<GetUserDto>(user);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
      var response = new ServiceResponse<string>();
      try
      {
        // Get User
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user is null) throw new Exception($"User with username {username} was not found.");
        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) throw new Exception("Wrong Password.");
        // Create response
        response.Message = $"User with username {username} has logged in and received a jwt-token.";
        response.Data = CreateToken(user);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<List<GetUserDto>>> GetUsers()
    {
      var response = new ServiceResponse<List<GetUserDto>>();
      try
      {
        // Get Users
        var users = await _context.Users.Include(u => u.Characters).ToListAsync();
        // Create response
        response.Message = "All users.";
        response.Data = users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<GetUserDto>> Delete(int userId)
    {
      var response = new ServiceResponse<GetUserDto>();
      try
      {
        // Get User
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) throw new Exception($"User With id {userId} was not found.");
        // Save
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        // Create response
        response.Message = $"User with id {userId} was deleted.";
        response.Data = _mapper.Map<GetUserDto>(user);
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }


    private async Task<bool> UserExists(string username)
    {
      if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
      {
        return true;
      }
      return false;
    }
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
      }
    }
    private string CreateToken(Models.User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
      };

      var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
      if (appSettingsToken is null)
        throw new Exception("Appsettings Token is null!");

      SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
        .GetBytes(appSettingsToken));

      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}