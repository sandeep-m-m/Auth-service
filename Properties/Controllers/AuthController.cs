using Auth_service.DAOModels;
using Auth_service.Models;
using AuthService.Data;
using Microsoft.AspNetCore.Mvc;


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
    #endregion
    
    private async Task<Response> addUser(CreateUser request)
    {
        Response response = new();
        var User = new UserDB()
        {
            Email = request.Email,
            Password = request.Password,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            
        };
        await _dbContext.AddAsync(User);
        await _dbContext.SaveChangesAsync();
        return response;
    }
}