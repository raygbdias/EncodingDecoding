using EncodingDecoding.src.DecodingScenario2;
using System.Text;

namespace SensitiveMessageEncryption
{
    public class SensitiveMessage
    {
        #region[Encrypt]
        static string Encrypt(string message, string password)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptedBytes = new byte[messageBytes.Length];

            for (int i = 0; i < messageBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(messageBytes[i] ^ passwordBytes[i % passwordBytes.Length]);
                passwordBytes[i % passwordBytes.Length] = (byte)((passwordBytes[i % passwordBytes.Length] + 1) % 256);
            }

            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
        #endregion

        #region[Decrypt]
        static string Decrypt(string encryptedHex, string password)
        {
            byte[] encryptedBytes = new byte[encryptedHex.Length / 2];
            for (int i = 0; i < encryptedHex.Length; i += 2)
                encryptedBytes[i / 2] = Convert.ToByte(encryptedHex.Substring(i, 2), 16);

            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] decryptedBytes = new byte[encryptedBytes.Length];

            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ passwordBytes[i % passwordBytes.Length]);
                passwordBytes[i % passwordBytes.Length] = (byte)((passwordBytes[i % passwordBytes.Length] + 1) % 256);
            }
            Dictionary<string, string> byteDict = new Dictionary<string, string>();
            try
            {
                new DecodingScenario2().DecodeMessageBlock(decryptedBytes, byteDict);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Encoding.ASCII.GetString(decryptedBytes);
        }
        #endregion

        #region[Encription]
        public void Encription()
        {
            string password = "eWater2023";

            string[] encryptedHexStrings = {
            "64617461140474686973206973207468652065617379206F6E65736E6472060D657761746572726376720A1F6368616C6C656E676572",
            "16390506637F57475347032A1A11070712195B5B173C05030B184D12405D012944180610155A46584E2F450C0619165C57450E2E050F187D28565F59073102140C1D4A",
            "16390506637F57475347032A10161001392E505C06350F13091351404C460D344655071D155D5A1A49280A150C025E5D58504A2B03171E57404758560C",
            "0417011D607C23213C3C2A3C450017111B46560B0754110453000F111F500F130A07550D1E540E1B03175108010B1D131452100918040A021F110157131A0717570F1910016A78030F190811051B1E0E077E72150F18151110160D1B0B"
        };

            foreach (string encryptedHex in encryptedHexStrings)
            {
                string decryptedMessage = Decrypt(encryptedHex, password);
                Console.WriteLine("Decrypted: " + decryptedMessage);
            }
        }
        #endregion
    }
}
