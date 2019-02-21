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
export class MemorysearchService {
baseUrl = environment.apiUrl + 'searchmemory/';

constructor(private http: HttpClient) {}

searchAllRoute(page?, itemsPerPage?): Observable<PaginatedResult<Route[]>> {
  const paginatedResult: PaginatedResult<Route[]> = new PaginatedResult<Route[]>();
  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }  
  
  return this.http.get<Route[]>(this.baseUrl + 'searchallroute', { observe: 'response', params })
    .pipe(
      map(response => {
        paginatedResult.result = response.body;        
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
}

getRoute(id): Observable<Route> {
  return this.http.get<Route>(this.baseUrl + id);
}

searchSourceAirport(airport, isOrigin): Observable<Airport[]> {
  const params = new HttpParams()
    .set('airportName', airport)
    .set('isOrigin', isOrigin);
    
  return this.http.get<Airport[]>(this.baseUrl + 'searchsourceairport', {params});  
}


searchRoute(sourceId, destId): Observable<Route[]>
{
  const params = new HttpParams()
    .set('SourceAirportId', sourceId)
    .set('DestinationAirportId', destId);

  return this.http.get<Route[]>(this.baseUrl + 'searchroute', {params});  
}

}
