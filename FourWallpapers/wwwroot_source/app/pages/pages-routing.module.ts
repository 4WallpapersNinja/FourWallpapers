import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

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

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'search', component: SearchComponent },
    { path: 'about', component: AboutComponent },
    { path: 'technical', component: TechnicalComponent },
    { path: 'disclaimer', component: DisclaimerComponent },
    { path: 'view/:imageId', component: ViewComponent },
    {
        path:
            'search/:criteria/:source/:resolution/:resolutionSearch/:classification/:ratio/:size',
        component: SearchComponent
    },
    {
        path:
            'search/:source/:resolution/:resolutionSearch/:classification/:ratio/:size',
        component: SearchComponent
    },
    { path: 'stats', component: StatsComponent },
    { path: 'top/:by/:value', component: TopComponent },
    { path: 'top', component: TopComponent },
    { path: 'recent', component: RecentComponent },
    { path: 'random', component: RandomComponent }
];

@NgModule(
    {
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
    })
export class PagesRoutingModule {}
