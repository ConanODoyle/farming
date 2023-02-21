//function overrides to Support_Automoderator to work with a custom lightweight middleman python layer
if (!isObject(AutomodBridgeOptions))
{
	new ScriptObject(AutomodBridgeOptions)
	{
		class = TCPClientOptions;
		connectionTimeout = 5000;
		connectionRetryWait = 2000;
		connectionRetryCount = 1;
		redirectWait = 500;
		debug = false;
		printErrors = true;
	};
}

//overrides original text analysis to pass to Python middleware hosted on same server
function startTextAnalysis(%cache)
{
	%text = %cache.payload;
	%blid = %cache.blid;

	%query = "text=" @ urlEnc(strLwr(%text));
	%connection = TCPClient("POST", "127.0.0.1", "28012", "", %query, "", "AutomodBridge", "AutomodBridgeOptions");
	%connection.cache = %cache;
	%connection.blid = %blid;
}

function AutomodBridge::buildRequest(%this)
{
	%len = strLen(%this.query);
	%path = %this.path;

	if(%len)
	{
		%type		= "Content-Type: application/x-www-form-urlencoded\r\n";
		if(%this.method $= "GET" || %this.method $= "HEAD")
		{
			%path	= %path @ "?" @ %this.query;
		}
		else
		{
			%length	= "Content-Length:" SPC %len @ "\r\n";
			%body	= %this.query;
		}
	}
	%requestLine	= %this.method SPC %path SPC %this.protocol @ "\r\n";
	%host			= "Host:" SPC %this.server @ "\r\n";
	%ua				= "User-Agent: Torque/1.3\r\n";
	%request = %requestLine @ %host @ %ua @ %length @ %type @ "\r\n" @ %body;
	// announce("Request: " @ %body);
	return %request;
}

function AutomodBridge::onLine(%this, %line)
{
	if (stripos(%line, "attribute") == 0)
	{
		processAnalysisResults(%line, %this.cache.blid);
	}
}

function processAnalysisResults(%result, %blid)
{
	%scorecard = 0;
	if(!isObject(%scorecard = nameToID("playerDemerits_" @ %blid)))
	{
		%scorecard = new ScriptObject("playerDemerits_" @ %blid)
		{
			blid = %blid;
			demerits["toxicity"] = 0;
			demerits["severe_Toxicity"] = 0;
			demerits["profanity"] = 0;
			demerits["sexually_Explicit"] = 0;
			demerits["identity_Attack"] = 0;
			demerits["flirtation"] = 0;
			demerits["threat"] = 0;
			demerits["insult"] = 0;
			demerits["incoherent"] = 0;
			demerits["inflammatory"] = 0;

			punishments["toxicity"] = -1;
			punishments["severe_Toxicity"] = -1;
			punishments["profanity"] = -1;
			punishments["sexually_Explicit"] = -1;
			punishments["identity_Attack"] = -1;
			punishments["flirtation"] = -1;
			punishments["threat"] = -1;
			punishments["insult"] = -1;
			punishments["incoherent"] = -1;
			punishments["inflammatory"] = -1;
		};

		autoModScorecardGroup.add(%scorecard);
	}

	%attribute = strLwr(getField(%result, 1));
	switch$ (%attribute)
	{
		case "toxicity": 			%toxicity = getField(%result, 2); if($AutomodDebug) announce("toxicity: " @ getField(%result, 2));
		case "severe_toxicity": 	%severe_Toxicity = getField(%result, 2); if($AutomodDebug) announce("severe_toxicity: " @ getField(%result, 2));
		case "profanity": 			%profanity = getField(%result, 2); if($AutomodDebug) announce("profanity: " @ getField(%result, 2));
		case "sexually_explicit": 	%sexually_Explicit = getField(%result, 2); if($AutomodDebug) announce("sexually_explicit: " @ getField(%result, 2));
		case "identity_attack": 	%identity_Attack = getField(%result, 2); if($AutomodDebug) announce("identity_attack: " @ getField(%result, 2));
		case "flirtation": 			%flirtation = getField(%result, 2); if($AutomodDebug) announce("flirtation: " @ getField(%result, 2));
		case "threat": 				%threat = getField(%result, 2); if($AutomodDebug) announce("threat: " @ getField(%result, 2));
		case "insult": 				%insult = getField(%result, 2); if($AutomodDebug) announce("insult: " @ getField(%result, 2));
	}

	if(%toxicity > $autoModMinimumProbability)
		%scorecard.demerits["toxicity"] += $autoModSensitivity["Toxicity"] * getDemeritMultiplier(%toxicity);
	else if(%scorecard.demerits["toxicity"] > 0)
		%scorecard.demerits["toxicity"] = getMax(%scorecard.demerits["toxicity"] - $automodDemeritReductionRate, 0);

	if(%severe_Toxicity > $autoModMinimumProbability)
		%scorecard.demerits["severe_Toxicity"] += $autoModSensitivity["severeToxicity"] * getDemeritMultiplier(%severe_Toxicity);
	else if(%scorecard.demerits["severe_Toxicity"] > 0)
		%scorecard.demerits["severe_Toxicity"] = getMax(%scorecard.demerits["severe_Toxicity"] - $automodDemeritReductionRate, 0);

	if(%profanity > $autoModMinimumProbability)
		%scorecard.demerits["profanity"] += $autoModSensitivity["Profanity"] * getDemeritMultiplier(%profanity);
	else if(%scorecard.demerits["profanity"] > 0)
		%scorecard.demerits["profanity"] = getMax(%scorecard.demerits["profanity"] - $automodDemeritReductionRate, 0);

	if(%sexually_Explicit > $autoModMinimumProbability)
		%scorecard.demerits["sexually_Explicit"] += $autoModSensitivity["SexuallyExplicit"] * getDemeritMultiplier(%sexually_Explicit);
	else if(%scorecard.demerits["sexually_Explicit"] > 0)
		%scorecard.demerits["sexually_Explicit"] = getMax(%scorecard.demerits["sexually_Explicit"] - $automodDemeritReductionRate, 0);

	if(%identity_Attack > $autoModMinimumProbability)
		%scorecard.demerits["identity_Attack"] += $autoModSensitivity["IdentityAttack"] * getDemeritMultiplier(%identity_Attack);
	else if(%scorecard.demerits["identity_Attack"] > 0)
		%scorecard.demerits["identity_Attack"] = getMax(%scorecard.demerits["identity_Attack"] - $automodDemeritReductionRate, 0);

	if(%flirtation > $autoModMinimumProbability)
		%scorecard.demerits["flirtation"] += $autoModSensitivity["Flirtation"] * getDemeritMultiplier(%flirtation);
	else if(%scorecard.demerits["flirtation"] > 0)
		%scorecard.demerits["flirtation"] = getMax(%scorecard.demerits["flirtation"] - $automodDemeritReductionRate, 0);

	if(%threat > $autoModMinimumProbability)
		%scorecard.demerits["threat"] += $autoModSensitivity["Threat"] * getDemeritMultiplier(%threat);
	else if(%scorecard.demerits["threat"] > 0)
		%scorecard.demerits["threat"] = getMax(%scorecard.demerits["threat"] - $automodDemeritReductionRate, 0);

	if(%insult > $autoModMinimumProbability)
		%scorecard.demerits["insult"] += $autoModSensitivity["Insult"] * getDemeritMultiplier(%insult);
	else if(%scorecard.demerits["insult"] > 0)
		%scorecard.demerits["insult"] = getMax(%scorecard.demerits["insult"] - $automodDemeritReductionRate, 0);


	if(%scorecard.demerits["toxicity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "toxicity");

	if(%scorecard.demerits["severe_Toxicity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "severe_Toxicity");

	if(%scorecard.demerits["profanity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "profanity");

	if(%scorecard.demerits["sexually_Explicit"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "sexually_Explicit");

	if(%scorecard.demerits["identity_Attack"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "identity_Attack");

	if(%scorecard.demerits["flirtation"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "flirtation");

	if(%scorecard.demerits["threat"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "threat");

	if(%scorecard.demerits["insult"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "insult");
}

function getDemeritMultiplier(%percent)
{
	return getMax((%percent - $autoModMinimumProbability) / (1 - $autoModMinimumProbability), 0.1);
}