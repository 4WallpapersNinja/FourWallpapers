import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ViewService } from './view.service';
import { AnalyticsService, SeoService, Image } from '@app/core';

@Component(
    {
        selector: 'fourwallpapers-view',
        templateUrl: './view.component.html',
        styleUrls: ['./view.component.scss']
    })
export class ViewComponent implements OnInit, OnDestroy {
    id: string;
    private sub: any;
    image = {} as Image;
    errorMessage: string;
    keywords: string;
    classification: string;

    constructor(private route: ActivatedRoute,
                private viewService: ViewService,
                private analyticsService: AnalyticsService,
                seoService: SeoService) {
        seoService.setTitle('Viewing Image');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(
            params => {
                this.id = params['imageId'];
            });

        this.getImage();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    getImage() {
        this.viewService.get(this.id)
            .subscribe(
                image => {
                    this.image = image;
                    this.keywords = image.tags;
                },
                error => this.errorMessage = (error as any)
            );
    }

    updateKeywords() {
        this.analyticsService.emitEvent(
            'image_update',
            'update_keywords',
            'click');
        this.viewService.updateKeywords(this.id, this.keywords)
            .subscribe(
                resp => '',
                error => this.errorMessage = (error as any)
            );
    }

    updateClass() {
        this.analyticsService.emitEvent(
            'image_update',
            'update_classification',
            'click');
        this.viewService.updateClass(this.id, this.classification)
            .subscribe(
                resp => '',
                error => this.errorMessage = (error as any)
            );
    }

    report() {
        this.analyticsService.emitEvent(
            'image_update',
            'report_image',
            'click');
        this.viewService.report(this.id)
            .subscribe(
                resp => {
                    this.back();
                },
                error => this.errorMessage = (error as any)
            );
    }

    back() {
        window.close();
    }

    imageUrl() {
        if (this.image != null && this.image.server != null) {
            return `//shinobi${this.image.server}.4wp.ninja/images/${this.image.filePath}`;
        }
        return '/assets/loadingStar.svg';
    }
}
