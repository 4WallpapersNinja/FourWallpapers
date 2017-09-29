using System;
using Xunit;

namespace FourWallpapers.Providers.TwoFactorAuthentication.Tests
{
    /// <summary>
    /// Initially Copied from https://github.com/andyedinborough/GoogleAuthenticator/tree/dotnetcore
    /// </summary>
    public class KeyFinderTest
    {
        public static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void FindIterationNumber()
        {
            string secretKey = "PJWUMZKAUUFQKJBAMD6VGJ6RULFVW4ZH";
            string targetCode = "267762";

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            long currentTime = 1416643820;

            for (long i = currentTime; i >= 0; i=i-10)
            {
                var result = tfa.GeneratePinAtInterval(secretKey, i);
                if (result == targetCode)
                {
                    Assert.True(true);
                    // ReSharper disable once HeuristicUnreachableCode
                    return;
                }
            }

            Assert.True(false);
        }
    }
}
