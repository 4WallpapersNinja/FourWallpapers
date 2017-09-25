import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchService } from './search.service';
import { SearchRequest, SearchResult, SeoService } from '@app/core';

@Component(
    {
        selector: 'fourwallpapers-search',
        templateUrl: './search.component.html',
        styleUrls: ['./search.component.scss']
    })
export class SearchComponent implements OnInit, OnDestroy {
    // infinite scroll related variables
    throttle = 300;
    scrollDistance = 1;
    scrollUpDistance = 2;
    columns: any = 5;

    // class specific variables
    searchReq = {} as SearchRequest;
    images: Array<SearchResult>;
    errorMessage: string;
    loadingCard = false;

    private sub: any;
    private initSearch = true;

    constructor(private route: ActivatedRoute,
                private searchService: SearchService,
                seoService: SeoService) {

        seoService.setTitle('The Wallpaper search results you requested');
        seoService.setMetaDescription('Wallpapers based on the search you did');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {
        this.columns = Math.round(window.innerWidth / 300);

        this.sub = this.route.params.subscribe(
            params => {
                let tempCriteria: string = params['criteria'];
                if (tempCriteria == null) {
                    tempCriteria = '';
                }
                this.searchReq = new SearchRequest(
                    params['classification'],
                    params['resolution'],
                    params['resolutionSearch'],
                    params['source'],
                    tempCriteria,
                    params['ratio'],
                    params['size']);
                this.processSearch();
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    onScroll(ev) {
        this.processSearch();
    }

    processSearch() {
        this.loadingCard = true;
        if (!this.initSearch) {
            this.searchReq.Page += 1;
        }

        this.searchService.get(this.searchReq)
            .subscribe(
                images => {
                    if (this.initSearch) {
                        this.images = images;
                        this.initSearch = false;
                    } else {
                        for (const image of images) {
                            this.images.push(image);
                        }
                    }
                    this.loadingCard = false;
                },
                error => this.errorMessage = (error as any)
            );
    }

    openImage(imageId: string) {
        const url = `/view/${imageId}`;
        window.open(url, '_blank');
    }

    imageUrl(image: SearchResult) {
        if (image != null && image.server != null) {
            return `//shinobi${image.server}.4wp.ninja/thumbnails/${image
                .imageId}.${image.fileExtension}`;
        }
        return '/assets/loadingStar.svg';
    }

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        const element = event.target.innerWidth;
        this.columns = Math.round(element / 300);
    }
}
