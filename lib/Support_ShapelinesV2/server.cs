//by Conan
//Shapelines V2 - straight upgrade to the older Support_Shapelines support script

$Pref::Shapelines::defaultColor = "1 0 0 0.5";

datablock StaticShapeData(Shapelines_Multishape)
{
	shapeFile = "./multishape_a.dts";
	//base scale of shape is .2 .2 .2
};

if (!isObject(ShapelineSimSet))
{
	$ShapelineSimSet = new SimSet(ShapelineSimSet);
}

function getShapelineShape(%type)
{
	%shape = new StaticShape(Shapeline)
	{
		dataBlock = Shapelines_Multishape;
	};

	%shape.setShapeType(%type);
	
	ShapelineSimSet.add(%shape);
	MissionCleanup.add(%shape);
	%shape.setNodeColor("ALL", $Pref::Shapelines::defaultColor);
	return %shape;
}

function drawLine(%pos1, %pos2, %color, %scale, %offset)
{
	%shape = getShapelineShape("cube");
	%shape.isLine = 1;
	%shape.drawLine(%pos1, %pos2, %color, %scale, %offset);
	return %shape;
}

function drawArrow(%pos, %vec, %color, %length, %scale, %offset)
{
	%shape = getShapelineShape("arrow");
	%shape.isLine = 1;
	%pos1 = %pos;
	%pos2 = vectorAdd(vectorScale(%vec, %length), %pos);
	%shape.drawLine(%pos1, %pos2, %color, %scale, %offset);
	return %shape;
}

function drawArrow2(%start, %end, %color, %scale, %offset)
{
	%shape = getShapelineShape("arrow");
	%shape.isLine = 1;
	%shape.drawLine(%start, %end, %color, %scale, %offset);
	return %shape;
}

function createBoxMarker(%pos, %color, %scale)
{
	if (%color $= "" || getWordCount(%color) < 4)
	{
		%color = $Pref::Shapelines::defaultColor;
	}

	%shape = getShapelineShape("cube");
	if (getWordCount(%scale) == 1)
	{
		%shape.setScale(%scale SPC %scale SPC %scale);
	}
	else
	{
		%shape.setScale(%scale);
	}
	%shape.setTransform(%pos);
	%shape.setNodeColor("ALL", %color);

	if (getWord(%color, 3) < 1) {
		%shape.startFade(0, 0, 1);
	} else {
		%shape.startFade(0, 0, 0);
	}
	return %shape;
}

function createSphereMarker(%pos, %color, %scale)
{
	%shape = createBoxMarker(%pos, %color, %scale);
	%shape.setShapeType("sphere");
	return %shape;
}

function createCylinderMarker(%pos, %color, %scale)
{
	%shape = createBoxMarker(%pos, %color, %scale);
	%shape.setShapeType("cylinder");
	return %shape;
}

function createRingMarker(%pos, %color, %scale)
{
	%shape = createBoxMarker(%pos, %color, %scale);
	%shape.setShapeType("ring");
	return %shape;
}

function createBoxOutline(%boundingbox, %color, %scale)
{
	//assume its two opposite corners
	%x1 = getWord(%boundingbox, 0);
	%y1 = getWord(%boundingbox, 1);
	%z1 = getWord(%boundingbox, 2);

	%x2 = getWord(%boundingbox, 3);
	%y2 = getWord(%boundingbox, 4);
	%z2 = getWord(%boundingbox, 5);

	%p1 = %x1 SPC %y1 SPC %z1;
		%p2 = %x1 SPC %y1 SPC %z2;
		%p3 = %x2 SPC %y1 SPC %z1;
		%p4 = %x1 SPC %y2 SPC %z1;
	%q1 = %x2 SPC %y2 SPC %z2;
		%q2 = %x2 SPC %y2 SPC %z1;
		%q3 = %x1 SPC %y2 SPC %z2;
		%q4 = %x2 SPC %y1 SPC %z2;
	
	drawLine(%p1, %p2, %color, %scale, %scale).isBox = 1;
	drawLine(%p1, %p3, %color, %scale, %scale).isBox = 1;
	drawLine(%p1, %p4, %color, %scale, %scale).isBox = 1;

	drawLine(%q1, %q2, %color, %scale, %scale).isBox = 1;
	drawLine(%q1, %q3, %color, %scale, %scale).isBox = 1;
	drawLine(%q1, %q4, %color, %scale, %scale).isBox = 1;

	drawLine(%p2, %q3, %color, %scale, -%scale).isBox = 1;
	drawLine(%p2, %q4, %color, %scale, -%scale).isBox = 1;

	drawLine(%p3, %q2, %color, %scale, -%scale).isBox = 1;
	drawLine(%p3, %q4, %color, %scale, -%scale).isBox = 1;

	drawLine(%p4, %q3, %color, %scale, -%scale).isBox = 1;
	drawLine(%p4, %q2, %color, %scale, -%scale).isBox = 1;
}







function StaticShape::setShapeType(%shape, %type)
{	
	if (%shape.dataBlock.getName() !$= "Shapelines_Multishape")
	{
		error("Shape is not a Shapelines_Multishape");
		return;
	}
	%shape.hideNode("ALL");

	switch$ (%type)
	{
		case "sphere": %shape.unhideNode("sphere");
		case "arrow": %shape.unhideNode("arrow");
		case "cube": %shape.unhideNode("cube");
		case "cylinder": %shape.unhideNode("cylinder");
		case "ring": %shape.unhideNode("ring");
		default: 
			%shape.unhideNode("cube"); 
			echo("Invalid shapeline type provided, defaulting to cube");
			echo("Valid types: sphere, arrow, cube, cylinder, ring");
	}
}


function StaticShape::drawLine(%this, %pos1, %pos2, %color, %scale, %offset)
{
	%len = vectorLen(vectorSub(%pos2, %pos1));
	if (%scale <= 0)
	{
		%scale = 1;
	}

	if (%color $= "" || getWordCount(%color) < 4)
	{
		%color = $Pref::Shapelines::defaultColor;
	}

	%vector = vectorNormalize(vectorSub(%pos2, %pos1));

	%xyz = vectorNormalize(vectorCross("1 0 0", %vector)); //rotation axis
	%u = mACos(vectorDot("1 0 0", %vector)) * -1; //rotation value

	%this.setTransform(vectorScale(vectorAdd(%pos1, %pos2), 0.5) SPC %xyz SPC %u);
	%this.setScale((%len + %offset) SPC %scale SPC %scale);
	%this.setNodeColor("ALL", %color);
	if (getWord(%color, 3) < 1)
	{
		%this.startFade(0, 0, 1);
	}
	else
	{
		%this.startFade(0, 0, 0);
	}

	return %this;
}