import { Component } from '@angular/core';
import { ApiClient } from './api-client';
import { Observable } from 'rxjs';
import { Supplement } from './models/response/supplement';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  supplements$: Observable<Supplement[]>;

  constructor(private api: ApiClient) {
    this.supplements$ = api.getSupplementList();
  }
}
