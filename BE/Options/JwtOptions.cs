namespace ChatBox.Api.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string SecretKey { get; set; } = "YourSuperSecretKeyThatIsAtLeast32CharactersLongForHS256Algorithm";

    public string Issuer { get; set; } = "IdentityService";

    public string Audience { get; set; } = "IdentityServiceClient";

    public int ExpiryMinutes { get; set; } = 60;

    public int RefreshTokenExpiryDays { get; set; } = 7;
}