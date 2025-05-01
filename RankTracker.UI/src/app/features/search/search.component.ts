import { Component, OnInit } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
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
  positions: number[] = [];
  errorMessage: string | null = null;

  private hasShownCaptchaHelp = false;

  constructor(
    private engineService: EngineService,
    private searchService: SearchService
  ) {}

  ngOnInit() {
    this.engineService.getEngines().subscribe((list) => {
      this.engines = list;
    });

    this.hasShownCaptchaHelp =
      sessionStorage.getItem('hasShownCaptchaHelp') === 'true';
  }

  onSearchButtonClick(event: Event, popover: NgbPopover) {
    if (!this.hasShownCaptchaHelp) {
      popover.open();
      this.hasShownCaptchaHelp = true;
      sessionStorage.setItem('hasShownCaptchaHelp', 'true');

      setTimeout(() => popover.close(), 15000);
    }
    this.onSearch();
  }

  onSearch() {
    if (!this.engine || !this.query || !this.url) return;

    this.loading = true;
    this.positions = [];
    this.errorMessage = null;

    this.searchService
      .search({ engine: this.engine, query: this.query, url: this.url })
      .subscribe(
        (res) => {
          this.positions = res.positions ?? [];
          this.loading = false;
        },
        (err) => {
          this.errorMessage =
            err.error?.message || 'Server error – please try again.';
          this.loading = false;
        }
      );
  }
}
