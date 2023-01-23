function RelativeVectorToRotation(%for, %up)
{
    %yaw = mAtan(getWord(%for, 0), -getWord(%for, 1)) + $pi;
    
    if(%yaw >= $pi)
        %yaw -= $m2pi;
    
    %rightAxis = vectorNormalize(vectorCross(%for, "0 0 1"));
    
    if(vectorLen(%rightAxis) == 0)
        %rightAxis = "-1 0 0";
    
    %upAxis = vectorNormalize(vectorCross(%rightAxis, %for));
    
    %rDot = vectorDot(%up, %rightAxis);
    %uDot = vectorDot(%up, %upAxis);
    
    %euler = mAsin(vectorDot(vectorNormalize(%for), "0 0 1")) SPC mAtan(%rDot, %uDot) SPC %yaw;
    %matrix = MatrixCreateFromEuler(%euler);
    return getWords(%matrix, 3, 6);
}