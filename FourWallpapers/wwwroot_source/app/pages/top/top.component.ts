import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SeoService, SearchResult } from '@app/core';

import { TopService } from './top.service';

@Component(
    {
        selector: 'fourwallpapers-top',
        templateUrl: './top.component.html',
        styleUrls: ['./top.component.scss']
    })
export class TopComponent implements OnInit, OnDestroy {
    by: string;
    value: string;
    private sub: any;
    errorMessage: string;
    images: Array<SearchResult>;
    columns: any = 5;

    loadingCard = false;

    sourceOptions = [
        { 'link': '/top/Source/0', 'label': '/w/' },
        { 'link': '/top/Source/1', 'label': '/v/' },
        { 'link': '/top/Source/2', 'label': '/wg/' },
        { 'link': '/top/Source/3', 'label': '/hr/' },
        { 'link': '/top/Source/100', 'label': '/wp/' },
        { 'link': '/top/Source/200', 'label': '/r/wallpapers' },
        { 'link': '/top/Source/201', 'label': '/r/wallpaper' },
        { 'link': '/top/Source/202', 'label': '/r/NSFW_Wallpapers' },
        { 'link': '/top/Source/203', 'label': '/r/multiwall' },
        { 'link': '/top/Source/204', 'label': '/r/EarthPorn' },
        { 'link': '/top/Source/206', 'label': '/r/SpacePorn' },
        { 'link': '/top/Source/207', 'label': '/r/Hi_Res' },
        { 'link': '/top/Source/208', 'label': '/r/WidescreenWallpaper' },
        { 'link': '/top/Source/209', 'label': '/r/WQHD_Wallpaper' },
        { 'link': '/top/Source/210', 'label': '/r/4to3Wallpapers' },
        { 'link': '/top/Source/211', 'label': '/r/wallpaperdump' },
        { 'link': '/top/Source/9998', 'label': 'Imgur (by way of reddit)' }
    ];
    classificationOptions = [
        { 'link': '/top/classification/0', 'label': 'Unrated' },
        { 'link': '/top/classification/1', 'label': 'Safe for Work' },
        { 'link': '/top/classification/2', 'label': 'Borderline' },
        { 'link': '/top/classification/3', 'label': 'Not Safe for Work' },
    ];

    constructor(private route: ActivatedRoute,
                private topService: TopService,
                seoService: SeoService) {
        seoService.setTitle('Viewing Top Results');
        seoService.setMetaDescription(
            'Get Most downloaded Wallpapers by source board or by classification');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {
        this.columns = Math.round(window.innerWidth / 300);

        this.sub = this.route.params.subscribe(
            params => {
                this.by = params['by'];
                this.value = params['value'];
                this.get();
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    get() {
        this.loadingCard = true;
        this.topService.get(this.by, this.value)
            .subscribe(
                images => {
                    this.images = images;
                    this.loadingCard = false;
                },
                error => this.errorMessage = (error as any)
            );
    }

    imageUrl(image: SearchResult) {
        if (image != null && image.server != null) {
            return `//shinobi${image.server}.4wp.ninja/thumbnails/${image
                .imageId}.${image.fileExtension}`;
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
