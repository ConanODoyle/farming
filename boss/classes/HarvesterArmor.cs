//----------------//
// Attack Sounds: //
//----------------//

datablock AudioProfile(HarvesterYellSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/yell1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterYellSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/yell2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterSmallYellSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/smallYell1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterSmallYellSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/smallYell2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterChargeSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/charge.wav";
	description = AudioDefault3d;
	preload = true;
};

//----------------//
// Damage Sounds: //
//----------------//

datablock AudioProfile(HarvesterWakeUpSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/wakeUp.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterDamageSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/damage.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterDeathSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/death1.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------------//
// Playertype: //
//-------------//

datablock PlayerData(HarvesterArmor : PlayerStandardArmor)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/harvester.dts";
	
	//-----------//
	// Gameplay: //
	//-----------//

	maxDamage = 45000;

	useCustomPainEffects = true;

	painSound = HarvesterWakeUpSound;
	deathSound = HarvesterDeathSound1;

	//----------//
	// Physics: //
	//----------//

	mass = 115.0;
	drag = 0.1;
	density = 0.85;

	//-----------//
	// Movement: //
	//-----------//

	canJet = false;

	//----------------//
	// Miscellaneous: //
	//----------------//

	uiName = "The Harvester";
};

/// @param	this	playertype
/// @param	player	player
function HarvesterArmor::applyAvatar(%this, %player)
{
	%bodyColor = "0.3 0.3 0.3 1";
	%clothesColor = "0.15 0.15 0.15 1";	
	%skinColor = "0.05 0.05 0.05 1";
	%armorColor = "0.5 0.5 0.5 1";
	%plateColor = "0.5 0.2 0.2 1";
	%capeColor = "0.6 0.3 0.2 1";

	%player.setNodeColor("ALL", %bodyColor);

	%player.setNodeColor("pants", %clothesColor);
	%player.setNodeColor("RShoe", %clothesColor);
	%player.setNodeColor("LShoe", %clothesColor);
	%player.setNodeColor("ShoulderPads", %clothesColor);

	%player.setNodeColor("RHand", %skinColor);
	%player.setNodeColor("LHand", %skinColor);
	%player.setNodeColor("HeadSkin", %skinColor);

	%player.setNodeColor("ArmorMount", %armorColor);
	%player.setNodeColor("ShoulderBars", %armorColor);
	%player.setNodeColor("TorsoBars", %armorColor);

	%player.setNodeColor("Helmet", %plateColor);
	%player.setNodeColor("ShoulderArmor", %plateColor);
	%player.setNodeColor("TorsoArmor", %plateColor);

	%player.setNodeColor("cloak", %capeColor);
}