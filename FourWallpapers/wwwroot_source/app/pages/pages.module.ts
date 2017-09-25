import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared';

import { InfiniteScrollModule } from 'ngx-infinite-scroll';

import { PagesRoutingModule } from './pages-routing.module';
import { AboutComponent } from './about/about.component';
import { DisclaimerComponent } from './disclaimer/disclaimer.component';
import { HomeComponent } from './home/home.component';
import { SearchComponent } from './search/search.component';
import { RandomComponent } from './search/random.component';
import { ViewComponent } from './view/view.component';
import { StatsComponent } from './stats/stats.component';
import { TechnicalComponent } from './technical/technical.component';
import { TopComponent } from './top/top.component';
import { RecentComponent } from './recent/recent.component';

import { ViewService } from './view/view.service';
import { SearchService } from './search/search.service';
import { StatsService } from './stats/stats.service';
import { TopService } from './top/top.service';
import { RecentService } from './recent/recent.service';

@NgModule(
    {
        imports: [
            SharedModule,
            PagesRoutingModule,
            InfiniteScrollModule
        ],
        providers: [
            ViewService,
            SearchService,
            StatsService,
            TopService,
            RecentService
        ],
        declarations: [
            AboutComponent,
            DisclaimerComponent,
            HomeComponent,
            SearchComponent,
            ViewComponent,
            StatsComponent,
            TechnicalComponent,
            TopComponent,
            RecentComponent,
            RandomComponent
        ]
    })
export class PagesModule {}
