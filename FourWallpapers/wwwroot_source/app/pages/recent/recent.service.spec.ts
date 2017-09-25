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

import { RecentService } from './recent.service';

describe(
    'RecentService',
    () => {
        beforeEach(
            () => {
                TestBed.configureTestingModule(
                    {
                        imports: [
                            CoreModule
                        ],
                        providers: [
                            RecentService,
                            { provide: XHRBackend, useClass: MockBackend }
                        ]
                    });
            });

        it(
            'should be created',
            inject(
                [RecentService],
                (service: RecentService) => {
                    expect(service).toBeTruthy();
                }));

        describe(
            'get()',
            () => {

                it(
                    'should return an SearchResult[]',
                    inject(
                        [RecentService, XHRBackend],
                        (recentService, mockBackend) => {

                            const mockResponse = [
                                {
                                    "imageId": "1",
                                    "fileExtension": "png",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                },
                                {
                                    "imageId": "2",
                                    "fileExtension": "jpg",
                                    "server": "002",
                                    "isThumbnailAvailable": false
                                },
                                {
                                    "imageId": "3",
                                    "fileExtension": "jpg",
                                    "server": "001",
                                    "isThumbnailAvailable": false
                                },
                                {
                                    "imageId": "4",
                                    "fileExtension": "jpg",
                                    "server": "001",
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
                            recentService.get()
                                .subscribe(
                                    recent => {
                                        {
                                            expect(recent.length).toBe(4);
                                            expect(recent[0].imageId)
                                                .toEqual('1');
                                            expect(recent[1].imageId)
                                                .toEqual('2');
                                            expect(recent[2].imageId)
                                                .toEqual('3');
                                            expect(recent[3].imageId)
                                                .toEqual('4');
                                        }
                                    },
                                    error => this.errorMessage = (error as any)
                                );
                        }));
            });
    });
