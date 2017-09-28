using Xunit;

namespace FourWallpapers.Providers.TwoFactorAuthentication.Tests
{
    public class QrCodeTest
    {
        [Fact]
        public void TwoFactorAuthenticator()
        {
            var twoFa = new TwoFactorAuthenticator();

            var setupCode = twoFa.GenerateSetupCode("FourWallpapers", "test@test.com", "SuperSecretPassword");

            Assert.NotEmpty(setupCode.QrCodeSetupImageUrl);
        }
    }
}
