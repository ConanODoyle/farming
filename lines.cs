
datablock StaticShapeData(C_SquareShape)
{
	shapeFile = "./box0.2.dts";
	//base scale of shape is .2 .2 .2
};

$defaultColor = "1 0 0 0.5";

function StaticShape::drawLine(%this, %pos1, %pos2, %color, %scale, %offset) {
	%len = vectorLen(vectorSub(%pos2, %pos1));
	if (%scale <= 0) {
		%scale = 1;
	}
	if (%color $= "" || getWordCount(%color) < 4) {
		%color = $defaultColor;
	}

	%vector = vectorNormalize(vectorSub(%pos2, %pos1));

	%xyz = vectorNormalize(vectorCross("1 0 0", %vector)); //rotation axis
	%u = mACos(vectorDot("1 0 0", %vector)) * -1; //rotation value

	%this.setTransform(vectorScale(vectorAdd(%pos1, %pos2), 0.5) SPC %xyz SPC %u);
	%this.setScale((%len/2 + %offset) SPC %scale SPC %scale);
	%this.setNodeColor("ALL", %color);
	if (getWord(%color, 3) < 1) {
		%this.startFade(0, 0, 1);
	} else {
		%this.startFade(0, 0, 0);
	}

	return %this;
}

function drawLine(%pos1, %pos2, %color, %scale, %offset) {
	%shape0 = new StaticShape(Lines) {
		datablock = C_SquareShape;
	};
	%shape0.drawLine(%pos1, %pos2, %color, %scale, %offset);
	return %shape0;
}

function clearLines() {
	while(isObject(Lines) && %count < 1000) {
		Lines.delete();
		%count++;
	}
}

function StaticShape::createBoxAt(%this, %pos, %color, %scale) {
	if (%scale <= 0) {
		%scale = 1;
	}
	if (%color $= "" || getWordCount(%color) < 4) {
		%color = $defaultColor;
	}

	%this.setTransform(%pos SPC "1 0 0 0");
	%this.setScale(%scale SPC %scale SPC %scale);
	%this.setNodeColor("ALL", %color);
	if (getWord(%color, 3) < 1) {
		%this.startFade(0, 0, 1);
	} else {
		%this.startFade(0, 0, 0);
	}
}

function createBoxAt(%pos, %color, %scale) {
	%shape0 = new StaticShape(Boxes) {
		datablock = C_SquareShape;
	};
	%shape0.createBoxAt(%pos, %color, %scale);
	return %shape0;
}

function clearBoxes() {
	while(isObject(Boxes) && %count < 1000) {
		Boxes.delete();
		%count++;
	}
}