import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TrendEntry } from '../models/trend-entry.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TrendsService {
  private readonly url = `${environment.apiBaseUrl}/api/Trends`;

  constructor(private http: HttpClient) {}

  getTrends(
    engine: string,
    query: string,
    urlToCheck: string
  ): Observable<TrendEntry[]> {
    const params = new HttpParams()
      .set('engine', engine)
      .set('query', query)
      .set('url', urlToCheck);

    return this.http.get<TrendEntry[]>(this.url, { params });
  }
}
