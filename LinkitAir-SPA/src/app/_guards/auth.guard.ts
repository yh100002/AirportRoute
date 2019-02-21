import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';


/*
Angular’s route guards are interfaces which can tell the router whether or not it should allow navigation to a requested route.
They make this decision by looking for a true or false return value from a class which implements the given guard interface.
*/
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) {}

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    this.alertify.error('You must not pass!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
