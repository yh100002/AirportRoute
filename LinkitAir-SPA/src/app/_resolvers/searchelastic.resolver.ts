import {Injectable} from '@angular/core';
import {Route} from '../_models/route';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ElasticsearchService } from '../_services/elasticsearch.service';
import { Airport } from '../_models/airport';

@Injectable()
export class SearchElasticResolver implements Resolve<Airport[]> {
    pageNumber = 1;
    pageSize = 12;

    constructor(private elasticsearchService: ElasticsearchService, private router: Router, private alertify: AlertifyService) {}


    resolve(model: ActivatedRouteSnapshot): Observable<Airport[]> {
        console.log('SearchSourceAirportResolver');
        console.log(model.params['airportname']);
        return this.elasticsearchService.searchAirport(model.params['airportname'], true).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
