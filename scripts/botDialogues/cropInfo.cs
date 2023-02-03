if (!isObject($CropInfoSet))
{
	$CropInfoSet = new SimSet(CropInfoSet);
}
$CropInfoSet.deleteAll();

$obj = new ScriptObject(CropInfoStart)
{
	response["Quit"] = "ExitResponse";

	messageCount = 2;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	message[1] = "I've got info on every crop!";
	messageTimeout[1] = 2;

	dialogueTransitionOnTimeout = "CropInfoCore";
};
$CropInfoSet.add($obj);

$obj = new ScriptObject(CropInfoCore)
{
	response["InvalidSelection"] = "CropInfoInvalidSelection";
	response["BasicInfo"] = "CropInfoSelection";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "Which crop would you like to know more about? Say which crop's name.";
	messageTimeout[0] = 2;

	waitForResponse = 1;
	responseParser = "cropInfoSelectionParser";
};
$CropInfoSet.add($obj);

$obj = new ScriptObject(CropInfoInvalidSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 1;
	message[0] = "I can't tell you anything about that, sorry...";
	messageTimeout[0] = 2;

	dialogueTransitionOnTimeout = "CropInfoCore";
};
$CropInfoSet.add($obj);

$obj = new ScriptObject(CropInfoSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 3;
	message[0] = "%data1%";
	messageTimeout[0] = 3;
	message[1] = "%data2%";
	messageTimeout[1] = 3;
	message[2] = "%data3%";
	messageTimeout[2] = 3;

	functionOnStart = "setupCropInfo";
	dialogueTransitionOnTimeout = "CropInfoCore";
};
$CropInfoSet.add($obj);







function cropInfoSelectionParser(%dataObj, %msg)
{
	%msg = trim(strLwr(%msg));
	%msg = strReplace(%msg, "cacti", "cactus");
	%msg = strReplace(%msg, "daisies", "daisy");
	%msg = strReplace(%msg, "lilies", "lily");
	%msg = strReplace(%msg, "blueberries", "blueberry");

	if (strLen(%msg) < 3)
	{
		return "Error";
	}

	%list = strLwr($ProduceList);
	%first = "";
	%firstSlot = 1000;
	for (%i = 0; %i < getFieldCount(%list); %i++)
	{
		%produce = getField(%list, %i);
		%pos = strPos(%msg, strLwr(%produce));
		if (%pos >= 0 && %pos < %firstSlot)
		{
			%first = %produce;
			%firstSlot = %pos;
		}
	}

	%dataObj.cropInfoSelection = %first;
	if (%first !$= "")
	{
		return "BasicInfo";
	}
	else
	{
		return "InvalidSelection";
	}
}

function setupCropInfo(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;
	%crop = %dataObj.cropInfoSelection;
	
	switch$ (%crop)
	{
		case "Potato": 
			%s1 = "Potatoes are very quick to grow, and drop between 2-4 potatoes and usually a seed.";
			%s2 = "Be careful not to let them grow too long or else they'll give less! Keep an eye on how many potatoes are showing.";
			%s3 = "[Required spacing: 2, required nutrients: none]";
		case "Carrot":
			%s1 = "Carrots are quick to grow, and give between 2-3 carrots and usually a seed. They sell for slightly more than potatoes!";
			%s2 = "Like potatoes, if they grow too long, they'll drop less, so keep an eye on them!";
			%s3 = "[Required spacing: 2, required nutrients: none]";
		case "Onion":
			%s1 = "Onions take a while to grow, but give a lot of experience and are carried in big stacks. Each plant drops 3-5 onions and often a seed.";
			%s2 = "You can harvest them early and still get full experience, but you'll get fewer onions and a lower chance for a seed.";
			%s3 = "[Required spacing: 2, required nutrients: none]";
		case "Turnip":
			%s1 = "Turnips are profitable cash crops, but take a decent bit to grow, and don't stack well. Use a trowel to boost their yield!";
			%s2 = "I hear sometimes they look like they're staring at you...";
			%s3 = "[Required spacing: 3, required nutrients: none]";
		case "Portobello":
			%s1 = "Portobellos are mushrooms sensitive to light. They can even grow with no light, but need nutrients to grow.";
			%s2 = "However, they still want some light - anything different than what they want will slow growth. Use an organic analyzer to check!";
			%s3 = "[Required spacing: 2, required nutrients: 4 nitrogen 4 phosphate]";
		case "Tomato":
			%s1 = "Tomatoes are cheap crops that can be harvested 12 times. They drop 2-5 tomatoes per harvest and take relatively little water.";
			%s2 = "They cost 10 experience to plant, and give 0-1 experience per harvest.";
			%s3 = "[Required spacing: 3 - 2x2 crop, required nutrients: none]";
		case "Corn":
			%s1 = "Corn can be harvested 3 times, and drop 3-4 per harvest, but need nutrients to finish growing. They can be harvested earlier for less yield.";
			%s2 = "They cost 9 experience to plant, and give 2-3 experience per harvest. Use an organic analyzer to figure out what nutrients they need!";
			%s3 = "[Required spacing: 2, required nutrients: 3 phosphate]";
		case "Wheat":
			%s1 = "Wheat is a more compact alternative to corn, dropping 3-5 per harvest. They take slightly longer to grow as well, but use less nutrients.";
			%s2 = "They cost 12 experience to plant, and give 3-4 experience per harvest. Use an organic analyzer to figure out what nutrients they need!";
			%s3 = "[Required spacing: 1, required nutrients: 2 phosphate]";
		case "Cabbage":
			%s1 = "Cabbage is a water-hungry crop, and drops 2-3 per harvest. However, they don't need nutrients to finish growing!";
			%s2 = "You can harvest them 4 times, giving 3-5 experience per harvest. They cost 20 experience to plant.";
			%s3 = "[Required spacing: 3, required nutrients: none]";
		case "Blueberry":
			%s1 = "Blueberries are profitable cash crops, able to be harvested 3 times for 2-6 berries per harvest.";
			%s2 = "However, they need nutrients to grow fully. They cost 20 experience to plant, and give 5-6 experience per harvest.";
			%s3 = "[Required spacing: 3, required nutrients: 3 nitrogen, 3 phosphate]";
		case "Chili":
			%s1 = "Chilis are a desert crop that use nearly no water and drop 2-5 per harvest. However, they need nutrients to fully grow.";
			%s2 = "They can be harvested 2 times, costing 12 experience and returning 5-6 experience per harvest.";
			%s3 = "[Required spacing: 2, required nutrients: 2 nitrogen, 2 phosphate]";
		case "Watermelon":
			%s1 = "Watermelon are an unusual desert crop, which needs lots of water in their second-to-last stage, and can revert a stage if left dry.";
			%s2 = "They drop 1-2 watermelons every harvest, and in total get 8 harvests. They cost 40 experience to plant, and give 3-5 experience per harvest.";
			%s3 = "[Required spacing: 3 - 2x2 crop, required nutrients: none]";
		case "Cactus":
			%s1 = "Cacti are small tree-like plants, able to grow with no water at all (but grow faster with). Each harvest gives 2-5 cactus fruit.";
			%s2 = "They can be harvested up to 15 times, giving 3-5 experience per harvest. They cost 60 experience to plant.";
			%s3 = "[Required spacing: 3, required nutrients: none]";
		case "Cacti":
			%s1 = "Cacti are small tree-like plants, able to grow with no water at all (but grow faster with). Each harvest gives 2-5 cactus fruit.";
			%s2 = "They can be harvested up to 15 times, giving 3-5 experience per harvest. They cost 60 experience to plant.";
			%s3 = "[Required spacing: 3, required nutrients: none]";
		case "Apple":
			%s1 = "Apples have the smallest plant radius and cast little shade on plants under it. Like all trees, it needs nutrients to grow.";
			%s2 = "They can be harvested up to 70 times for 8-14 apples, and can be pruned during its flower or pre-flower stage for around 4 more apples.";
			%s3 = "[Required spacing: 8 - 2x2 crop, required nutrients: 8 nitrogen, 20 phosphate to fully grow, and 4n + 5p per harvest]";
		case "Mango":
			%s1 = "Mangoes cover a wide area and cast significant shade on crops underneath, which can significantly increase the growth time of plants underneath.";
			%s2 = "They can be harvested up to 70 times, dropping 11-16 per harvest.";
			%s3 = "[Required spacing: 16 - 2x2 crop, required nutrients: 15 nitrogen, 15 phosphate to fully grow, and 10n + 10p per harvest]";
		case "Peach":
			%s1 = "Peach trees require less water than other trees, but need 3x more nutrients to reach its harvesting stage.";
			%s2 = "They can be harvested up to 100 times, dropping 6-12 per harvest.";
			%s3 = "[Required spacing: 8 - 2x2 crop, required nutrients: 10 nitrogen, 10 phosphate to fully grow, and 30n + 30p per harvest]";
		case "Date":
			%s1 = "Date trees require no water to grow, and in fact grow significantly slower if water is present. They have the same nutrient requirement as peaches.";
			%s2 = "Just like peaches, they can be harvested up to 100 times, dropping 6-12 per harvest.";
			%s3 = "[Required spacing: 8 - 2x2 crop, required nutrients: 10 nitrogen, 10 phosphate to fully grow, and 30n + 30p per harvest]";

		case "Lily":
			%s1 = "Lilies are flowers that add 2 nitrogen to soil every 30 seconds, in their final flowering stage.";
			%s2 = "They die after wilting, but if you cut them before they wilt, they will grow again. You can use a reclaimer to reclaim unwilted flowers!";
			%s3 = "[Required spacing: 1 - 2x2 crop, required nutrients: none]";
		case "Lilies":
			%s1 = "Lilies are flowers that add 2 nitrogen to soil every 30 seconds, in their final flowering stage.";
			%s2 = "They die after wilting, but if you cut them before they wilt, they will grow again. You can use a reclaimer to reclaim unwilted flowers!";
			%s3 = "[Required spacing: 1 - 2x2 crop, required nutrients: none]";
		case "Daisy":
			%s1 = "Daisies are flowers that add 2 phosphate to soil every 30 seconds, in their final flowering stage.";
			%s2 = "They die after wilting, but if you cut them before they wilt, they will grow again. You can use a reclaimer to reclaim unwilted flowers!";
			%s3 = "[Required spacing: 1 - 2x2 crop, required nutrients: none]";
		case "Daisies":
			%s1 = "Daisies are flowers that add 2 phosphate to soil every 30 seconds, in their final flowering stage.";
			%s2 = "They die after wilting, but if you cut them before they wilt, they will grow again. You can use a reclaimer to reclaim unwilted flowers!";
			%s3 = "[Required spacing: 1 - 2x2 crop, required nutrients: none]";
		case "Rose":
			%s1 = "Roses are flowers that add 1 nitrogen and 1 phosphate to soil every 30 seconds, in their final flowering stage.";
			%s2 = "They die after wilting, but if you cut them before they wilt, they will grow again. You can use a reclaimer to reclaim unwilted flowers!";
			%s3 = "[Required spacing: 1 - 2x2 crop, required nutrients: none]";
		default:
			%s1 = "...";
			%s2 = "...I don't know anything about that, sorry.";
			%s3 = "...";
	}

	%dataObj.var_data1 = %s1;
	%dataObj.var_data2 = %s2;
	%dataObj.var_data3 = %s3;
}