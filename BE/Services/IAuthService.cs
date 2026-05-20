using ChatBox.Api.Contracts;

namespace ChatBox.Api.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<AuthResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
}