import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SearchRequest } from '../models/search-request.model';
import { SearchResult } from '../models/search-result.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private readonly url = `${environment.apiBaseUrl}/api/Search`;

  constructor(private http: HttpClient) {}

  search(request: SearchRequest): Observable<SearchResult> {
    return this.http.post<SearchResult>(this.url, request);
  }
}
