-- 15_Dishes.sql
-- Seed Data for Dishes (全量復原，僅移除泰、韓、日式餐點)
-- 依賴：05_Categories.sql 必須先執行

DECLARE @MainId    int = (SELECT TOP 1 Id FROM dbo.Categories WHERE CategoryName = N'主餐');
DECLARE @DrinkId   int = (SELECT TOP 1 Id FROM dbo.Categories WHERE CategoryName = N'飲料');
DECLARE @DessertId int = (SELECT TOP 1 Id FROM dbo.Categories WHERE CategoryName = N'甜點');
DECLARE @SoupId    int = (SELECT TOP 1 Id FROM dbo.Categories WHERE CategoryName = N'湯品');
DECLARE @SideId    int = (SELECT TOP 1 Id FROM dbo.Categories WHERE CategoryName = N'附餐');

IF @MainId IS NULL OR @DrinkId IS NULL OR @DessertId IS NULL OR @SoupId IS NULL OR @SideId IS NULL
    THROW 50001, 'Seed failed: Categories not found. Please run 05_Categories.sql first.', 1;

-- ==================== 1. 主餐 (移除：泰式打拋豬、日式唐揚雞、韓式石鍋拌飯) ====================
IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式番茄義大利麵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'義式番茄義大利麵', 180.00, 1, GETDATE(), N'精選San Marzano番茄慢熬醬汁，搭配彈牙手工寬麵，撒上帕馬森起司增添層次風味', 1, 0, 1, 0, 0, 0, N'[{"name":"手工寬麵","subDesc":"杜蘭小麥72小時熟成"},{"name":"San Marzano番茄","subDesc":"義大利進口罐裝"},{"name":"帕馬森起司","subDesc":"36個月熟成刨絲"},{"name":"新鮮羅勒","subDesc":"每日現摘"},{"name":"特級初榨橄欖油","subDesc":"西西里冷壓"},{"name":"有機大蒜","subDesc":"新鮮蒜頭爆香"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'奶油培根燉飯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'奶油培根燉飯', 200.00, 1, GETDATE(), N'Arborio米以白酒慢燉至奶香濃郁，搭配煙燻培根與帕馬森起司，入口即化', 1, 0, 0, 1, 0, 0, N'[{"name":"Arborio米","subDesc":"義大利卡納羅利品種"},{"name":"煙燻培根","subDesc":"德國黑森林工法"},{"name":"帕馬森起司","subDesc":"24個月熟成"},{"name":"白酒","subDesc":"義大利白葡萄酒"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"雞高湯","subDesc":"每日鮮熬8小時"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'青醬海鮮義大利麵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'青醬海鮮義大利麵', 220.00, 1, GETDATE(), N'龍蝦高湯熬煮醬底配鮮蝦花枝，淋上現磨羅勒青醬，海味草香完美融合', 1, 0, 1, 1, 0, 0, N'[{"name":"新鮮蝦仁","subDesc":"台灣本土活草蝦"},{"name":"花枝圈","subDesc":"每日新鮮現切"},{"name":"手工細麵","subDesc":"義大利進口"},{"name":"羅勒青醬","subDesc":"新鮮羅勒現磨"},{"name":"松子","subDesc":"義大利進口烘焙"},{"name":"佩科里諾起司","subDesc":"薩丁尼亞熟成"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'松露野菇燉飯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'松露野菇燉飯', 250.00, 1, GETDATE(), N'新鮮黑松露薄片覆其上，以雞高湯慢燉Arborio米，融合四種野菇深厚鮮味', 1, 0, 0, 0, 1, 0, N'[{"name":"新鮮黑松露","subDesc":"法國佩里戈爾產區"},{"name":"牛肝菌","subDesc":"義大利乾燥重組"},{"name":"杏鮑菇","subDesc":"台灣有機栽培"},{"name":"鴻喜菇","subDesc":"日本品種鮮採"},{"name":"Arborio米","subDesc":"義大利進口"},{"name":"法式發酵奶油","subDesc":"法國Normandie產區"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'香烤雞腿排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'香烤雞腿排', 260.00, 1, GETDATE(), N'醃製24小時去骨雞腿高溫炙烤，外皮金黃酥脆，肉汁豐盈，佐檸檬奶油醬', 1, 0, 1, 0, 0, 0, N'[{"name":"去骨雞腿排","subDesc":"台灣放牧土雞"},{"name":"迷迭香","subDesc":"新鮮地中海品種"},{"name":"有機大蒜","subDesc":"蒜頭泥醃製"},{"name":"黃檸檬","subDesc":"台灣本土現榨"},{"name":"法式奶油","subDesc":"Président品牌有鹽"},{"name":"季節蔬菜","subDesc":"每日市場現採"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'BBQ豬肋排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'BBQ豬肋排', 320.00, 1, GETDATE(), N'精選豬背肋排低溫烘烤4小時至骨肉分離，刷上主廚秘製BBQ醬，焦糖表皮入口留香', 1, 0, 0, 0, 0, 0, N'[{"name":"豬背肋排","subDesc":"台灣黑豬背肋"},{"name":"主廚BBQ醬","subDesc":"美式煙燻秘製配方"},{"name":"蘋果醋","subDesc":"有機蘋果釀製"},{"name":"黑糖","subDesc":"沖繩原糖"},{"name":"煙燻木屑","subDesc":"美國山核桃木"},{"name":"七香料醃料","subDesc":"七種香草調配"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'香煎鮭魚排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'香煎鮭魚排', 280.00, 1, GETDATE(), N'挪威大西洋鮭魚奶油煎至兩面金黃，淋上白酒檸檬奶油醬，配時令蔬菜清爽呈現', 1, 0, 0, 0, 0, 0, N'[{"name":"挪威大西洋鮭魚","subDesc":"ASC認證新鮮空運"},{"name":"法式發酵奶油","subDesc":"有鹽發酵奶油"},{"name":"夏布利白酒","subDesc":"法國Chablis AOC"},{"name":"西西里黃檸檬","subDesc":"鮮榨現用"},{"name":"新鮮刁草","subDesc":"地中海品種"},{"name":"鹽漬酸豆","subDesc":"義大利進口"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'瑪格麗特披薩')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'瑪格麗特披薩', 240.00, 1, GETDATE(), N'拿坡里72小時冷藏發酵麵團，搭配新鮮莫札瑞拉與番茄醬，木柴窯烤至完美', 1, 0, 0, 1, 1, 0, N'[{"name":"00麵粉薄底","subDesc":"拿坡里72小時冷藏發酵"},{"name":"San Marzano番茄醬","subDesc":"義大利進口"},{"name":"水牛莫札瑞拉","subDesc":"新鮮水牛乳製作"},{"name":"新鮮羅勒葉","subDesc":"每日現摘"},{"name":"特級初榨橄欖油","subDesc":"西西里冷壓"},{"name":"帕馬森起司","subDesc":"36個月熟成刨絲"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'牛肉漢堡排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'牛肉漢堡排', 290.00, 1, GETDATE(), N'100%澳洲和牛手工捏製漢堡排煎至五分熟，配主廚蘑菇醬與焦糖洋蔥', 1, 0, 0, 0, 0, 0, N'[{"name":"澳洲和牛碎肉","subDesc":"M5等級粗絞"},{"name":"焦糖洋蔥","subDesc":"慢炒45分鐘"},{"name":"牛肝菌蘑菇醬","subDesc":"牛高湯調製"},{"name":"切達起司","subDesc":"英國老熟成"},{"name":"主廚特製醬汁","subDesc":"秘製配方"},{"name":"新鮮時蔬","subDesc":"每日市場現採"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'總匯三明治')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'總匯三明治', 150.00, 1, GETDATE(), N'三層厚片吐司夾煙燻火腿、培根、荷包蛋與切達起司，塗抹自製蜂蜜芥末醬', 1, 0, 0, 0, 0, 0, N'[{"name":"厚片鮮奶吐司","subDesc":"每日現烤"},{"name":"煙燻火腿","subDesc":"伊比利豬腿肉"},{"name":"煙燻培根","subDesc":"德國黑森林工法"},{"name":"放牧荷包蛋","subDesc":"台灣放牧雞蛋"},{"name":"切達起司","subDesc":"英國熟成"},{"name":"蜂蜜芥末醬","subDesc":"自製每日配製"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'墨西哥雞肉捲')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'墨西哥雞肉捲', 190.00, 1, GETDATE(), N'香煎嫩雞胸佐莎莎醬酸奶與墨西哥起司，包入香脆薄餅，辛香料醃製風味十足', 1, 0, 0, 0, 0, 1, N'[{"name":"有機雞胸肉","subDesc":"放牧雞醃製炙烤"},{"name":"新鮮莎莎醬","subDesc":"番茄洋蔥現切"},{"name":"酸奶","subDesc":"法國crème fraîche"},{"name":"墨西哥三種起司","subDesc":"混合刨絲"},{"name":"小茴香","subDesc":"墨西哥進口香料"},{"name":"手工玉米薄餅","subDesc":"每日現烤"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'起司焗烤通心粉')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'起司焗烤通心粉', 200.00, 1, GETDATE(), N'三種起司調製絲滑白醬，拌入彎管通心粉，高溫焗烤至表面金黃起泡，濃郁療癒', 1, 0, 0, 0, 1, 0, N'[{"name":"彎管通心粉","subDesc":"義大利De Cecco品牌"},{"name":"葛呂耶爾起司","subDesc":"瑞士進口熟成"},{"name":"切達起司","subDesc":"英國老熟成"},{"name":"帕馬森起司","subDesc":"36個月熟成"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"現烤麵包屑","subDesc":"酥脆表層"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'蒜香奶油蝦義大利麵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'蒜香奶油蝦義大利麵', 230.00, 1, GETDATE(), N'蒜末奶油爆香炒入鮮蝦，淋入白酒收汁，拌入天使細麵，清爽鮮美不膩口', 1, 0, 0, 0, 0, 0, N'[{"name":"天使細麵","subDesc":"義大利進口Capellini"},{"name":"活草蝦","subDesc":"台灣在地養殖"},{"name":"有機大蒜","subDesc":"現切蒜末爆香"},{"name":"法式無鹽奶油","subDesc":"發酵奶油"},{"name":"Pinot Grigio白酒","subDesc":"義大利進口"},{"name":"新鮮巴西里","subDesc":"每日現摘"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'法式洋蔥湯牛排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'法式洋蔥湯牛排', 380.00, 1, GETDATE(), N'慢炒兩小時焦糖洋蔥熬製法式金湯，搭配8oz嫩肩牛排，起司麵包浮島畫龍點睛', 1, 0, 0, 0, 0, 0, N'[{"name":"澳洲穀飼嫩肩牛排","subDesc":"8oz分切"},{"name":"法式洋蔥湯","subDesc":"焦糖洋蔥慢熬2小時"},{"name":"格呂耶爾起司","subDesc":"瑞士進口"},{"name":"法棍麵包","subDesc":"每日烘焙"},{"name":"牛高湯","subDesc":"鮮熬12小時"},{"name":"干邑白蘭地","subDesc":"法國進口提香"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'香草烤半雞')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'香草烤半雞', 300.00, 1, GETDATE(), N'迷迭香百里香大蒜醃製整夜，旋轉爐低溫慢烤至表皮金黃酥脆，肉汁飽滿', 1, 0, 0, 0, 0, 0, N'[{"name":"台灣放牧土雞半雞","subDesc":"本土品種"},{"name":"新鮮迷迭香","subDesc":"地中海品種"},{"name":"新鮮百里香","subDesc":"每日栽培"},{"name":"有機大蒜","subDesc":"整顆蒜頭醃製"},{"name":"西西里黃檸檬皮屑","subDesc":"增添清香"},{"name":"特級橄欖油","subDesc":"西班牙冷壓"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'海鮮總匯披薩')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'海鮮總匯披薩', 270.00, 1, GETDATE(), N'白醬取代番茄醬底，鋪滿蝦仁花枝透抽淡菜，瑪祖里拉起司焗烤至拉絲', 1, 0, 0, 0, 0, 0, N'[{"name":"00麵粉薄底","subDesc":"拿坡里72小時冷藏發酵"},{"name":"新鮮蝦仁","subDesc":"台灣活草蝦"},{"name":"花枝圈","subDesc":"每日新鮮現切"},{"name":"法國布列塔尼淡菜","subDesc":"進口"},{"name":"奶油白醬","subDesc":"麵糊基底手工"},{"name":"水牛莫札瑞拉","subDesc":"新鮮拉絲"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'燻鴨胸沙拉')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'燻鴨胸沙拉', 210.00, 1, GETDATE(), N'低溫煙燻鴨胸薄切入盤，搭配芝麻葉核桃與藍紋起司，淋上紅酒油醋醬', 1, 0, 0, 0, 0, 0, N'[{"name":"法式低溫煙燻鴨胸","subDesc":"薄切入盤"},{"name":"嫩芝麻葉","subDesc":"每日新鮮現採"},{"name":"法國核桃","subDesc":"Périgord產區"},{"name":"Roquefort藍紋起司","subDesc":"法國進口"},{"name":"Modena紅酒醋","subDesc":"義大利陳釀"},{"name":"特級橄欖油","subDesc":"西西里冷壓"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'辣味肉醬千層麵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'辣味肉醬千層麵', 230.00, 1, GETDATE(), N'辣味牛豬混合肉醬與貝夏美白醬層層交疊，配義大利千層麵皮，高溫焗烤金黃', 1, 0, 0, 0, 0, 2, N'[{"name":"牛豬混合絞肉","subDesc":"粗絞辣味配方"},{"name":"De Cecco千層麵皮","subDesc":"義大利進口"},{"name":"貝夏美白醬","subDesc":"手工熬製"},{"name":"帕馬森起司","subDesc":"36個月熟成"},{"name":"莫札瑞拉起司","subDesc":"新鮮現磨"},{"name":"卡拉布里亞辣椒","subDesc":"義大利南部進口"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'檸檬奶油鱈魚排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'檸檬奶油鱈魚排', 270.00, 1, GETDATE(), N'大西洋鱈魚排香煎至外皮酥脆，淋上白酒檸檬奶油醬，搭配時令蔬菜清爽上桌', 1, 0, 0, 0, 0, 0, N'[{"name":"冰島大西洋鱈魚排","subDesc":"MSC認證"},{"name":"法式有鹽奶油","subDesc":"發酵奶油"},{"name":"勃艮地白酒","subDesc":"法國AOC認證"},{"name":"有機西西里檸檬","subDesc":"鮮榨現用"},{"name":"新鮮刁草","subDesc":"地中海品種"},{"name":"義大利鹽漬酸豆","subDesc":"提味"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'紅酒燉牛肉飯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'紅酒燉牛肉飯', 280.00, 1, GETDATE(), N'澳洲草飼牛腱以法國Burgundy紅酒慢燉四小時，根莖蔬菜相伴，肉質軟爛入味', 1, 0, 0, 0, 0, 0, N'[{"name":"澳洲草飼牛腱","subDesc":"低溫慢燉4小時"},{"name":"法國Burgundy紅酒","subDesc":"AOC認證"},{"name":"牛高湯","subDesc":"鮮熬12小時"},{"name":"有機胡蘿蔔","subDesc":"法式蔬菜"},{"name":"西芹","subDesc":"法式蔬菜三寶"},{"name":"新鮮迷迭香","subDesc":"地中海品種"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'培根菠菜鹹派')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'培根菠菜鹹派', 180.00, 1, GETDATE(), N'奶油酥脆派皮填入蛋奶液，加入培根菠菜與格呂耶爾起司，低溫烤至凝固完美', 1, 0, 0, 0, 0, 0, N'[{"name":"法式奶油酥皮","subDesc":"法式酥皮工法"},{"name":"德國煙燻培根","subDesc":"黑森林工法"},{"name":"新鮮菠菜","subDesc":"每日現採"},{"name":"格呂耶爾起司","subDesc":"瑞士進口熟成"},{"name":"台灣放牧雞蛋","subDesc":"每日新鮮"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'番茄羅勒燉雞')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'番茄羅勒燉雞', 240.00, 1, GETDATE(), N'雞腿肉以白酒與San Marzano番茄慢燉，加入新鮮羅勒，南歐家常風味暖心上桌', 1, 0, 0, 0, 0, 0, N'[{"name":"台灣放牧土雞腿","subDesc":"每日新鮮處理"},{"name":"San Marzano番茄","subDesc":"義大利進口"},{"name":"新鮮羅勒","subDesc":"每日現摘"},{"name":"Chianti白酒","subDesc":"義大利進口"},{"name":"特級初榨橄欖油","subDesc":"冷壓"},{"name":"有機大蒜","subDesc":"新鮮整顆蒜頭"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'脆皮烤豬五花飯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'脆皮烤豬五花飯', 220.00, 1, GETDATE(), N'帶皮豬五花鹽醃隔夜高溫烤至外皮完美脆泡，搭配滷汁白飯與醃漬蔬菜', 1, 0, 0, 0, 0, 0, N'[{"name":"台灣黑豬帶皮五花","subDesc":"三層肉精選"},{"name":"法國Guérande海鹽","subDesc":"鹽醃隔夜"},{"name":"中式五香粉","subDesc":"香料調配"},{"name":"老滷汁","subDesc":"慢熬增味"},{"name":"台南越光米","subDesc":"每日現煮"},{"name":"醃漬蔬菜","subDesc":"每日新鮮醃製"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'焗烤海鮮飯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'焗烤海鮮飯', 260.00, 1, GETDATE(), N'白酒奶油煮綜合海鮮拌入Arborio米至吸飽鮮味，撒起司高溫焗烤金黃', 1, 0, 0, 0, 0, 0, N'[{"name":"Arborio米","subDesc":"義大利進口"},{"name":"台灣活草蝦","subDesc":"每日現撈"},{"name":"北海道干貝","subDesc":"冷凍乾貝"},{"name":"花枝","subDesc":"每日新鮮現切"},{"name":"義大利白葡萄酒","subDesc":"提鮮"},{"name":"水牛莫札瑞拉","subDesc":"新鮮焗烤拉絲"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'夏威夷雞肉披薩')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'夏威夷雞肉披薩', 250.00, 1, GETDATE(), N'嫩雞胸夏威夷鳳梨搭配煙燻火腿，番茄醬底覆瑪祖里拉起司，甜鹹平衡', 1, 0, 0, 0, 0, 0, N'[{"name":"00麵粉薄底","subDesc":"拿坡里72小時冷藏發酵"},{"name":"有機放牧雞胸肉","subDesc":"香煎"},{"name":"夏威夷鳳梨","subDesc":"新鮮現切"},{"name":"伊比利煙燻火腿","subDesc":"豬腿肉"},{"name":"San Marzano番茄醬","subDesc":"義大利進口"},{"name":"水牛莫札瑞拉","subDesc":"拉絲焗烤"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'季節限定主廚套餐')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson, StartDate, EndDate)
    VALUES (@MainId, N'季節限定主廚套餐', 350.00, 1, GETDATE(), N'主廚依季節精選頂級食材每週更新，涵蓋前菜主餐甜點三道精緻料理', 1, 1, 1, 0, 0, 0, N'[{"name":"當季時令食材","subDesc":"每週主廚精選"},{"name":"在地有機蔬菜","subDesc":"農場直送"},{"name":"頂級蛋白質","subDesc":"依季節調整來源"},{"name":"手工主廚醬汁","subDesc":"當日特製"},{"name":"精選配菜","subDesc":"依主菜搭配調整"},{"name":"主廚創作甜點","subDesc":"當日限量"}]',
    CAST(GETDATE() AS DATE), CAST(DATEADD(DAY, 30, GETDATE()) AS DATE));
ELSE
    UPDATE dbo.Dishes SET StartDate = CAST(GETDATE() AS DATE), EndDate = CAST(DATEADD(DAY, 30, GETDATE()) AS DATE) WHERE DishName = N'季節限定主廚套餐' AND StartDate IS NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'龍蝦奶油義大利麵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson, StartDate, EndDate)
    VALUES (@MainId, N'龍蝦奶油義大利麵', 480.00, 1, GETDATE(), N'波士頓活龍蝦剖半炙烤，搭配龍蝦高湯奶油醬與新鮮寬麵，奢華極致享受', 0, 1, 1, 1, 0, 0, N'[{"name":"波士頓活龍蝦","subDesc":"空運每日鮮活"},{"name":"義大利新鮮寬麵","subDesc":"手工現製"},{"name":"龍蝦高湯","subDesc":"龍蝦殼熬製4小時"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"Cognac XO干邑白蘭地","subDesc":"法國進口"},{"name":"帕馬森起司","subDesc":"36個月熟成"}]',
    CAST(GETDATE() AS DATE), CAST(DATEADD(DAY, 7, GETDATE()) AS DATE));
ELSE
    UPDATE dbo.Dishes SET StartDate = CAST(GETDATE() AS DATE), EndDate = CAST(DATEADD(DAY, 7, GETDATE()) AS DATE) WHERE DishName = N'龍蝦奶油義大利麵' AND StartDate IS NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'香煎鱸魚排')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IsRecommended, IsPopular, IsVegetarian, SpicyLevel, IngredientsJson)
    VALUES (@MainId, N'香煎鱸魚排', 320.00, 1, GETDATE(), N'台灣海水鱸魚奶油煎至兩面金黃，搭配番紅花奶油醬與烤蘆筍，清淡優雅', 1, 0, 1, 1, 0, 0, N'[{"name":"台灣海水鱸魚排","subDesc":"活體現殺每日"},{"name":"西班牙番紅花","subDesc":"La Mancha產區"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"台灣蘆筍","subDesc":"在地栽培烤製"},{"name":"法式有鹽奶油","subDesc":"發酵奶油"},{"name":"夏布利白酒","subDesc":"法國AOC認證"}]');

-- ==================== 2. 飲料 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'可樂')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'可樂', 50.00, 1, GETDATE(), N'冰涼透心美國可口可樂，搭配滿杯碎冰，暢快解渴，最佳餐桌良伴', 1, 0, N'[{"name":"可口可樂","subDesc":"美國原裝進口"},{"name":"碎冰","subDesc":"RO逆滲透純水製冰"},{"name":"檸檬片","subDesc":"台灣黃檸檬裝飾"},{"name":"食品級碳酸氣體","subDesc":"充氣保鮮"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"可樂原裝糖漿","subDesc":"美國原廠配方"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'柳橙汁')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'柳橙汁', 65.00, 1, GETDATE(), N'每日鮮榨台灣本土甜橙，不添加糖分與防腐劑，原汁原味維生素C滿滿', 1, 0, N'[{"name":"台灣甜橙","subDesc":"每日鮮採現榨"},{"name":"鮮榨檸檬汁","subDesc":"少量提味增鮮"},{"name":"橙皮精油","subDesc":"榨汁自然釋放"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"冰塊","subDesc":"純水製冰"},{"name":"新鮮薄荷葉","subDesc":"裝飾"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'招牌鮮奶茶')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'招牌鮮奶茶', 70.00, 1, GETDATE(), N'精選台灣日月潭紅茶濃泡，搭配有機牧場鮮奶，茶香奶香完美平衡', 1, 0, N'[{"name":"日月潭紅茶","subDesc":"台中南投產區"},{"name":"有機全脂鮮奶","subDesc":"每日新鮮配送"},{"name":"台灣本土蔗糖","subDesc":"細白砂糖"},{"name":"冰塊","subDesc":"純水製冰"},{"name":"伯爵茶葉","subDesc":"調配增添花香"},{"name":"打發鮮奶油","subDesc":"點綴裝飾"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'抹茶拿鐵')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'抹茶拿鐵', 80.00, 1, GETDATE(), N'京都宇治抹茶粉以熱水調製注入義式濃縮，搭配打發鮮奶，草香苦甘交融', 1, 0, N'[{"name":"京都宇治抹茶粉","subDesc":"Ceremonial grade一級"},{"name":"義式濃縮咖啡","subDesc":"深烘焙配方豆"},{"name":"有機全脂鮮奶","subDesc":"每日新鮮配送"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"細白砂糖","subDesc":"可選添加"},{"name":"抹茶粉","subDesc":"表面撒粉裝飾"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'美式黑咖啡')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'美式黑咖啡', 60.00, 1, GETDATE(), N'衣索比亞單一產地豆中深烘焙展現果酸層次，義式萃取加水，風味純粹', 1, 0, N'[{"name":"衣索比亞耶加雪菲豆","subDesc":"單一產地莊園"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"義式濃縮萃取","subDesc":"現場現點現萃"},{"name":"冰塊","subDesc":"可選冷飲版"},{"name":"中深烘焙配方","subDesc":"每週新鮮烘焙"},{"name":"咖啡師手沖","subDesc":"每杯現點現沖"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'卡布奇諾')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'卡布奇諾', 85.00, 1, GETDATE(), N'義大利傳統配比雙份義式濃縮為基底，搭配細緻打發奶泡，撒上肉桂粉提香', 1, 0, N'[{"name":"義大利進口配方咖啡豆","subDesc":"義式濃縮"},{"name":"有機全脂鮮奶","subDesc":"每日新鮮配送"},{"name":"手工Steam奶泡","subDesc":"細膩打發"},{"name":"斯里蘭卡錫蘭肉桂粉","subDesc":"表面撒粉"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"比利時可可粉","subDesc":"裝飾用"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'草莓奶昔')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'草莓奶昔', 90.00, 1, GETDATE(), N'新鮮台灣大湖草莓與香草冰淇淋現打，不添加人工色素，粉紅濃郁清甜迷人', 1, 0, N'[{"name":"台灣大湖草莓","subDesc":"當季新鮮現採"},{"name":"義式香草冰淇淋","subDesc":"Gelato配方"},{"name":"有機全脂鮮奶","subDesc":"每日新鮮配送"},{"name":"打發鮮奶油","subDesc":"頂部裝飾"},{"name":"新鮮草莓醬","subDesc":"草莓熬製"},{"name":"冰塊","subDesc":"純水製冰"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'西瓜汁')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson, StartDate, EndDate)
    VALUES (@DrinkId, N'西瓜汁', 65.00, 1, GETDATE(), N'夏季限定花蓮特大西瓜現打，鮮甜多汁不加水不加糖，冰涼消暑聖品', 1, 1, N'[{"name":"花蓮特大西瓜","subDesc":"夏季限定現採"},{"name":"冰塊","subDesc":"純水製冰"},{"name":"鮮榨檸檬汁","subDesc":"少量提味"},{"name":"新鮮薄荷葉","subDesc":"裝飾提香"},{"name":"飲用水","subDesc":"RO逆滲透純水"},{"name":"細鹽","subDesc":"少量點綴提甜"}]',
    '2026-06-01', '2026-08-31');
ELSE
    UPDATE dbo.Dishes SET StartDate = '2026-06-01', EndDate = '2026-08-31' WHERE DishName = N'西瓜汁' AND StartDate IS NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'熱可可')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'熱可可', 75.00, 1, GETDATE(), N'比利時Callebaut 70%黑巧克力融入溫熱鮮奶，覆蓋棉花糖與肉桂奶泡，暖心享受', 1, 0, N'[{"name":"比利時Callebaut黑巧克力","subDesc":"70%可可含量"},{"name":"有機全脂鮮奶","subDesc":"每日新鮮配送"},{"name":"迷你棉花糖","subDesc":"頂部裝飾"},{"name":"斯里蘭卡錫蘭肉桂粉","subDesc":"撒粉提香"},{"name":"打發鮮奶油","subDesc":"頂部裝飾"},{"name":"比利時可可粉","subDesc":"表面撒粉"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'蜂蜜檸檬氣泡水')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DrinkId, N'蜂蜜檸檬氣泡水', 60.00, 1, GETDATE(), N'台灣龍眼蜂蜜與現榨檸檬汁調製，注入天然氣泡礦泉水，清爽甘甜解渴怡人', 1, 0, N'[{"name":"台灣龍眼蜂蜜","subDesc":"嘉義梅山蜂農直供"},{"name":"台灣本土黃檸檬","subDesc":"現榨"},{"name":"法國Perrier氣泡礦泉水","subDesc":"天然氣泡"},{"name":"冰塊","subDesc":"純水製冰"},{"name":"薄切檸檬片","subDesc":"裝飾"},{"name":"新鮮薄荷葉","subDesc":"裝飾提香"}]');

-- ==================== 3. 甜點 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'提拉米蘇')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DessertId, N'提拉米蘇', 120.00, 1, GETDATE(), N'Mascarpone起司與蛋黃調製絲滑奶霜，浸泡義式濃縮的手指餅乾層層堆疊', 1, 0, N'[{"name":"義大利Mascarpone起司","subDesc":"進口"},{"name":"義大利Ladyfinger手指餅乾","subDesc":"浸濃縮咖啡"},{"name":"雙份義式濃縮咖啡","subDesc":"現場萃取"},{"name":"台灣放牧蛋黃","subDesc":"每日新鮮"},{"name":"義大利Marsala甜酒","subDesc":"西西里產"},{"name":"比利時頂級可可粉","subDesc":"表面篩粉"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'巧克力熔岩蛋糕')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DessertId, N'巧克力熔岩蛋糕', 130.00, 1, GETDATE(), N'外層烤至定型而內心保持融化狀態，72%黑巧克力，搭配香草冰淇淋冷熱交融', 1, 0, N'[{"name":"法芙娜72%黑巧克力","subDesc":"法國進口"},{"name":"法式無鹽發酵奶油","subDesc":"Normandie產區"},{"name":"台灣放牧雞蛋","subDesc":"每日新鮮"},{"name":"日本製菓低筋麵粉","subDesc":"薄力粉"},{"name":"義式香草冰淇淋","subDesc":"Gelato配方"},{"name":"糖粉","subDesc":"裝飾表面"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'法式烤布蕾')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DessertId, N'法式烤布蕾', 115.00, 1, GETDATE(), N'香草莢調製蛋奶液低溫烤製，表面撒砂糖以噴火槍炙出完美焦糖脆殼', 1, 0, N'[{"name":"馬達加斯加香草莢","subDesc":"Bourbon品種"},{"name":"台灣放牧蛋黃","subDesc":"每日新鮮"},{"name":"法國動物性鮮奶油","subDesc":"乳脂38%"},{"name":"全脂牛奶","subDesc":"每日新鮮配送"},{"name":"台灣本土細砂糖","subDesc":"蔗糖"},{"name":"焦糖用砂糖","subDesc":"噴槍炙燒表面"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'草莓千層蛋糕')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@DessertId, N'草莓千層蛋糕', 150.00, 1, GETDATE(), N'手工薄餅皮二十層交疊，每層塗抹香草卡士達奶油，配鮮採大湖草莓細膩精緻', 1, 0, N'[{"name":"手工薄餅皮","subDesc":"每日現製二十層"},{"name":"香草卡士達奶油","subDesc":"香草莢現製"},{"name":"台灣大湖草莓","subDesc":"當季鮮採"},{"name":"法國打發鮮奶油","subDesc":"乳脂38%"},{"name":"馬達加斯加香草莢","subDesc":"Bourbon品種"},{"name":"糖粉","subDesc":"裝飾表面"}]');

-- ==================== 4. 湯品 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式蔬菜礦工湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'義式蔬菜礦工湯', 120.00, 1, GETDATE(), N'源自托斯卡尼農家食譜，匯聚季節蔬菜番茄與義式香料，豐盛暖心健康', 1, 0, N'[{"name":"台灣玉女番茄","subDesc":"新鮮"},{"name":"西芹","subDesc":"法式蔬菜三寶"},{"name":"有機紅蘿蔔","subDesc":"栽培"},{"name":"義大利腰豆","subDesc":"進口罐裝"},{"name":"San Marzano番茄糊","subDesc":"義大利進口"},{"name":"帕馬森起司皮","subDesc":"增味熬煮"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'羅宋牛腩湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'羅宋牛腩湯', 150.00, 1, GETDATE(), N'俄式經典食譜慢火熬煮三小時，甜菜根賦予深紅色澤，牛腩軟爛入味配酸奶', 1, 0, N'[{"name":"台灣黑牛牛腩","subDesc":"低溫慢燉3小時"},{"name":"波蘭有機甜菜根","subDesc":"進口"},{"name":"台灣本土高麗菜","subDesc":"栽培"},{"name":"台灣馬鈴薯","subDesc":"本土品種"},{"name":"牛高湯","subDesc":"鮮熬12小時"},{"name":"法國酸奶","subDesc":"crème fraîche"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'巧達海鮮濃湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'巧達海鮮濃湯', 130.00, 1, GETDATE(), N'美式新英格蘭風格以培根奶油炒香，加入馬鈴薯與綜合海鮮，濃郁暖心', 1, 0, N'[{"name":"台灣新鮮蛤蜊","subDesc":"每日現開"},{"name":"台灣活草蝦","subDesc":"每日現撈"},{"name":"台灣馬鈴薯","subDesc":"本土品種"},{"name":"德國煙燻培根","subDesc":"工法煙燻"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"魚高湯","subDesc":"每日現熬"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'奶油菠菜濃湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'奶油菠菜濃湯', 110.00, 1, GETDATE(), N'新鮮菠菜以奶油炒香加入高湯慢燉，均質機打至絲滑，點綴帕馬森起司', 1, 0, N'[{"name":"有機新鮮菠菜","subDesc":"每日現採"},{"name":"法式無鹽發酵奶油","subDesc":""},{"name":"雞高湯","subDesc":"每日鮮熬8小時"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"帕馬森起司","subDesc":"36個月熟成刨絲"},{"name":"現磨肉豆蔻","subDesc":"提香"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'牛肝菌菇奶油湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'牛肝菌菇奶油湯', 160.00, 1, GETDATE(), N'義大利乾燥牛肝菌重組以奶油炒香，加入雞高湯均質打製，深邃鮮味令人回味', 1, 0, N'[{"name":"義大利Porcini牛肝菌","subDesc":"乾燥重組"},{"name":"法式無鹽發酵奶油","subDesc":""},{"name":"雞高湯","subDesc":"每日鮮熬8小時"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"},{"name":"有機大蒜","subDesc":"新鮮蒜頭"},{"name":"帕馬森起司","subDesc":"36個月熟成"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'法式南瓜培根濃湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'法式南瓜培根濃湯', 120.00, 1, GETDATE(), N'奶油炒香培根後加入南瓜慢燉均質打至絲滑，撒上烤南瓜子，甘甜濃郁', 1, 0, N'[{"name":"台灣栗子南瓜","subDesc":"每日新鮮處理"},{"name":"德國煙燻培根","subDesc":"黑森林工法"},{"name":"法式無鹽奶油","subDesc":"發酵奶油"},{"name":"雞高湯","subDesc":"每日鮮熬8小時"},{"name":"烤南瓜子","subDesc":"點綴裝飾"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'法式洋蔥起司湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'法式洋蔥起司湯', 110.00, 1, GETDATE(), N'奶油慢炒兩小時深焦糖色洋蔥熬製金湯，覆格呂耶爾起司麵包烤至融化拉絲', 1, 0, N'[{"name":"黃洋蔥","subDesc":"慢炒焦糖化2小時"},{"name":"法式無鹽奶油","subDesc":"發酵奶油"},{"name":"牛高湯","subDesc":"鮮熬12小時"},{"name":"格呂耶爾起司","subDesc":"瑞士進口熟成"},{"name":"法棍薄片","subDesc":"每日烘焙"},{"name":"干邑白蘭地","subDesc":"少量提香"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式茄汁海鮮湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'義式茄汁海鮮湯', 180.00, 1, GETDATE(), N'San Marzano番茄醬底加入蛤蜊淡菜與蝦仁，白酒提鮮，義式漁港風味', 1, 0, N'[{"name":"San Marzano番茄","subDesc":"義大利進口"},{"name":"台灣新鮮蛤蜊","subDesc":"每日現開"},{"name":"法國布列塔尼淡菜","subDesc":"進口"},{"name":"台灣活草蝦","subDesc":"每日現撈"},{"name":"義大利白葡萄酒","subDesc":"提鮮"},{"name":"新鮮羅勒","subDesc":"每日現摘"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式蒜味蛤蜊清湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'義式蒜味蛤蜊清湯', 140.00, 1, GETDATE(), N'蒜末橄欖油爆香後白酒燜開鮮蛤蜊，海水鹽分與蒜香完美融合，清甜鮮美', 1, 0, N'[{"name":"台灣鮮蛤蜊","subDesc":"每日現開確保鮮活"},{"name":"有機大蒜","subDesc":"新鮮蒜末"},{"name":"西西里特級橄欖油","subDesc":"冷壓"},{"name":"義大利Vermentino白酒","subDesc":"提鮮"},{"name":"新鮮巴西里","subDesc":"每日現摘"},{"name":"義大利風乾紅椒片","subDesc":"提味"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'黑松露野菇濃湯')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SoupId, N'黑松露野菇濃湯', 200.00, 1, GETDATE(), N'新鮮黑松露薄切融入四種野菇濃湯，奢華土地香氣層層堆疊，每一口都是極致', 1, 0, N'[{"name":"法國佩里戈爾新鮮黑松露","subDesc":"每日鮮送"},{"name":"義大利Porcini牛肝菌","subDesc":"乾燥重組"},{"name":"台灣有機杏鮑菇","subDesc":"栽培"},{"name":"日本鴻喜菇","subDesc":"鮮採"},{"name":"雞高湯","subDesc":"每日鮮熬8小時"},{"name":"動物性鮮奶油","subDesc":"乳脂36%"}]');

-- ==================== 5. 附餐 ====================
IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'松露起司薯條')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'松露起司薯條', 90.00, 1, GETDATE(), N'現炸比利時雙炸薯條撒黑松露鹽與現磨帕馬森起司，淋松露油，香氣四溢', 1, 0, N'[{"name":"比利時薯條","subDesc":"雙炸工法酥脆"},{"name":"義大利黑松露鹽","subDesc":"進口"},{"name":"帕馬森起司","subDesc":"36個月熟成現磨"},{"name":"白松露油","subDesc":"義大利白松露浸製"},{"name":"自製蛋黃醬","subDesc":"每日配製"},{"name":"義大利乾燥香芹粉","subDesc":"撒粉"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式香料大蒜麵包')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'義式香料大蒜麵包', 60.00, 1, GETDATE(), N'鄉村酸種麵包塗抹蒜香奶油撒義式混合香料，高溫烤至外酥內軟，香氣逼人', 1, 0, N'[{"name":"鄉村酸種麵包","subDesc":"每日烘焙"},{"name":"法式有鹽發酵奶油","subDesc":""},{"name":"有機大蒜","subDesc":"新鮮蒜泥"},{"name":"義式混合香草","subDesc":"羅勒百里香迷迭香"},{"name":"帕馬森起司","subDesc":"表面撒粉"},{"name":"西西里特級橄欖油","subDesc":"冷壓"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'義式香草烤雞翅')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'義式香草烤雞翅', 120.00, 1, GETDATE(), N'雞翅以迷迭香百里香橄欖油醃製隔夜，高溫烤至外皮金黃酥脆，肉嫩多汁', 1, 0, N'[{"name":"台灣放牧雞翅","subDesc":"每日新鮮處理"},{"name":"新鮮迷迭香","subDesc":"地中海品種"},{"name":"新鮮百里香","subDesc":"每日栽培"},{"name":"有機大蒜蒜泥","subDesc":"醃製"},{"name":"西班牙特級橄欖油","subDesc":"冷壓"},{"name":"西西里黃檸檬皮屑","subDesc":"增添清香"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'酥炸墨魚圈')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'酥炸墨魚圈', 150.00, 1, GETDATE(), N'新鮮墨魚切圈裹義式香料麵包屑高溫炸至金黃酥脆，搭配自製檸檬蒜泥醬', 1, 0, N'[{"name":"新鮮墨魚","subDesc":"每日現切處理"},{"name":"義式香料麵包屑","subDesc":"現製"},{"name":"台灣放牧雞蛋","subDesc":"裹粉用"},{"name":"日本薄力低筋麵粉","subDesc":""},{"name":"自製檸檬蒜泥醬","subDesc":"每日配製"},{"name":"義大利乾燥巴西里","subDesc":"香料"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'酥炸洋蔥圈')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'酥炸洋蔥圈', 80.00, 1, GETDATE(), N'甜洋蔥厚切圈裹啤酒麵糊炸至金黃酥脆，搭配煙燻辣椒蒜泥醬，外酥內甜', 1, 0, N'[{"name":"台灣本土甜洋蔥","subDesc":"厚切圓圈"},{"name":"台灣啤酒麵糊","subDesc":"調製"},{"name":"日本薄力低筋麵粉","subDesc":""},{"name":"自製煙燻辣椒蒜泥醬","subDesc":"每日配製"},{"name":"法國Guérande海鹽","subDesc":"調味"},{"name":"乾燥巴西里","subDesc":"撒粉裝飾"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'凱薩經典沙拉')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'凱薩經典沙拉', 100.00, 1, GETDATE(), N'新鮮羅馬萵苣搭配自製鯷魚凱薩醬，加入現炸麵包丁與帕馬森起司薄片', 1, 0, N'[{"name":"羅馬萵苣","subDesc":"每日現採"},{"name":"自製凱薩醬","subDesc":"鯷魚蒜頭蛋黃調製"},{"name":"帕馬森起司薄片","subDesc":"36個月熟成"},{"name":"現炸酥脆麵包丁","subDesc":"鄉村麵包"},{"name":"義大利橄欖油漬鯷魚","subDesc":"裝罐"},{"name":"現磨粗粒黑胡椒","subDesc":"提味"}]');

IF NOT EXISTS (SELECT 1 FROM dbo.Dishes WHERE DishName = N'分享拼盤')
    INSERT INTO dbo.Dishes (CategoryId, DishName, Price, IsActive, CreatedAt, Description, IsTakeOut, IsLimited, IngredientsJson)
    VALUES (@SideId, N'分享拼盤', 320.00, 1, GETDATE(), N'精選薯條墨魚圈雞翅洋蔥圈四種經典小食組合，適合四人分享，附三款醬料', 1, 0, N'[{"name":"比利時雙炸薯條","subDesc":"酥脆"},{"name":"新鮮墨魚圈","subDesc":"每日現切"},{"name":"台灣放牧雞翅","subDesc":"烤至金黃"},{"name":"台灣甜洋蔥圈","subDesc":"啤酒麵糊"},{"name":"自製蒜泥醬","subDesc":"每日配製"},{"name":"主廚BBQ醬","subDesc":"特製秘方"}]');

-- ==================== 限定供應設定 ====================
-- 先清除所有限定（已在各 INSERT 帶入，此區統一管理）
UPDATE dbo.Dishes SET IsLimited = 0, StartDate = NULL, EndDate = NULL;

-- 主餐：供應中（倒數計時）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(GETDATE() AS DATE),                       EndDate=CAST(DATEADD(DAY, 7,GETDATE()) AS DATE) WHERE DishName=N'龍蝦奶油義大利麵';
-- 主餐：即將開始（明天才開始，< 24 小時）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,1,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY, 8,GETDATE()) AS DATE) WHERE DishName=N'法式洋蔥湯牛排';
-- 主餐：尚未供應（5 天後才開始）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,5,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY,12,GETDATE()) AS DATE) WHERE DishName=N'香草烤半雞';
-- 主餐：已結束
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,-10,GETDATE()) AS DATE),      EndDate=CAST(DATEADD(DAY, -1,GETDATE()) AS DATE) WHERE DishName=N'燻鴨胸沙拉';

-- 飲料：尚未供應（夏季限定）
UPDATE dbo.Dishes SET IsLimited=1, StartDate='2026-06-01',                                   EndDate='2026-08-31'                            WHERE DishName=N'西瓜汁';
-- 飲料：供應中
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(GETDATE() AS DATE),                       EndDate=CAST(DATEADD(DAY, 3,GETDATE()) AS DATE) WHERE DishName=N'抹茶拿鐵';
-- 飲料：即將開始（明天）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,1,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY, 4,GETDATE()) AS DATE) WHERE DishName=N'草莓奶昔';
-- 飲料：已結束
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,-10,GETDATE()) AS DATE),      EndDate=CAST(DATEADD(DAY, -2,GETDATE()) AS DATE) WHERE DishName=N'蜂蜜檸檬氣泡水';

-- 甜點：供應中
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(GETDATE() AS DATE),                       EndDate=CAST(DATEADD(DAY, 2,GETDATE()) AS DATE) WHERE DishName=N'巧克力熔岩蛋糕';
-- 甜點：即將開始（明天）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,1,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY, 5,GETDATE()) AS DATE) WHERE DishName=N'草莓千層蛋糕';
-- 甜點：尚未供應（7 天後）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,7,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY,14,GETDATE()) AS DATE) WHERE DishName=N'法式烤布蕾';
-- 甜點：已結束
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,-12,GETDATE()) AS DATE),      EndDate=CAST(DATEADD(DAY, -1,GETDATE()) AS DATE) WHERE DishName=N'提拉米蘇';

-- 湯品：供應中
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(GETDATE() AS DATE),                       EndDate=CAST(DATEADD(DAY, 5,GETDATE()) AS DATE) WHERE DishName=N'黑松露野菇濃湯';
-- 湯品：即將開始（明天）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,1,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY, 6,GETDATE()) AS DATE) WHERE DishName=N'義式茄汁海鮮湯';
-- 湯品：尚未供應（3 天後）
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,3,GETDATE()) AS DATE),        EndDate=CAST(DATEADD(DAY,10,GETDATE()) AS DATE) WHERE DishName=N'牛肝菌菇奶油湯';
-- 湯品：已結束
UPDATE dbo.Dishes SET IsLimited=1, StartDate=CAST(DATEADD(DAY,-15,GETDATE()) AS DATE),      EndDate=CAST(DATEADD(DAY, -1,GETDATE()) AS DATE) WHERE DishName=N'羅宋牛腩湯';

GO
