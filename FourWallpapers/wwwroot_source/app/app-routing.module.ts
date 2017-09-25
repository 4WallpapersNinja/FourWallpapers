import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '**',
        redirectTo: ''
    }
];

@NgModule(
    {
        // useHash supports github.io demo page, remove in your app
        imports: [RouterModule.forRoot(routes)],
        exports: [RouterModule]
    })
export class AppRoutingModule {}
