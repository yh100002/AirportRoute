import { Component, OnInit } from '@angular/core';
import { Airport } from '../_models/airport';
import { Pagination } from '../_models/pagination';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-searchelastic',
  templateUrl: './searchelastic.component.html',
  styleUrls: ['./searchelastic.component.css']
})
export class SearchelasticComponent implements OnInit {
  model: any = {};
  airports: Airport[];
  routeParams: any = {};
  pagination: Pagination;
  sourceAirport: Airport;
  destinationAirport: Airport;  
  
  constructor(private alertify: AlertifyService, private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {this.airports = data['airports']; });
  }

  
  sourceSelected($event) {
    this.sourceAirport = $event;    
  }

  destinationSelected($event) {
    this.destinationAirport = $event;    
  }

  getRoute()
  {   
    this.router.navigate(['/searchmemory/searchroute', { sourceAirportId: this.sourceAirport.airportId, destinationAirportId: this.destinationAirport.airportId}]);
    
  }

}
