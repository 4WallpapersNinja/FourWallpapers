import { Injectable } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';

@Injectable()
export class SeoService {
    /**
     * Angular 2 Title Service
     */
    private titleService: Title;

    private metaService: Meta;

    /**
     * Inject the Angular 2 Title Service
     * @param titleService
     */
    constructor(titleService: Title, metaService: Meta) {
        this.titleService = titleService;
        this.metaService = metaService;
    }

    getTitle(): string {
        return this.titleService.getTitle();
    }

    setTitle(newTitle: string) {
        this.titleService.setTitle(newTitle + ' :: 4Wallpapers Ninja');
    }

    getMetaDescription(): string {
        const selector = 'name=\'description\'';
        return this.metaService.getTag(selector).content;
    }

    setMetaDescription(description: string) {
        return this.metaService.updateTag(
            {
                content: description
            },
            'name=\'description\''
        );
    }

    getMetaRobots(): string {
        const selector = 'name=\'robots\'';
        return this.metaService.getTag(selector).content;
    }

    setMetaRobots(robots: string) {
        return this.metaService.updateTag(
            {
                content: robots
            },
            'name=\'robots\''
        );
    }
}
