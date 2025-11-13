using AuthService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    private static DbContext _dbContext;

    public AuthController(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public IActionResult AddUser()
    {
        var User = new User()
        {
            Email = "Admin",
            Password = "Admin@123"
        };
        _dbContext.Add(User);
        _dbContext.SaveChanges();
        return Ok();
    }
}