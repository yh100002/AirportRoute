/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SearchelasticComponent } from './searchelastic.component';

describe('SearchelasticComponent', () => {
  let component: SearchelasticComponent;
  let fixture: ComponentFixture<SearchelasticComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchelasticComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchelasticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
