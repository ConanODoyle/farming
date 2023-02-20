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

	%query = "text=" @ urlEnc(%text);
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
	// announce("AMB: " @ %line);
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
			punishments["severeToxicity"] = -1;
			punishments["profanity"] = -1;
			punishments["sexuallyExplicit"] = -1;
			punishments["identityAttack"] = -1;
			punishments["flirtation"] = -1;
			punishments["threat"] = -1;
			punishments["insult"] = -1;
			punishments["incoherent"] = -1;
			punishments["inflammatory"] = -1;
		};

		autoModScorecardGroup.add(%scorecard);
	}

	%attribute = getField(%result, 1);
	switch$ (%attribute)
	{
		case "toxicity": 			%toxicity = getField(%result, 2);
		case "severeToxicity": 		%severeToxicity = getField(%result, 2);
		case "profanity": 			%profanity = getField(%result, 2);
		case "sexuallyExplicit": 	%sexuallyExplicit = getField(%result, 2);
		case "identityAttack": 		%identityAttack = getField(%result, 2);
		case "flirtation": 			%flirtation = getField(%result, 2);
		case "threat": 				%threat = getField(%result, 2);
		case "insult": 				%insult = getField(%result, 2);
	}

	if(%toxicity > $autoModMinimumProbability)
		%scorecard.demerits["toxicity"] += $autoModSensitivity["Toxicity"] * ((%toxicity - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["toxicity"] > 0)
		%scorecard.demerits["toxicity"]--;

	if(%severeToxicity > $autoModMinimumProbability)
		%scorecard.demerits["severeToxicity"] += $autoModSensitivity["severeToxicity"] * ((%severeToxicity - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["severeToxicity"] > 0)
		%scorecard.demerits["severeToxicity"]--;

	if(%profanity > $autoModMinimumProbability)
		%scorecard.demerits["profanity"] += $autoModSensitivity["Profanity"] * ((%profanity - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["profanity"] > 0)
		%scorecard.demerits["profanity"]--;

	if(%sexuallyExplicit > $autoModMinimumProbability)
		%scorecard.demerits["sexuallyExplicit"] += $autoModSensitivity["SexuallyExplicit"] * ((%sexuallyExplicit - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["sexuallyExplicit"] > 0)
		%scorecard.demerits["sexuallyExplicit"]--;

	if(%identityAttack > $autoModMinimumProbability)
		%scorecard.demerits["identityAttack"] += $autoModSensitivity["IdentityAttack"] * ((%identityAttack - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["identityAttack"] > 0)
		%scorecard.demerits["identityAttack"]--;

	if(%flirtation > $autoModMinimumProbability)
		%scorecard.demerits["flirtation"] += $autoModSensitivity["Flirtation"] * ((%flirtation - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["flirtation"] > 0)
		%scorecard.demerits["flirtation"]--;

	if(%threat > $autoModMinimumProbability)
		%scorecard.demerits["threat"] += $autoModSensitivity["Threat"] * ((%threat - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["threat"] > 0)
		%scorecard.demerits["threat"]--;

	if(%insult > $autoModMinimumProbability)
		%scorecard.demerits["insult"] += $autoModSensitivity["Insult"] * ((%insult - $autoModMinimumProbability) / (1 - $autoModMinimumProbability));
	else if(%scorecard.demerits["insult"] > 0)
		%scorecard.demerits["insult"]--;


	if(%scorecard.demerits["toxicity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "toxicity");

	if(%scorecard.demerits["severeToxicity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "severeToxicity");

	if(%scorecard.demerits["profanity"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "profanity");

	if(%scorecard.demerits["sexuallyExplicit"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "sexuallyExplicit");

	if(%scorecard.demerits["identityAttack"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "identityAttack");

	if(%scorecard.demerits["flirtation"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "flirtation");

	if(%scorecard.demerits["threat"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "threat");

	if(%scorecard.demerits["insult"] >= $autoModSensitivityLimit)
		issuePunishment(%scorecard, "insult");
}
