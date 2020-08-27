
%error = forceRequiredAddon("Server_Automoderator");

if (%error == $Error::Addon_NotFound)
{
	error("ERROR: Server_Farming/lib/automodSettings.cs - required add-on Server_Automoderator not found!");
	error("Will not continue, please install Server_Automoderator");
	crash();
}


// The name of the AutoModerator.
$autoModName = "AutoMod";

// Amount of seconds to batch together messages from the same user.
// Lower time could cause long API queues during periods of intense activity.
// The actual amount of time will vary due to the way the queue is processed.
$autoModAnalysisDelaySeconds = 6; 

// While true, automod will only notify admins of the actions it /would/ take, but without doing them.
// This will also cause messages in the console expressing test results as they are recieved from the API, which can help with deciding how your sensitivity values should look.
$autoModExperimentalMode = false; 

// Minimum probability to be considered for demerits in any category. 0.6 is recommended.
$autoModMinimumProbability = 0.6; 


// Mute punishment values.
$autoModStartingMuteTime = 120; // Initial mute time in seconds. The first mute will always be this length. (Players get a warning before being muted for anything)
$autoModMuteTimeExponent = 0.2; // Initial mute time is increased exponentially on each additional punishment. muteTime = startingMuteTime ^ (1 + punishments * muteTimeExponent)


// Sensitivity values. Lower value = more leniency for that category. A value of zero will completely disable that category.
$autoModSensitivity["Toxicity"] = 1.5;
$autoModSensitivity["severeToxicity"] = 4;
$autoModSensitivity["IdentityAttack"] = 11;
$autoModSensitivity["Insult"] = 4;
$autoModSensitivity["Profanity"] = 0;
$autoModSensitivity["Threat"] = 2;
$autoModSensitivity["SexuallyExplicit"] = 0;
$autoModSensitivity["Flirtation"] = 0;

$autoModSensitivityLimit = 10; // Number of accumulated demerits required before a punishment will be assessed. I don't recommend messing with this since it'll throw off the balancing of everything else.


// Warning messages. Copy one of these and use one of the categories above to specify a message for a certain category.
$autoModWarningMessage["DEFAULT"] = "Be nice. Continued poor chat etiquette may result in a penalty being assessed.";
$autoModWarningMessage["profanity"] = "Excessive profanity is discouraged here. Please reduce the frequency of profane messages or else a penalty may be assessed.";
$autoModWarningMessage["SexuallyExplicit"] = "Excessive sexually explicit messages are not tolerated here. Continuing to send sexually explicit messages may result in a penalty being assessed.";
$autoModWarningMessage["Flirtation"] = "Excessively flirtatious messages can make recipients uncomfortable, and are often obnoxious to others. Please reduce the frequency of flirtatious messages or else a penalty may be assessed.";


// Middleware configuration. Only change these if you are running your own middleware server.
$autoModMiddlewareHostname = "pecon.us";
$autoModMiddlewarePath = "/perspective/";
$autoModMiddlewarePort = 80;



//for reference
package autoModeratorEnforcement
{	
	function serverCmdMessageSent(%client, %message)
	{
		if($autoModeratorMute[%client.BL_ID] > $sim::time)
		{
			%seconds = $autoModeratorMute[%client.BL_ID] - $sim::time;
			%client.chatMessage("\c3You are muted. Your mute will expire in" SPC formatSeconds(%seconds) @ ".");
			serverCmdStopTalking(%client);
			return;
		}

		parent::serverCmdMessageSent(%client, %message);
	}

	function serverCmdTeamMessageSent(%client, %message)
	{
		if($autoModeratorMute[%client.BL_ID] > $sim::time)
		{
			%seconds = $autoModeratorMute[%client.BL_ID] - $sim::time;
			%client.chatMessage("\c3You are muted. Your mute will expire in" SPC formatSeconds(%seconds) @ ".");
			serverCmdStopTalking(%client);
			return;
		}

		parent::serverCmdTeamMessageSent(%client, %message);
	}

	function serverCmdStartTalking(%client)
	{
		if($autoModeratorMute[%client.BL_ID] > $sim::time)
		{
			%client.isTalking = true;
			%client.displayMuteCountdown();
			return;
		}

		parent::serverCmdStartTalking(%client);
	}
};
activatePackage(autoModeratorEnforcement);

function unmute(%bl_id)
{
	deleteVariable("$autoModeratorMute" @ %bl_id);
}

function serverCmdUnmute(%cl, %a, %b, %c, %d)
{
	%name = trim(%a SPC %b SPC %c SPC %d);
	if (!isObject(%target = findClientByName(%name)))
	{
		if (%name $= %name + 0)
		{
			%target = %name + 0;
		}
		else
		{
			messageClient(%cl, '', "Cannot find player with name " @ %name);
			return;
		}
	}

	if (%target == %cl.bl_id || %target == %cl)
	{
		messageClient(%cl, '', "You cannot unmute yourself!");
	}

	if (%name $= %name + 0)
		unmute(%target);
	else
		unmute(%target.bl_id);

	messageClient(%cl, '', "\c6Unmuted " @ %target);
	if (isObject(%target))
	{
		messageClient(%target, '', "\c6" @ %cl.name @ " unmuted you.");
	}
	echo("[" @ getDateTime() @ "] " @ %cl.name @ " unmuted " @ %target @ " (" @ %target.name SPC %target.bl_id @ ")");
}

function resetPunishment(%bl_id)
{
	("playerDemerits_" @ %bl_id).delete();
}