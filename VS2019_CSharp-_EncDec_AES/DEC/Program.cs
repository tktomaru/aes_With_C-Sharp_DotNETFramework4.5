using System;
using System.IO;
using System.Security.Cryptography;

namespace com.tukutano
{
    class Program
    {
        public static readonly byte[] defaultIV = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        static void Main(string[] args)
        {
            Console.WriteLine("書式");
            Console.WriteLine("aes EncDecFlg(0=dec,1=enc) dataFile keyFile OutputFile IVFile cypherMode paddingMode keySize blockSize");
            if (args.Length < 4)
            {
                Console.WriteLine("引数個数 エラー 最低4引数が必要です");
                return;
            }
            string argEncDecFlg = args[0];
            string argDataFile = args[1];
            string argkeyFile = args[2];
            string argOutFile = args[3];
            string argCypherMode = "1"; // CBC がデフォルト
            //平文の文字列
            byte[] data = null;
            //鍵
            byte[] key = null;

            if (File.Exists(argDataFile))
            {
                data = ReadBinaryFromFile(argDataFile);
            }
            else
            {
                Console.WriteLine("データファイル エラー ファイルが存在しません");
                return;
            }

            if (File.Exists(argkeyFile))
            {
                key = ReadBinaryFromFile(argkeyFile);
            }
            else
            {
                Console.WriteLine("キーファイル エラー ファイルが存在しません");
                return;
            }

            // 初期化ベクトル
            byte[] iv = defaultIV;
            if (args.Length > 4)
            {
                string argIV = args[4];

                if (File.Exists(argIV))
                {
                    iv = ReadBinaryFromFile(argIV);
                }
                else
                {
                    Console.WriteLine("IVファイル エラー ファイルが存在しません");
                    return;
                }
            }

            if (args.Length > 5)
            {
                argCypherMode = args[5];
            }
            string argPaddingMode = "1"; // None がデフォルト
            if (args.Length > 6)
            {
                argPaddingMode = args[6];
            }
            string argKeySizes = "128"; // 128 がデフォルト
            if (args.Length > 7)
            {
                argKeySizes = args[7];
            }
            string argBlockSize = "128"; // 128 がデフォルト
            if (args.Length > 8)
            {
                argBlockSize = args[8];
            }

            bool argCheckResult = ArgCheck(argCypherMode, argPaddingMode, argKeySizes, argBlockSize);
            if (false == argCheckResult)
            {
                // 引数エラー時は終了
                return;
            }

            Crypt wd = new Crypt();

            int encDecFlg = parseStringToInt(argEncDecFlg);
            // 暗号モード
            CipherMode cypherMode = (CipherMode)parseStringToInt(argCypherMode);
            // パディングモード
            PaddingMode paddingMode = (PaddingMode)parseStringToInt(argPaddingMode);
            // キーサイズ
            int keySizes = parseStringToInt(argKeySizes);
            // ブロックサイズ
            int blockSize = parseStringToInt(argBlockSize);

            byte[] dest = null;
            if (0 == encDecFlg)
            {
                dest = wd.Decrypt(data, key, iv,
                 cypherMode, paddingMode, keySizes, blockSize);
            }
            else
            {
                dest = wd.Encrypt(data, key, iv,
                 cypherMode, paddingMode, keySizes, blockSize);
            }

            WriteBinaryToFile(argOutFile, dest);
        }

        static bool ArgCheck(
            string cypherMode, string paddingMode,
            string keySizes, string blockSize)
        {
            int intCypherMode = -1;
            intCypherMode = parseStringToInt(cypherMode);

            if (!(1 <= intCypherMode && intCypherMode <= 5))
            {
                Console.WriteLine("cypherMode エラー");
                Console.WriteLine("CBC = 1");
                Console.WriteLine("ECB = 2");
                Console.WriteLine("OFB = 3");
                Console.WriteLine("CFB = 4");
                Console.WriteLine("CTS = 5");
                return false;
            }

            int intPaddingMode = -1;
            intPaddingMode = parseStringToInt(paddingMode);

            if (!(1 <= intPaddingMode && intPaddingMode <= 4))
            {
                Console.WriteLine("paddingMode エラー");
                Console.WriteLine("None = 1 埋め込みなし");
                Console.WriteLine("PKCS7 = 2 それぞれが追加されたパディング バイトの合計数と等しく、バイト");
                Console.WriteLine("Zeros = 3 バイトを 0");
                Console.WriteLine("ANSIX923 = 4 長さの前にゼロで埋められたバイト");
                Console.WriteLine("ISO10126 = 5 ランダムなデータ");
                return false;
            }

            int intKeySizes = -1;
            intKeySizes = parseStringToInt(keySizes);

            if (!(1 <= intKeySizes))
            {
                Console.WriteLine("keySizes エラー");
                return false;
            }

            int intBlockSize = -1;
            intBlockSize = parseStringToInt(blockSize);

            if (!(1 <= intBlockSize))
            {
                Console.WriteLine("blockSize エラー");
                return false;
            }

            return true;
        }

        static int parseStringToInt(string input)
        {
            int output = -1;
            try
            {
                output = int.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{input}'");
            }
            return output;
        }

        // バイナリデータをファイルに書き込み(書き込み先のフォルダがない場合は作成する)
        // https://qiita.com/tera1707/items/65025aca202ef0535fa5
        public static void WriteBinaryToFile(string path, byte[] data)
        {
            path = Path.GetFullPath(path);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                using (var sw = new BinaryWriter(fs))
                {
                    sw.Write(data);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"WriteBinaryToFile" + e.Message);
            }
        }

        // バイナリデータをファイルから読み込み
        // https://qiita.com/tera1707/items/65025aca202ef0535fa5
        public static byte[] ReadBinaryFromFile(string path)
        {
            path = Path.GetFullPath(path);
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open))
                using (var sr = new BinaryReader(fs))
                {
                    int len = (int)fs.Length;
                    byte[] data = new byte[len];
                    sr.Read(data, 0, len);

                    return data;
                }
            }

            return null;
        }
    }
}
