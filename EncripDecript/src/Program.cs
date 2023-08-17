using EncodingDecoding.src.DecodingScenario2;
using EncodingDecoding.src.EncodingScenario1;
using System.Text;

Console.WriteLine("Hey everyone\n");
Console.WriteLine("If you would like to see the encoded message, type 1, to see the decoded message, type 2");

var console = Console.ReadLine();

if (console == "1")
{
    EncodingScenario1.EncodingData();
}
if(console == "2")
{
    new DecodingScenario2().DecodingData();
}

