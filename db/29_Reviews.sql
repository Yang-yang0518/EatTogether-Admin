USE [EatTogetherDB]
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE [dbo].[Reviews] (
        [Id]        INT IDENTITY(1,1) NOT NULL,
        [DishId]    INT NOT NULL,
        [Nickname]  NVARCHAR(20) NOT NULL,
        [Content]   NVARCHAR(200) NOT NULL,
        [CreatedAt] DATETIME2(0) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Reviews_Dishes] FOREIGN KEY ([DishId]) REFERENCES [dbo].[Dishes] ([Id])
    );
END
GO