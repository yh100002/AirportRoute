import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot, Route, RouterStateSnapshot} from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MemorysearchService } from '../_services/memorysearch.service';

@Injectable()
export class SearchRouteResolver implements Resolve<Route[]> {


    constructor(private memorysearchService: MemorysearchService, private router: Router, private alertify: AlertifyService) {}


    resolve(activatedRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Route[]> {        
        console.log('SearchRouteResolver :activatedRoute.params:' + activatedRoute.params['sourceAirportId']);              
        return this.memorysearchService.searchRoute(activatedRoute.params['sourceAirportId'], activatedRoute.params['destinationAirportId']).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );   
    }
}

