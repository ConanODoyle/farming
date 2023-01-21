$starBitmap = "<bitmap:base/client/ui/ci/star>";
$DefaultDialogueRange = 5;

if (isObject($exampleDialogueObj1))
{
	$exampleDialogueObj1.delete();
	$exampleDialogueObj2.delete();
	$exampleDialogueObj3.delete();
	$exampleDialogueObj4.delete();
	$exampleDialogueObj5.delete();
}
$exampleDialogueObj1 = new ScriptObject(ExampleDialogue1)
{
	response["Yes"] = "CorrectResponse";
	response["No"] = "IncorrectResponse";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 2;
	message[0] = "I have a request for you...";
	messageTimeout[0] = 1;
	message[1] = "Please say yes!";
	messageTimeout[1] = 1;
	
	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "defaultResponseParser";
	functionOnStart = "";

	maxRange = 3;
};

$exampleDialogueObj2 = new ScriptObject(CorrectResponse)
{
	messageCount = 1;
	message[0] = "Correct!";
	messageTimeout[0] = 1;
	
	dialogueTransitionOnTimeout = "";
};

$exampleDialogueObj3 = new ScriptObject(IncorrectResponse)
{
	messageCount = 1;
	message[0] = "You didn't say yes...";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "";
};

$exampleDialogueObj4 = new ScriptObject(ExitResponse)
{
	messageCount = 1;
	message[0] = "Bye!";
	messageTimeout[0] = 1;
};

$exampleDialogueObj5 = new ScriptObject(ErrorResponse)
{
	messageCount = 1;
	message[0] = "Um... ok...";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "ExitResponse";
};






////////
//Core//
////////


package Support_DialogueSystem
{
	function serverCmdMessageSent(%cl, %msg)
	{
		%dataObj = %cl.player.dialogueData;
		if (%dataObj.dialogueObject.waitForResponse)
		{
			//parse message for dialogue
			messageClient(%cl, '', $starBitmap @ " \c3" @ %cl.name @ "\c6: " @ %msg);
			schedule(1000, %dataObj, handleResponse, %cl, %dataObj, %msg);
		}
		else
		{
			return parent::serverCmdMessageSent(%cl, %msg);
		}
	}

	function serverCmdStartTalking(%cl)
	{
		%dataObj = %cl.player.dialogueData;
		if (%dataObj.dialogueObject.waitForResponse)
		{
			// Don't show that they're typing when their message is just gonna get intercepted by a bot
			return;
		}

		return parent::serverCmdStartTalking(%cl);
	}

	function Armor::onRemove(%this, %obj)
	{
		if (isObject(%obj.dialogueData))
		{
			%obj.dialogueData.delete();
		}
		return parent::onRemove(%this, %obj);
	}
};
schedule(2000, 0, activatePackage, Support_DialogueSystem);

function Player::startDialogue(%pl, %speaker, %dialogueObject)
{
	if (!isObject(%pl.client) || !isObject(%speaker) || !isObject(%dialogueObject))
	{
		return;
	}


	%maxRange = %dialogueObject.maxRange <= 0 ? $DefaultDialogueRange : %dialogueObject.maxRange;
	if (vectorDist(%speaker.position, %pl.position) >= %maxRange)
	{
		return;
	}

	if (isObject(%pl.dialogueData))
	{
		%pl.dialogueData.clearScheduledMessages();
		%pl.dialogueData.delete();
	}

	%pl.dialogueData = new ScriptObject()
	{
		class = "DialogueData";
		speaker = %speaker;
		player = %pl;
	};

	%pl.dialogueData.originalRange = vectorDist(%pl.position, %speaker.position);
	%pl.dialogueData.enterDialogue(%dialogueObject);
	%pl.dialogueLoop();
}

function Player::dialogueLoop(%pl)
{
	cancel(%pl.dialogueLoopSched);
	%dataObj = %pl.dialogueData;
	if (!isObject(%dataObj.speaker) || !isObject(%dataObj.dialogueObject)
		|| vectorDist(%pl.position, %dataObj.speaker.position) > %dataObj.maxRange
		|| vectorDist(%pl.position, %dataObj.speaker.position) > %dataObj.originalRange + 1.5)
	{
		%pl.quitDialogue();
		return;
	}
	%pl.dialogueLoopSched = %pl.schedule(300, dialogueLoop);
}

function Player::quitDialogue(%pl)
{
	%dataObj = %pl.dialogueData;
	if (isObject(%dataObj))
	{
		if (isObject(%dataObj.getResponseObject("Quit")))
		{
			%dataObj.enterDialogue(%dataObj.getResponseObject("Quit"));
		}
		cancel(%dataObj.timeoutSched);

		%dataObj.delete();
	}
}

function handleResponse(%cl, %dataObj, %msg)
{
	if (!isObject(%dataObj) || %msg $= "")
	{
		return;
	}

	%func = %dataObj.dialogueObject.responseParser;
	if (!isFunction(%func))
	{
		%func = "defaultResponseParser";
	}
	%dialogueKey = call(%func, %cl.player.dialogueData, %msg);
	%dialogueObject = %dataObj.getResponseObject(%dialogueKey);
	if (!isObject(%dialogueObject))
	{
		%dialogueObject = %dataObj.getResponseObject("Error");
	}

	if (isObject(%dialogueObject))
	{
		%dataObj.enterDialogue(%dialogueObject);
	}
	else //exit cause there is no designated response or error response
	{
		%dataObj.delete();
	}
}

function defaultResponseParser(%dataObj, %msg)
{
	%msg = stripChars(%msg, "!@#$%^&*().,/?<>{}[]\\|-=+~`\";:'");
	%msg = strReplace(%msg, " ", "_");

	return %msg;
}

function yesNoResponseParser(%dataObj, %msg)
{
	%lwr = " " @ strLwr(%msg) @ " ";
	%lwr = stripChars(%lwr, "!@#$%^&*()[];,.<>/?[]{}\\|-_=+");
	%yes = "yes\tyeah\tye\tyea\ty\tok\talright\ti guess\tig";
	%no = "no\tn\tnope\tcancel\tquit\tfuck off";

	for (%i = 0; %i < getFieldCount(%yes); %i++)
	{
		%word = " " @ getField(%yes, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			return "Yes";
		}
	}

	for (%i = 0; %i < getFieldCount(%no); %i++)
	{
		%word = " " @ getField(%no, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			return "No";
		}
	}

	return "";
}

function AIPlayer::startDialogue(%bot, %dialogueObj, %cl)
{
	if (!isObject(%dialogueObj) || !isObject(%cl.player))
	{
		return;
	}

	%cl.player.startDialogue(%bot, %dialogueObj);
}
registerOutputEvent("Bot", "startDialogue", "string 200 200", 1);






/////////
//Class//
/////////


function DialogueData::enterDialogue(%dataObj, %dialogueObject)
{
	if (!isObject(%dataObj.player) || !isObject(%dialogueObject) || !isObject(%dataObj.speaker))
	{
		return;
	}
	%dataObj.dialogueObject = %dialogueObject;
	%dataObj.maxRange = %dialogueObject.maxRange <= 0 ? $DefaultDialogueRange : %dialogueObject.maxRange;
	if (isFunction(%dialogueObject.functionOnStart))
	{
		%exit = call(%dialogueObject.functionOnStart, %dataObj);
	}

	if (%dataObj.dialogueObject != %dialogueObject || %exit == 1)
	{
		//functionOnStart redirected this function call into a different dialogueobject, skip doing anything
		return;
	}
	%talkTime = %dataObj.sendDialogue();

	if (isObject(%dialogueObject.dialogueTransitionOnTimeout))
	{
		cancel(%dataObj.timeoutSched);
		%dataObj.timeoutSched = %dataObj.schedule(%talkTime * 1000, enterDialogue, %dialogueObject.dialogueTransitionOnTimeout);
	}
}

function DialogueData::getResponseObject(%dataObj, %responseKey)
{
	if (isObject(%dataObj.dialogueObject.response[%responseKey]))
	{
		return %dataObj.dialogueObject.response[%responseKey];
	}
	else
	{
		return 0;
	}
}

function DialogueData::sendDialogue(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;
	%speaker = %dataObj.speaker;
	%dialogueObject = %dataObj.dialogueObject;

	if (!isObject(%cl) || !isObject(%speaker) || !isObject(%dialogueObject))
	{
		return;
	}

	%dataObj.clearScheduledMessages();
	for (%i = 0; %i < %dialogueObject.messageCount; %i++)
	{
		%msg = %dialogueObject.message[%i];

		%searchStart = 0;
		while ((%pos = strPos(%msg, "%", %searchStart)) >= 0)
		{
			%searchStart = %pos + 1;
			%next = strPos(%msg, "%", %searchStart);
			if (%next < 0)
			{
				break;
			}
			
			%varName = getSubStr(%msg, %pos + 1, %next - %pos - 1);
			if (%varName $= stripChars(%varName, "!@#$%^&*().,/?<>{}[]\\|-=+~`\";:' ")
				&& %varName !$= "")
			{
				%msg = strReplace(%msg, "%" @ %varName @ "%", %dataObj.var_[%varName]);
			}
		}
		%dataObj.scheduleMessage(%time * 1000, %cl, %msg);
		%time += %dialogueObject.messageTimeout[%i];
	}

	%time = getMax(%time, 0.5);
	if (isObject(%speaker) && %speaker.getType() & $Typemasks::PlayerObjectType)
	{
		%speaker.playThread(0, talk);
		%speaker.schedule(%time * 1000, playThread, 0, root);
	}
	return %time;
}

function DialogueData::scheduleMessage(%dataObj, %time, %cl, %msg)
{
	%count = %dataObj.scheduledMessageCount + 0;

	%name = %dataObj.speaker.name;
	if (%name $= "")
	{
		if (isObject(%dataObj.speaker))
		{
			%name = %dataObj.speaker.getDatablock().uiname;
			if (%name $= "")
			{
				%name = %dataObj.speaker.getClassName();
			}
		}
		else
		{
			%name = "NONE";
		}
	}

	%prefix = $starBitmap @ " \c3" @ %name @ "\c6: ";

	%dataObj.messageSchedule[%count] = schedule(%time, %cl, messageClient, %cl, '', %prefix @ %msg);
	%dataObj.scheduledMessageCount++;
}

function DialogueData::clearScheduledMessages(%dataObj)
{
	for (%i = 0; %i < %dataObj.scheduledMessageCount; %i++)
	{
		cancel(%dataObj.messageSchedule[%i]);
		%dataObj.messageSchedule = "";
	}
}