export class StatsResponse {
    constructor(
        public topStats: Result[],
        public topKeywords: Result[],
        public asOf: Date) {}
}

export class Result {
    constructor(
        public keyword: string,
        public count: number) {}
}
