# RankTracker

![.NET 8 + Angular](https://img.shields.io/badge/.NET%208+Angular-15-blue)

RankTracker is a comprehensive SEO ranking analysis tool that automatically checks search engine results for specified keywords and tracks your website’s ranking positions over time.

## Features

- **Search Engine Scraping**  
  Checks Google and Bing (extensible to Yahoo) for URL ranking positions.  
- **Historical Tracking**  
  Persists daily search results in SQL Server.  
- **Ranking Over Time Dashboard**  
  Visualizes how your positions change day-to-day.  
- **Multi-engine Support**  
  Compare rankings across multiple search engines.  
- **Caching Layer**  
  In-memory cache prevents IP blocking by throttling repeat requests within 5 minutes.

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Quick Start (Recommended)

1. Run (once) `setup-containers.bat` to:
   - Launch SQL Server in a container
   - Initialize the database
   - Build and run the Angular UI

2. Run `run-app.bat` to start the .NET API

3. Access the application at [http://localhost:51420](http://localhost:51420)


### Manual Setup (Alternative)

1. **Open Solution**:
   - Launch Visual Studio
   - Open `RankTracker.sln` solution file

2. **Build Solution**:
   - Run full solution build (Ctrl+Shift+B)
   - This will:
     - Restore all NuGet packages
     - Install Angular dependencies (`node_modules`)
     - Build all projects

3. **Database Setup**:
   - In Solution Explorer, navigate to `src/infra/RankTracker.DbDeploy`
   - Right-click the project and select "Publish"
   - Complete the database publishing wizard

4. **Run Application**:
   - Select 'RunApp' launch profile & Press F5 or click the Visual Studio play button
   - This will automatically:
     - Start the .NET API backend
     - Launch the Angular frontend
     - Open both in your default browser

## Architecture

```
/RankTracker.sln
│
├── RankTracker.Api/          ← ASP.NET Core Web API  
├── RankTracker.Service/      ← Playwright headless clients & DI setup  
├── RankTracker.Persistence/  ← Dapper repositories  
├── RankTracker.Domain/       ← Domain interfaces  
├── RankTracker.Application/  ← DTOs for requests/responses  
└── RankTracker.DbDeploy/     ← SQL Server database project 
└── RankTracker.UI/           ← Angular application
```


## Technical Stack

- **Backend**: .NET 8 WebAPI
- **Frontend**: Angular 15
- **Database**: SQL Server Express (via Docker)
- **ORM**: Dapper (micro-ORM)
- **Browser Automation**: Playwright (for Google search)

## Important Notes

1. **Google CAPTCHA Challenge**: Due to Google's anti-bot measures, the first search will open a Chromium instance where you must manually complete the CAPTCHA. Subsequent searches within 5 minutes will use cached results.
   
   > Reference: [Google's Automated Query Guidelines](https://developers.google.com/search/docs/advanced/guidelines/automated-queries)

2. **Bing Limitations**: Bing may return fewer than 100 results (capped at 50) depending on server load, despite requesting 100 results.

   > Reference: [Bing API Documentation](https://learn.microsoft.com/en-us/bing/search-apis/bing-web-search/overview)

3. **Development Constraints**: Due to time limitations, unit tests and comprehensive logging were not implemented in this version.

## Implementation Details

- **Caching**: In-memory for 5 minutes per unique `(engine, query, url)` triplet.  
- **Playwright**: Drives Chromium to handle Google’s CAPTCHA once per session.  
- **Dapper**: Simple, fast micro-ORM for all SQL operations.  
- **Angular Services**: Separate services for engines, search and trends in `src/app/core/services`.