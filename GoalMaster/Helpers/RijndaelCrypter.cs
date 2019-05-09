using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace GoalMaster.Helpers
{
    class RijndaelCrypter
    {
        private RijndaelManaged _rijndael { get; set; }
        private const string _key = "dF5zc3dvcmQ9c4p9bpk0A3==";

        public RijndaelCrypter()
        {
            _rijndael = new RijndaelManaged();
            _rijndael.KeySize = 256;
            _rijndael.BlockSize = 128;
            _rijndael.Mode = CipherMode.CBC;
            _rijndael.Padding = PaddingMode.PKCS7;
            _rijndael.IV = Convert.FromBase64String(_key);
            _rijndael.Key = Convert.FromBase64String(_key);
        }

        public string Encode(string plainString)
        {
            if (null == plainString)
            {
                return String.Empty;
            }

            try
            {
                var plainText = ASCIIEncoding.UTF8.GetBytes(plainString);
                var crypto = _rijndael.CreateEncryptor();
                var cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);

                return Convert.ToBase64String(cipherText);
            }
            catch
            {
                return plainString;
            }
        }

        public string Decode(string encryptedString)
        {
            try
            {
                if (null == encryptedString)
                {
                    return encryptedString;
                }

                var decryptor = _rijndael.CreateDecryptor();
                var cipherText = Convert.FromBase64String(encryptedString ?? String.Empty);

                if (null != cipherText && cipherText.Length > 0)
                {
                    var decryptedText = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

                    return ASCIIEncoding.UTF8.GetString(decryptedText); ;
                }
                else
                {
                    return encryptedString;
                }
            }
            catch
            {
                return encryptedString;
            }
        }
    }
}

