export class Image {
    constructor(
        public imageId: string,
        public filePath: string,
        public classification: string,
        public indexSource: string,
        public who: string,
        public resolution: string,
        public tags: string,
        public dateDownloaded: Date,
        public downloads: number,
        public server: string,
        public reported: number) {}
}

export class UpdateRequest {
    constructor(
        public Key: string,
        public Value: string) {}
}
