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

    public AuthController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Public Methods
    [HttpPost]
    public async Task<Response> AddUser(CreateUser request)
    {
        return await addUser(request);
    }

        
    [HttpPost]
    public async Task<Response> Login(Login request)
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
            Password = Bcrypt.HashPassword(request.Password), 
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            
        };
        await _dbContext.AddAsync(User);
        await _dbContext.SaveChangesAsync();
        return response;
    }

    private async Task<Response> login(Login request)
    {
        Response response = new();
        var user = await _dbContext.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
        if(user != null)
        {
            var isMatch = Bcrypt.Verify(request.Password, user.Password);
            if(isMatch)
            {
                response.IsSuccess = true;
            }
        }

        return response;
    }
}