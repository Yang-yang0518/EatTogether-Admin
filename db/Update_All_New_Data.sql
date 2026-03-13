-- =============================================
-- 一鍵更新：新增義式分類、湯品、附餐、套餐及關聯
-- =============================================

-- 1. 確保分類存在
IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = N'湯品') INSERT INTO Categories (CategoryName) VALUES (N'湯品');
IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = N'附餐') INSERT INTO Categories (CategoryName) VALUES (N'附餐');
IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = N'套餐') INSERT INTO Categories (CategoryName) VALUES (N'套餐');

DECLARE @SoupId INT = (SELECT Id FROM Categories WHERE CategoryName = N'湯品');
DECLARE @SideId INT = (SELECT Id FROM Categories WHERE CategoryName = N'附餐');

-- 2. 新增 10 種湯品 (若已存在則略過)
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'義式蔬菜礦工湯', 120, N'道地義式蔬菜湯', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「義」鳴驚人', N'義式蔬菜礦工湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'羅宋牛腩湯', 150, N'慢火熬煮羅宋湯', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「羅」曼蒂克', N'羅宋牛腩湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'巧達海鮮濃湯', 130, N'濃郁海鮮風味', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「巧」遇濃情', N'巧達海鮮濃湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'奶油菠菜濃湯', 110, N'絲滑奶油口感', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「菠」光粼粼', N'奶油菠菜濃湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'牛肝菌菇奶油湯', 160, N'精選牛肝菌', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「菌」臨天下', N'牛肝菌菇奶油湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'法式南瓜培根濃湯', 120, N'香甜南瓜風味', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「南」得糊塗', N'法式南瓜培根濃湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'法式洋蔥起司湯', 110, N'經典法式風味', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「洋」洋得意', N'法式洋蔥起司湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'義式茄汁海鮮湯', 180, N'酸甜茄汁底', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「海」誓山盟', N'義式茄汁海鮮湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'義式蒜味蛤蜊清湯', 140, N'清甜蒜香感', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「蛤」哩勒', N'義式蒜味蛤蜊清湯'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SoupId, N'黑松露野菇濃湯', 200, N'奢華松露香', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「松」露你心', N'黑松露野菇濃湯'));

-- 3. 新增 6 種附餐
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'松露起司薯條', 90, N'現炸薯條配起司', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「薯」於你的', N'松露起司薯條'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'義式香料大蒜麵包', 60, N'香氣逼人', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「麵」面俱到', N'義式香料大蒜麵包'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'義式香草烤雞翅', 120, N'香嫩多汁', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「雞」不可失', N'義式香草烤雞翅'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'酥炸墨魚圈', 150, N'海鮮炸物', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「墨」守成規', N'酥炸墨魚圈'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'酥炸洋蔥圈', 80, N'外酥內軟', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「洋」蔥走開', N'酥炸洋蔥圈'));
INSERT INTO Dishes (CategoryId, DishName, Price, Description, IsActive, CreatedAt)
SELECT @SideId, N'凱薩經典沙拉', 100, N'清爽解膩', 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM Dishes WHERE DishName IN (N'「沙」拉嘿呦', N'凱薩經典沙拉'));

-- 4. 新增 5 種套餐
INSERT INTO SetMeals (SetMealName, SetPrice, Description, DiscountType, DiscountValue, IsActive, CreatedAt)
SELECT N'獨享單人套餐', 399, N'專為一人設計', 'fixed', 0, 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM SetMeals WHERE SetMealName IN (N'「單」點不孤單', N'獨享單人套餐'));
INSERT INTO SetMeals (SetMealName, SetPrice, Description, DiscountType, DiscountValue, IsActive, CreatedAt)
SELECT N'飽足份量個人餐', 450, N'超大飽足感', 'fixed', 0, 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM SetMeals WHERE SetMealName IN (N'「身」邊沒人餐', N'飽足份量個人餐'));
INSERT INTO SetMeals (SetMealName, SetPrice, Description, DiscountType, DiscountValue, IsActive, CreatedAt)
SELECT N'豪華炸物拼盤餐', 350, N'派對必備', 'fixed', 0, 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM SetMeals WHERE SetMealName IN (N'「狗」延殘喘餐', N'豪華炸物拼盤餐'));
INSERT INTO SetMeals (SetMealName, SetPrice, Description, DiscountType, DiscountValue, IsActive, CreatedAt)
SELECT N'經典白醬培根套餐', 380, N'奶香濃郁', 'fixed', 0, 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM SetMeals WHERE SetMealName IN (N'「諧」老終身餐', N'經典白醬培根套餐'));
INSERT INTO SetMeals (SetMealName, SetPrice, Description, DiscountType, DiscountValue, IsActive, CreatedAt)
SELECT N'香煎鱸魚排套餐', 420, N'精緻海鮮', 'fixed', 0, 1, GETUTCDATE() WHERE NOT EXISTS (SELECT 1 FROM SetMeals WHERE SetMealName IN (N'「梗」在喉頭餐', N'香煎鱸魚排套餐'));

-- 5. 建立關聯 (SetMealItems)
DECLARE @Soup1 INT = (SELECT Id FROM Dishes WHERE DishName = N'義式蔬菜礦工湯');
DECLARE @Soup2 INT = (SELECT Id FROM Dishes WHERE DishName = N'羅宋牛腩湯');
DECLARE @Side1 INT = (SELECT Id FROM Dishes WHERE DishName = N'松露起司薯條');
DECLARE @Side2 INT = (SELECT Id FROM Dishes WHERE DishName = N'義式香草烤雞翅');
DECLARE @Side3 INT = (SELECT Id FROM Dishes WHERE DishName = N'酥炸墨魚圈');

DECLARE @Set1 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'獨享單人套餐');
DECLARE @Set2 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'飽足份量個人餐');
DECLARE @Set3 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'豪華炸物拼盤餐');
DECLARE @Set4 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'經典白醬培根套餐');
DECLARE @Set5 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'香煎鱸魚排套餐');

IF @Set1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SetMealItems WHERE SetMealId = @Set1)
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, DisplayOrder) VALUES (@Set1, @Soup1, 1, 0, 1), (@Set1, @Side1, 1, 0, 2);
END
IF @Set2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SetMealItems WHERE SetMealId = @Set2)
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, DisplayOrder) VALUES (@Set2, @Side1, 1, 0, 1), (@Set2, @Side2, 1, 0, 2);
END
IF @Set3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SetMealItems WHERE SetMealId = @Set3)
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, DisplayOrder) VALUES (@Set3, @Side1, 1, 0, 1), (@Set3, @Side2, 1, 0, 2), (@Set3, @Side3, 1, 0, 3);
END
IF @Set4 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SetMealItems WHERE SetMealId = @Set4)
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, DisplayOrder) VALUES (@Set4, @Soup2, 1, 0, 1);
END
IF @Set5 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SetMealItems WHERE SetMealId = @Set5)
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, DisplayOrder) VALUES (@Set5, @Soup1, 1, 0, 1), (@Set5, @Side3, 1, 0, 2);
END
