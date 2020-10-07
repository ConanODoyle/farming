


$exampleDialogueSystem = new ScriptObject(ExampleDialogue1)
{
	isDialogueObject = 1;

	response["Yes"] = CorrectResponse;
	response["No"] = IncorrectResponse;

	messageCount = 2;
	message[0] = "I have a request for you...";
	messageTimeout[0] = 0.5;
	message[1] = "Please say yes!";

	responseParser = "defaultResponseParser";
};





package Support_DialogueSystem
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (%cl.player.inDialogue)
		{
			//parse message for dialogue
		}
		else
		{
			return parent::serverCmdMessageSent(%cl, %msg);
		}
	}

	function Armor::onRemove(%this, %obj)
	{

	}
};
schedule(1000, 0, activatePackage, Support_DialogueSystem);

function startDialogue(%pl, %bot, %dialogueObject)
{
	if (!isObject(%pl.client) || !isObject(%dialogueObject))
	{
		return;
	}

	%pl.inDialogue = 1;
	%pl.dialogueObject = %dialogueObject;
	%pl.dialogueData = new ScriptObject();
	%pl.dialogueBot = %bot;

	sendDialogue(%pl, %bot, %dialogueObject);
}

function sendDialogue(%pl, %bot, %dialogueObj)
{

}