import { Component, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';

import { StatsResponse, SeoService } from '@app/core';

import { StatsService } from './stats.service';

@Component(
    {
        selector: 'fourwallpapers-stats',
        templateUrl: './stats.component.html',
        styleUrls: ['./stats.component.scss']
    })
export class StatsComponent implements OnInit {
    displayedColumns = ['keyword', 'count'];
    stats = {} as StatsResponse;
    errorMessage: string;
    columns: any = 5;

    constructor(private router: Router,
                private statsService: StatsService,
                seoService: SeoService) {

        seoService.setTitle('The Statistics behind 4wallpapers ninja');
        seoService.setMetaDescription(
            'Some interesting statistics behind 4wallpapers ninja and its database');
        seoService.setMetaRobots('Index, Follow');
    }

    ngOnInit() {
        this.columns = Math.round(window.innerWidth / 400);
        this.getStats();
    }

    getStats() {
        this.statsService.get()
            .subscribe(
                stats => this.stats = stats,
                error => this.errorMessage = (error as any)
            );
    }

    searchKeyword(keyword: string) {
        const url = `/search/${keyword}/9999/1280x720/1/-1/Any Ratio/0`;
        this.router.navigate([url]);
    }

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        const element = event.target.innerWidth;
        this.columns = Math.round(element / 400);
    }
}
