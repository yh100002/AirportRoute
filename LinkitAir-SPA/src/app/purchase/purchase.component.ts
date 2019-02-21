import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '../../../node_modules/@angular/router';
import { Route } from '../_models/route';
import { MemorysearchService } from '../_services/memorysearch.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.css']
})
export class PurchaseComponent implements OnInit {
 route: Route;
  constructor(private authService: AuthService, private memorysearchService: MemorysearchService, private alertify: AlertifyService, private activatedRoute: ActivatedRoute) { }
 ngOnInit() {
    this.activatedRoute.data.subscribe(data => {this.route = data['route']; });
  }   

}
