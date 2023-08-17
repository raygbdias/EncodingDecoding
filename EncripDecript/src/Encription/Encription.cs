using System.Text;

namespace EncripDecript.src.Encription;

public class Encription
{
    #region[CalculateChecksum]
    static byte CalculateChecksum(byte[] data)
    {
        try
        {
            byte checksum = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                checksum ^= data[i];
            }
            return checksum;
        }
        catch (Exception ex)
        {
            throw new Exception($"block header did not pass it's own sum: {ex}");
        }
    }
    #endregion

    #region[EncodeMessageBlock]
    static byte[] EncodeMessageBlock(string blockType, string data)
    {
        try
        {
            byte[] header = Encoding.ASCII.GetBytes(blockType);
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            byte[] lengthByte = { (byte)dataBytes.Length };
            byte checksum = CalculateChecksum(header);

            byte[] block = new byte[header.Length + lengthByte.Length + dataBytes.Length + 1];
            Buffer.BlockCopy(header, 0, block, 0, header.Length);
            Buffer.BlockCopy(lengthByte, 0, block, header.Length, lengthByte.Length);
            Buffer.BlockCopy(dataBytes, 0, block, header.Length + lengthByte.Length, dataBytes.Length);
            block[header.Length + lengthByte.Length + dataBytes.Length] = checksum;

            return block;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error Encoding the message Block:{ex}");
        }
    }
    #endregion

    #region[EncodeMessage]
    static byte[] EncodeMessage(Dictionary<string, string> data)
    {
        List<byte> message = new List<byte>();
        string[] knownBlockTypes = { "sndr", "rcvr", "kind", "sens", "data", "time" };

        foreach (KeyValuePair<string, string> kvp in data)
        {
            try
            {
                //Here is checking if the key exists in the knownBlockTypes, I need to find a way to check if does not exists it won't check
                //the others so it saves memory
                if (Array.Exists(knownBlockTypes, type => type == kvp.Key))
                {
                    byte[] messageBlock = EncodeMessageBlock(kvp.Key, kvp.Value);
                    message.AddRange(messageBlock);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Block Header is Unkown {ex}");
            }

        }

        return message.ToArray();
    }
    #endregion

    #region[EncriptionData]
    public static void EncriptionData()
    {
        var input1 = new Dictionary<string, string>
        {
            { "snd", "ewater" },
            { "rcvr", "foo-works" },
            { "sens", "heart-beat" },
            { "time", "2023-08-16T13:07" }
        };

        var input2 = new Dictionary<string, string>
        {
            { "sndr", "ewater" },
            { "rcvr", "foo-works" },
            { "sens", "temperature" },
            { "data", "15" }
        };

        var input3 = new Dictionary<string, string>
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
    #endregion
}