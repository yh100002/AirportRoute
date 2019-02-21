import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MemorysearchService } from '../_services/memorysearch.service';
import { Airport } from '../_models/airport';

@Injectable()
export class SearchSourceAirportResolver implements Resolve<Airport[]> {


    constructor(private memorysearchService: MemorysearchService, private router: Router, private alertify: AlertifyService) {}


    resolve(model: ActivatedRouteSnapshot): Observable<Airport[]> {
        console.log('SearchSourceAirportResolver');
        console.log(model.params['airportname']);
        return this.memorysearchService.searchSourceAirport(model.params['airportname'], true).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
