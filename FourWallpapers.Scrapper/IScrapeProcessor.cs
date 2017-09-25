using AngleSharp.Dom;
using FourWallpapers.Core;

namespace FourWallpapers.Scrapper
{
    public interface IScrapeProcessor
    {
        void Process(Enums.Sources source, Enums.Classes classification = Enums.Classes.Any);

        bool GetImageUrl(IElement element, ImageDetail image);
        bool GetWhoStamp(IElement element, ImageDetail image);
        bool GetResolution(IElement element, ImageDetail image);
        bool GetTags(IElement element, ImageDetail image);
    }
}