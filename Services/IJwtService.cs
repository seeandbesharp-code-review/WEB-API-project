using Entities;

public interface IJwtService
{
    public string GenerateToken(User user);
}