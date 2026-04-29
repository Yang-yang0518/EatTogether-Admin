USE [EatTogetherDB]
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'義式番茄義大利麵') AND Nickname = N'麵控Jenny')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'麵控Jenny', N'番茄醬汁酸甜平衡得很好，帕馬森起司香氣濃郁，麵條彈牙好吃！' FROM dbo.Dishes WHERE DishName = N'義式番茄義大利麵';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'義式番茄義大利麵') AND Nickname = N'義大利控')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'義大利控', N'San Marzano番茄的味道真的不一樣，吃得出用料實在' FROM dbo.Dishes WHERE DishName = N'義式番茄義大利麵';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'松露野菇燉飯') AND Nickname = N'饕客Ray')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'饕客Ray', N'松露香氣撲鼻，燉飯濃稠度剛剛好，四種野菇層次豐富，值得！' FROM dbo.Dishes WHERE DishName = N'松露野菇燉飯';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'松露野菇燉飯') AND Nickname = N'米其林獵人')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'米其林獵人', N'這道是我來這裡必點的，每次都驚豔，朋友來必推' FROM dbo.Dishes WHERE DishName = N'松露野菇燉飯';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'墨西哥雞肉捲') AND Nickname = N'辣味愛好者')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'辣味愛好者', N'雞肉醃得很入味，莎莎醬新鮮，薄餅脆度剛好，份量也很紮實' FROM dbo.Dishes WHERE DishName = N'墨西哥雞肉捲';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'墨西哥雞肉捲') AND Nickname = N'Carol')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'Carol', N'不會太辣，香料味很對味，配料很豐富，下次還要點' FROM dbo.Dishes WHERE DishName = N'墨西哥雞肉捲';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'檸檬奶油鱈魚排') AND Nickname = N'海鮮控阿哲')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'海鮮控阿哲', N'鱈魚肉質嫩到不行，檸檬奶油醬清爽不膩，魚完全沒有腥味' FROM dbo.Dishes WHERE DishName = N'檸檬奶油鱈魚排';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'檸檬奶油鱈魚排') AND Nickname = N'輕食派Lisa')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'輕食派Lisa', N'清爽又有飽足感，蔬菜搭配得很好，健康美味兼顧' FROM dbo.Dishes WHERE DishName = N'檸檬奶油鱈魚排';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'紅酒燉牛肉飯') AND Nickname = N'肉食主義者')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'肉食主義者', N'牛肉燉到超軟嫩，紅酒醬汁香氣迷人，拌飯一起吃超滿足' FROM dbo.Dishes WHERE DishName = N'紅酒燉牛肉飯';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'紅酒燉牛肉飯') AND Nickname = N'週末熟客')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'週末熟客', N'每次假日必來點這道，份量大又好吃，CP值很高' FROM dbo.Dishes WHERE DishName = N'紅酒燉牛肉飯';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'龍蝦奶油義大利麵') AND Nickname = N'海鮮達人小陳')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'海鮮達人小陳', N'龍蝦份量很實在，奶油醬濃郁但不膩，麵條吸飽醬汁超香' FROM dbo.Dishes WHERE DishName = N'龍蝦奶油義大利麵';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'龍蝦奶油義大利麵') AND Nickname = N'慶生首選')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'慶生首選', N'帶女友來慶生點這道，她超感動，擺盤精緻很有儀式感' FROM dbo.Dishes WHERE DishName = N'龍蝦奶油義大利麵';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'抹茶拿鐵') AND Nickname = N'抹茶控Mia')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'抹茶控Mia', N'抹茶味道很純正，不會太甜，茶香和奶香比例完美' FROM dbo.Dishes WHERE DishName = N'抹茶拿鐵';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'抹茶拿鐵') AND Nickname = N'下午茶必備')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'下午茶必備', N'每次用餐一定配這杯，抹茶濃度剛好，喝完很舒服' FROM dbo.Dishes WHERE DishName = N'抹茶拿鐵';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'草莓奶昔') AND Nickname = N'甜點女孩')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'甜點女孩', N'草莓味道很真實，不是那種人工香精味，濃稠度剛剛好好喝' FROM dbo.Dishes WHERE DishName = N'草莓奶昔';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'草莓奶昔') AND Nickname = N'夏天必喝')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'夏天必喝', N'清爽又香甜，小孩超愛，每次來都要點一杯' FROM dbo.Dishes WHERE DishName = N'草莓奶昔';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'熱可可') AND Nickname = N'巧克力控')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'巧克力控', N'可可味道很厚實，甜度適中，冬天喝超幸福' FROM dbo.Dishes WHERE DishName = N'熱可可';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'熱可可') AND Nickname = N'暖心飲品')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'暖心飲品', N'濃郁不死甜，喝起來很有質感，推薦給喜歡巧克力的人' FROM dbo.Dishes WHERE DishName = N'熱可可';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'巧克力熔岩蛋糕') AND Nickname = N'甜點獵人')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'甜點獵人', N'切開那瞬間岩漿緩緩流出，巧克力濃度很高，配冰淇淋絕了' FROM dbo.Dishes WHERE DishName = N'巧克力熔岩蛋糕';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'巧克力熔岩蛋糕') AND Nickname = N'IG必拍款')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'IG必拍款', N'視覺和味覺雙重享受，朋友看到照片都問我在哪裡吃的' FROM dbo.Dishes WHERE DishName = N'巧克力熔岩蛋糕';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'草莓千層蛋糕') AND Nickname = N'千層控Amy')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'千層控Amy', N'每一層都很細緻，草莓新鮮酸甜，奶油不會太甜膩，完美' FROM dbo.Dishes WHERE DishName = N'草莓千層蛋糕';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'草莓千層蛋糕') AND Nickname = N'甜點品評家')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'甜點品評家', N'層次感十足，可以吃得出手工製作的用心，值得專程來吃' FROM dbo.Dishes WHERE DishName = N'草莓千層蛋糕';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'牛肝菌菇奶油湯') AND Nickname = N'湯品愛好者')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'湯品愛好者', N'菌菇香氣超濃，奶油湯底絲滑，配麵包沾著喝超幸福' FROM dbo.Dishes WHERE DishName = N'牛肝菌菇奶油湯';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'牛肝菌菇奶油湯') AND Nickname = N'秋冬必點')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'秋冬必點', N'每次天氣涼就想來喝這碗，暖胃又暖心，真的很厲害' FROM dbo.Dishes WHERE DishName = N'牛肝菌菇奶油湯';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'義式蒜味蛤蜊清湯') AND Nickname = N'蛤蜊愛好者')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'蛤蜊愛好者', N'蛤蜊都是開口的新鮮貨，湯頭清甜帶蒜香，喝完整個人都舒暢' FROM dbo.Dishes WHERE DishName = N'義式蒜味蛤蜊清湯';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'義式蒜味蛤蜊清湯') AND Nickname = N'海鮮控小玲')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'海鮮控小玲', N'清爽不油膩，蛤蜊肉很飽滿，湯汁鮮甜到想直接喝光' FROM dbo.Dishes WHERE DishName = N'義式蒜味蛤蜊清湯';

IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'分享拼盤') AND Nickname = N'聚餐首選')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'聚餐首選', N'品項豐富，每樣都有一定水準，適合多人共享，非常推薦' FROM dbo.Dishes WHERE DishName = N'分享拼盤';
IF NOT EXISTS (SELECT 1 FROM dbo.Reviews WHERE DishId = (SELECT Id FROM dbo.Dishes WHERE DishName = N'分享拼盤') AND Nickname = N'家庭聚餐')
    INSERT INTO dbo.Reviews (DishId, Nickname, Content) SELECT Id, N'家庭聚餐', N'帶家人來吃，大家都說好吃，份量夠多，點一盤大家都很滿足' FROM dbo.Dishes WHERE DishName = N'分享拼盤';
GO