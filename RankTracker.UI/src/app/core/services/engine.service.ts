import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Engine } from '../models/engine.model';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class EngineService {
  private readonly url = `${environment.apiBaseUrl}/api/Engines`;

  constructor(private http: HttpClient) {}

  getEngines(): Observable<Engine[]> {
    return this.http.get<Engine[]>(this.url);
  }
}
