$slurs = "nigger";
$warningExpireTime = 2 * 60;

package chatFilter
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (%cl.warningExpireTime > $Sim::Time)
		{
			%cl.hasBeenWarned = 0;
		}

		if (%cl.hasBeenWarned)
		{
			%checkMsg = stripChars(%msg, " .!@#$%^&*(),.<>[]{}|\\/?_-+=");
		}
		else if (!%cl.hasBeenWarned)
		{
			%checkMsg = %msg;
		}

		for (%i = 0; %i < getWordCount($slurs); %i++)
		{
			%currWord = getWord($slurs, %i);
			if (strPos(strLwr(%checkMsg), %currWord) >= 0)
			{
				if (!%cl.hasBeenWarned)
				{
					messageClient(%cl, '', "No racist or bigoted slurs. You have been warned. Bypassing this filter is not tolerated.");
					%cl.hasBeenWarned = 1;
					%cl.warningExpires = $Sim::Time + $warningExpireTime;
				}
				else
				{
					%cl.setScore(%cl.score * 0.5);
					%cl.delete("Kicked for triggering the slur filter.<br><br>You have been fined half of your money.");
				}
				return;
			}
		}
		parent::serverCmdMessageSent(%cl, %msg);
	}
};
schedule(10000, 0, activatePackage, chatFilter);