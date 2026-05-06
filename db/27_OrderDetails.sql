USE EatTogetherDB;
GO

-- 暫時停用 FK 約束
ALTER TABLE [dbo].[OrderDetails] NOCHECK CONSTRAINT [FK_OrderDetails_ParentDetail];
GO

-- Step 1：INSERT 所有有效明細，ParentDetailId 先帶入 PreOrderDetails 的舊 Id
INSERT INTO [dbo].[OrderDetails] 
(
    [OrderId], 
    [ProductId], 
    [ProductName], 
    [UnitPrice], 
    [Qty], 
    [SubTotal],
    [ParentDetailId],
    [PreOrderDetailId]
)
SELECT 
    O.[Id],
    PD.[ProductId], 
    PD.[ProductName], 
    PD.[UnitPrice], 
    PD.[Qty], 
    PD.[SubTotal],
    PD.[ParentDetailId],  -- 暫存 PreOrderDetails 的舊 Id
    PD.[Id]
FROM [dbo].[PreOrderDetails] AS PD
INNER JOIN [dbo].[Orders] AS O ON PD.[PreOrderId] = O.[PreOrderId]
WHERE PD.[DoneOrCancel] = 1;
GO

-- Step 2：換算 ParentDetailId 成 OrderDetails 的正確 Id
UPDATE Child
SET Child.[ParentDetailId] = Parent.[Id]
FROM [dbo].[OrderDetails] AS Child
INNER JOIN [dbo].[OrderDetails] AS Parent
    ON Parent.[PreOrderDetailId] = Child.[ParentDetailId]
WHERE Child.[ParentDetailId] IS NOT NULL;
GO

-- 重新啟用 FK 約束並驗證資料
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_ParentDetail];
GO