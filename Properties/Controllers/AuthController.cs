using Auth_service.Constants;
using Auth_service.DAOModels;
using Auth_service.Models;
using AuthService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bcrypt = BCrypt.Net.BCrypt;


[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    /// <summary>
    /// The database context
    /// </summary>
    private static AppDbContext _dbContext;

    /// <summary>
    /// The JWT service
    /// </summary>
    private static JwtService _jwtService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="_jwtService"></param>
    public AuthController(AppDbContext dbContext, JwtService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    #region Public Methods
    [HttpPost]
    public async Task<Response> AddUser(CreateUser request)
    {
        return await addUser(request);
    }


    [HttpPost]
    public async Task<LoginResponse> Login(Login request)
    {
        return await login(request);
    }
    #endregion

    private async Task<Response> addUser(CreateUser request)
    {
        Response response = new();
        var User = new UserDB()
        {
            Email = request.Email,
            Name = request.Name,
            Password = Bcrypt.HashPassword(request.Password),
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Role = UserTypes.USER,

        };
        await _dbContext.AddAsync(User);
        await _dbContext.SaveChangesAsync();
        response.IsSuccess = true;
        return response;
    }

    private async Task<LoginResponse> login(Login request)
    {
        LoginResponse response = new();
        var user = await _dbContext.UserAuth.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
        if (user != null)
        {
            var isMatch = Bcrypt.Verify(request.Password, user.Password);
            if (isMatch)
            {
                response.Token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Role.ToString());
                response.IsSuccess = true;
            }
            else
            {
                response.ErrorMessage = "Invalid Password";
            }
        }
        else
        {
            response.ErrorMessage = "User not found";
        }

        return response;
    }
}