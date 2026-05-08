-- 06_SetMeals.sql
-- Seed Data for SetMeals (全量復原原有套餐，並僅追加新套餐)

-- ==================== 1. 復原：原有基礎套餐 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'全家分享餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartDate, EndDate, StartTime, EndTime)
    VALUES (N'全家分享餐', N'percent', 15.00, 1, GETDATE(), 899.00, N'適合4-6人共享的豐盛套餐', '2026-03-01', '2026-12-31', '10:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'情人節限定套餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartDate, EndDate, StartTime, EndTime)
    VALUES (N'情人節限定套餐', N'percent', 20.00, 1, GETDATE(), 520.00, N'情人節浪漫限定套餐', '2026-02-01', '2026-02-14', '17:00:00', '22:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'過年限定套餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartDate, EndDate, StartTime, EndTime)
    VALUES (N'過年限定套餐', N'fixed', 100.00, 1, GETDATE(), 888.00, N'新年團圓限定套餐', '2026-01-20', '2026-02-10', '11:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'商務午餐套餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartDate, EndDate, StartTime, EndTime)
    VALUES (N'商務午餐套餐', N'percent', 10.00, 1, GETDATE(), 240.00, N'【限時優惠】主餐搭配飲料，快速美味的商務午餐', '2026-01-01', '2026-12-31', '11:00:00', '13:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'歡樂雙人套餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'歡樂雙人套餐', N'percent', 15.00, 1, GETDATE(), 580.00, N'兩人共享主餐、飲料與甜點的超值組合', '10:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'下午茶甜蜜套餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'下午茶甜蜜套餐', N'percent', 10.00, 1, GETDATE(), 199.00, N'精選甜點搭配飲料，午後甜蜜時光', '14:00:00', '16:00:00');

-- ==================== 2. 新增：諧音梗特色套餐 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'「單」點不孤單')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'「單」點不孤單', N'fixed', 0.00, 1, GETDATE(), 388.00, N'單人獨享：主餐(4選1) + 湯品(2選1) + 飲品(3選1)', '10:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'「身」邊沒人餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'「身」邊沒人餐', N'fixed', 0.00, 1, GETDATE(), 499.00, N'飽足個人：主餐(4選1) + 湯(2選1) + 附餐(2選1) + 飲品(3選1)', '10:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'「狗」延殘喘餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'「狗」延殘喘餐', N'fixed', 0.00, 1, GETDATE(), 520.00, N'派對小食：披薩(3選1) + 附餐(4選2) + 飲品(3選1)', '11:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'「諧」老終身餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'「諧」老終身餐', N'fixed', 0.00, 1, GETDATE(), 999.00, N'雙人同樂：主餐(4選2) + 湯品(2選2) + 飲品(3選2) + 甜點(2選1)', '10:00:00', '21:00:00');

IF NOT EXISTS (SELECT 1 FROM dbo.SetMeals WHERE SetMealName = N'「梗」在喉頭餐')
    INSERT INTO dbo.SetMeals (SetMealName, DiscountType, DiscountValue, IsActive, CreatedAt, SetPrice, Description, StartTime, EndTime)
    VALUES (N'「梗」在喉頭餐', N'fixed', 0.00, 1, GETDATE(), 666.00, N'豪華全餐：波士頓龍蝦 + 豪華湯品 + 特色附餐 + 飲品 + 甜點', '11:00:00', '21:00:00');
GO
