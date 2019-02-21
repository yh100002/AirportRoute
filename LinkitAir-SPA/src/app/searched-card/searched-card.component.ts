import { Component, OnInit, Input } from '@angular/core';
import { Route } from '../_models/route';

@Component({
  selector: 'app-searched-card',
  templateUrl: './searched-card.component.html',
  styleUrls: ['./searched-card.component.css']
})
export class SearchedCardComponent implements OnInit {
@Input() route: Route;

  constructor() { }

  ngOnInit() {
  }

}
