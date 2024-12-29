using System.Linq;
using System.Security.Cryptography;

namespace AP.Cryptography
{
    public class TripleDESCryptoService : SymmetricCryptoServiceBase
    {
        protected override SymmetricAlgorithm CreateEncryptionProvider()
        {
            return new TripleDESCryptoServiceProvider { Key = this.GetSaltBytes(), IV = this.InitializationVector, Padding = PaddingMode.PKCS7, Mode = CipherMode.ECB };
        }

        protected override byte[] GetSaltBytes()
        {
            byte[] saltBytes = base.GetSaltBytes();
            return saltBytes.Skip<byte>((saltBytes.Length - 0x18)).Take<byte>(0x18).ToArray<byte>();
        }
    }
}

