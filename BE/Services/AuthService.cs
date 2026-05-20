using ChatBox.Api.Contracts;
using ChatBox.Api.Data;
using ChatBox.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBox.Api.Services;

public sealed class AuthService : IAuthService
{
    private const int DefaultUserRoleId = 2;

    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext dbContext, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = NormalizeEmail(request.Email);

        var existingUser = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email da ton tai.");
        }

        var fullName = request.FullName.Trim();
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            RoleId = DefaultUserRoleId,
            IsActive = true
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            User = ToUserDto(user)
        };
    }

    public async Task<AuthResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        var email = NormalizeEmail(request.Email);

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email hoac mat khau khong dung.");
        }

        return new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            User = ToUserDto(user)
        };
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }
}