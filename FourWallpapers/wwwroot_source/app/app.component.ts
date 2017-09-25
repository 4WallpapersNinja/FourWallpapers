import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { OverlayContainer } from '@angular/material';
import { Router, NavigationEnd } from "@angular/router";

import { Store } from '@ngrx/store';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';

import { AnalyticsService } from '@app/core';
import { environment as env } from '@env/environment';

import { selectorSettings } from './settings';

// Declare ga function as ambient
declare const ga: Function;


@Component(
    {
        selector: 'fourwallpapers-root',
        templateUrl: './app.component.html',
        styleUrls: ['./app.component.scss']
    })
export class AppComponent implements OnInit, OnDestroy {

    private unsubscribe$: Subject<void> = new Subject<void>();

    @HostBinding('class')
    componentCssClass;

    version = env.versions.app;
    year = new Date().getFullYear();
    logo = require('../assets/logo.svg');
    navigation = [
        { link: 'home', label: 'Home' },
        { link: 'top', label: 'Top Images' },
        { link: 'recent', label: 'Most Recent' },
        { link: 'stats', label: 'Stats' },
        { link: 'disclaimer', label: 'Disclaimer' },
        { link: 'about', label: 'About' }
    ];
    navigationSideMenu = [
        ...this.navigation
    ];

    constructor(public router: Router,
                public analyticsService: AnalyticsService,
                public overlayContainer: OverlayContainer,
                private store: Store<any>) {
        this.router.events.subscribe(
            event => {
                if (event instanceof NavigationEnd) {
                    analyticsService.logPageView(null, event.urlAfterRedirects);
                }
            });
    }

    ngOnInit(): void {
        this.store
            .select(selectorSettings)
            .takeUntil(this.unsubscribe$)
            .map(({ theme }) => theme.toLowerCase())
            .subscribe(
                theme => {
                    this.componentCssClass = theme;
                    this.overlayContainer['themeClass'] = theme;
                });
    }

    ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
    }
}