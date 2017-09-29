import { Component, OnInit, HostListener } from '@angular/core';

import { SearchResult, SeoService } from '@app/core';

import { RecentService } from './recent.service';

@Component(
    {
        selector: 'fourwallpapers-recent',
        templateUrl: './recent.component.html',
        styleUrls: ['./recent.component.scss']
    })
export class RecentComponent implements OnInit {

    errorMessage: string;
    images: Array<SearchResult> = [];
    columns: any = 5;

    loadingCard = false;

    constructor(private recentService: RecentService, seoService: SeoService) {

        seoService.setTitle('Recently indexed images');
        seoService.setMetaDescription(
            'Recently indexed images that may need to be rated and classified');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {
        this.columns = Math.round(window.innerWidth / 300);
        this.get();
    }

    get() {
        this.loadingCard = true;
        this.recentService.get()
            .subscribe(
                recent => {
                    this.images = recent;
                    this.loadingCard = false;
                },
                error => this.errorMessage = (error as any)
            );
    }

    imageUrl(image: SearchResult) {
        if (image != null && image.server != null) {
            return `//shinobi${image.server}.4wp.ninja/thumbnails/${image.filePath}`;
        }
        return '/assets/loadingStar.svg';
    }

    openImage(imageId: string) {
        const url = `/view/${imageId}`;
        window.open(url, '_blank');
    };

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        const element = event.target.innerWidth;
        this.columns = Math.round(element / 300);
    }
}
