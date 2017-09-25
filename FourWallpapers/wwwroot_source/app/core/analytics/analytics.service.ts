import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AppInsights } from 'applicationinsights-js';

// Declare ga function as ambient
declare const ga: Function;

@Injectable()
export class AnalyticsService {
    private config: Microsoft.ApplicationInsights.IConfig = {
        instrumentationKey: environment.appInsights.instrumentationKey
    };

    constructor() {
        if (!AppInsights.config) {
            AppInsights.downloadAndSetup(this.config);
        }
    }

    logPageView(name?: string,
                url?: string,
                properties?: any,
                measurements?: any,
                duration?: number) {
        AppInsights.trackPageView(
            name,
            url,
            properties,
            measurements,
            duration);
        ga('set', 'page', url);
        ga('send', 'pageview');
    }

    emitEvent(eventCategory: string,
              eventAction: string,
              eventLabel: string = null,
              eventValue: number = null) {
        ga(
            'send',
            'event',
            {
                eventCategory: eventCategory,
                eventLabel: eventLabel,
                eventAction: eventAction,
                eventValue: eventValue
            });
    }
}
