-- 28_SchedulerLogs.sql
USE EatTogetherDB;
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SchedulerLogs')
BEGIN
    CREATE TABLE SchedulerLogs (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        ExecutedAt     DATETIME NOT NULL DEFAULT GETDATE(),
        DishesEnabled  INT NOT NULL DEFAULT 0,
        DishesDisabled INT NOT NULL DEFAULT 0,
        MealsEnabled   INT NOT NULL DEFAULT 0,
        MealsDisabled  INT NOT NULL DEFAULT 0,
        TriggerType    NVARCHAR(10) NOT NULL DEFAULT N'自動',
        DetailJson     NVARCHAR(MAX) NULL
    );
END
GO
