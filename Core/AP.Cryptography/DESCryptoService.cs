using System.Linq;
using System.Security.Cryptography;

namespace AP.Cryptography
{
    public class DESCryptoService : SymmetricCryptoServiceBase
    {
        protected override SymmetricAlgorithm CreateEncryptionProvider()
        {
            return new DESCryptoServiceProvider { Key = this.GetSaltBytes(), IV = this.InitializationVector, Padding = PaddingMode.PKCS7, Mode = CipherMode.ECB };
        }

        protected override byte[] GetSaltBytes()
        {
            byte[] saltBytes = base.GetSaltBytes();
            int l = saltBytes.Length;

            if (l > 8)
                return saltBytes.Skip<byte>((saltBytes.Length - 8)).Take<byte>(8).ToArray<byte>();
            else if (l < 8)
            {
                byte[] sb = new byte[8];

                for (int i = 0; i < 8; i++)
                {
                    if (i < l)
                        sb[i] = saltBytes[i];
                    else
                        sb[i] = 0;
                }
            }                
            return saltBytes;
        }
    }
}

