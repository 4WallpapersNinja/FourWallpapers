using System;
using System.Collections.Generic;
using System.Text;

namespace FourWallpapers.Providers.TwoFactorAuthentication
{
    /// <summary>
    /// Initially Copied from https://github.com/andyedinborough/GoogleAuthenticator/tree/dotnetcore
    /// </summary>
    public class SetupCode
    {
        public string Account { get; internal set; }
        public string AccountSecretKey { get; internal set; }
        public string ManualEntryKey { get; internal set; }
        public string QrCodeSetupImageUrl { get; internal set; }
    }
}
