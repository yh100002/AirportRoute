import {Injectable} from '@angular/core';
import {Route} from '../_models/route';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MemorysearchService } from '../_services/memorysearch.service';

@Injectable()
export class PurchaseResolver implements Resolve<Route> {
    constructor(private memorysearchService: MemorysearchService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Route> {
        return this.memorysearchService.getRoute(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/search']);
                return of(null);
            })
        );
    }
}
