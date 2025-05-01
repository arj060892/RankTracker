SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA config;
GO
CREATE SCHEMA ranking;
GO

CREATE TABLE config.SearchEngines
(
    EngineId   INT           IDENTITY(1,1) PRIMARY KEY,
    Name       NVARCHAR(50)  NOT NULL UNIQUE
);
GO

CREATE TABLE ranking.SearchHistory
(
    HistoryId   BIGINT        IDENTITY(1,1) PRIMARY KEY,
    EngineId    INT           NOT NULL REFERENCES config.SearchEngines(EngineId),
    QueryText   NVARCHAR(500) NOT NULL,
    TargetUrl   NVARCHAR(500) NOT NULL,
    CheckedAt   DATETIME2(3)  NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE ranking.SearchHistoryPosition
(
    PositionId  BIGINT   IDENTITY(1,1) PRIMARY KEY,
    HistoryId   BIGINT   NOT NULL REFERENCES ranking.SearchHistory(HistoryId) ON DELETE CASCADE,
    Position    TINYINT  NOT NULL
);
GO

CREATE NONCLUSTERED INDEX IX_SearchHistory_Engine_Query_Url_CheckedAt
    ON ranking.SearchHistory(EngineId, QueryText, TargetUrl, CheckedAt);
GO

CREATE NONCLUSTERED INDEX IX_SHP_HistoryId
    ON ranking.SearchHistoryPosition(HistoryId);
GO

CREATE NONCLUSTERED INDEX IX_SHP_Position
    ON ranking.SearchHistoryPosition(Position);
GO

CREATE PROCEDURE config.GetSearchEngines
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EngineId, Name FROM config.SearchEngines WITH (NOLOCK);
END
GO

CREATE PROCEDURE ranking.InsertSearchHistory
    @EngineName NVARCHAR(50),
    @QueryText  NVARCHAR(500),
    @TargetUrl  NVARCHAR(500),
    @Positions  NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @EngineId INT = (SELECT EngineId FROM config.SearchEngines WHERE Name = @EngineName);
    
    INSERT INTO ranking.SearchHistory (EngineId, QueryText, TargetUrl)
    VALUES (@EngineId, @QueryText, @TargetUrl);
    
    DECLARE @HistoryId BIGINT = SCOPE_IDENTITY();
    
    INSERT INTO ranking.SearchHistoryPosition (HistoryId, Position)
    SELECT @HistoryId, CAST(value AS TINYINT)
    FROM STRING_SPLIT(@Positions, ',');
END
GO

CREATE PROCEDURE ranking.GetSearchTrends
    @EngineName NVARCHAR(50),
    @QueryText  NVARCHAR(500),
    @TargetUrl  NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT
        sh.HistoryId,
        sh.CheckedAt,
        (
            SELECT STRING_AGG(CAST(sht.Position AS NVARCHAR(3)), ',') 
            FROM ranking.SearchHistoryPosition sht WITH (NOLOCK)
            WHERE sht.HistoryId = sh.HistoryId
        ) AS Positions
    FROM ranking.SearchHistory sh WITH (NOLOCK)
    JOIN config.SearchEngines se WITH (NOLOCK) ON se.EngineId = sh.EngineId
    WHERE se.Name = @EngineName
        AND sh.QueryText = @QueryText
        AND sh.TargetUrl = @TargetUrl
    ORDER BY sh.CheckedAt;
END
GO