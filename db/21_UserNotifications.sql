USE [EatTogetherDB]
GO

--說明
--Id非自行指定，讓系統自行去遞增新增
--[Message] 是訊息的補充說明，可以自己增添
--目前ORDER BY 依文章發佈時間以及會員id去遞增訊息

INSERT INTO [dbo].[UserNotifications]
    ([MemberId], [Type], [ReferenceType], [ReferenceId], [Title], [Message], [IsRead], [CreatedAt])
SELECT
    m.Id          AS MemberId,
    'NEWS'        AS Type,
    'Article'     AS ReferenceType,
    a.ArticleId   AS ReferenceId,
    N'親愛的會員，' + a.Title AS Title,
    NULL          AS Message,
    CASE 
        WHEN a.PublishDate < '2026-03-01' THEN 1
        ELSE 0
    END           AS IsRead,
    a.PublishDate AS CreatedAt
FROM (
    -- 正常會員 52 位（排除黑名單 29,30,67,68 與已刪除 31,32,69,70,71,72）
    SELECT  1 AS Id UNION SELECT  2 UNION SELECT  3 UNION SELECT  4 UNION SELECT  5 UNION
    SELECT  6        UNION SELECT  7 UNION SELECT  8 UNION SELECT  9 UNION SELECT 10 UNION
    SELECT 11        UNION SELECT 12 UNION SELECT 13 UNION SELECT 14 UNION SELECT 15 UNION
    SELECT 16        UNION SELECT 17 UNION SELECT 18 UNION SELECT 19 UNION SELECT 20 UNION
    SELECT 21        UNION SELECT 22 UNION SELECT 23 UNION SELECT 24 UNION SELECT 25 UNION
    SELECT 26        UNION SELECT 27 UNION SELECT 28 UNION SELECT 33 UNION SELECT 34 UNION
    SELECT 35        UNION SELECT 36 UNION SELECT 37 UNION SELECT 38 UNION SELECT 39 UNION
    SELECT 40        UNION SELECT 41 UNION SELECT 42 UNION SELECT 43 UNION SELECT 44 UNION
    SELECT 45        UNION SELECT 46 UNION SELECT 47 UNION SELECT 48 UNION SELECT 49 UNION
    SELECT 50        UNION SELECT 51 UNION SELECT 52 UNION SELECT 53 UNION SELECT 54 UNION
    SELECT 55        UNION SELECT 56 UNION SELECT 57 UNION SELECT 58 UNION SELECT 59 UNION
    SELECT 60        UNION SELECT 61 UNION SELECT 62 UNION SELECT 63 UNION SELECT 64 UNION
    SELECT 65        UNION SELECT 66
) AS m
CROSS JOIN (
    SELECT  9 AS ArticleId, N'2026 年春節營業公告'                            AS Title, CAST('2026-01-15' AS datetime2(0)) AS PublishDate
    UNION ALL SELECT 12, N'春季新菜單上線：清爽地中海風沙拉系列',                   '2026-02-15'
    UNION ALL SELECT 13, N'新品上市：全家分享餐 699 元起澎湃登場',                  '2026-02-21'
    UNION ALL SELECT 15, N'學生專屬：憑學生證提拉米蘇免費請你吃',                  '2026-03-01'
    UNION ALL SELECT 16, N'初夏海鮮祭：來自北海道的鮮甜干貝',                      '2026-03-08'
    UNION ALL SELECT 18, N'義起吃週年慶開跑：感謝三年來每一位老朋友',              '2026-03-18'
    UNION ALL SELECT 20, N'季節限定：春末松露慶典盛大展開',                         '2026-04-15'
    UNION ALL SELECT 21, N'五一連假微放鬆：滿 800 立折 80，好好吃一頓',            '2026-05-01'
    UNION ALL SELECT 22, N'五月寵愛母親節，全桌 85 折優惠公告',                    '2026-05-01'
    UNION ALL SELECT 23, N'義起吃 5/20 員工旅遊暫停營業一日',                      '2026-05-10'
) AS a
ORDER BY a.PublishDate, m.Id;


-- 外帶訂單通知（示範資料，MemberId=1 的兩筆外帶單）
INSERT INTO [dbo].[UserNotifications]
    ([MemberId], [Type], [ReferenceType], [ReferenceId], [Title], [Message], [IsRead], [CreatedAt])
VALUES
    (1, 'TAKEOUT_CREATED', 'Order', 1,  N'外帶訂單已成立｜20260301-0001', NULL, 1, '2026-03-01 11:30:00'),
    (1, 'TAKEOUT_CREATED', 'Order', 25, N'外帶訂單已成立｜20260304-0001', NULL, 1, '2026-03-04 11:30:00');


GO