using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FourWallpapers.Helpers {
    public static class Network {
        /// <summary>
        ///     Gets the first ip address for the provided dns string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IPAddress GetIpAddress(string input) {
            var parsed = IPAddress.TryParse(input, out IPAddress ip);

            if (parsed) return ip;

            var addresses = Task.Run(() => Dns.GetHostAddressesAsync(input)).Result;

            foreach (var address in addresses.Where(address => address.AddressFamily == AddressFamily.InterNetwork)) {
                ip = address;
                break;
            }
            return ip;
        }
    }
}