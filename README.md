# 義起吃（EatTogether）後台管理系統

> 義起吃後台為義式餐廳管理系統，以 **ASP.NET Core MVC（.NET 8）** 開發，提供餐廳管理人員對餐點、套餐、分類等資料的完整 CRUD 操作與排程自動化，透過 Web API 與前台共用同一 SQL Server 資料庫。

---

## 目錄
- [專案簡介](#專案簡介)
- [功能總覽](#功能總覽)
- [技術架構](#技術架構)
- [專案結構](#專案結構)
- [資料庫建置說明](#資料庫建置說明)
- [權限控管](#權限控管)
- [開發注意事項](#開發注意事項)
- [畫面截圖](#畫面截圖)
---

## 專案簡介

義起吃後台提供餐廳管理人員完整的餐點、套餐、分類管理功能，整合排程自動化服務，並開放 JSON API 供前台 Vue 3 即時同步資料。

---

## 功能總覽

### 餐點管理（Dishes）
- 餐點的新增、編輯、刪除（支援圖片上傳，採 Cropper.js 裁切）
- 啟用 / 停用單筆或批次切換
- 拖曳排序（`UpdateOrder`），即時調整顧客端菜單呈現順序
- 庫存狀態管理（`PATCH /api/Dishes/{id}/Stock`）：供應中 / 剩餘不多 / 售完
- 提供顧客端 JSON API：`GetAllJson`、`GetActiveJson`、`GetByIdJson`

### 套餐管理（SetMeals）
- 套餐的新增、編輯、複製（Clone）、刪除
- 套餐品項（SetMealItems）動態管理，支援多品項組合
- 時間區間設定（`StartDate / EndDate`、`StartTime / EndTime`）
- 折扣類型與折扣值設定（`DiscountType / DiscountValue`）
- 啟用 / 停用與批次操作，拖曳排序
- 提供前台 `GetActiveJson` 即時同步

### 分類管理（Categories）
- 分類的 Modal 式新增 / 編輯（無頁面跳轉，UX 流暢）
- 支援父子分類階層（Parent Category）
- 批次停用 / 啟用，自動連動旗下所有餐點
- 批次刪除
- 拖曳排序
- 分類圖片上傳（base64 → 存至 `wwwroot/images/categories/`）
- 動態展示該分類下的餐點列表（`Details` API）

### 排程管理（Scheduler）
- 整合 `DishSchedulerService`，依時間區間自動啟用 / 停用套餐與餐點
- 排程執行紀錄列表（最近 50 筆）
- 單筆紀錄詳情：顯示本次執行中被啟用 / 停用的餐點與套餐清單
- 支援**手動觸發排程**（`TriggerNow`），方便測試與緊急處理

### 評論審核（ReviewManage）
- 顯示顧客端匿名提交的餐點評論
- 支援以餐點名稱關鍵字搜尋
- 管理員可刪除不當留言，刪除後顯示操作成功提示
- 採用 `[Authorize]` 保護，須登入方可存取

### 評分 API（DishRatings）
- `POST /api/Dishes/{id}/Rate`：接收顧客端評分（1–5 星）
- 自動計算並回傳最新平均分與評分人數
- 開放匿名存取（`[AllowAnonymous]`）

---

## 技術架構

| 層次 | 技術 |
|---|---|
| 後端框架 | ASP.NET Core MVC（.NET 8）|
| 資料存取 | EF Core（Database First）|
| 資料庫 | SQL Server |
| 前端互動 | Bootstrap 5、JavaScript、Cropper.js |
| 排程服務 | 自定義 `DishSchedulerService`（後台定時任務）|
| 驗證授權 | ASP.NET Core Identity + 自定義 `[RequirePermission]` Attribute |
| 圖片處理 | base64 解碼後存檔至 `wwwroot/images/` |
| 版本控制 | Git / GitHub |

---

## 專案結構

```
Solution/
├── Controllers/
│   ├── DishesController.cs          # 餐點 CRUD + 庫存 + 排序 + JSON API
│   ├── SetMealsController.cs        # 套餐 CRUD + 複製 + 品項管理 + 排序
│   ├── CategoriesController.cs      # 分類 CRUD + 父子分類 + 圖片上傳
│   ├── SchedulerController.cs       # 排程紀錄查看 + 手動觸發
│   ├── ReviewManageController.cs    # 顧客評論審核與刪除
│   ├── DishRatingsController.cs     # 餐點評分 API
│   └── DishStockController.cs       # 餐點庫存狀態 API
│
├── Models/
│   ├── EfModels/                    # EF Core Database First 生成的 Entity
│   ├── DTOs/                        # 資料傳輸物件
│   ├── ViewModels/                  # MVC 頁面 ViewModel
│   ├── Services/                    # 業務邏輯層（DishService、SetMealService…）
│   ├── Repositories/                # 資料存取介面與實作
│   └── Infra/                       # 共用基礎建設（RequirePermission、擴充方法）
│
├── Views/
│   ├── Dishes/                      # Index / Create / Edit
│   ├── SetMeals/                    # Index / Create / Edit
│   ├── Categories/                  # Index（含 Modal）
│   └── Scheduler/                   # Index / Detail
│
└── DB/（資料庫 Seed 腳本，依編號順序執行）
    ├── 05_Categories.sql
    ├── 06_SetMeals.sql
    ├── 15_Dishes.sql
    ├── 18_1_Products.sql
    ├── 18_SetMealItems.sql
    ├── 28_SchedulerLogs.sql
    └── 29_Reviews.sql
```

---

## 資料庫建置說明

本專案採 **Database First** 策略，資料庫結構由 `CreateDatabase.sql` 定義，各資料表種子資料依 `DB/` 資料夾內的編號腳本依序執行。

```bash
# 依序執行（以 sqlcmd 為例）
sqlcmd -S .\SQLEXPRESS -i CreateDatabase.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/05_Categories.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/06_SetMeals.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/15_Dishes.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/18_1_Products.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/18_SetMealItems.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/28_SchedulerLogs.sql
sqlcmd -S .\SQLEXPRESS -d EatTogetherDB -i DB/29_Reviews.sql
```

> 新增資料表時，直接於 `CreateDatabase.sql` 補充 `CREATE TABLE` 語法，並新增對應編號的 Seed 腳本（例：`30_LimitedNotifications.sql`）。

---

## 權限控管

後台路由採兩層保護機制：

- `[Authorize]`：確認使用者已登入（ASP.NET Core Identity）
- `[RequirePermission("Menu_Manage")]`：確認使用者具備菜單管理權限

未授權存取將導向登入頁，確保後台資料安全。

---

## 開發注意事項

- **時區**：後台統一使用 `DateTime.Now`（台灣本地時間），避免與 `UtcNow` 混用造成排程時間誤差。
- **圖片命名**：圖片以餐點 / 套餐名稱命名，存入 `wwwroot/images/`。若名稱含非法字元（`/`、`:` 等），會自動替換為底線。
- **前後台同步**：前台 Vue 3 透過 Polling 定期呼叫 `GetActiveJson`，後台異動後無需額外操作即可同步至顧客端。
- **軟刪除**：目前餐點 / 套餐採停用（`IsActive = false`）而非實體刪除，保留歷史資料完整性。

## 畫面截圖

### 餐點管理總覽
<img width="1601" height="817" alt="餐點管理" src="https://github.com/user-attachments/assets/081c43ce-491c-4281-9213-43d8de71b7ed" />

### 餐點編輯
<img width="1583" height="835" alt="餐點-編輯" src="https://github.com/user-attachments/assets/b5368d54-3bf8-4e0a-8158-144bb602914f" />

### 批次上下架
<img width="1476" height="222" alt="支援批次上下架" src="https://github.com/user-attachments/assets/33dca423-a412-4907-8f0c-54458d68fce7" />

### 供應排程記錄
<img width="1642" height="637" alt="餐點自動排程上下架" src="https://github.com/user-attachments/assets/1b1b65e7-20cf-4639-aa05-98fa4bcee767" />

### 套餐內容編輯
<img width="1577" height="717" alt="套餐內容編輯" src="https://github.com/user-attachments/assets/024f6d1c-1036-40e8-98e6-9a2d94fa9463" />

### 留言管理
<img width="1601" height="793" alt="餐點留言管理" src="https://github.com/user-attachments/assets/53d94aa9-c632-4d56-aead-abce66df83e6" />
