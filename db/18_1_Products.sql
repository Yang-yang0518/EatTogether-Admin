-- 18_1_Products.sql
-- Seed Data for Products (追加模式，逐筆檢查避免重複)
-- 依賴：15_Dishes.sql、06_SetMeals.sql 必須先執行

-- 單點 Dishes 的 ID 取得
-- (這裡省略所有 DECLARE，直接用子查詢確保最新)

-- ==================== 單點 Dishes ====================
INSERT INTO dbo.Products (ProductType, DishId, SetMealId)
SELECT N'Dish', d.Id, NULL
FROM dbo.Dishes d
LEFT JOIN dbo.Products p ON d.Id = p.DishId
WHERE p.Id IS NULL;

-- ==================== 套餐 SetMeals ====================
INSERT INTO dbo.Products (ProductType, DishId, SetMealId)
SELECT N'SetMeal', NULL, s.Id
FROM dbo.SetMeals s
LEFT JOIN dbo.Products p ON s.Id = p.SetMealId
WHERE p.Id IS NULL;
GO
