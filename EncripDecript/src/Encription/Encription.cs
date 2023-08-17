using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EncripDecript.src.Encription;

public class Encription
{
    public byte[] MessageEncoder(Dictionary<string, string> input)
    {
        List<byte[]> blocks = new List<byte[]>();
        return null;
    }

    Dictionary<string, string> input1 = new Dictionary<string, string>() {
      {"sndr", "ewater"},
      {"rcvr", "foo-works"},
      {"sens", "heart-beat"},
      {"time", "2023-08-16T13:07"}
    };

    Dictionary<string, string> input2 = new Dictionary<string, string>() {
      {"sndr", "ewater"},
      {"rcvr", "foo-works"},
      {"sens", "temperature"},
      {"data", "15"}
    };

    Dictionary<string, string> input3 = new Dictionary<string, string>() {
      {"sndr", "ewater"},
      {"rcvr", "foo-works"},
      {"sens", "body-temperature"},
      {"dat", "36"}
    };

}