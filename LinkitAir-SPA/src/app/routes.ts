import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { PurchaseComponent } from './purchase/purchase.component';
import { PurchaseResolver } from './_resolvers/purchase.resolver';
import { SearchMemoryResolver } from './_resolvers/searchmemory.resolver';
import { SearchSourceAirportResolver } from './_resolvers/searchsourceairport.resolver';
import { SearchlistComponent } from './searchlist/searchlist.component';
import { SearchmemoryComponent } from './searchmemory/searchmemory.component';
import { RouteListComponent } from './RouteList/routelist.component';
import { SearchRouteResolver } from './_resolvers/searchroute.resolver';
import { AdminComponent } from './admin/admin.component';
import { SearchElasticResolver } from './_resolvers/searchelastic.resolver';
import { SearchelasticComponent } from './searchelastic/searchelastic.component';

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], //Now the guard can be applied to any routes.
        children: [         

            {path: 'searchmemory/searchairport', component: SearchmemoryComponent},
            {path: 'searchmemory/searchroute', component: RouteListComponent, resolve: {routes: SearchRouteResolver}},       
            {path: 'searchmemory/searchallroute', component: SearchlistComponent, resolve: {routes: SearchMemoryResolver}},     
            {path: 'searchmemory/searchsourceairport/:airportname', component: SearchmemoryComponent, resolve: {airports: SearchSourceAirportResolver}},
            {path: 'purchase/:id', component: PurchaseComponent,  resolve: {route: PurchaseResolver}},

            {path: 'searchelastic/searchairport', component: SearchelasticComponent},
            {path: 'searchelastic/autocomplete/:airportname', component: SearchelasticComponent, resolve: {airports: SearchElasticResolver}},

            {path: 'admin', component: AdminComponent }          

        ]
    },
    {path: '**', redirectTo: 'home', pathMatch: 'full'},
];
