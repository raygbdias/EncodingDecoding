
using EncodingDecoding.src.DecodingScenario2;

namespace EncodingDecoding.Tests
{
    public class DecodingTests
    {
        DecodingScenario2 decodingScenario2;

        [SetUp]
        public void Setup()
        {
            decodingScenario2 = new DecodingScenario2();
        }

        [Test]
        public void ConvertHexStringToByteArray_ShouldConvertHexStringToByteArray()
        {
            //Arrange
            string hexString = "45776174657232303233";

            byte[] expectedByteArray = new byte[] {69, 119, 97, 116, 101, 114, 50, 48, 50, 51};

            //Act
            byte[] mesasageConvertionToByteArray = decodingScenario2.ConvertHexStringToByteArray(hexString);

            //Assert
            Assert.That(expectedByteArray, Is.EqualTo(mesasageConvertionToByteArray));
        }

        [Test]
        public void DecodeMessageBlock_ShouldDecodeABlockofMessage()
        {
            //Arrange
            DecodingScenario2 decodingScenario2 = new DecodingScenario2();
            byte[] block = new byte[] {115, 110, 100, 114, 6, 13, 101, 119, 97 , 116, 101, 114, 114, 99, 118, 114, 10, 31, 99, 104, 97, 108, 108, 101, 110, 103, 101, 114, 100, 97, 116, 97, 8, 24 ,99, 111, 114,
                                       114, 101, 99, 116, 33};

            Dictionary<string, string> expectedBlock = new Dictionary<string, string>
            {
                { "sndr", "ewater" },
                { "rcvr", "challenger" },
                { "data", "correct!" }
            };
            //Act
            Dictionary<string, string> decodedData = new Dictionary<string, string>();
            decodingScenario2.DecodeMessageBlock(block, decodedData);

            //Assert
            foreach (var kvp in expectedBlock)
            {
                Assert.IsTrue(decodedData.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, decodedData[kvp.Key]);
            }
        }
    }
}
