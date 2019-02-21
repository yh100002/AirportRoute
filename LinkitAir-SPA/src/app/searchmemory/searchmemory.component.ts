import { Component, OnInit } from '@angular/core';
import { MemorysearchService } from '../_services/memorysearch.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Pagination } from '../_models/pagination';
import { Airport } from '../_models/airport';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-searchmemory',
  templateUrl: './searchmemory.component.html',
  styleUrls: ['./searchmemory.component.css']
})
export class SearchmemoryComponent implements OnInit {
  model: any = {};
  airports: Airport[];
  routeParams: any = {};
  pagination: Pagination;
  sourceAirport: Airport;
  destinationAirport: Airport;  

  constructor(private memorysearchService: MemorysearchService, private alertify: AlertifyService, private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {      
      this.airports = data['airports'];   
      
    });    

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

