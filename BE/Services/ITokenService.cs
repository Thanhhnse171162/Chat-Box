using ChatBox.Api.Models;

namespace ChatBox.Api.Services;

public interface ITokenService
{
    string CreateToken(User user);
}