$starBitmap = "<bitmap:base/client/ui/ci/star>";


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
	isDialogueObject = 1;

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

	maxRange = 10;
};

$exampleDialogueObj2 = new ScriptObject(CorrectResponse)
{
	isDialogueObject = 1;

	messageCount = 1;
	message[0] = "Correct!";
	messageTimeout[0] = 1;
	
	dialogueTransitionOnTimeout = "";
};

$exampleDialogueObj3 = new ScriptObject(IncorrectResponse)
{
	isDialogueObject = 1;

	messageCount = 1;
	message[0] = "You didn't say yes...";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "";
};

$exampleDialogueObj4 = new ScriptObject(ExitResponse)
{
	isDialogueObject = 1;

	messageCount = 1;
	message[0] = "Bye!";
	messageTimeout[0] = 1;
};

$exampleDialogueObj5 = new ScriptObject(ErrorResponse)
{
	isDialogueObject = 1;

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

	function Armor::onRemove(%this, %obj)
	{
		if (isObject(%obj.dialogueData))
		{
			%obj.dialogueData.delete();
		}
		return parent::onRemove(%this, %obj);
	}
};
schedule(1000, 0, activatePackage, Support_DialogueSystem);

function Player::startDialogue(%pl, %speaker, %dialogueObject)
{
	if (!isObject(%pl.client) || !isObject(%speaker) || !isObject(%dialogueObject))
	{
		return;
	}


	%maxRange = %dialogueObject.maxRange <= 0 ? 3 : %dialogueObject.maxRange;
	if (vectorDist(%speaker.position, %pl.position) >= %maxRange)
	{
		return;
	}

	if (isObject(%pl.dialogueData))
	{
		%pl.dialogueData.delete();
	}

	%pl.dialogueData = new ScriptObject()
	{
		class = "DialogueData";
		speaker = %speaker;
		player = %pl;
	};

	%pl.dialogueData.enterDialogue(%dialogueObject);
	%pl.dialogueLoop();
}

function Player::dialogueLoop(%pl)
{
	cancel(%pl.dialogueLoopSched);
	%dData = %pl.dialogueData;
	if (!isObject(%dData.speaker) || !isObject(%dData.dialogueObject)
		|| vectorDist(%pl.position, %dData.speaker.position) > %dData.maxRange)
	{
		%pl.quitDialogue();
		return;
	}
	%pl.dialogueLoopSched = %pl.schedule(300, dialogueLoop);
}

function Player::quitDialogue(%pl)
{
	%dataObj = %pl.dialogueData;
	if (isObject(%dialogueData))
	{
		if (isObject(%dataObj.getResponseObject("Quit")))
		{
			%dataObj.enterDialogue(%dataObj.getResponseObject("Quit"));
		}
		%dataObj.delete();
	}
}

function handleResponse(%cl, %dataObj, %msg)
{
	if (!isObject(%dataObj) || %msg $= "")
	{
		return;
	}

	%func = %cl.player.dialogueData.responseParser;
	if (!isFunction(%func))
	{
		%func = "defaultResponseParser";
	}
	%dialogueObject = call(%func, %cl.player.dialogueData, %msg);

	if (isObject(%dialogueObject))
	{
		%dataObj.enterDialogue(%dialogueObject);
	}
	else //exit cause they didnt do anything to check
	{
		%dataObj.delete();
	}
}

function defaultResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;
	if (!isObject(%pl) || !isObject(%dataObj.dialogueObject) || !isObject(%dataObj.speaker))
	{
		%pl.quitDialogue(%pl);
		return 0;
	}

	%msg = stripChars(%msg, "!@#$%^&*().,/?<>{}[]\\|-=+~`\";:'");
	%msg = strReplace(%msg, " ", "_");

	%next = %dataObj.dialogueObject.response[%msg];
	if (!isObject(%next))
	{
		%next = %dataObj.dialogueObject.response["Error"];
	}

	return %next;
}





/////////
//Class//
/////////


function DialogueData::enterDialogue(%dataObj, %dialogueObject)
{
	if (!isObject(%dataObj.player) || !isObject(%dialogueObject) || !isObject(%dataObj.speaker) 
		|| !%dialogueObject.isDialogueObject)
	{
		return;
	}
	%dataObj.dialogueObject = %dialogueObject;
	%dataObj.maxRange = %dialogueObject.maxRange <= 0 ? 3 : %dialogueObject.maxRange;
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
	else if (!%dialogueObject.waitForResponse) //no responses, no transition on timeout - final state. exit.
	{
		%dataObj.delete();
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
			%varName = getSubStr(%msg, %pos, %next - %pos);
			if (%varName $= stripChars(%varName, "!@#$%^&*().,/?<>{}[]\\|-=+~`\";:' "))
			{
				strReplace(%msg, "%" @ %varName @ "%", %dataObj.var_[%varName]);
			}
		}
		%dataObj.scheduleMessage(%time * 1000, %cl, %msg);
		%time += %dialogueObject.messageTimeout[%i];
	}

	%time = getMax(%time, 0.5);
	if (isObject(%speaker) && %speaker.getType() & $Typemasks::PlayerObjectType)
	{
		%speaker.playThread(3, talk);
		%speaker.schedule(%time * 1000, playThread, 3, root);
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