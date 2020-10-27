$str = "";
$str = $str SPC "package ZoneBricks {";
$str = $str SPC "	function brickzone1x4x5::onLoadplant(%data,%this)";
$str = $str SPC "	{";
$str = $str SPC "		Parent::onLoadPlant(%this);";
$str = $str SPC "		brickzone1x4x5::LoadParameters(%this);";
$str = $str SPC "	}";
$str = $str SPC "};";

schedule(100, 0, eval, $str);