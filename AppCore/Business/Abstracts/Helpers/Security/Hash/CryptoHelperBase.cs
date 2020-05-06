using AppCore.Business.Concretes.Models.Security.Hash;
using System.Security.Cryptography;
using System.Text;

namespace AppCore.Business.Abstracts.Helpers.Security.Hash
{
    public abstract class CryptoHelperBase
    {
        private readonly HMAC _hmac;

        protected CryptoHelperBase()
        {
            _hmac = new HMACSHA256();
        }

        protected CryptoHelperBase(HMAC hmac)
        {
            _hmac = hmac;
        }

        protected virtual byte[] ComputeHash(string value, byte[] salt)
        {
            var hash = Encoding.UTF8.GetBytes(value);
            _hmac.Key = salt;
            return _hmac.ComputeHash(hash);
        }

        public virtual Crypto CreateHash(string value)
        {
            return new Crypto()
            {
                Salt = _hmac.Key,
                Hash = ComputeHash(value, _hmac.Key)
            };
        }

        public virtual bool VerifyHash(string value, byte[] hash, byte[] salt)
        {
            var computedHash = ComputeHash(value, salt);
            if (computedHash.Length != hash.Length)
            {
                return false;
            }
            bool result = true;
            for (int i = 0; i < computedHash.Length && result; i++)
            {
                if (computedHash[i] != hash[i])
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
