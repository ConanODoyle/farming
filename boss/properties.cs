//--------//
// Armor: //
//--------//

$Harvester::Armor::BaseHealth = 6000.0;
$Harvester::Armor::ExtraHealthPerFighter = 7500.0;

$Harvester::Armor::Avatar::BodyColor = "0.3 0.3 0.3 1.0";
$Harvester::Armor::Avatar::ClothesColor = "0.15 0.15 0.15 1.0";	
$Harvester::Armor::Avatar::SkinColor = "0.05 0.05 0.05 1.0";
$Harvester::Armor::Avatar::ArmorColor = "0.5 0.5 0.5 1.0";
$Harvester::Armor::Avatar::PlateColor = "0.5 0.2 0.2 1.0";
$Harvester::Armor::Avatar::CapeColor = "0.6 0.3 0.2 1.0";

$Harvester::AncientWarrior::Armor::BaseHealth = 80.0;

$Harvester::AncientWarrior::Armor::Avatar::Color = "0.5 0.7 1.0 0.6";

//--------//
// Blade: //
//--------//

$Harvester::Blade::Damage = 35.0;

//-------------//
// Beam Rifle: //
//-------------//

$Harvester::BeamRifle::Range = 64.0;
$Harvester::BeamRifle::Damage = 45.0;
$Harvester::BeamRifle::DamageFalloff = 0.9;

//---------------//
// Cluster Bomb: //
//---------------//

$Harvester::Bomb::Radius = 5.0;
$Harvester::Bomb::RadiusDamage = 50.0;
$Harvester::Bomb::MinSplitTimeMS = 750;
$Harvester::Bomb::MaxSplitTimeMS = 1250;

//--------//
// Spike: //
//--------//

$Harvester::Spike::Radius = 3.0;
$Harvester::Spike::RadiusDamage = 30.0;
$Harvester::Spike::Iteration::StepTimeMS = 140;
$Harvester::Spike::Iteration::StepLength = 4.0;
$Harvester::Spike::Iteration::MaxSteps = 12;

//-------------//
// Master Key: //
//-------------//

$Harvester::MasterKey::ConcDamage = 45.0;
$Harvester::MasterKey::ProjectileDamage = 20.0;
$Harvester::MasterKey::Melee::Damage = 65.0;
$Harvester::MasterKey::Melee::CooldownMS = 1000;

//-----//
// AI: //
//-----//

$Harvester::AI::ArenaRadius = 64.0;