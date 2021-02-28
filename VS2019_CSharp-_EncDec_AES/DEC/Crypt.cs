using System.Security.Cryptography;
using System.Globalization;
using System.Collections.ObjectModel;

namespace com.tukutano
{
    public class Crypt
    {
        public static readonly System.Collections.Generic.IReadOnlyList<byte> defaultIV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        static byte[] zeroIV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new System.ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                                          "The binary key cannot have an odd number of digits: {0}", hex));
            }

            byte[] HexAsBytes = new byte[hex.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hex.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }


        public byte[] Encrypt(byte[] input, byte[] key, byte[] iv,
            CipherMode cypherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.Zeros,
            int keySizes = 128, int blockSize = 128)
        {
            byte[] output = null;
            try
            {
                var aesAlg = new AesManaged
                {
                    KeySize = keySizes,
                    Key = key,
                    BlockSize = blockSize,
                    Mode = cypherMode,
                    Padding = paddingMode,
                    IV = iv
                };

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                output = encryptor.TransformFinalBlock(input, 0, input.Length);
            }
            catch (CryptographicException e)
            {
                output = null;
                System.Console.WriteLine("CryptographicException:" + e.Message);
                System.Console.WriteLine(e.StackTrace);
            }
            return output;
        }


        public byte[] Decrypt(byte[] input, byte[] key, byte[] iv,
            CipherMode cypherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.Zeros,
            int keySizes = 128, int blockSize = 128)
        {
            var aesAlg = new AesManaged
            {
                KeySize = keySizes,
                Key = key,
                BlockSize = blockSize,
                Mode = cypherMode,
                Padding = paddingMode,
                IV = iv
            };

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            return decryptor.TransformFinalBlock(input, 0, input.Length);
        }
    }
}
