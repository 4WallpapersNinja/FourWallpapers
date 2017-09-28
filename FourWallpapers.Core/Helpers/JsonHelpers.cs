using Newtonsoft.Json;

namespace FourWallpapers.Core.Helpers
{
    public static class Json
    {
        /// <summary>
        ///     Converts a object (input) into a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nullValueHandler"></param>
        /// <param name="formatting"></param>
        /// <param name="referenceLoopHandling"></param>
        /// <returns></returns>
        public static string ToJson(this object input, NullValueHandling nullValueHandler = NullValueHandling.Include,
            Formatting formatting = Formatting.Indented,
            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Error)
        {
            return JsonConvert.SerializeObject(
                input,
                formatting,
                new JsonSerializerSettings
                {
                    NullValueHandling = nullValueHandler,
                    ReferenceLoopHandling = referenceLoopHandling
                }
            );
        }

        /// <summary>
        ///     Converts a String(Input) into a object of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="nullValueHandler"></param>
        /// <param name="referenceLoopHandling"></param>
        /// <returns></returns>
        public static T LoadJson<T>(string input, NullValueHandling nullValueHandler = NullValueHandling.Ignore,
            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Error)
        {
            //check for null string specifically, let empty strings go through the parser
            if (input == null)
                return default(T);


            //Done
            return JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings
            {
                NullValueHandling = nullValueHandler,
                ReferenceLoopHandling = referenceLoopHandling
            });
        }
    }
}