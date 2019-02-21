import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Route } from '../_models/route';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Airport } from '../_models/airport';


@Injectable({
  providedIn: 'root'
})
export class ElasticsearchService {
  baseUrl = environment.apiUrl + 'searchelastic/';

  constructor(private http: HttpClient) {}

  
searchAirport(airport, isOrigin): Observable<Airport[]> {
  const params = new HttpParams()
    .set('airportName', airport)
    .set('isOrigin', isOrigin);
    
  return this.http.get<Airport[]>(this.baseUrl + 'autocomplete', {params});  
}

}
