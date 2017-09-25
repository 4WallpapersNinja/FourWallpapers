using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FourWallpapers.Extensions {
    public static class HttpContextExtensions {
        /// <summary>
        ///     Used to get the current clients ip based off some known headers for hiding it (for cdns and proxys and such)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCurrentClientIp(this HttpContext context) {
            if (context.Request.Headers.Any(h => h.Key == "True-Client-IP"))
                return context.Request.Headers["True-Client-IP"];

            if (context.Request.Headers.Any(h => h.Key == "X-Forwarded-For"))
                return context.Request.Headers["X-Forwarded-For"];

            //Use the client's IP address, as defined by TCP
            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}