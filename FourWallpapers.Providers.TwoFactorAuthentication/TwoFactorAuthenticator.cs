﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ImageSharp;
using ZXing.QrCode;
using SixLabors.ImageSharp.PixelFormats;

namespace FourWallpapers.Providers.TwoFactorAuthentication
{
    /// <summary>
    /// Initially Copied from https://github.com/andyedinborough/GoogleAuthenticator/tree/dotnetcore
    /// </summary>
    public class TwoFactorAuthenticator
    {
        public static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public TimeSpan DefaultClockDriftTolerance { get; set; }

        public TwoFactorAuthenticator()
        {
            DefaultClockDriftTolerance = TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// Generate a setup code for a Google Authenticator user to scan (with issuer ID).
        /// </summary>
        /// <param name="issuer">Issuer ID (the name of the system, i.e. 'MyApp')</param>
        /// <param name="accountTitleNoSpaces">Account Title (no spaces)</param>
        /// <param name="accountSecretKey">Account Secret Key</param>
        /// <returns>SetupCode object</returns>
        public SetupCode GenerateSetupCode(string issuer, string accountTitleNoSpaces, string accountSecretKey)
        {
            if (string.IsNullOrWhiteSpace(accountTitleNoSpaces)) { throw new NullReferenceException("Account Title is null"); }

            accountTitleNoSpaces = accountTitleNoSpaces.Replace(" ", "");

            SetupCode sC = new SetupCode
            {
                Account = accountTitleNoSpaces,
                AccountSecretKey = accountSecretKey,
                ManualEntryKey = EncodeAccountSecretKey(accountSecretKey)
            };

            string provisionUrl = string.IsNullOrEmpty(issuer) ? $"otpauth://totp/{accountTitleNoSpaces}?secret={sC.ManualEntryKey}" : $"otpauth://totp/{accountTitleNoSpaces}?secret={ sC.ManualEntryKey}&issuer={UrlEncode(issuer)}";

            string url = GenerateQrImageString(provisionUrl);

            sC.QrCodeSetupImageUrl = url;

            return sC;
        }

        private string GenerateQrImageString(string qrCodeContent)
        {
            var qrCodeWriter = new ZXing.BarcodeWriterSvg()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250,
                    Margin = 0
                }
            };
            var pixelData = qrCodeWriter.Write(qrCodeContent);
            return pixelData.Content;
        }

        private static string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (char symbol in value)
            {
                if (validChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString().Replace(" ", "%20");
        }

        private string EncodeAccountSecretKey(string accountSecretKey)
        {
            return Base32Encode(Encoding.UTF8.GetBytes(accountSecretKey));
        }

        private static string Base32Encode(IReadOnlyList<byte> data)
        {
            const int inByteSize = 8;
            const int outByteSize = 5;
            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

            int i = 0, index = 0;
            StringBuilder result = new StringBuilder((data.Count + 7) * inByteSize / outByteSize);

            while (i < data.Count)
            {
                var currentByte = (data[i] >= 0) ? data[i] : (data[i] + 256);

                /* Is the current digit going to span a byte boundary? */
                var digit = 0;
                if (index > (inByteSize - outByteSize))
                {
                    int nextByte;
                    if ((i + 1) < data.Count)
                        nextByte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                    else
                        nextByte = 0;

                    digit = currentByte & (0xFF >> index);
                    index = (index + outByteSize) % inByteSize;
                    digit <<= index;
                    digit |= nextByte >> (inByteSize - index);
                    i++;
                }
                else
                {
                    digit = (currentByte >> (inByteSize - (index + outByteSize))) & 0x1F;
                    index = (index + outByteSize) % inByteSize;
                    if (index == 0)
                        i++;
                }
                result.Append(alphabet[digit]);
            }

            return result.ToString();
        }

        public string GeneratePinAtInterval(string accountSecretKey, long counter, int digits = 6)
        {
            return GenerateHashedCode(accountSecretKey, counter, digits);
        }

        internal string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
        {
            byte[] key = Encoding.UTF8.GetBytes(secret);
            return GenerateHashedCode(key, iterationNumber, digits);
        }

        internal string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            HMACSHA1 hmac = new HMACSHA1(key);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            int binary =
                ((hash[offset] & 0x7f) << 24)
                | (hash[offset + 1] << 16)
                | (hash[offset + 2] << 8)
                | (hash[offset + 3]);

            int password = binary % (int)Math.Pow(10, digits);
            return password.ToString(new string('0', digits));
        }

        private long GetCurrentCounter()
        {
            return GetCurrentCounter(DateTime.UtcNow, Epoch, 30);
        }

        private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        }


        public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient)
        {
            return ValidateTwoFactorPin(accountSecretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance);
        }

        public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance)
        {
            var codes = GetCurrentPiNs(accountSecretKey, timeTolerance);
            return codes.Any(c => c == twoFactorCodeFromClient);
        }

        public string GetCurrentPin(string accountSecretKey)
        {
            return GeneratePinAtInterval(accountSecretKey, GetCurrentCounter());
        }

        public string GetCurrentPin(string accountSecretKey, DateTime now)
        {
            return GeneratePinAtInterval(accountSecretKey, GetCurrentCounter(now, Epoch, 30));
        }

        public string[] GetCurrentPiNs(string accountSecretKey)
        {
            return GetCurrentPiNs(accountSecretKey, DefaultClockDriftTolerance);
        }

        public string[] GetCurrentPiNs(string accountSecretKey, TimeSpan timeTolerance)
        {
            List<string> codes = new List<string>();
            long iterationCounter = GetCurrentCounter();
            int iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
            }

            long iterationStart = iterationCounter - iterationOffset;
            long iterationEnd = iterationCounter + iterationOffset;

            for (long counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(GeneratePinAtInterval(accountSecretKey, counter));
            }

            return codes.ToArray();
        }
    }
}