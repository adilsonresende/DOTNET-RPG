namespace DOTNET_RPG.Utility
{
    public static class PasswordHashHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmacsha = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmacsha.Key;
                passwordHash = hmacsha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}