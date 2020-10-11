
$count = 0;
if (isObject($TravelDialogue1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($TravelDialogue[%i]))
		{
			$TravelDialogue[%i].delete();
		}
	}
}

$TravelDialogue[$count++] = new ScriptObject(BusInfoDialogue)
{
	messageCount = 3;
	message[0] = "Bus stops are really cool! They're free, and bring you around really fast.";
	messageTimeout[0] = 2;
	message[1] = "The bus line info shows all the stops available to travel to when you click it!";
	messageTimeout[1] = 3;
	message[2] = "That said, I haven't seen a bus in all the time I've been standing here...";
	messageTimeout[2] = 2;
};