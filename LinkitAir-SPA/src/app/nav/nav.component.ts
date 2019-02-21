import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { Pagination, PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  search: any = {};
  routes: Route[];
  routeParams: any = {};
  pagination: Pagination;

  // tslint:disable-next-line:max-line-length
  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() { }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/search']);
    });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }  
}
