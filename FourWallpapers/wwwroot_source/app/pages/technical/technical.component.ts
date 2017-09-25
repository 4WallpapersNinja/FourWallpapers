import { Component, OnInit } from '@angular/core';

import { SeoService } from '@app/core';

@Component(
    {
        selector: 'fourwallpapers-technical',
        templateUrl: './technical.component.html',
        styleUrls: ['./technical.component.scss']
    })
export class TechnicalComponent implements OnInit {

    constructor(seoService: SeoService) {
        seoService.setTitle('The Technology behind 4Wallpapers Ninja');
        seoService.setMetaDescription(
            'The technology we use for 4wallpapers Ninja is C#, javascript, and html');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {}

}
