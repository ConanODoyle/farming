$Farming::QuestCooldown = 600;
$Farming::QuestAcceptTime = 15;
$Farming::QuestCompleteCooldown = 300;

/////////////////////////////////////////////////////////////////////////////////////////////////////
/// requests ////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////

new ScriptObject(BasicRequests) { class = "ShopObject"; };

BasicRequests.option[BasicRequests.count++ - 1]  = "Potato"	TAB 20;
BasicRequests.option[BasicRequests.count++ - 1]  = "Carrot"	TAB 20;


new ScriptObject(AdvancedRequests) { class = "ShopObject"; };

AdvancedRequests.option[AdvancedRequests.count++ - 1]  = "Tomato"	TAB 40;
AdvancedRequests.option[AdvancedRequests.count++ - 1]  = "Corn"		TAB 40;
AdvancedRequests.option[AdvancedRequests.count++ - 1]  = "Wheat"	TAB 40;
AdvancedRequests.option[AdvancedRequests.count++ - 1]  = "Cabbage"	TAB 40;
AdvancedRequests.option[AdvancedRequests.count++ - 1]  = "Onion"	TAB 40;


new ScriptObject(SpecialRequests) { class = "ShopObject"; };

NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Blueberry"	TAB 10;
NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Turnip"		TAB 10;
NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Cactus"		TAB 10;
NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Portobello"	TAB 10;
NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Watermelon"	TAB 10;
NonTreeRareRequests.option[NonTreeRareRequests.count++ - 1]  = "Chili"		TAB 10;


new ScriptObject(RareRequests) { class = "ShopObject"; };

RareRequests.option[RareRequests.count++ - 1]  = "Blueberry"	TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Turnip"		TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Cactus"		TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Portobello"	TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Watermelon"	TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Chili"		TAB 10;
RareRequests.option[RareRequests.count++ - 1]  = "Apple"		TAB 3;
RareRequests.option[RareRequests.count++ - 1]  = "Peach"		TAB 3;
RareRequests.option[RareRequests.count++ - 1]  = "Mango"		TAB 3;
RareRequests.option[RareRequests.count++ - 1]  = "Date"			TAB 3;


new ScriptObject(RareAndEthanolRequests) { class = "ShopObject"; };

RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Ethanol"		TAB 108;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Blueberry"	TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Turnip"		TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Cactus"		TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Portobello"	TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Watermelon"	TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Chili"		TAB 10;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Apple"		TAB 3;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Peach"		TAB 3;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Mango"		TAB 3;
RareAndEthanolRequests.option[RareAndEthanolRequests.count++ - 1]  = "Date"			TAB 3;


new ScriptObject(CommonRequests) { class = "ShopObject"; };

CommonRequests.option[CommonRequests.count++ - 1]  = "Potato"	TAB 20;
CommonRequests.option[CommonRequests.count++ - 1]  = "Carrot"	TAB 20;
CommonRequests.option[CommonRequests.count++ - 1]  = "Tomato"	TAB 40;
CommonRequests.option[CommonRequests.count++ - 1]  = "Corn"		TAB 40;
CommonRequests.option[CommonRequests.count++ - 1]  = "Wheat"	TAB 40;
CommonRequests.option[CommonRequests.count++ - 1]  = "Cabbage"	TAB 40;
CommonRequests.option[CommonRequests.count++ - 1]  = "Onion"	TAB 40;


new ScriptObject(NonTreeRequests) { class = "ShopObject"; };

NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Potato"		TAB 20;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Carrot"		TAB 20;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Tomato"		TAB 40;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Corn"		TAB 40;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Wheat"		TAB 40;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Cabbage"	TAB 40;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Onion"		TAB 40;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Blueberry"	TAB 10;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Turnip"		TAB 10;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Cactus"		TAB 10;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Portobello"	TAB 10;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Watermelon"	TAB 10;
NonTreeRequests.option[NonTreeRequests.count++ - 1]  = "Chili"		TAB 10;


new ScriptObject(NonBasicRequests) { class = "ShopObject"; };

NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Tomato"		TAB 40;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Corn"			TAB 40;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Wheat"		TAB 40;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Cabbage"		TAB 40;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Onion"		TAB 40;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Blueberry"	TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Turnip"		TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Cactus"		TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Portobello"	TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Watermelon"	TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Chili"		TAB 10;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Apple"		TAB 3;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Peach"		TAB 3;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Mango"		TAB 3;
NonBasicRequests.option[NonBasicRequests.count++ - 1]  = "Date"			TAB 3;


new ScriptObject(AdvancedAndSpecialRequests) { class = "ShopObject"; };

AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Tomato"		TAB 40;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Corn"			TAB 40;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Wheat"		TAB 40;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Cabbage"		TAB 40;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Onion"		TAB 40;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Blueberry"	TAB 10;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Turnip"		TAB 10;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Cactus"		TAB 10;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Portobello"	TAB 10;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Watermelon"	TAB 10;
AdvancedAndSpecialRequests.option[AdvancedAndSpecialRequests.count++ - 1]  = "Chili"		TAB 10;


new ScriptObject(AllRequests) { class = "ShopObject"; };

AllRequests.option[AllRequests.count++ - 1]  = "Potato"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Carrot"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Tomato"		TAB 40;
AllRequests.option[AllRequests.count++ - 1]  = "Corn"		TAB 40;
AllRequests.option[AllRequests.count++ - 1]  = "Wheat"		TAB 40;
AllRequests.option[AllRequests.count++ - 1]  = "Cabbage"	TAB 40;
AllRequests.option[AllRequests.count++ - 1]  = "Onion"		TAB 40;
AllRequests.option[AllRequests.count++ - 1]  = "Blueberry"	TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Turnip"		TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Cactus"		TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Portobello"	TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Watermelon"	TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Chili"		TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Apple"		TAB 3;
AllRequests.option[AllRequests.count++ - 1]  = "Peach"		TAB 3;
AllRequests.option[AllRequests.count++ - 1]  = "Mango"		TAB 3;
AllRequests.option[AllRequests.count++ - 1]  = "Date"		TAB 3;


new ScriptObject(CompostRequests) { class = "ShopObject"; };

AllRequests.option[AllRequests.count++ - 1]  = "Potato"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Carrot"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Tomato"		TAB 30;
AllRequests.option[AllRequests.count++ - 1]  = "Turnip"		TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Portobello"	TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Watermelon"	TAB 20;


new ScriptObject(FertilizerRequests) { class = "ShopObject"; };

AllRequests.option[AllRequests.count++ - 1]  = "Phosphate"	TAB 480;
AllRequests.option[AllRequests.count++ - 1]  = "Potato"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Carrot"		TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Tomato"		TAB 30;
AllRequests.option[AllRequests.count++ - 1]  = "Turnip"		TAB 10;
AllRequests.option[AllRequests.count++ - 1]  = "Portobello"	TAB 20;
AllRequests.option[AllRequests.count++ - 1]  = "Watermelon"	TAB 20;


new ScriptObject(MineralRequests) { class = "ShopObject"; };

MineralRequests.option[MineralRequests.count++ - 1]  = "Phosphate"	TAB 1;
MineralRequests.option[MineralRequests.count++ - 1]  = "Coal"		TAB 1;

/////////////////////////////////////////////////////////////////////////////////////////////////////
/// rewards /////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////

new ScriptObject(HarvestToolQuestRewards) { class = "ShopObject"; };

HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "TrowelItem"			TAB 40;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "ClipperItem"		TAB 40;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "WeedCutterItem"		TAB 40;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "TreeClipperItem"	TAB 20;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "HoeItem"			TAB 30;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "SickleItem"			TAB 30;
HarvestToolQuestRewards.option[HarvestToolQuestRewards.count++ - 1]  = "ReclaimerItem"		TAB 10;


new ScriptObject(WateringToolQuestRewards) { class = "ShopObject"; };

WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "WateringCan2Item"		TAB 40;
WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "WateringCan3Item"		TAB 20;
WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "HoseItem"				TAB 10;
WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "HoseV2Item"			TAB 5;
WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "WateringCatItem"		TAB 1;
WateringToolQuestRewards.option[WateringToolQuestRewards.count++ - 1]  = "WateringSnakeItem"	TAB 1;


new ScriptObject(WeedKillerQuestRewards) { class = "ShopObject"; };

WeedKillerQuestRewards.option[WeedKillerQuestRewards.count++ - 1]  = "WeedKiller0Item"	TAB 1;


new ScriptObject(MiscToolQuestRewards) { class = "ShopObject"; };

MiscToolQuestRewards.option[MiscToolQuestRewards.count++ - 1]  = "PlanterItem"			TAB 7;
MiscToolQuestRewards.option[MiscToolQuestRewards.count++ - 1]  = "PlanterV2Item"		TAB 3;
MiscToolQuestRewards.option[MiscToolQuestRewards.count++ - 1]  = "OrganicAnalyzerItem"	TAB 10;
MiscToolQuestRewards.option[MiscToolQuestRewards.count++ - 1]  = "ShovelItem"			TAB 15;


new ScriptObject(BasicElectricQuestRewards) { class = "ShopObject"; };

BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "MediumPumpItem"			TAB 5;
// BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "EthanolRefineryItem"	TAB 8;
BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "IndoorLightItem"		TAB 10;
BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "BatteryItem"			TAB 15;


new ScriptObject(AdvancedElectricQuestRewards) { class = "ShopObject"; };

BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "LargePumpItem"			TAB 5;
// BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "EthanolGeneratorItem"	TAB 18;
// BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "LargeIndoorLightItem"	TAB 14;
// BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "Combiner"				TAB 10;
// BasicElectricQuestRewards.option[BasicElectricQuestRewards.count++ - 1]  = "LargeBatteryItem"		TAB 11;


new ScriptObject(CompostQuestRewards) { class = "ShopObject"; };

CompostQuestRewards.option[CompostQuestRewards.count++ - 1]  = "PlanterItem"			TAB 7;


new ScriptObject(FertilizerQuestRewards) { class = "ShopObject"; };

FertilizerQuestRewards.option[FertilizerQuestRewards.count++ - 1]  = "PlanterItem"			TAB 7;


new ScriptObject(RareSeedQuestRewards) { class = "ShopObject"; };

RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "BlueberrySeedItem"	TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "TurnipSeedItem"		TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "CactusSeedItem"		TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "PortobelloSeedItem"	TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "WatermelonSeedItem"	TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "ChiliSeedItem"		TAB 10;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "DaisySeedItem"		TAB 1;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "LilySeedItem"			TAB 1;
RareSeedQuestRewards.option[RareSeedQuestRewards.count++ - 1]  = "RoseSeedItem"			TAB 1;


new ScriptObject(TreeSeedQuestRewards) { class = "ShopObject"; };

TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "AppleSeedItem"	TAB 40;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "PeachSeedItem"	TAB 30;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "MangoSeedItem"	TAB 20;
TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "DateSeedItem"		TAB 10;


new ScriptObject(PotQuestRewards) { class = "ShopObject"; };

TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "FlowerPotItem"	TAB 1;


new ScriptObject(BoxQuestRewards) { class = "ShopObject"; };

TreeSeedQuestRewards.option[TreeSeedQuestRewards.count++ - 1]  = "PlanterBoxItem"	TAB 1;

/////////////////////////////////////////////////////////////////////////////////////////////////////
/// quests //////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////

new ScriptObject(BigBuyerQuestType) {
	class = "QuestType";
	questWeight = 10;

	maxBudget = 1000;

	rewardsItems = false;

	minCashReward = 25;
	maxCashReward = 100;
	cashRewardIncrement = 10;

	minBonusFactor = 1.8;
	maxBonusFactor = 2.2;

	requestTable = RareRequests;
	budgetPerRequestItem = 400;
	maxRequestItems = 3;
};

new ScriptObject(SupermarketBuyerQuestType) {
	class = "QuestType";
	questWeight = 10;

	maxBudget = 250;

	rewardsItems = false;

	minCashReward = 5;
	maxCashReward = 25;
	cashRewardIncrement = 10;

	minBonusFactor = 1.2;
	maxBonusFactor = 1.5;

	requestTable = CommonRequests;
	budgetPerRequestItem = 75;
	maxRequestItems = 4;
};

new ScriptObject(WateringToolQuestType) {
	class = "QuestType";
	questWeight = 13;

	maxBudget = 2500;

	rewardsItems = true;
	rewardTable = WateringToolQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 2;
	maxBonusFactor = 2.5;

	requestTable = CommonRequests;
	budgetPerRequestItem = 800;
	maxRequestItems = 4;
};

new ScriptObject(HarvestToolQuestType) {
	class = "QuestType";
	questWeight = 10;

	maxBudget = 1600;

	rewardsItems = true;
	rewardTable = HarvestToolQuestRewards;
	maxRewardItems = 2;

	minBonusFactor = 1.5;
	maxBonusFactor = 2;

	requestTable = RareRequests;
	budgetPerRequestItem = 500;
	maxRequestItems = 4;
};

new ScriptObject(WeedKillerQuestType) {
	class = "QuestType";
	questWeight = 16;

	maxBudget = 200;

	rewardsItems = true;
	rewardTable = WeedKillerQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 1.2;
	maxBonusFactor = 1.5;

	requestTable = BasicRequests;
	budgetPerRequestItem = 100;
	maxRequestItems = 2;
};

new ScriptObject(MiscToolQuestType) {
	class = "QuestType";
	questWeight = 10;

	maxBudget = 2000;

	rewardsItems = true;
	rewardTable = MiscToolQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 1.8;
	maxBonusFactor = 2.2;

	requestTable = AdvancedAndSpecialRequests;
	budgetPerRequestItem = 350;
	maxRequestItems = 4;
};

new ScriptObject(BasicElectricQuestType) {
	class = "QuestType";
	questWeight = 20;

	maxBudget = 2000;

	rewardsItems = true;
	rewardTable = BasicElectricQuestRewards;
	maxRewardItems = 2;

	minBonusFactor = 1.8;
	maxBonusFactor = 2.2;

	requestTable = RareRequests;
	budgetPerRequestItem = 250;
	maxRequestItems = 5;
};

new ScriptObject(AdvancedElectricQuestType) {
	class = "QuestType";
	questWeight = 5;

	maxBudget = 3500;

	rewardsItems = true;
	rewardTable = AdvancedElectricQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 1.8;
	maxBonusFactor = 2.2;

	requestTable = RareAndEthanolRequests;
	budgetPerRequestItem = 350;
	maxRequestItems = 6;
};

new ScriptObject(CompostQuestType) {
	class = "QuestType";
	questWeight = 5;

	maxBudget = 240;

	rewardsItems = true;
	rewardTable = CompostQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 1.2;
	maxBonusFactor = 1.4;

	requestTable = CompostRequests;
	maxRequestItems = 1;
};

new ScriptObject(FertilizerQuestType) {
	class = "QuestType";
	questWeight = 5;

	maxBudget = 120;

	rewardsItems = true;
	rewardTable = FertilizerQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 0.8;
	maxBonusFactor = 1.1;

	requestTable = FertilizerRequests;
	maxRequestItems = 3;
};

new ScriptObject(RareSeedQuestType) {
	class = "QuestType";
	questWeight = 40;

	maxBudget = 600;

	rewardsItems = true;
	rewardTable = RareSeedQuestRewards;
	maxRewardItems = 2;

	minCashReward = 1;
	maxCashReward = 10;
	cashRewardIncrement = 10;

	minBonusFactor = 1.3;
	maxBonusFactor = 1.8;

	requestTable = NonTreeRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};

new ScriptObject(TreeSeedQuestType) {
	class = "QuestType";
	questWeight = 4;

	maxBudget = 2000;

	rewardsItems = true;
	rewardTable = TreeSeedQuestRewards;
	maxRewardItems = 1;

	minCashReward = 5;
	maxCashReward = 25;
	cashRewardIncrement = 10;

	minBonusFactor = 1.8;
	maxBonusFactor = 2.2;

	requestTable = AllRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};

new ScriptObject(PotQuestType) {
	class = "QuestType";
	questWeight = 3;

	maxBudget = 100;

	rewardsItems = true;
	rewardTable = PotQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 0.6;
	maxBonusFactor = 0.8;

	requestTable = AllRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};

new ScriptObject(BoxQuestType) {
	class = "QuestType";
	questWeight = 2;

	maxBudget = 300;

	rewardsItems = true;
	rewardTable = BoxQuestRewards;
	maxRewardItems = 1;

	minBonusFactor = 0.6;
	maxBonusFactor = 0.8;

	requestTable = AllRequests;
	budgetPerRequestItem = 150;
	maxRequestItems = 4;
};

new ScriptObject(SmallMineralQuestType) {
	class = "QuestType";
	questWeight = 5;

	maxBudget = 30;

	rewardsItems = false;

	minCashReward = 1;
	maxCashReward = 3;
	cashRewardIncrement = 10;

	minBonusFactor = 1;
	maxBonusFactor = 1;

	requestTable = MineralRequests;
	maxRequestItems = 1;
};

new ScriptObject(LargeMineralQuestType) {
	class = "QuestType";
	questWeight = 1;

	maxBudget = 50;

	rewardsItems = false;

	minCashReward = 4;
	maxCashReward = 5;
	cashRewardIncrement = 10;

	minBonusFactor = 1;
	maxBonusFactor = 1;

	requestTable = MineralRequests;
	budgetPerRequestItem = 25;
	maxRequestItems = 2;
};