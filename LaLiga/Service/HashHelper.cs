using System.Security.Cryptography;
using System.Text;

namespace LaLiga.Service;

public class HashHelper
{

    public static string HashMD5(string password)
    {
        using var md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }
}