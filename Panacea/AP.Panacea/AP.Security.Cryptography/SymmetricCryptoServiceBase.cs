using System;
using System.Security.Cryptography;
using System.Text;

namespace AP.Security.Cryptography
{
    public abstract class SymmetricCryptoServiceBase
    {
        private static readonly byte[] _defaultInitializationVector = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xAB, 0xCD, 0xEF };
        
        public virtual System.Text.Encoding Encoding { get; set; }

        public virtual byte[] InitializationVector { get; set; }

        public virtual string Salt { get; set; }


        public SymmetricCryptoServiceBase()
        {
            this.InitializationVector = _defaultInitializationVector;
            this.Encoding = System.Text.Encoding.Default;
        }

        protected abstract SymmetricAlgorithm CreateEncryptionProvider();

        public virtual string Decrypt(string encryptedText)
        {
            using (SymmetricAlgorithm algorithm = this.CreateEncryptionProvider())
            {
                byte[] inputBuffer = new byte[encryptedText.Length / 2];
                int startIndex = 0;

                for (int i = 0; startIndex < encryptedText.Length; i++)
                {
                    inputBuffer[i] = Convert.ToByte(encryptedText.Substring(startIndex, 2), 0x10);
                    startIndex += 2;
                }
                
                using (ICryptoTransform transform = algorithm.CreateDecryptor())
                {
                    inputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                    algorithm.Clear();
                }
                
                return this.Encoding.GetString(inputBuffer);
            }
        }

        public virtual string Encrypt(string plainText)
        {
            using (SymmetricAlgorithm algorithm = this.CreateEncryptionProvider())
            {
                byte[] bytes = this.Encoding.GetBytes(plainText);
                using (ICryptoTransform transform = algorithm.CreateEncryptor())
                {
                    bytes = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                    algorithm.Clear();
                }
                StringBuilder builder = new StringBuilder(bytes.Length * 2);
                foreach (byte num in bytes)
                {
                    builder.Append(num.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected virtual byte[] GetSaltBytes()
        {
            return this.Encoding.GetBytes(this.Salt);
        }
    }
}