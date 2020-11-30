function resetOres() {
	
	%group = BrickGroup_888888;

	for(%i = 0; %i < %group.getCount(); %i++) {
		
		%brick = %group.getObject(%i);
		%data = %brick.getDataBlock();
		
		if(%data == nameToID("brick2x2x1OctoConeData") || %data == nameToID("brick2x2x2OctoConeData") || %data == nameToID("brick2x2x1OctoBrickData")) {
			
			if(!%brick.isRaycasting() && !%brick.isColliding() && !%brick.isRendering()) {
			
				%brick.setRaycasting(true);
				%brick.setColliding(true);
				%brick.setRendering(true);
			}
		}	
	}
}