INSERT INTO config.SearchEngines (Name) VALUES ('google'),('bing');
GO
DECLARE @StartDate DATETIME2(3) = DATEADD(DAY, -90, SYSUTCDATETIME());
DECLARE @EndDate DATETIME2(3) = SYSUTCDATETIME();
DECLARE @GoogleId INT = (SELECT EngineId FROM config.SearchEngines WHERE Name = 'google');
DECLARE @BingId INT = (SELECT EngineId FROM config.SearchEngines WHERE Name = 'bing');
DECLARE @i INT = 0;
DECLARE @QueryText NVARCHAR(500) = 'land registry searches';
DECLARE @TargetUrl NVARCHAR(500) = 'www.infotrack.co.uk';
DECLARE @CheckedAt DATETIME2(3);
DECLARE @HistoryId BIGINT;
DECLARE @PositionCount INT;
DECLARE @j INT;
DECLARE @Position TINYINT;

WHILE @i < 50
BEGIN
    SET @CheckedAt = DATEADD(SECOND, ABS(CHECKSUM(NEWID())) % 7776000, @StartDate);
    
    INSERT INTO ranking.SearchHistory (EngineId, QueryText, TargetUrl, CheckedAt)
    VALUES (@GoogleId, @QueryText, @TargetUrl, @CheckedAt);
    
    SET @HistoryId = SCOPE_IDENTITY();
    SET @PositionCount = ABS(CHECKSUM(NEWID())) % 6 + 1;
    SET @j = 0;
    
    WHILE @j < @PositionCount
    BEGIN
        SET @Position = ABS(CHECKSUM(NEWID())) % 101;
        INSERT INTO ranking.SearchHistoryPosition (HistoryId, Position)
        VALUES (@HistoryId, @Position);
        SET @j = @j + 1;
    END
    
    SET @i = @i + 1;
END

SET @i = 0;

WHILE @i < 50
BEGIN
    SET @CheckedAt = DATEADD(SECOND, ABS(CHECKSUM(NEWID())) % 7776000, @StartDate);
    
    INSERT INTO ranking.SearchHistory (EngineId, QueryText, TargetUrl, CheckedAt)
    VALUES (@BingId, @QueryText, @TargetUrl, @CheckedAt);
    
    SET @HistoryId = SCOPE_IDENTITY();
    SET @PositionCount = ABS(CHECKSUM(NEWID())) % 6 + 1;
    SET @j = 0;
    
    WHILE @j < @PositionCount
    BEGIN
        SET @Position = ABS(CHECKSUM(NEWID())) % 101;
        INSERT INTO ranking.SearchHistoryPosition (HistoryId, Position)
        VALUES (@HistoryId, @Position);
        SET @j = @j + 1;
    END
    
    SET @i = @i + 1;
END