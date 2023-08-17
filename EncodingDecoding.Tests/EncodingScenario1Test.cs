using AutoFixture;
using EncodingDecoding.src.EncodingScenario1;

namespace EncodingDecoding.Tests
{
    public class Tests
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
            byte expectedCheckSum = 11;
            byte[] data = new byte[] { 115, 110, 100, 114};
            byte checksum = EncodingScenario1.CalculateChecksum(data);
            Assert.That(checksum, Is.EqualTo(expectedCheckSum));
        }


        [TestCase( "sndr", "rcvr", "kind", "sens", "data", "time")]
        public void EncodeMessage_ShouldEncodeTheMessage(params string[] args)
        {
            //Arrange
            var expectedFirstFourByte = new int[] {115, 110, 100, 114}; 
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
            var fixture = new Fixture();
            var dictionary = new Dictionary<TKey, TValue>();

            foreach (var key in keys)
            {
                TValue value = fixture.Create<TValue>();
                dictionary[key] = value;
            }

            return dictionary;
        }
    }   
}