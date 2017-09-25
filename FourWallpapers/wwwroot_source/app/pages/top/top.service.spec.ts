import { TestBed, inject } from '@angular/core/testing';

import { CoreModule } from '@app/core';

import { TopService } from './top.service';

describe(
    'TopService',
    () => {
        beforeEach(
            () => {
                TestBed.configureTestingModule(
                    {
                        imports: [
                            CoreModule
                        ],
                        providers: [
                            TopService
                        ]
                    });
            });

        it(
            'should be created',
            inject(
                [TopService],
                (service: TopService) => {
                    expect(service).toBeTruthy();
                }));
    });
