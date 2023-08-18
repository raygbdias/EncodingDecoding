using System.Text;
namespace EncodingDecoding.src.EncodingScenario1;

public class EncodingScenario1
{
    #region[CalculateChecksum]
    public static byte CalculateChecksum(byte[] data)
    {
        try
        {
            byte checksum = data[0];
            if (data.Length == 5) 
            {
                for (int i = 1; i < data.Length; i++)
                {
                    checksum ^= data[i];
                }
                return checksum;
            }
            else
            {
                throw new Exception($"block header did not pass it's own sum");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"block header did not pass it's own sum: {ex.Message}");
        }
    }
    #endregion

    #region[EncodeMessageBlock]
    public static byte[] EncodeMessageBlock(string blockType, string data)
    {
        try
        {
            byte[] header = Encoding.ASCII.GetBytes(blockType);
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            byte[] lengthByte = { (byte)dataBytes.Length };

            byte[] headerChecker = new byte[header.Length + lengthByte.Length];
            Buffer.BlockCopy(header, 0, headerChecker, 0, header.Length);
            Buffer.BlockCopy(lengthByte, 0, headerChecker, header.Length, lengthByte.Length);
            byte checksum = CalculateChecksum(headerChecker);
            byte[] checkSumArray = new byte[] { checksum };

            byte[] block = new byte[header.Length + lengthByte.Length + dataBytes.Length + 1];
            Buffer.BlockCopy(header, 0, block, 0, header.Length);
            Buffer.BlockCopy(lengthByte, 0, block, header.Length, lengthByte.Length);
            Buffer.BlockCopy(checkSumArray, 0, block, header.Length + lengthByte.Length, checkSumArray.Length);
            Buffer.BlockCopy(dataBytes, 0, block, header.Length + lengthByte.Length + checkSumArray.Length, dataBytes.Length);

            return block;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while encoding block: {ex.Message}");
            return new byte[0];
        }
    }
    #endregion

    #region[EncodeMessage]
    public static byte[] EncodeMessage(Dictionary<string, string> data)
    {
        List<byte> message = new List<byte>();

        foreach (KeyValuePair<string, string> kvp in data)
        {
            try
            {
                byte[] messageBlock = EncodeMessageBlock(kvp.Key, kvp.Value);
                message.AddRange(messageBlock);
            }
            catch (Exception ex)
            {
                Console.Write($"Block header error: {ex.Message}");
            }
        }

        return message.ToArray();
    }
    #endregion

    #region[EncodingData]
    public static void EncodingData()
    {
        try
        {
            Dictionary<string, string> input1 = new Dictionary<string, string>
        {
            { "snd", "ewater" },
            { "rcvr", "foo-works" },
            { "sens", "heart-beat" },
            { "time", "2023-08-16T13:07" }
        };

            Dictionary<string, string> input2 = new Dictionary<string, string>
        {
            { "sndr", "ewater" },
            { "rcvr", "foo-works" },
            { "sens", "temperature" },
            { "data", "15" }
        };

            Dictionary<string, string> input3 = new Dictionary<string, string>
        {
            { "sndr", "ewater" },
            { "rcvr", "foo-works" },
            { "sens", "body-temperature" },
            { "dat", "36" }
        };

            byte[] messageBytes1 = EncodeMessage(input1);
            Console.WriteLine("Input 1 Message Bytes: " + BitConverter.ToString(messageBytes1));

            byte[] messageBytes2 = EncodeMessage(input2);
            Console.WriteLine("Input 2 Message Bytes: " + BitConverter.ToString(messageBytes2));

            byte[] messageBytes3 = EncodeMessage(input3);
            Console.WriteLine("Input 3 Message Bytes: " + BitConverter.ToString(messageBytes3));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in EncodingData: {ex.Message}");
        }

    }
    #endregion
}