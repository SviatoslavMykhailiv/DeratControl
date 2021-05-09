import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Supplement } from './models/response/supplement';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: "root" })
export class ApiClient {
  constructor(private httpClient: HttpClient) { }

  getSupplementList(): Observable<Supplement[]> {
    return this.httpClient.get<Supplement[]>(`${environment.apiUrl}/supplements`);
  }
}
