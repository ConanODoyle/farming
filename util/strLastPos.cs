
function strLastPos(%str, %search, %offset) {
	if(%offset > 0)
		%pos = %offset;
	else {
		%str = getSubStr(%str, 0, strLen(%str) + %offset);
		%pos = 0;
	}
	%lastPos = -1;
	while((%pos = strPos(%str, %search, %pos+1)) > 0) {
		%lastPos = %pos;
		if(%break++ >= 500) //Pretty sure strings have a max length shorter than this, should be fine
			return -2;
	}
	return %lastPos;
}