import { Component, OnInit } from '@angular/core';

import { environment as env } from '@env/environment';

@Component(
    {
        selector: 'fourwallpapers-features',
        templateUrl: './disclaimer.component.html',
        styleUrls: ['./disclaimer.component.scss']
    })
export class DisclaimerComponent implements OnInit {

    versions = env.versions;

    ngOnInit() {}

    openLink(link: string) {
        window.open(link, '_blank');
    }

}
