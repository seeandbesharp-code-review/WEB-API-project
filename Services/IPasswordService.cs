namespace Services
{
    public interface IPasswordService
    {
        int GetPasswordScore(string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}