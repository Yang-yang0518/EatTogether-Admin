-- 18_SetMealItems.sql
-- Seed Data for SetMealItems (全量復原並新增，確保每個套餐都有餐點)

-- ==================== 1. ID 準備區 ====================
-- 主餐
DECLARE @TomatoId INT = (SELECT Id FROM Dishes WHERE DishName = N'義式番茄義大利麵');
DECLARE @CreamId  INT = (SELECT Id FROM Dishes WHERE DishName = N'奶油培根燉飯');
DECLARE @BasilId  INT = (SELECT Id FROM Dishes WHERE DishName = N'青醬海鮮義大利麵');
DECLARE @TruffleId INT = (SELECT Id FROM Dishes WHERE DishName = N'松露野菇燉飯');
DECLARE @ChickenId INT = (SELECT Id FROM Dishes WHERE DishName = N'香烤雞腿排');
DECLARE @SalmonId  INT = (SELECT Id FROM Dishes WHERE DishName = N'香煎鮭魚排');
DECLARE @KarageId  INT = (SELECT Id FROM Dishes WHERE DishName = N'日式唐揚雞定食');
DECLARE @BeefId    INT = (SELECT Id FROM Dishes WHERE DishName = N'紅酒燉牛肉飯');
DECLARE @BassId    INT = (SELECT Id FROM Dishes WHERE DishName = N'香煎鱸魚排');
DECLARE @LobsterId INT = (SELECT Id FROM Dishes WHERE DishName = N'龍蝦奶油義大利麵');
-- 披薩
DECLARE @Piz1 INT = (SELECT Id FROM Dishes WHERE DishName = N'瑪格麗特披薩');
DECLARE @Piz2 INT = (SELECT Id FROM Dishes WHERE DishName = N'海鮮總匯披薩');
DECLARE @Piz3 INT = (SELECT Id FROM Dishes WHERE DishName = N'夏威夷雞肉披薩');
-- 湯品
DECLARE @Soup1 INT = (SELECT Id FROM Dishes WHERE DishName = N'義式蔬菜礦工湯');
DECLARE @Soup2 INT = (SELECT Id FROM Dishes WHERE DishName = N'巧達海鮮濃湯');
DECLARE @Soup3 INT = (SELECT Id FROM Dishes WHERE DishName = N'羅宋牛腩湯');
DECLARE @Soup4 INT = (SELECT Id FROM Dishes WHERE DishName = N'義式茄汁海鮮湯');
-- 附餐
DECLARE @Side1 INT = (SELECT Id FROM Dishes WHERE DishName = N'松露起司薯條');
DECLARE @Side2 INT = (SELECT Id FROM Dishes WHERE DishName = N'義式香草烤雞翅');
DECLARE @Side3 INT = (SELECT Id FROM Dishes WHERE DishName = N'酥炸墨魚圈');
DECLARE @Side4 INT = (SELECT Id FROM Dishes WHERE DishName = N'凱薩經典沙拉');
DECLARE @PlatterId INT = (SELECT Id FROM Dishes WHERE DishName = N'分享拼盤');
-- 飲料
DECLARE @Drink1 INT = (SELECT Id FROM Dishes WHERE DishName = N'招牌鮮奶茶');
DECLARE @Drink2 INT = (SELECT Id FROM Dishes WHERE DishName = N'可樂');
DECLARE @Drink3 INT = (SELECT Id FROM Dishes WHERE DishName = N'美式黑咖啡');
DECLARE @Drink4 INT = (SELECT Id FROM Dishes WHERE DishName = N'柳橙汁');
-- 甜點
DECLARE @Des1 INT = (SELECT Id FROM Dishes WHERE DishName = N'法式烤布蕾');
DECLARE @Des2 INT = (SELECT Id FROM Dishes WHERE DishName = N'提拉米蘇');

-- 套餐 ID 抓取
DECLARE @F1 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'全家分享餐');
DECLARE @F2 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'情人節限定套餐');
DECLARE @F3 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'過年限定套餐');
DECLARE @F4 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'商務午餐套餐');
DECLARE @F5 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'歡樂雙人套餐');
DECLARE @F6 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'下午茶甜蜜套餐');

DECLARE @N1 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'「單」點不孤單');
DECLARE @N2 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'「身」邊沒人餐');
DECLARE @N3 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'「狗」延殘喘餐');
DECLARE @N4 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'「諧」老終身餐');
DECLARE @N5 INT = (SELECT Id FROM SetMeals WHERE SetMealName = N'「梗」在喉頭餐');

-- ==================== 2. 內容建立區 (清除後重新建立以確保最新規格) ====================
DELETE FROM SetMealItems;

-- 1. 全家分享餐 ($899)：主餐(4選4) + 附餐(4選2) + 飲料(4選4) + 甜點(固定1) + 拼盤(固定1)
IF @F1 IS NOT NULL
BEGIN
    -- Group 1: 主餐 4選4
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F1, @TomatoId, 1, 1, 1, 4, 1), (@F1, @CreamId, 1, 1, 1, 4, 2), (@F1, @BasilId, 1, 1, 1, 4, 3), (@F1, @ChickenId, 1, 1, 1, 4, 4);
    -- Group 2: 附餐 4選2
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F1, @Side1, 1, 1, 2, 2, 5), (@F1, @Side2, 1, 1, 2, 2, 6), (@F1, @Side3, 1, 1, 2, 2, 7), (@F1, @Side4, 1, 1, 2, 2, 8);
    -- Group 3: 飲料 4選4
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F1, @Drink1, 1, 1, 3, 4, 9), (@F1, @Drink2, 1, 1, 3, 4, 10), (@F1, @Drink3, 1, 1, 3, 4, 11), (@F1, @Drink4, 1, 1, 3, 4, 12);
    -- 固定品項
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F1, @Des1, 1, 0, NULL, NULL, 13), (@F1, @PlatterId, 1, 0, NULL, NULL, 14);
END

-- 2. 情人節限定套餐：主餐(4選2) + 飲品(4選2) + 甜點(2選2)
IF @F2 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F2, @TomatoId, 1, 1, 1, 2, 1), (@F2, @CreamId, 1, 1, 1, 2, 2), (@F2, @BasilId, 1, 1, 1, 2, 3), (@F2, @SalmonId, 1, 1, 1, 2, 4),
    (@F2, @Drink1, 1, 1, 2, 2, 5), (@F2, @Drink4, 1, 1, 2, 2, 6), (@F2, @Drink2, 1, 1, 2, 2, 7), (@F2, @Drink3, 1, 1, 2, 2, 8),
    (@F2, @Des1, 1, 1, 3, 2, 9), (@F2, @Des2, 1, 1, 3, 2, 10);
END

-- 3. 過年限定套餐：主餐固定 + 拼盤固定 + 飲品固定2 + 甜點固定2
IF @F3 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F3, @TomatoId, 2, 0, NULL, NULL, 1), (@F3, @CreamId, 2, 0, NULL, NULL, 2), (@F3, @PlatterId, 1, 0, NULL, NULL, 3),
    (@F3, @Drink1, 2, 0, NULL, NULL, 4), (@F3, @Des1, 2, 0, NULL, NULL, 5);
END

-- 4. 商務午餐套餐：主餐(4選1) + 飲料(3選1)
IF @F4 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F4, @TomatoId, 1, 1, 1, 1, 1), (@F4, @CreamId, 1, 1, 1, 1, 2), (@F4, @KarageId, 1, 1, 1, 1, 3), (@F4, @BeefId, 1, 1, 1, 1, 4),
    (@F4, @Drink2, 1, 1, 2, 1, 5), (@F4, @Drink1, 1, 1, 2, 1, 6), (@F4, @Drink3, 1, 1, 2, 1, 7);
END

-- 5. 歡樂雙人套餐：主餐(4選2) + 飲料(4選2) + 提拉米蘇(固定)
IF @F5 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F5, @ChickenId, 1, 1, 1, 2, 1), (@F5, @SalmonId, 1, 1, 1, 2, 2), (@F5, @TomatoId, 1, 1, 1, 2, 3), (@F5, @CreamId, 1, 1, 1, 2, 4),
    (@F5, @Drink4, 1, 1, 2, 2, 5), (@F5, @Drink1, 1, 1, 2, 2, 6), (@F5, @Drink2, 1, 1, 2, 2, 7), (@F5, @Des2, 1, 0, NULL, NULL, 8);
END

-- 6. 下午茶甜蜜套餐：甜點(4選1) + 飲料(4選1)
IF @F6 IS NOT NULL
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@F6, @Des2, 1, 1, 1, 1, 1), (@F6, @Des1, 1, 1, 1, 1, 2), (@F6, @Drink1, 1, 1, 2, 1, 3), (@F6, @Drink4, 1, 1, 2, 1, 4);

-- 7. 「單」點不孤單 ($388)：主餐(4選1) + 湯品(2選1) + 飲料(3選1)
IF @N1 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@N1, @TomatoId, 1, 1, 1, 1, 1), (@N1, @CreamId, 1, 1, 1, 1, 2), (@N1, @BasilId, 1, 1, 1, 1, 3), (@N1, @TruffleId, 1, 1, 1, 1, 4),
    (@N1, @Soup1, 1, 1, 2, 1, 5), (@N1, @Soup2, 1, 1, 2, 1, 6),
    (@N1, @Drink1, 1, 1, 3, 1, 7), (@N1, @Drink2, 1, 1, 3, 1, 8), (@N1, @Drink3, 1, 1, 3, 1, 9);
END

-- 8. 「身」邊沒人餐 ($499)：主餐(4選1) + 湯品(2選1) + 附餐(2選1) + 飲料(3選1)
IF @N2 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@N2, @TomatoId, 1, 1, 1, 1, 1), (@N2, @CreamId, 1, 1, 1, 1, 2), (@N2, @BasilId, 1, 1, 1, 1, 3), (@N2, @TruffleId, 1, 1, 1, 1, 4),
    (@N2, @Soup1, 1, 1, 2, 1, 5), (@N2, @Soup3, 1, 1, 2, 1, 6),
    (@N2, @Side1, 1, 1, 3, 1, 7), (@N2, @Side4, 1, 1, 3, 1, 8),
    (@N2, @Drink1, 1, 1, 4, 1, 9), (@N2, @Drink2, 1, 1, 4, 1, 10), (@N2, @Drink4, 1, 1, 4, 1, 11);
END

-- 9. 「狗」延殘喘餐 ($520)：披薩(3選1) + 附餐(4選2) + 飲料(3選1)
IF @N3 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@N3, @Piz1, 1, 1, 1, 1, 1), (@N3, @Piz2, 1, 1, 1, 1, 2), (@N3, @Piz3, 1, 1, 1, 1, 3),
    (@N3, @Side1, 1, 1, 2, 2, 4), (@N3, @Side2, 1, 1, 2, 2, 5), (@N3, @Side3, 1, 1, 2, 2, 6), (@N3, @Side4, 1, 1, 2, 2, 7),
    (@N3, @Drink1, 1, 1, 3, 1, 8), (@N3, @Drink2, 1, 1, 3, 1, 9), (@N3, @Drink3, 1, 1, 3, 1, 10);
END

-- 10. 「諧」老終身餐 ($999)：主餐(4選2) + 湯品(2選2) + 飲料(3選2) + 甜點(2選1)
IF @N4 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@N4, @BasilId, 1, 1, 1, 2, 1), (@N4, @TruffleId, 1, 1, 1, 2, 2), (@N4, @ChickenId, 1, 1, 1, 2, 3), (@N4, @BassId, 1, 1, 1, 2, 4),
    (@N4, @Soup2, 1, 1, 2, 2, 5), (@N4, @Soup3, 1, 1, 2, 2, 6),
    (@N4, @Drink1, 1, 1, 3, 2, 7), (@N4, @Drink2, 1, 1, 3, 2, 8), (@N4, @Drink4, 1, 1, 3, 2, 9),
    (@N4, @Des1, 1, 1, 4, 1, 10), (@N4, @Des2, 1, 1, 4, 1, 11);
END

-- 11. 「梗」在喉頭餐 ($666)：波士頓龍蝦(固定) + 豪華湯品 + 特色附餐 + 飲料 + 甜點
IF @N5 IS NOT NULL
BEGIN
    INSERT INTO SetMealItems (SetMealId, DishId, Quantity, IsOptional, OptionGroupNo, PickLimit, DisplayOrder) VALUES 
    (@N5, @LobsterId, 1, 0, NULL, NULL, 1),
    (@N5, @Soup4, 1, 0, NULL, NULL, 2),
    (@N5, @Side3, 1, 0, NULL, NULL, 3),
    (@N5, @Drink1, 1, 0, NULL, NULL, 4),
    (@N5, @Des1, 1, 0, NULL, NULL, 5);
END
GO
