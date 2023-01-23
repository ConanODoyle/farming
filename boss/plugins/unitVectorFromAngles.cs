/// @param	alpha	number (in degrees)
/// @param	beta	number (in degrees)
/// @return	normalized 3-element vector
function unitVectorFromAngles(%alpha, %beta)
{
	%alpha = mDegToRad(%alpha);
	%beta = mDegToRad(%beta);
	
	%x = mSin(%alpha) * mCos(%beta);
	%y = mCos(%alpha) * mCos(%beta);
	%z = mSin(%beta);
	
	return %x SPC %y SPC %z;
}