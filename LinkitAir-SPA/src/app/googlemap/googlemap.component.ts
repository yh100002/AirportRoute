/// <reference types="@types/googlemaps" />
import { Component, OnInit, Input} from '@angular/core';
import { ViewChild } from '@angular/core';
import { Route } from '../_models/route';


@Component({
  selector: 'app-googlemap',
  templateUrl: './googlemap.component.html',
  styleUrls: ['./googlemap.component.css']
})
export class GooglemapComponent implements OnInit {
  @Input() route: Route;
  @ViewChild('gmap') gmapElement: any;
  map: google.maps.Map;  
  sourceLat: any;
  sourceLon: any;
  destinationLat: any;
  destinationLon: any;  

  ngOnInit() {   
    this.sourceLat = this.route.sourceLat;
    this.sourceLon = this.route.sourceLon;  
    this.destinationLat = this.route.destinationLat;
    this.destinationLon =this.route.destinationLon;    
  }
  ngAfterContentInit() {   
    
    let mapProp = {
      center: new google.maps.LatLng(this.sourceLat, this.sourceLon),
      zoom: 4,
      mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
    let flightPlanCoordinates = [
      {lat: this.sourceLat, lng: this.sourceLon},
      {lat: this.destinationLat, lng: this.destinationLon}     
    ];
    
    let flightPath = new google.maps.Polyline({
      path: flightPlanCoordinates,
      geodesic: true,
      strokeColor: '#FF0000',
      strokeOpacity: 1.0,
      strokeWeight: 2
    });    

    flightPath.setMap(this.map);    

  }
}

