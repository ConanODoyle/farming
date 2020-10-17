datablock ItemData(QuestItem)
{
    category = "Item";
    className = "Item";

    shapeFile = "./resources/questPaperItem.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    uiName = "Quest Slip";
    iconName = "Add-Ons/Server_Farming/crops/icons/Quest";
    doColorShift = false;
    colorShiftColor = "0.7 0.7 0.7 1";

    image = QuestImage;
    canDrop = true;

    hasDataID = true;
    canPickupMultiple = true;

    requiredStorageType = "Quests";
};

datablock ShapeBaseImageData(QuestImage)
{
    shapeFile = "./resources/questPaperImage.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0; //"0.7 1.2 -0.5";
    rotation = eulerToMatrix( "0 0 0" );

    item = QuestItem;

    armReady = true;

    doColorShift = QuestItem.doColorShift;
    colorShiftColor = QuestItem.colorShiftColor;

    toolTip = "Click to toggle requests/rewards";

    stateName[0] = "init";
    stateTimeoutValue[0] = 0.1;
    stateWaitForTimeout[0] = false;
    stateTransitionOnTimeout[0] = "displayQuest";
    stateTransitionOnTriggerDown[0] = "buffer";
    stateScript[0] = "setQuestDisplayRequest";

    stateName[1] = "displayQuest";
    stateTimeoutValue[1] = 0.1;
    stateWaitForTimeout[1] = false;
    stateTransitionOnTimeout[1] = "delay";
    stateTransitionOnTriggerDown[1] = "buffer";
    stateScript[1] = "displayQuest";

    stateName[2] = "delay";
    stateTimeoutValue[2] = 0.1;
    stateWaitForTimeout[2] = false;
    stateTransitionOnTimeout[2] = "displayQuest";
    stateTransitionOnTriggerDown[2] = "buffer";

    stateName[3] = "buffer";
    stateWaitForTimeout[3] = false;
    stateTransitionOnTriggerUp[3] = "toggle";

    stateName[4] = "toggle";
    stateTimeoutValue[4] = 0.05;
    stateWaitForTimeout[4] = false;
    stateTransitionOnTimeout[4] = "displayQuest";
    stateTransitionOnTriggerDown[4] = "buffer";
    stateScript[4] = "toggleQuestDisplay";
    stateSound[4] = PageTurnSound;
};

function QuestImage::displayQuest(%image, %player, %slot)
{
    %client = %player.client;
    %itemSlot = %player.currTool;

    if (%client.getClassName() !$= "GameConnection")
    {
        return;
    }

    %client.displayQuest(%player.toolDataID[%itemSlot], %player.questDisplayMode);
}

function QuestImage::setQuestDisplayRequest(%image, %player, %slot)
{
    %player.questDisplayMode = false;

    %image.displayQuest(%player, %slot);
}

function QuestImage::toggleQuestDisplay(%image, %player, %slot)
{
    %player.questDisplayMode = !%player.questDisplayMode;
    %player.playThread(2, "rotcw");

    %image.displayQuest(%player, %slot);
}

function QuestImage::onUnMount(%image, %player, %slot)
{
    %client = %player.client;
    if (%client.getClassName() !$= "GameConnection")
    {
        return;
    }
    %client.centerPrint(" ");
}
