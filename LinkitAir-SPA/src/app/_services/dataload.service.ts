import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable()
export class DataInitializer {
  baseUrl = environment.apiUrl + 'dataloader/';

  constructor(private http: HttpClient) {}

  init(model: any) {
    console.log(this.baseUrl);
    return this.http.get(this.baseUrl + 'init');
  }

}
