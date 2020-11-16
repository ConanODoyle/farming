$Farming::QuestCooldown = 600;
$Farming::QuestAcceptTime = 15;
$Farming::QuestCompleteCooldown = 300;

new ScriptObject(SeedQuestType) {
	class = "QuestType";

	maxBudget = 3700;
	rewardTable = BS_Seeds_Rare;
	maxRewardItems = 2;
	minCashReward = 1;
	maxCashReward = 10;
	cashRewardIncrement = 10;

	minBonusFactor = 1.3;
	maxBonusFactor = 1.7;
	requestTable = BS_BigBuyer;
	maxRequestItems = 4;
};