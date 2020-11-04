// brapps

function centerString(%string, %width) {
	if (strLen(%string) % 2 != strLen(%width) % 2) {
		%string = %string @ " ";
	}

	if (strLen(%string) > %width) {
		%string = getSubStr(%string, 0, %width);
	}

	while (strLen(%string) < %width) {
		%string = " " @ %string @ " ";
	}

	return %string;
}

function alphaNumericToPrintID(%char) {

	switch$ (%char) {
		case "&": %char = "-and";
		case "'": %char = "-apostrophe";
		case "*": %char = "-asterisk";
		case "@": %char = "-at";
		case "!": %char = "-bang";
		case "^": %char = "-caret";
		case "$": %char = "-dollar";
		case "=": %char = "-equals";
		case ">": %char = "-greater_than";
		case "<": %char = "-less_than";
		case "-": %char = "-minus";
		case "%": %char = "-percent";
		case ".": %char = "-period";
		case "+": %char = "-plus";
		case "#": %char = "-pound";
		case "?": %char = "-qmark";
		case " " or "": %char = "-space";
	}

	%name = "Letters/" @ %char;
	return $printNameTable[%name];
}

function setBappsMatText(%string) {
	%string = centerString(%string, 6);
	for (%i = 0; %i < 6; %i++) {
		%brick = "_BappsWelcomeMat" @ %i;
		if (!isObject(%brick))
		{
			continue;
		}
		%printID = alphaNumericToPrintID(getSubStr(%string, %i, 1));
		%brick.setPrint(%printID);
	}
}

function addBappsMatString(%string) {
	$Farming::BappsMatStrings = trim($Farming::BappsMatStrings TAB getSubStr(%string, 0, 6));
}

function removeBappsMatString(%index) {
	if (%index $= "last") {
		%index = getFieldCount($Farming::BappsMatStrings) - 1;
	}

	if (%index < 0 || %index > getFieldCount($Farming::BappsMatStrings) || %index $= "") {
		return;
	}

	$Farming::BappsMatStrings = removeField($Farming::BappsMatStrings, %index);
}

function setRandomBappsMatString() {
	%index = getRandom(1, getFieldCount($Farming::BappsMatStrings)) - 1;
	%sentinel = 0;
	while ((%string = getField($Farming::BappsMatStrings, %index)) $= $Farming::LastBappsMatString) {
		if (%sentinel > 100) break;

		%index = getRandom(1, getFieldCount($Farming::BappsMatStrings)) - 1;
		%sentinel++;
	}

	setBappsMatText(%string);
}

function randomBappsMatLoop() {
	cancel($BappsMatSchedule);

	setRandomBappsMatString();

	$BappsMatSchedule = schedule($Farming::BappsMatCycleTime, 0, randomBappsMatLoop);
}