import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule, TabsModule, PaginationModule, ButtonsModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { NgxJsonViewerModule } from 'ngx-json-viewer';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { SearchedCardComponent } from './searched-card/searched-card.component';
import { PurchaseComponent } from './purchase/purchase.component';
import { PurchaseResolver } from './_resolvers/purchase.resolver';
import { SearchboxComponent } from './searchbox/searchbox.component';
import { SearchMemoryResolver } from './_resolvers/searchmemory.resolver';
import { MemorysearchService } from './_services/memorysearch.service';
import { SearchSourceAirportResolver } from './_resolvers/searchsourceairport.resolver';
import { SearchlistComponent } from './searchlist/searchlist.component';
import { SearchmemoryComponent } from './searchmemory/searchmemory.component';
import { RouteListComponent } from './RouteList/routelist.component';
import { SearchRouteResolver } from './_resolvers/searchroute.resolver';
import { GooglemapComponent } from './googlemap/googlemap.component';
import { AdminComponent } from './admin/admin.component';
import { SearchelasticComponent } from './searchelastic/searchelastic.component';
import { SearchElasticResolver } from './_resolvers/searchelastic.resolver';
import { SearchelasticboxComponent } from './searchelasticbox/searchelasticbox.component';


export function tokenGetter() {
   return localStorage.getItem('token');
 }

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      SearchedCardComponent,
      PurchaseComponent,
      SearchboxComponent,
      SearchlistComponent,
      SearchmemoryComponent,
      RouteListComponent,
      GooglemapComponent,
      AdminComponent,
      SearchelasticComponent,
      SearchelasticboxComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      TabsModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
           tokenGetter: tokenGetter,
           whitelistedDomains: ['localhost:5000'],
           //the authentication endpoint doesn’t need to receive it because there’s no point: The token is typically null when it’s called anyway.
           blacklistedRoutes: ['localhost:5000/api/auth']
         }
       }),
       NgbModule,
       NgxJsonViewerModule
     ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      PurchaseResolver,
      MemorysearchService,
      SearchMemoryResolver,
      SearchSourceAirportResolver,
      SearchRouteResolver,
      SearchElasticResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
