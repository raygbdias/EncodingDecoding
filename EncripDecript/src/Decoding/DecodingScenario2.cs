using System.Text;

namespace EncodingDecoding.src.DecodingScenario2
{
    public class DecodingScenario2
    {
        #region[DecodeMessageBlock]
        public void DecodeMessageBlock(byte[] block, Dictionary<string, string> dictByte)
        {
            string blockType = Encoding.ASCII.GetString(block, 0, 4);
            int dataLength = block[4];
            byte checksum = block[5];
            byte calculatedChecksum = (byte)(block[0] ^ block[1] ^ block[2] ^ block[3] ^ block[4]);
            if (calculatedChecksum != checksum)
            {
                throw new Exception("Block header checksum verification failed.");
            }

            string data = Encoding.ASCII.GetString(block, 6, dataLength);

            byte[] byteArray = block.Skip(dataLength + blockType.Length + 2).ToArray();

            dictByte[blockType] = data;

            Console.WriteLine($"{blockType} : {data}");

            if (byteArray.Length == 0)
            {
                return;
            }
            DecodeMessageBlock(byteArray, dictByte);
        }

        #endregion

        #region[DecodingData]
        public void DecodingData(byte[] messageBytes)
        {
            try
            {
                Dictionary<string, string> dictByte = new Dictionary<string, string>();
                DecodeMessageBlock(messageBytes, dictByte);
                Console.WriteLine("-----");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Decoding Error: {ex.Message}");
            }
        }
        #endregion

        #region[DecodingExamples]
        public void DecodingData()
        {
            string input1Hex = "736E6472060D657761746572726376720A1F6368616C6C656E676572646174610818636F727265637421";
            string input2Hex = "736E6472060D657761746572626565700416626F6F706461746118087468696E67732063616E20626520756E6578706563746564726376720A1F6368616C6C656E676572";
            string input3Hex = "736E6472060D657761746572726376720A1F6368616C6C656E67657273656E7322296F68206E6F2C20736F6D657468696E672077656E742077726F6E67";
            string input4Hex = "736E6472060D657761746572726376720A1F6368616C6C656E67657273656E7322296F68206E6F2C20736F6D657468696E672077656E742077726F6E67";

            byte[] input1Bytes = ConvertHexStringToByteArray(input1Hex);
            byte[] input2Bytes = ConvertHexStringToByteArray(input2Hex);
            byte[] input3Bytes = ConvertHexStringToByteArray(input3Hex);
            byte[] input4Bytes = ConvertHexStringToByteArray(input4Hex);

            DecodingData(input1Bytes);
            DecodingData(input2Bytes);
            DecodingData(input3Bytes);
            DecodingData(input4Bytes);
        }
        #endregion

        #region[ConvertHexStringToByteArray]
        public byte[] ConvertHexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace("-", "");
            byte[] byteArray = new byte[hexString.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }
        #endregion
    }
}

