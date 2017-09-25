import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { PagesModule } from '../pages.module';

import { TopComponent } from './top.component';

describe(
    'TopComponent',
    () => {
        let component: TopComponent;
        let fixture: ComponentFixture<TopComponent>;

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
                fixture = TestBed.createComponent(TopComponent);
                component = fixture.componentInstance;
                fixture.detectChanges();
            });

        it(
            'should be created',
            () => {
                expect(component).toBeTruthy();
            });
    });
