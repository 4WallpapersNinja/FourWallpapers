import { TestBed, inject } from '@angular/core/testing';
import {
    HttpModule,
    Http,
    Response,
    ResponseOptions,
    XHRBackend
    } from '@angular/http';
import { MockBackend } from '@angular/http/testing';

import { CoreModule } from '@app/core';

import { StatsService } from './stats.service';

describe(
    'StatsService',
    () => {
        beforeEach(
            () => {
                TestBed.configureTestingModule(
                    {
                        imports: [
                            CoreModule
                        ],
                        providers: [
                            StatsService,
                            { provide: XHRBackend, useClass: MockBackend }
                        ]
                    });
            });

        it(
            'should be created',
            inject(
                [StatsService],
                (service: StatsService) => {
                    expect(service).toBeTruthy();
                }));

        describe(
            'get()',
            () => {

                it(
                    'should return an SearchResult[]',
                    inject(
                        [StatsService, XHRBackend],
                        (statsService, mockBackend) => {

                            const mockResponse = {
                                "topStats": [
                                    {
                                        "keyword": "Total Images",
                                        "count": 1958220
                                    }, {
                                        "keyword": "Total Images from 4chan",
                                        "count": 1868264
                                    }
                                ],
                                "topKeywords": [
                                    {
                                        "keyword": "anime",
                                        "count": 16558
                                    }, {
                                        "keyword": "blue",
                                        "count": 9677
                                    }
                                ],
                                "asOf": "2017-09-11T01:01:01.0000000-04:00"
                            };

                            mockBackend.connections.subscribe(
                                (connection) => {
                                    connection.mockRespond(
                                        new Response(
                                            new ResponseOptions(
                                                {
                                                    body: JSON.stringify(
                                                        mockResponse)
                                                })));
                                });
                            statsService.get()
                                .subscribe(
                                    stat => {
                                        {
                                            expect(stat.topStats.length)
                                                .toBe(2);
                                            expect(stat.topKeywords.length)
                                                .toBe(2);
                                            expect(stat.topStats[0].count)
                                                .toEqual(1958220);
                                            expect(stat.topKeywords[0].count)
                                                .toEqual(16558);
                                        }
                                    },
                                    error => this.errorMessage = (error as any)
                                );
                        }));
            });
    });
