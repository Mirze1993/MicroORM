using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MicroORM.Tools
{
    public class HashCreate
    {
        const int saltSize = 10, hashSize = 20;
        byte[] salt, hash;

        void SaltCreate()
        {
            salt = new byte[saltSize];
            using var randomCreate = RandomNumberGenerator.Create();
            randomCreate.GetBytes(salt);
        }

        string CreateHash(string password)
        {
            SaltCreate();
            hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 50,
                numBytesRequested: hashSize
                );
            return Convert.ToBase64String(hash);
        }

        byte[] CreateHash(string password, byte[] salt)
        {
            hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 50,
                numBytesRequested: hashSize
                );
            return hash;
        }

        public string CreateHashString(string password)
        {
            CreateHash(password);
            var a = new byte[saltSize + hashSize];
            Buffer.BlockCopy(salt, 0, a, 0, saltSize);
            Buffer.BlockCopy(hash, 0, a, saltSize, hashSize);
            return Convert.ToBase64String(a);
        }

        public bool VerfiyPassword(string stringPassword, string hasPassword)
        {
            if (string.IsNullOrEmpty(stringPassword) || string.IsNullOrEmpty(hasPassword))
                return false;
            var oldHashPas = Convert.FromBase64String(hasPassword);
            var saltOld = new byte[saltSize];
            Buffer.BlockCopy(oldHashPas, 0, saltOld, 0, saltSize);
            var newHash = CreateHash(stringPassword, saltOld);
            for (int i = 0; i < newHash.Length; i++)
                if (newHash[i] != oldHashPas[saltSize + i]) return false;
            return true;
        }
    }
}
