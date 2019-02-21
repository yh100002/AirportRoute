import { Component, OnInit } from '@angular/core';
import { Route } from '../_models/route';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '../../../node_modules/@angular/router';
import { MemorysearchService } from '../_services/memorysearch.service';
import { Pagination, PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-searchlist',
  templateUrl: './searchlist.component.html',
  styleUrls: ['./searchlist.component.css']
})
export class SearchlistComponent implements OnInit {
  routes: Route[];
  routeParams: any = {};
  pagination: Pagination;

  constructor(private memorysearchService: MemorysearchService, private alertify: AlertifyService, private activatedRoute: ActivatedRoute) { }
  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.routes = data['routes'].result;
      this.pagination = data['routes'].pagination;      
    });
  }

pageChanged(event: any): void {
  this.pagination.currentPage = event.page;
  this.loadAll();  
}

loadAll() {
  console.log('loadRoutes');
  this.memorysearchService.searchAllRoute(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<Route[]>) => {
      this.routes = res.result;
      this.pagination = res.pagination;
  }, error => {
    this.alertify.error(error);
  });
}

}
