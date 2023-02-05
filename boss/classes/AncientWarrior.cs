//--------------//
// Lily Sounds: //
//--------------//

datablock AudioProfile(AncientWarriorLilyExplosionSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/lilyExplode.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------------//
// Playertype: //
//-------------//

datablock PlayerData(AncientWarriorArmor : PlayerStandardArmor)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/harvester.dts";
	
	//-----------//
	// Gameplay: //
	//-----------//

	maxDamage = $Harvester::AncientWarrior::Armor::BaseHealth;

	useCustomPainEffects = true;
	
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

	uiName = "Ancient Warrior";
};

/// @param	this	playertype
/// @param	player	player
function AncientWarriorArmor::applyAvatar(%this, %player)
{
	%player.setNodeColor("ALL", $Harvester::AncientWarrior::Armor::Avatar::Color);
	%player.startFade(0, 0, true);
	
	%player.hideNode("Helmet");
	%player.hideNode("ArmorMount");
	%player.hideNode("ShoulderBars");
	%player.hideNode("ShoulderArmor");
	%player.hideNode("TorsoBars");
	%player.hideNode("TorsoArmor");
}