using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConsultationApp.Models;
using ConsultationApp.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using ConsultationApp.Dto;

namespace ConsultationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Client>> Register(CreateClientDto dto)
        {
            if (await _context.Clients.AnyAsync(c => c.Email == dto.Email))
            {
                return BadRequest("Пользователь с таким email уже существует.");
            }

            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email
            };
            client.SetPassword(dto.Password); // ← Хешируем пароль

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel login)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == login.Email);

            // Сравниваем пароль
            if (client == null || !client.VerifyPassword(login.Password)) // ← Так
            {
                return Unauthorized("Неверный email или пароль.");
            }

            var token = GenerateJwtToken(client);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Client client)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("Jwt:Key не настроен.");
            }
            var key = Encoding.ASCII.GetBytes(keyString); // ← Так
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                    new Claim(ClaimTypes.Name, client.Name),
                    new Claim(ClaimTypes.Email, client.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("me")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}