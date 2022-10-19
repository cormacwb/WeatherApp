import { HttpClient } from '@angular/common/http';
import { fakeAsync, TestBed, tick } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    let httpSpy = jasmine.createSpyObj('HttpClient', ['get']);
    await TestBed.configureTestingModule({
      declarations: [
        AppComponent
      ],
      providers: [{
        provide: HttpClient,
        useValue: httpSpy
      }]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Weather App'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('Weather App');
  });

  it('should have select with 3 cities', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const select = fixture.debugElement.nativeElement.querySelector('#city');

    expect(select[0].innerText).toBe('');
    expect(select[1].innerText).toBe('Dublin');
    expect(select[2].innerText).toBe('Vancouver');
    expect(select[3].innerText).toBe('Tokyo');
  });
});
