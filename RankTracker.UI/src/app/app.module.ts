import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { SearchComponent } from './features/search/search.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TrendDashboardComponent } from './features/trend-dashboard/trend-dashboard.component';

@NgModule({
  declarations: [AppComponent, SearchComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    TrendDashboardComponent,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
