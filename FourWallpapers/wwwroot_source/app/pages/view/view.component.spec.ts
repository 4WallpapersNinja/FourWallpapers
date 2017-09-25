import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { PagesModule } from '../pages.module';

import { ViewComponent } from './view.component';

describe(
    'ViewComponent',
    () => {
        let component: ViewComponent;
        let fixture: ComponentFixture<ViewComponent>;

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
                fixture = TestBed.createComponent(ViewComponent);
                component = fixture.componentInstance;
                fixture.detectChanges();
            });

        it(
            'should be created',
            () => {
                expect(component).toBeTruthy();
            });
    });
