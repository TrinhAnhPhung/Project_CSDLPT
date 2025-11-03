using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
public static class Security
{
    public static byte[] CreateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public static byte[] HashPassword(string password, byte[] salt)
    {
        using var sha = SHA256.Create();
        var pwdBytes = Encoding.UTF8.GetBytes(password);
        var input = new byte[pwdBytes.Length + salt.Length];
        Buffer.BlockCopy(pwdBytes, 0, input, 0, pwdBytes.Length);
        Buffer.BlockCopy(salt, 0, input, pwdBytes.Length, salt.Length);
        return sha.ComputeHash(input);
    }
}