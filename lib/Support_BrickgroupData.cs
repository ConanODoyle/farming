//provided by Buddy, slightly edited by Conan

function saveBrickGroupTrust()
{
    %file = new FileObject();
    %file.openForWrite("config/server/SE_AX_BRICKGORUP_TRUST.txt");

    //vars to save: (TRUST[] is set automatically in serverCmdTrustListUpload_Done)
    //    potentialTrust[]
    //    potentialTrustEntry[]
    //    potentialTrustCount

    %mBG = mainBrickGroup;
    for(%i = 0; %i < %mBG.getCount(); %i++)
    {
        %BG = %mBG.getObject(%i);
        %trustCount = mFloor(%BG.potentialTrustCount);
        
        %file.writeLine("BrickGroup_" @ %BG.BL_ID);
        %file.writeLine(%BG.name);
        %file.writeLine(%trustCount);

        %saveLine = "";
        for(%k = 0; %k < %trustCount; %k++)
        {
            %pBLID = %BG.potentialTrustEntry[%k];
            %pLevel = %BG.potentialTrust[%pBLID];

            if(%k == 0)
                %saveLine = %pBLID SPC %pLevel;
            else
                %saveLine = %saveLine TAB %pBLID SPC %pLevel;
        }

        %file.writeLine(%saveLine);
    }

    %file.close();
    %file.delete();
}

function loadBrickGroupTrust()
{
    %file = new FileObject();
    %file.openForRead("config/server/SE_AX_BRICKGORUP_TRUST.txt");

    while(!%file.isEOF())
    {
        %brickGroup = %file.readLine();
        %name       = %file.readLine();
        %trustCount = %file.readLine();
        %trustData  = %file.readLine();
        
        if(!isObject(%brickGroup))
            continue;

        if(isObject(%brickGroup.client))
            continue;

        %brickGroup.name = %name;

        //%brickGroup.potentialTrustCount = mFloor(%trustCount);
        %fieldCount = getFieldCount(%trustData);
        for(%i = 0; %i < %fieldCount; %i++)
        {
            %field = getField(%trustData, %i);
            %bl_id = mFloor(firstWord(%field));
            %level = mFloor(restWords(%field));

            %brickGroup.addPotentialTrust(%bl_id, %level);
            //%brickGroup.potentialTrustEntry[%i] = %bl_id;
            //%brickGroup.potentialTrust[%bl_id] = %level;
        }
    }

    %file.close();
    %file.delete();
}