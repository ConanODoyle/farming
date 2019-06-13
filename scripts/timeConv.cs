// time converter function
// converts time in seconds into days, hours, minutes, seconds
// returns a string
function convTime(%time) {
	%time = mCeil(%time);

	if (%time < 0)
		%minus = "negative ";

	if (%time < 60 && %time >= 0)
		if (%time == 1)
			return "1 second";
		else
			return %time @ " seconds";

	%sec = %time % 60;
	%min = ((%time - %sec) % 3600) / 60;
	%hr = ((%time - %sec - %min * 60) % 86400) / 3600;
	%day = mFloor((%time - %sec - %min * 60 - %hr * 3600) / 86400);

	if (%sec > 0 && ((%min > 0 && %hr > 0) || (%min > 0 && %day > 0) || (%hr > 0 && %day > 0)))
	{
		if (%min > 0) {
			%minComma = ",";
			if (%day > 0)
				%dayComma = ", ";
			if (%hr > 0)
				%hrComma = ", ";
		} else {
			%dayComma = ", ";
			%hrComma = ",";
		}
		%andSec = " and ";
	}
	else if (%min > 0 && %hr > 0 && %day > 0)
	{
		%hrComma = ",";
		%dayComma = ", ";
		%andMin = " and ";
	}
	else if (%sec > 0 && (%min > 0 || %hr > 0 || %day > 0))
		%andSec = " and ";
	else if (%min > 0 && (%hr > 0 || %day > 0))
		%andMin = " and ";
	else if (%hr > 0 && %day > 0)
		%andHr = " and ";

	%secStr = (%sec > 1 || %sec == 0) ? ((%sec > 0) ? %sec @ " seconds" : "") : "1 second";
	%minStr = (%min > 1 || %min == 0) ? ((%min > 0) ? %min @ " minutes" : "") : "1 minute";
	%hrStr = (%hr > 1 || %hr == 0) ? ((%hr > 0) ? %hr @ " hours" : "") : "1 hour";
	%dayStr = (%day > 1 || %day == 0) ? ((%day > 0) ? %day @ " days" : "") : "1 day";

	return %minus @ %dayStr @ %dayComma @ %andHr @ %hrStr @ %hrComma
		@ %andMin @ %minStr @ %minComma @ %andSec @ %secStr;
}
