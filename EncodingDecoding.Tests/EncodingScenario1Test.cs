using AutoFixture;
using EncodingDecoding.src.EncodingScenario1;

namespace EncodingDecoding.Tests
{
    public class EncodingTests
    {
        EncodingScenario1 encodingScenario1;

        [SetUp]
        public void Setup()
        {
            encodingScenario1 = new EncodingScenario1();
        }

        [Test]
        public void CalculateChecksum_ShouldCheckTheSum()
        {
            //Arrange
            byte expectedCheckSum = 13;
            byte[] data = new byte[] { 115, 110, 100, 114, 6};

            //Act
            byte checksum = EncodingScenario1.CalculateChecksum(data);

            //Assert
            Assert.That(checksum, Is.EqualTo(expectedCheckSum));
        }


        [TestCase("sndr", "rcvr", "kind", "sens", "data", "time")]
        public void EncodeMessage_ShouldEncodeTheMessage(params string[] args)
        {
            //Arrange
            int[] expectedFirstFourByte = new int[] { 115, 110, 100, 114 };
            Dictionary<string, string> expectedDict = CreateDictionary<string, string>(args.ToList());

            //Act
            byte[] messageEncoding = EncodingScenario1.EncodeMessage(expectedDict);

            //Assert
            Assert.IsTrue(messageEncoding.Length >= 4);
            for (int i = 0; i < expectedFirstFourByte.Length; i++)
            {
                Assert.That(messageEncoding[i], Is.EqualTo(expectedFirstFourByte[i]));
            }
        }
        private Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(IEnumerable<TKey> keys)
        {
            Fixture fixture = new Fixture();
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (TKey key in keys)
            {
                TValue value = fixture.Create<TValue>();
                dictionary[key] = value;
            }
            return dictionary;
        }
        [Test]
        public void EncodeMessageBlock_ShouldEncodeABlockOfMessage()
        {
            //Arrange
            string? blockType = "sndr";
            string? Data = "eWater";
            byte[] expectedMessageBlock = new byte[] { 115, 110, 100, 114, 6, 13, 101, 87, 97, 116, 101, 114};

            //Act
            byte[] encodeMessageBlock = EncodingScenario1.EncodeMessageBlock(blockType, Data);

            //Assert
            Assert.That(expectedMessageBlock, Is.EqualTo(encodeMessageBlock));
        }
    }
}