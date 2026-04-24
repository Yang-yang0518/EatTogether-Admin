USE EatTogetherDB;
GO

-- =============================================
-- 自動生成 MemberFavorites 資料
-- 對象：所有正常會員 (IsBlacklisted=0, IsDeleted=0, IsConfirmed=1)
-- 數量：每人隨機 1~3 個收藏品
-- =============================================

-- 1. 定義暫存表存放正常會員 ID
DECLARE @ActiveMembers TABLE (MemberId INT);
INSERT INTO @ActiveMembers (MemberId)
SELECT Id FROM dbo.Members 
WHERE IsBlacklisted = 0 
  AND IsDeleted = 0 
  AND IsConfirmed = 1;

-- 2. 定義暫存表存放可用的產品 ID
DECLARE @AvailableProducts TABLE (ProductId INT);
INSERT INTO @AvailableProducts (ProductId)
SELECT Id FROM dbo.Products;

-- 3. 使用游標或迴圈為每個會員插入隨機收藏
DECLARE @CurrentMemberId INT;
DECLARE Member_Cursor CURSOR FOR SELECT MemberId FROM @ActiveMembers;

OPEN Member_Cursor;
FETCH NEXT FROM Member_Cursor INTO @CurrentMemberId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- 隨機決定該會員要收藏幾個 (1~3個)
    DECLARE @FavoriteCount INT = FLOOR(RAND() * 3) + 1;

    -- 插入隨機產品 (確保不重複收藏同一個 ProductId)
    INSERT INTO [dbo].[MemberFavorites] ([MemberId], [ProductId], [CreatedAt])
    SELECT TOP (@FavoriteCount) 
        @CurrentMemberId, 
        ProductId, 
        DATEADD(MINUTE, -FLOOR(RAND() * 10000), GETDATE()) -- 隨機產生過去的時間
    FROM @AvailableProducts
    ORDER BY NEWID(); -- 隨機排序

    FETCH NEXT FROM Member_Cursor INTO @CurrentMemberId;
END

CLOSE Member_Cursor;
DEALLOCATE Member_Cursor;
GO