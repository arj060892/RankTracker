import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgApexchartsModule } from 'ng-apexcharts';
import { TrendEntry } from 'src/app/core/models/trend-entry.model';
import { EngineService } from 'src/app/core/services/engine.service';
import { TrendsService } from 'src/app/core/services/trends.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trend-dashboard',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule, FormsModule],
  templateUrl: './trend-dashboard.component.html',
  styleUrls: [],
})
export class TrendDashboardComponent implements OnInit {
  engines: string[] = [];
  selectedEngine = 'google';

  urlValue = 'www.infotrack.co.uk';
  phraseValue = 'land registry searches';

  rankingData: TrendEntry[] = [];

  loading = false;
  errorMessage: string | null = null;

  public chartOptions: any = {
    chart: {
      type: 'scatter',
      height: 250,
      zoom: { enabled: true },
      toolbar: { show: true },
    },
    colors: [({ value }: any) => (value === 0 ? '#FF4560' : '#3e95cd')],
    series: [{ name: 'Position', data: [] }],
    stroke: { curve: 'smooth', width: 2 },
    markers: { size: 6 },
    xaxis: {
      type: 'datetime',
      labels: {
        datetimeFormatter: {
          year: 'yyyy',
          month: "MMM 'yy",
          day: 'dd MMM',
          hour: 'HH:mm',
        },
      },
    },
    yaxis: { reversed: true, title: { text: 'Position' }, min: 0 },
    tooltip: { x: { format: 'dd MMM yyyy HH:mm' } },
  };

  constructor(
    private engineService: EngineService,
    private trendsService: TrendsService
  ) {}

  ngOnInit(): void {
    this.engineService.getEngines().subscribe((list) => {
      this.engines = list.map((e) => e.name!).filter((n) => !!n);
      if (this.engines.length) {
        this.loadTrends();
      }
    });
  }

  onSearch(): void {
    this.loadTrends();
  }

  private loadTrends(): void {
    if (!this.selectedEngine || !this.phraseValue || !this.urlValue) return;

    this.loading = true;
    this.errorMessage = null;
    this.trendsService
      .getTrends(this.selectedEngine, this.phraseValue, this.urlValue)
      .subscribe({
        next: (entries) => {
          this.rankingData = entries;
          this.updateChart();
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to load trends';
          this.loading = false;
        },
      });
  }

  private updateChart(): void {
    const allPoints = this.rankingData.flatMap((item) =>
      (item.positions || [0]).map((pos) => ({
        x: new Date(item.checkedAt).getTime(),
        y: pos,
      }))
    );
    this.chartOptions = {
      ...this.chartOptions,
      series: [{ name: 'Position', data: allPoints }],
    };
  }

  getAverageStatus() {
    if (!this.rankingData.length) return null;
    const latest = this.rankingData[0];
    const list = latest.positions || [0];
    const sum = list.reduce((a, b) => a + b, 0);
    return {
      position: Math.trunc(sum / list.length),
      date: latest.checkedAt,
    };
  }
}
