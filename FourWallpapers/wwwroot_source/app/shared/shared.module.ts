import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { LazyLoadImageModule } from 'ng-lazyload-image';

import {
    MdButtonModule,
    MdToolbarModule,
    MdMenuModule,
    MdSelectModule,
    MdTabsModule,
    MdInputModule,
    MdGridListModule,
    MdChipsModule,
    MdSidenavModule,
    MdCheckboxModule,
    MdCardModule,
    MdListModule,
    MdIconModule,
    MdTooltipModule,
    MdTableModule
    } from '@angular/material';


@NgModule(
    {
        imports: [
            CommonModule,
            FormsModule,
            MdButtonModule,
            MdToolbarModule,
            MdSelectModule,
            MdTabsModule,
            MdInputModule,
            MdGridListModule,
            MdChipsModule,
            MdCardModule,
            MdSidenavModule,
            MdCheckboxModule,
            MdListModule,
            MdMenuModule,
            MdIconModule,
            MdTooltipModule,
            MdTableModule,
            LazyLoadImageModule
        ],
        declarations: [],
        exports: [
            CommonModule,
            FormsModule,
            MdButtonModule,
            MdMenuModule,
            MdTabsModule,
            MdChipsModule,
            MdInputModule,
            MdGridListModule,
            MdCheckboxModule,
            MdCardModule,
            MdSidenavModule,
            MdListModule,
            MdSelectModule,
            MdToolbarModule,
            MdIconModule,
            MdTooltipModule,
            MdTableModule,
            LazyLoadImageModule
        ]
    })
export class SharedModule {}
