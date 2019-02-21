import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  baseUrl = environment.apiUrl + 'logview/Overall/';
  kibanaUrl = environment.kibanaUrl;
  data: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get(this.baseUrl).subscribe(res => {
      this.data = res;
    });    
  }

  getAggreagatedLogs () {
    return JSON.stringify(this.data, null, 2);
  }

}
