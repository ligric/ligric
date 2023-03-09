using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ligric.Server.Application.Providers.Security
{
	//TODO: Back IDisposable once everything will be moved to Core
	public class DefaultCryptoProvider : ICryptoProvider/*, IDisposable*/
    {
        #region Fields

        private readonly HashAlgorithm _hashAlgorithm;
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;

        #endregion

        public DefaultCryptoProvider()
        {
			_hashAlgorithm = new SHA512Managed();

            var aesAlgorithm = new AesManaged { Mode = CipherMode.CBC };
            var cryptoKey = Convert.FromBase64String("RnObFfS6tQKWfHJmwCC1yGLtl/1T3oP+FGMZOyr77us=");
            var cryptoVector = Convert.FromBase64String("JOgQUaVye+6qoThNrqZDjA==");

            _encryptor = aesAlgorithm.CreateEncryptor(cryptoKey, cryptoVector);
            _decryptor = aesAlgorithm.CreateDecryptor(cryptoKey, cryptoVector);
        }

        public bool VerifyHash(string password, string hash, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            var inputHash = GetHash(password, salt);

            var result = string.CompareOrdinal(inputHash, hash) == 0;

            return result;
        }

        public string GetSalt(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            var plainTextWithSaltBytes = new byte[plainTextBytes.Length];

            plainTextBytes.CopyTo(plainTextWithSaltBytes, 0);

            var hashBytes = _hashAlgorithm.ComputeHash(plainTextWithSaltBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public string GetHash(string value, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var plainTextBytes = Encoding.UTF8.GetBytes(value);

            var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            plainTextBytes.CopyTo(plainTextWithSaltBytes, 0);
            saltBytes.CopyTo(plainTextWithSaltBytes, plainTextBytes.Length);

            var hashBytes = _hashAlgorithm.ComputeHash(plainTextWithSaltBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public string Encrypt(string value)
        {
	        if (string.IsNullOrEmpty(value))
	        {
		        return string.Empty;
	        }

	        using (var ms = new MemoryStream())
	        {
		        using (var cs = new CryptoStream(ms, _encryptor, CryptoStreamMode.Write))
		        {
			        using (var sw = new StreamWriter(cs))
			        {
				        sw.Write(value);
			        }

			        return Convert.ToBase64String(ms.ToArray());
		        }
	        }
        }

        public string Decrypt(string value)
        {
	        if (string.IsNullOrEmpty(value))
	        {
		        return value;
	        }

	        using (var ms = new MemoryStream(Convert.FromBase64String(value)))
	        {
		        using (var cs = new CryptoStream(ms, _decryptor, CryptoStreamMode.Read))
		        {
			        using (var sr = new StreamReader(cs))
			        {
				        return sr.ReadToEnd();
			        }
		        }
	        }
        }

		public void Dispose()
		{
			_hashAlgorithm.Dispose();
		}
	}
}