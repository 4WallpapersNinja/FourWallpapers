import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { SearchRequest, SeoService } from '@app/core';

@Component(
    {
        selector: 'fourwallpapers-home',
        templateUrl: './home.component.html',
        styleUrls: ['./home.component.scss']
    })
export class HomeComponent implements OnInit {

    search: SearchRequest;

    resolutions = [
        { 'value': '1024x768', 'label': '1024x768' },
        { 'value': '1152x768', 'label': '1152x768' },
        { 'value': '1280x720', 'label': '1280x720', 'selected': 'selected' },
        { 'value': '1280x768', 'label': '1280x768' },
        { 'value': '1280x800', 'label': '1280x800' },
        { 'value': '1280x854', 'label': '1280x854' },
        { 'value': '1280x960', 'label': '1280x960' },
        { 'value': '1280x1024', 'label': '1280x1024' },
        { 'value': '1366x768', 'label': '1366x768' },
        { 'value': '1400x1050', 'label': '1400x1050' },
        { 'value': '1440x900', 'label': '1440x900' },
        { 'value': '1440x960', 'label': '1440x960' },
        { 'value': '1600x900', 'label': '1600x900' },
        { 'value': '1600x1200', 'label': '1600x1200' },
        { 'value': '1680x1050', 'label': '1680x1050' },
        { 'value': '1920x1080', 'label': '1920x1080' },
        { 'value': '1920x1200', 'label': '1920x1200' },
        { 'value': '2048x1536', 'label': '2048x1536' },
        { 'value': '2560x1080', 'label': '2560x1080' },
        { 'value': '2560x1440', 'label': '2560x1440' },
        { 'value': '2560x1600', 'label': '2560x1600' },
        { 'value': '2560x2048', 'label': '2560x2048' },
        { 'value': '3440x1440', 'label': '3440x1440' },
        { 'value': '3840x2160', 'label': '3840x2160' },
        { 'value': '2048x768', 'label': '2x 1024x768' },
        { 'value': '2560x960', 'label': '2x 1280x960' },
        { 'value': '2560x1024', 'label': '2x 1280x1024' },
        { 'value': '3360x1050', 'label': '2x 1680x1050' },
        { 'value': '3150x1680', 'label': '3x 1680x1050 in portrait' },
        { 'value': '3840x1080', 'label': '2x 1920x1080' },
        { 'value': '5760x1080', 'label': '3x 1920x1080' },
        { 'value': '3840x1024', 'label': '2x 1920x1024' },
        { 'value': '5120x1440', 'label': '2x 2560x1440' },
        { 'value': '1080x1920', 'label': '1920x1080 in portrait' },
        { 'value': '2160x3840', 'label': '3840x2160 in portrait' }
    ];

    sources = [
        { 'value': '9999', 'label': 'Any', 'selected': 'selected' },
        { 'value': '0', 'label': '4chan /w/' },
        { 'value': '1', 'label': '4chan /v/' },
        { 'value': '2', 'label': '4chan /wg/' },
        { 'value': '3', 'label': '4chan /hr/' },
        { 'value': '100', 'label': '7chan /wp/' },
        { 'value': '200', 'label': 'reddit /r/wallpapers' },
        { 'value': '201', 'label': 'reddit /r/wallpaper' },
        { 'value': '202', 'label': 'reddit /r/NSFW_Wallpapers' },
        { 'value': '203', 'label': 'reddit /r/multiwall' },
        { 'value': '204', 'label': 'reddit /r/EarthPorn' },
        { 'value': '206', 'label': 'reddit /r/SpacePorn' },
        { 'value': '207', 'label': 'reddit /r/Hi_Res' },
        { 'value': '208', 'label': 'reddit /r/WidescreenWallpaper' },
        { 'value': '209', 'label': 'reddit /r/WQHD_Wallpaper' },
        { 'value': '210', 'label': 'reddit /r/4to3Wallpapers' },
        { 'value': '211', 'label': 'reddit /r/wallpaperdump' },
        { 'value': '300', 'label': '8chan /w/' },
        { 'value': '301', 'label': '8chan /wg/' },
        { 'value': '9998', 'label': 'Imgur (by way of reddit)' }
    ];

    classifications = [
        { 'value': '-1', 'label': 'All', 'selected': 'selected' },
        { 'value': '0', 'label': 'Unrated' },
        { 'value': '1', 'label': 'Safe for Work' },
        { 'value': '2', 'label': 'Borderline' },
        { 'value': '3', 'label': 'Not Safe for Work' }
    ];

    constructor(private router: Router, seoService: SeoService) {
        seoService.setTitle('Search for some wallpapers');
        seoService.setMetaDescription(
            'Search for some wallpapers. We index reddit, 4chan, and 7chan wallpaper boards');
        seoService.setMetaRobots('Index, Follow');
        this.search = new SearchRequest(
            '-1',
            '1280x720',
            '1',
            '9999',
            '',
            'Any Ratio',
            '0');
        this.ratio('1280x720');
    }

    ngOnInit() {}

    processQuerySearch() {
        const newUrl =
            `/search/${this.search.Criteria}/${this.search.Source}/${
                this.search.Resolution}/${this.search.ResolutionSearch}/${this
                .search.Class}/${this.search.Ratio}/${this.search.Size}`;
        this.router.navigate([newUrl]);
    }

    processQueryRandom() {
        const newUrl = '/random';

        this.router.navigate([newUrl]);
    }

    private gcd(a, b) {
        return (b === 0) ? a : this.gcd(b, a % b);
    };

    ratio(resolution) {
        if (resolution === '') {
            this.search.Ratio = 'Any Ratio';
        }

        const x = parseInt(resolution.split('x')[0], 10);
        const y = parseInt(resolution.split('x')[1], 10);

        const r = this.gcd(x, y);
        this.search.Ratio = (x / r) + ':' + (y / r);

    }
}
