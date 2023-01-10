using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Tools
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]); //fills an array with a strong sequence of random bytes
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); //creates a key from the password and the salt
            byte[] hash = pbkdf2.GetBytes(20); //creates a hash from the key filled with random key bytes
            byte[] hashBytes = new byte[36]; //creates a byte array with a length of 36
            Array.Copy(salt, 0, hashBytes, 0, 16); //copies the salt to the first 16 bytes of the hashBytes array
            Array.Copy(hash, 0, hashBytes, 16, 20); //copies the hash to the next 20 bytes of the hashBytes array
            string savedPasswordHash = Convert.ToBase64String(hashBytes);  //converts the byte array to a string
            return savedPasswordHash;
        }

        public static bool Validate(string plainText, string HashedPass)
        {
            if (HashedPass == "unknown password") return false; //if the password is unknown, it is not valid
            byte[] hashBytes = Convert.FromBase64String(HashedPass); //converts the string to a byte array
            byte[] salt = new byte[16]; //creates a new byte array with 16 bytes
            Array.Copy(hashBytes, 0, salt, 0, 16); //copies the first 16 bytes of the hashBytes array to the salt array
            var pbkdf2 = new Rfc2898DeriveBytes(plainText, salt, 10000); //creates a key from the password and the salt
            byte[] hash = pbkdf2.GetBytes(20); //creates a hash from the key
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i]) //compares the hashBytes array with the hash array
                    return false; //if they are not equal, the password is not valid
            return true; //if they are equal, the password is valid
        }
    }
}
