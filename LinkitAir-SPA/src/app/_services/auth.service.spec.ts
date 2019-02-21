/* tslint:disable:no-unused-variable */

import { TestBed, async, inject, getTestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('Service: Auth', () => {
  let authService: AuthService;
  let injector: TestBed;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });

    injector = getTestBed();
    authService = injector.get(AuthService);
    httpMock = injector.get(HttpTestingController);

  });

  it('should return true from loggedIn when there is a token', inject([AuthService], (service: AuthService) => {
    expect(authService.loggedIn()).toBeTruthy();
  }));


  afterEach(() => {
    httpMock.verify();
  });

});

