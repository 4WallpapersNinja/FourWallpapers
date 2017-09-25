import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';

import { PagesModule } from '../pages.module';

import { RandomComponent } from './random.component';

describe(
    'RandomComponent',
    () => {
        let component: RandomComponent;
        let fixture: ComponentFixture<RandomComponent>;

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
                fixture = TestBed.createComponent(RandomComponent);
                component = fixture.componentInstance;
                fixture.detectChanges();
            });

        it(
            'should be created',
            () => {
                expect(component).toBeTruthy();
            });
    });
