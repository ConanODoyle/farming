$Farming::QuestCooldown = 600;
$Farming::QuestAcceptTime = 15;
$Farming::QuestCompleteCooldown = 300;

new ScriptObject(RareSeedQuestRewards) { class = "ShopObject"; };

RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "BlueberrySeedItem"	TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "TurnipSeedItem"		TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "DaisySeedItem"		TAB 2;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "LilySeedItem"			TAB 2;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "RoseSeedItem"			TAB 2;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "CactusSeedItem"		TAB 30;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "PortobelloSeedItem"	TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "WatermelonSeedItem"	TAB 10;

new ScriptObject(RareSeedQuestRequests) { class = "ShopObject"; };

RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Potato"			TAB 60;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Carrot"			TAB 70;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Tomato"			TAB 60;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Corn"			TAB 40;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Wheat"			TAB 30;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Portobello"		TAB 20;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Cabbage"		TAB 15;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Onion"			TAB 50;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Blueberry"		TAB 8;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Turnip"			TAB 8;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Chili"			TAB 8;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Cactus"			TAB 8;
RareSeedQuestRequests.option[RareSeedQuestRequests.count++ - 1]  = "Watermelon"		TAB 4;

new ScriptObject(RareSeedQuestType) {
	class = "QuestType";

	maxBudget = 600;
	rewardTable = RareSeedQuestRewards;
	maxRewardItems = 2;
	minCashReward = 1;
	maxCashReward = 10;
	cashRewardIncrement = 10;

	minBonusFactor = 2;
	maxBonusFactor = 2.5;
	requestTable = RareSeedQuestRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};

new ScriptObject(TreeSeedQuestRewards) { class = "ShopObject"; };

TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "AppleSeedItem"			TAB 8;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "MangoSeedItem"			TAB 4;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "DateSeedItem"			TAB 5;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "PeachSeedItem"			TAB 5;

new ScriptObject(TreeSeedQuestRequests) { class = "ShopObject"; };

TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Potato"			TAB 60;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Carrot"			TAB 70;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Tomato"			TAB 60;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Corn"			TAB 40;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Wheat"			TAB 30;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Portobello"		TAB 20;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Cabbage"		TAB 15;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Onion"			TAB 50;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Blueberry"		TAB 8;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Turnip"			TAB 8;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Apple"			TAB 8;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Mango"			TAB 4;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Chili"			TAB 8;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Cactus"			TAB 8;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Watermelon"		TAB 4;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Date"			TAB 2;
TreeSeedQuestRequests.option[TreeSeedQuestRequests.count++ - 1]  = "Peach"			TAB 2;

new ScriptObject(RareSeedQuestType) {
	class = "QuestType";

	maxBudget = 1500;
	rewardTable = TreeSeedQuestRewards;
	maxRewardItems = 1;
	minCashReward = 5;
	maxCashReward = 25;
	cashRewardIncrement = 10;

	minBonusFactor = 2;
	maxBonusFactor = 2.5;
	requestTable = TreeSeedQuestRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};