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

	maxDamage = $Harvester::Armor::MaxDamage;

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
	%player.setNodeColor("ALL", $Harvester::Armor::Avatar::BodyColor);

	%player.setNodeColor("pants", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("RShoe", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("LShoe", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("ShoulderPads", $Harvester::Armor::Avatar::ClothesColor);

	%player.setNodeColor("RHand", $Harvester::Armor::Avatar::SkinColor);
	%player.setNodeColor("LHand", $Harvester::Armor::Avatar::SkinColor);
	%player.setNodeColor("HeadSkin", $Harvester::Armor::Avatar::SkinColor);

	%player.setNodeColor("ArmorMount", $Harvester::Armor::Avatar::ArmorColor);
	%player.setNodeColor("ShoulderBars", $Harvester::Armor::Avatar::ArmorColor);
	%player.setNodeColor("TorsoBars", $Harvester::Armor::Avatar::ArmorColor);

	%player.setNodeColor("Helmet", $Harvester::Armor::Avatar::PlateColor);
	%player.setNodeColor("ShoulderArmor", $Harvester::Armor::Avatar::PlateColor);
	%player.setNodeColor("TorsoArmor", $Harvester::Armor::Avatar::PlateColor);

	%player.setNodeColor("cloak", $Harvester::Armor::Avatar::CapeColor);
}