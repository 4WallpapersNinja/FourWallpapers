export class SearchRequest {

    Page = 1;
    constructor(
        public Class: string,
        public Resolution: string,
        public ResolutionSearch: string,
        public Source: string,
        public Criteria: string,
        public Ratio: string,
        public Size: string) {}

}

export class SearchResult {
    constructor(
        public imageId: string,
        public fileExtension: string,
        public server: string,
        public isThumbnailAvailable: boolean) {}
}
