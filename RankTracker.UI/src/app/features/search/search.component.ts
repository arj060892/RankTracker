import { Component, OnInit } from '@angular/core';
import { Engine } from 'src/app/core/models/engine.model';
import { EngineService } from 'src/app/core/services/engine.service';
import { SearchService } from 'src/app/core/services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
})
export class SearchComponent implements OnInit {
  engines: Engine[] = [];
  engine = 'google';
  query = '';
  url = '';
  loading = false;
  positions: number[] = [0];
  errorMessage: string | null = null;

  constructor(
    private engineService: EngineService,
    private searchService: SearchService
  ) {}

  ngOnInit() {
    this.engineService.getEngines().subscribe((list) => {
      this.engines = list;
    });
  }

  onSearch() {
    if (!this.engine || !this.query || !this.url) return;

    this.loading = true;
    this.positions = [];

    this.searchService
      .search({ engine: this.engine, query: this.query, url: this.url })
      .subscribe(
        (res) => {
          this.positions = res.positions ?? [];
          this.loading = false;
        },
        (err) => {
          this.errorMessage = err.error?.message
            ? err.error.message
            : 'Server error – please try again.';
          this.loading = false;
        },
        () => {
          this.loading = false;
        }
      );
  }
}
