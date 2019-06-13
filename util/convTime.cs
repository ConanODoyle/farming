// time converter function
// converts time in seconds into days, hours, minutes, seconds
// returns a string
function convTime(%time) {
	%time = mCeil(%time);

	%minus = (%time < 0) ? "negative " : "";

	%time = mAbs(%time);

	if (%time < 60 && %time >= 0)
		if (%time == 1)
			return "1 second";
		else
			return %minus @ %time @ " seconds";

	%sec = %time % 60;
	%min = ((%time - %sec) % 3600) / 60;
	%hr = ((%time - %sec - %min * 60) % 86400) / 3600;
	%day = ((%time - %sec - %min * 60 - %hr * 3600) % 604800) / 86400;
    %week = ((%time - %sec - %min * 60 - %hr * 3600 - %day * 86400) / 604800);

    if (%sec > 0 && ((%min > 0 && %hr > 0) || (%min > 0 && %day > 0) || (%hr > 0 && %day > 0)
                     || (%week > 0 && %min > 0) || (%week > 0 && %hr > 0) || (%week > 0 && %day > 0)))
    {
        if (%min > 0)
        {
            %minComma = ",";
            if (%day > 0)
                %dayComma = ", ";
            if (%hr > 0)
                %hrComma = ", ";
            if (%week > 0)
                %weekComma = ", ";
        }
        else if (%hr > 0)
        {
            %hrComma = ",";
            if (%day > 0)
                %dayComma = ", ";
            if (%week > 0)
                %weekComma = ", ";
        }
        else
        {
            %dayComma = ", ";
            %weekComma = ", ";
        }
        %andSec = " and ";
    }
    else if (%min > 0 && ((%hr > 0 && %day > 0) || (%hr > 0 && %week > 0) || (%day > 0 && %week > 0)))
	{
        if (%hr > 0) {
            %hrComma = ",";
            if (%day > 0)
                %dayComma = ", ";
            if (%week > 0)
                %weekComma = ", ";
        }
        else
        {
            %dayComma = ",";
            %weekComma = ", ";
        }
        %andMin = " and ";
	}
    else if (%hr > 0 && %day > 0 && %week > 0)
    {
        %dayComma = ",";
        %weekComma = ", ";
        %andHr = " and ";
    }
	else if (%sec > 0 && (%min > 0 || %hr > 0 || %day > 0 || %week > 0)) {
		%andSec = " and ";
    }
	else if (%min > 0 && (%hr > 0 || %day > 0 || %week > 0)) {
		%andMin = " and ";
    }
	else if (%hr > 0 && (%day > 0 || %week > 0)){
		%andHr = " and ";
    }
    else if (%day > 0 && %week > 0) {
        %andDay = " and ";
    }

	%secStr = (%sec > 1 || %sec == 0) ? ((%sec > 0) ? %sec @ " seconds" : "") : "1 second";
	%minStr = (%min > 1 || %min == 0) ? ((%min > 0) ? %min @ " minutes" : "") : "1 minute";
	%hrStr = (%hr > 1 || %hr == 0) ? ((%hr > 0) ? %hr @ " hours" : "") : "1 hour";
	%dayStr = (%day > 1 || %day == 0) ? ((%day > 0) ? %day @ " days" : "") : "1 day";
	%weekStr = (%week > 1 || %week == 0) ? ((%week > 0) ? %week @ " weeks" : "") : "1 week";

        return %minus @ %weekStr @ %weekComma @ %andDay @ %dayStr @ %dayComma @ %andHr @ %hrStr @ %hrComma
		@ %andMin @ %minStr @ %minComma @ %andSec @ %secStr;
}
