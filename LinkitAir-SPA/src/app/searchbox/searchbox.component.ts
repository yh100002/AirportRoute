import {Component, EventEmitter, Injectable, Output, Input} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable, of} from 'rxjs';
import {catchError, debounceTime, distinctUntilChanged, map, tap, switchMap} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Airport } from '../_models/airport';

const URL = environment.apiUrl + 'searchmemory/searchdestinationairport/';

@Injectable()
export class RemoteQueryService {
  constructor(private http: HttpClient) {}
  
  search(airport, isOrigin): Observable<Airport[]> {
    if (airport === '') {
      return of([]);
    }
    const params = new HttpParams()
    .set('airportName', airport)
    .set('isOrigin', isOrigin);    
    return this.http.get<Airport[]>(URL, {params});
  }

  private extractData(res: Response) {
    const body = res.json();
    console.log(body);
    return body || { };
  }

}

@Component({
  selector: 'app-searchbox',
  templateUrl: './searchbox.component.html',
  providers: [RemoteQueryService],
  styles: [`.form-control { width: 300px; display: inline; }`]
})
export class SearchboxComponent {
  model: any;
  searching = false;
  searchFailed = false;
  didSelected = false;
  @Output() selectedOutput = new EventEmitter<Airport>();

  constructor(private _service: RemoteQueryService) {}
  formatMatches = (value: any) => value.airportName || '';
  
  search = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term =>
        this._service.search(term, true).pipe(
          tap(() => this.searchFailed = false),
          catchError(() => {
            this.searchFailed = true;
            return of([]);
          }))
      ),
      tap(() => this.searching = false)      
    )

    selected() {
      console.log('selected');
      this.selectedOutput.emit(this.model);      
    }

}
