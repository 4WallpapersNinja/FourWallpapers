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

import { ViewService } from './view.service';

describe(
    'ViewService',
    () => {
        beforeEach(
            () => {
                TestBed.configureTestingModule(
                    {
                        imports: [
                            CoreModule
                        ],
                        providers: [
                            ViewService,
                            { provide: XHRBackend, useClass: MockBackend }
                        ]
                    });
            });

        it(
            'should be created',
            inject(
                [ViewService],
                (service: ViewService) => {
                    expect(service).toBeTruthy();
                }));

        describe(
            'get()',
            () => {

                it(
                    'should return an SearchResult[]',
                    inject(
                        [ViewService, XHRBackend],
                        (viewService, mockBackend) => {

                            const mockResponse = {
                                "imageId": "testimage",
                                "filename": null,
                                "fileExtension": "jpg",
                                "classification": "Safe for work",
                                "indexSource": "/wg/",
                                "who": "Anonymous",
                                "tripcode": null,
                                "resolution": "1680x1050",
                                "tags": "fork bomb",
                                "dateDownloaded": "1900-01-01T00:00:00",
                                "reported": 0,
                                "ratio": "8:5",
                                "downloads": 6731,
                                "server": "001"
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
                            viewService.get("testimage")
                                .subscribe(
                                    image => {
                                        {
                                            expect(image.imageId)
                                                .toEqual('testimage');
                                            expect(image.fileExtension)
                                                .toEqual('jpg');
                                            expect(image.ratio).toEqual('8:5');
                                            expect(image.server).toEqual('001');
                                        }
                                    },
                                    error => this.errorMessage = (error as any)
                                );
                        }));
            });
    });
