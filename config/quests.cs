$Farming::QuestCooldown = 600;
$Farming::QuestAcceptTime = 15;
$Farming::QuestCompleteCooldown = 300;

new ScriptObject(SeedQuestType) {
	class = "QuestType";

	maxBudget = 1200;
	rewardTable = BS_Seeds_Rare;
	maxRewardItems = 2;
	minCashReward = 1;
	maxCashReward = 10;
	cashRewardIncrement = 10;

	minBonusFactor = 2;
	maxBonusFactor = 2.5;
	requestTable = BS_BigBuyer;
	budgetPerRequestItem = 300;
	maxRequestItems = 4;
};