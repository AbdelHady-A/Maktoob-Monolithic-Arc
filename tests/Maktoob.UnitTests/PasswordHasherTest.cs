
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Maktoob.Infrastructure.Security;
using System.Threading.Tasks;
using Xunit;

namespace Maktoob.UnitTests
{
    public class PasswordHasherTest
    {
        [Theory]
        [InlineData("password----1")]
        [InlineData("passwooooord1")]
        [InlineData("AQAAAAEAAAPoAAAAEEku9pN9ZJjokBOLFmHCEZDIZpQRLU4RtiKNf76OeRkcMLxh0ABWDHfn13/dGtWkPA==")]
        [InlineData("AQAAAAEAACcQAAAAEAABAgMEBQYHCAkKCwwNDg+yWU7rLgUwPZb1Itsmra7cbxw2EFpwpVFIEtP+JIuUEw==")]
        [InlineData("AQAAAAEAAAPoAAAAEEku9pN9ZrwehkBOLFmHCEZDIZpQRLU4Rti24366Nf76OeRkcMLxh0ABWDHfn13/dGtWkPA12453")]

        public void FullRoundTrip(string password)
        {
            // arrange
            IPasswordHasher hasher = new PasswordHasher();

            // act & assert - success case
            var hashedPassword = hasher.Hash(password);
            var successResult = hasher.Verify(password, hashedPassword);
            Assert.Equal(PasswordVerificationResult.SUCCESS ,successResult);

            // act & assert - failure case
            var failedResult = hasher.Verify("pwregin", hashedPassword);
            Assert.Equal(PasswordVerificationResult.FAILED ,failedResult);
        }


        [Theory]
        [InlineData("password----1")]
        [InlineData("passwooooord1")]
        [InlineData("AQAAAAEAAAPoAAAAEEku9pN9ZJjokBOLFmHCEZDIZpQRLU4RtiKNf76OeRkcMLxh0ABWDHfn13/dGtWkPA==")]
        [InlineData("AQAAAAEAACcQAAAAEAABAgMEBQYHCAkKCwwNDg+yWU7rLgUwPZb1Itsmra7cbxw2EFpwpVFIEtP+JIuUEw==")]
        [InlineData("AQAAAAEAAAPoAAAAEEku9pN9ZrwehkBOLFmHCEZDIZpQRLU4Rti24366Nf76OeRkcMLxh0ABWDHfn13/dGtWkPA12453")]
        public void SamePasswordMustHaveDifferentHashes(string password)
        {
            // Arrange
            var hasher = new PasswordHasher();

            // Act
            string hash1 =  hasher.Hash(password);

            string hash2 = hasher.Hash(password);
            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [InlineData("AQAAAAIAAAAyAAAAEOMwvh3+FZxqkdMBz2ekgGhwQ4B6pZWND6zgESBuWiHw", PasswordVerificationResult.SUCCESS)] // SHA512, 50 iterations, 128-bit salt, 128-bit subkey
        [InlineData("AQAAAAIAAAD6AAAAIJbVi5wbMR+htSfFp8fTw8N8GOS/Sje+S/4YZcgBfU7EQuqv4OkVYmc4VJl9AGZzmRTxSkP7LtVi9IWyUxX8IAAfZ8v+ZfhjCcudtC1YERSqE1OEdXLW9VukPuJWBBjLuw==", PasswordVerificationResult.SUCCESS)] // SHA512, 250 iterations, 256-bit salt, 512-bit subkey
        [InlineData("AQAAAAAAAAD6AAAAEAhftMyfTJylOlZT+eEotFXd1elee8ih5WsjXaR3PA9M", PasswordVerificationResult.SUCCESS)] // SHA1, 250 iterations, 128-bit salt, 128-bit subkey
        [InlineData("AQAAAAEAA9CQAAAAIESkQuj2Du8Y+kbc5lcN/W/3NiAZFEm11P27nrSN5/tId+bR1SwV8CO1Jd72r4C08OLvplNlCDc3oQZ8efcW+jQ=", PasswordVerificationResult.SUCCESS)] // SHA256, 250000 iterations, 256-bit salt, 256-bit subkey
        public void VerifyHashedPassword_SuccessCases(string hashedPassword, PasswordVerificationResult expectedResult)
        {
            // Arrange
            var hasher = new PasswordHasher();

            // Act
            var actualResult = hasher.Verify("my password", hashedPassword);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("AQAAAAAAAAD6AAAAEAhftMyfTJyAAAAAAAAAAAAAAAAAAAih5WsjXaR3PA9M")] // incorrect password
        [InlineData("AQAAAAIAAAAyAAAAEOMwvh3+FZxqkdMBz2ekgGhwQ4A=")] // too short
        [InlineData("AQAAAAIAAAAyAAAAEOMwvh3+FZxqkdMBz2ekgGhwQ4B6pZWND6zgESBuWiHwAAAAAAAAAAAA")] // extra data at end
        public void VerifyHashedPassword_FailureCases(string hashedPassword)
        {
            // Arrange
            var hasher = new PasswordHasher();

            // Act
            var result = hasher.Verify("my password", hashedPassword);

            // Assert
            Assert.Equal(PasswordVerificationResult.FAILED, result);
        }

    }
}
