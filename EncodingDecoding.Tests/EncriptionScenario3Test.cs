using SensitiveMessageEncryption;

namespace EncodingDecoding.Tests
{
    public class EncriptionTest
    {
        SensitiveMessage sensitiveMessage;

        [SetUp]
        public void Setup()
        {
            sensitiveMessage = new SensitiveMessage();
        }

        [Test]
        public void Decrypt_ShouldDecryptAHexString()
        {
            //Arrange
            string encryptedHex = "16390506637F57475347032A1A11070712195B5B173C05030B184D12405D012944180610155A46584E2F450C0619165C57450E2E050F187D28565F59073102140C1D4A";

            string password = "eWater2023";

            string expectedMessage = "sndr\u0006\rewaterxdat!(hopefully this one isn't too hardrcvr\u000a\u001fchallenger";
            //Act
            string? messageDecrypt = sensitiveMessage.Decrypt(encryptedHex, password);

            //Assert
            Assert.That(expectedMessage, Is.EqualTo(messageDecrypt));
        }
    }
}
