using EncodingDecoding.src.DecodingScenario2;
using EncodingDecoding.src.EncodingScenario1;
using SensitiveMessageEncryption;

Console.WriteLine("Hey everyone\n");
Console.WriteLine("If you would like to see the encoded message, type 1\n to see the decoded message, type 2\n To encrypt data type 3");

string? console = Console.ReadLine();

if (console == "1")
{
    EncodingScenario1.EncodingData();
}
if (console == "2")
{
    new DecodingScenario2().DecodingData();
}
if (console == "3")
{
    new SensitiveMessage().Encription();
}

