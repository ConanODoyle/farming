datablock fxDTSBrickData(brickBulletinBoardData)
{
	brickFile = "./resources/bulletinboard.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Bulletin Board";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting1Data)
{
	brickFile = "./resources/posting1.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting1";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting2Data)
{
	brickFile = "./resources/posting2.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting2";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting3Data)
{
	brickFile = "./resources/posting3.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting3";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting4Data)
{
	brickFile = "./resources/posting4.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting4";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting5Data)
{
	brickFile = "./resources/posting5.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting5";
	cost = -1;
};

datablock fxDTSBrickData(brickPosting6Data)
{
	brickFile = "./resources/posting6.blb";

	category = "Farming";
	subCategory = "Bulletin Board";
	uiName = "Posting6";
	cost = -1;
};

datablock fxDTSBrickData(brickQuestPadHorizontal1x1Data)
{
	brickFile = "./resources/questPadHorizontal1x1.blb";

	category = "Farming";
	subCategory = "Quests";
	uiName = "Horizontal 1x1f Quest Pad";
	cost = -1;
};

datablock fxDTSBrickData(brickQuestPadHorizontal1x2Data)
{
	brickFile = "./resources/questPadHorizontal1x2.blb";

	category = "Farming";
	subCategory = "Quests";
	uiName = "Horizontal 1x2f Quest Pad";
	cost = -1;
};

datablock fxDTSBrickData(brickQuestPadVerticalData)
{
	brickFile = "./resources/questPadVertical.blb";

	category = "Farming";
	subCategory = "Quests";
	uiName = "Vertical 1x1 Quest Pad";
	cost = -1;
};

datablock fxDTSBrickData(brickDepositBoxClosedData)
{
	brickFile = "./resources/depositBoxClosed.blb";

	category = "Farming";
	subCategory = "Quests";
	uiName = "Deposit Box";
	cost = -1;
	isQuestSubmissionPoint = true;
	openDatablock = brickDepositBoxOpenData;
};

datablock fxDTSBrickData(brickDepositBoxOpenData)
{
	brickFile = "./resources/depositBoxOpen.blb";

	category = "";
	subCategory = "";
	uiName = "Open Deposit Box";
	cost = -1;
	isQuestSubmissionPoint = true;
	closedDatablock = brickDepositBoxClosedData;
};



function addQuestDepositEvent(%this, %botForm)
{
	for (%i = 1; %i < 5; %i++)
	{
		%param[%i] = %this.eventOutputParameter[0, %i];
	}
	%this.clearEvents();

	%enabled = 1;
	if (%botForm)
	{
		%inputEvent = "onBotActivated";
	}
	else
	{
		%inputEvent = "onActivate";
	}
	%delay = 0;
	%target = "Self";
	%outputEvent = "displayActiveQuest";
	if (%param1 $= "")
	{
		%param1 = $Farming::QuestDepositPointPrefix @ getRandomHash("depositPoint");
	}

	if (!isObject(%this.getGroup().client))
	{
		%this.getGroup().client = new ScriptObject(DummyClient)
		{
			isAdmin = 1;
			isSuperAdmin = 1;
			wrenchBrick = %this;
			bl_id = %this.getGroup().bl_id;
			brickGroup = %this.getGroup();
		};
		%this.getGroup().client.client = %this.getGroup().client;
		%dummy = 1;
	}
	%prev = %this.getGroup().client.isAdmin;
	%this.getGroup().client.isAdmin = 1;
	%this.addEvent(%enabled, %delay, %inputEvent, %target, %outputEvent, %param1, %param2, %param3, %param4);
	%this.getGroup().client.isAdmin = %prev;
	if (%dummy)
	{
		%this.getGroup().client.delete();
	}
}

package QuestBricks {
	function fxDTSBrick::onAdd(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isQuestSubmissionPoint)
		{
			schedule(100, %this, addQuestDepositEvent, %this);
		}

		return parent::onAdd(%this);
	}
}