import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { PagesModule } from '../pages.module';

import { HomeComponent } from './home.component';

describe(
    'HomeComponent',
    () => {
        let component: HomeComponent;
        let fixture: ComponentFixture<HomeComponent>;

        beforeEach(
            async(
                () => {
                    TestBed.configureTestingModule(
                            {
                                imports: [
                                    NoopAnimationsModule,
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
                fixture = TestBed.createComponent(HomeComponent);
                component = fixture.componentInstance;
                fixture.detectChanges();
            });

        it(
            'should be created',
            () => {
                expect(component).toBeTruthy();
            });
    });
