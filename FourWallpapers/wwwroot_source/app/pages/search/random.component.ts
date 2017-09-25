import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchService } from './search.service';
import { SearchResult } from '@app/core';

@Component(
    {
        selector: 'fourwallpapers-random',
        templateUrl: './search.component.html',
        styleUrls: ['./search.component.scss']
    })
export class RandomComponent implements OnInit, OnDestroy {
    // infinite scroll related variables
    throttle = 300;
    scrollDistance = 1;
    scrollUpDistance = 2;
    columns: any = 5;

    // class related variables
    images: Array<SearchResult>;
    errorMessage: string;
    loadingCard = false;
    classification = -1;

    private sub: any;
    private initSearch = true;

    constructor(private route: ActivatedRoute,
                private searchService: SearchService) {}

    ngOnInit() {
        this.columns = Math.round(window.innerWidth / 300);
        this.processSearch();
    }

    ngOnDestroy() {}

    onScroll(ev) {
        this.processSearch();
    }

    processSearch() {
        this.loadingCard = true;

        this.searchService.random()
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
