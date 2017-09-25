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

import { SearchService } from './search.service';
import { SearchRequest } from '@app/core';

describe(
    'SearchService',
    () => {
        beforeEach(
            () => {
                TestBed.configureTestingModule(
                    {
                        imports: [
                            CoreModule
                        ],
                        providers: [
                            SearchService,
                            { provide: XHRBackend, useClass: MockBackend }
                        ]
                    });
            });

        it(
            'should be created',
            inject(
                [SearchService],
                (service: SearchService) => {
                    expect(service).toBeTruthy();
                }));

        describe(
            'get()',
            () => {

                it(
                    'should return an SearchResult[]',
                    inject(
                        [SearchService, XHRBackend],
                        (searchService, mockBackend) => {

                            const mockResponse = [
                                {
                                    "imageId": "1",
                                    "fileExtension": "png",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }, {
                                    "imageId": "2",
                                    "fileExtension": "png",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }, {
                                    "imageId": "3",
                                    "fileExtension": "jpg",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }, {
                                    "imageId": "4",
                                    "fileExtension": "jpg",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }
                            ];

                            const request = new SearchRequest(
                                '-1',
                                '1280x720',
                                '1',
                                '9999',
                                '',
                                'Any Ratio',
                                '0');

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

                            searchService.get(request)
                                .subscribe(
                                    search => {
                                        {
                                            expect(search.length).toBe(4);
                                            expect(search[0].imageId)
                                                .toEqual('1');
                                            expect(search[1].imageId)
                                                .toEqual('2');
                                            expect(search[2].imageId)
                                                .toEqual('3');
                                            expect(search[3].imageId)
                                                .toEqual('4');
                                        }
                                    },
                                    error => this.errorMessage = (error as any)
                                );
                        }));
            });

        describe(
            'random()',
            () => {

                it(
                    'should return an SearchResult[]',
                    inject(
                        [SearchService, XHRBackend],
                        (searchService, mockBackend) => {

                            const mockResponse = [
                                {
                                    "imageId": "1",
                                    "fileExtension": "png",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }, {
                                    "imageId": "2",
                                    "fileExtension": "png",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                }
                            ];

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

                            searchService.random()
                                .subscribe(
                                    random => {
                                        {
                                            expect(random.length).toBe(2);
                                            expect(random[0].imageId)
                                                .toEqual('1');
                                            expect(random[1].imageId)
                                                .toEqual('2');
                                        }
                                    },
                                    error => this.errorMessage = (error as any)
                                );
                        }));
            });
    });
