import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { PagesModule } from '../pages.module';

import { SearchComponent } from './search.component';

describe(
    'SearchComponent',
    () => {
        let component: SearchComponent;
        let fixture: ComponentFixture<SearchComponent>;

        beforeEach(
            async(
                () => {
                    TestBed.configureTestingModule(
                            {
                                imports: [
                                    RouterTestingModule,
                                    CoreModule,
                                    SharedModule,
                                    PagesModule
                                ]
                            })
                        .compileComponents();
                }));

        beforeEach(
            () => {
                fixture = TestBed.createComponent(SearchComponent);
                component = fixture.componentInstance;
                fixture.detectChanges();
            });

        it(
            'should be created',
            () => {
                expect(component).toBeTruthy();
            });
    });
