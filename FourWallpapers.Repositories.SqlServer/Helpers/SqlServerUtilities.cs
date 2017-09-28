using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FourWallpapers.Core.Database.Entities;

namespace FourWallpapers.Repositories.SqlServer.Helpers
{
    public static class SqlServerUtilities
    {
        /// <summary>
        ///     Private Holder for Fetched Embedded Resources
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> SqlQueries =
            new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     Helps build the joins properly into the object
        /// </summary>
        /// <param name="image"></param>
        /// <param name="keyword"></param>
        /// <param name="lookupImages"></param>
        /// <returns></returns>
        public static Image FormatSqlResultsData(Image image, Keyword keyword,
            Dictionary<Guid, Image> lookupImages)
        {
            try
            {
                if (keyword == null) return image;
                Image imageData;
                if (!lookupImages.TryGetValue(image.Id, out imageData))
                    lookupImages.Add(image.Id, imageData = image);

                imageData.Keywords.Add(keyword.Value);

                return imageData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///     get a list of embedded file for the resourceAssembly provided
        /// </summary>
        /// <param name="resourceAssembly"></param>
        /// <returns></returns>
        public static string[] GetEmbeddedFileNames(
            string resourceAssembly = "FourWallpapers.Repositories.SqlServer.Queries")
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceNames()
                .Where(r => r.StartsWith(resourceAssembly)).ToArray();
        }

        /// <summary>
        ///     open the requested embedded file
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetEmbeddedFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        ///     Returns the contents of the SQL File Requested (from project embedded resources)
        /// </summary>
        /// <param name="queryName">the name of the sql file you are looking for </param>
        /// <param name="resourceAssembly">the assembly to look in </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetQuery(string queryName,
            string resourceAssembly = "FourWallpapers.Repositories.SqlServer.Queries")
        {
            //Check if we already loaded this stored query
            if (!SqlQueries.ContainsKey(queryName.ToUpper()))
            {
                // Query not in internal Dictionary

                // Assemble the Embedded Resource Name
                var resourceName = $"{resourceAssembly}.{queryName}.sql";

                // get content
                var content = GetEmbeddedFile(resourceName);

                // null or empty check 
                if (string.IsNullOrWhiteSpace(content))
                {
                    var foundName = GetEmbeddedFileNames(resourceAssembly)
                        .FirstOrDefault(r => r.ToLower().Equals(resourceName.ToLower()));

                    if (string.IsNullOrWhiteSpace(foundName))
                        throw new ArgumentNullException("stream", $"Can not find {queryName} query file ");

                    // get content (via case insensitivity
                    content = GetEmbeddedFile(foundName);
                }

                // Add it to the collection of SqlQueries
                SqlQueries.TryAdd(queryName.ToUpper(), content);
            }

            // check one more time incase it wasnt found in the previous step
            if (SqlQueries.ContainsKey(queryName.ToUpper()))
                return SqlQueries[queryName.ToUpper()];
            throw new Exception("Stored query " + queryName + " not found!");
        }
    }
}