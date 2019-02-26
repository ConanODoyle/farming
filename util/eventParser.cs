function applyEventsFromText(%brick, %str) {
  %brick.numEvents = 0;
  // shamelessly stolen from duplicator
  for(%i = 0; %i < getLineCount(%str); %i++) {
    %line = getLine(%str, %i);
    if(getField(%line, 0) $= "+-EVENT") {
      %num = getField(%line,1);
      %brick.eventEnabled[%num] = getField(%line, 2);
      %brick.eventInput[%num] = getField(%line, 3);
      %brick.eventInputIdx[%num] = inputEvent_GetInputEventIdx(%brick.eventInput[%num]);
      %brick.eventDelay[%num] = getField(%line, 4);
      %brick.eventTarget[%num] = getField(%line, 5);
      %brick.eventTargetIdx[%num] = inputEvent_GetTargetIndex("fxDtsBrick", %brick.eventInputIdx[%num], %brick.eventTarget[%num]);
      %brick.eventNT[%num] = getField(%line, 6);
      %outputClass = %brick.eventTargetIdx[%num] == -1 ? "fxDtsBrick" : inputEvent_GetTargetClass("fxDtsBrick", %brick.eventInputIdx[%num], %brick.eventTargetIdx[%num]);
      %brick.eventOutput[%num] = getField(%line, 7);
      %brick.eventOutputIdx[%num] = outputEvent_GetOutputEventIdx(%outputClass,%brick.eventOutput[%num]);
      %brick.eventOutputAppendClient[%num] = $OutputEvent_AppendClient[%outputClass,%brick.eventOutputIdx[%num]];

      for(%e = 1; %e < 5; %e++) {
        %eventParamType = getField($OutputEvent_parameterList[%outputClass, %brick.eventOutputIdx[%num]], %e - 1);
        if(getWord(%eventParamType, 0) $= "dataBlock" && isObject(getField(%line, %e + 7)))
          %brick.eventOutputParameter[%num,%e] = getField(%line, %e + 7).getId();
        else
          %brick.eventOutputParameter[%num, %e] = getField(%line, %e + 7);
      }
      %brick.numEvents++;
      %brick.implicitCancelEvents = 0;
      if(strpos(%brick.eventOutput[%num],"fireRelay") != -1)
      {
        %output = %brick.eventOutput[%num];
        %dir = getSubStr(%output,strLen("fireRelay"),strLen(%output));
        switch$(%dir)
        {
          case "North":
            %dir = 0;
          case "West":
            %dir = 1;
          case "South":
            %dir = 2;
          case "East":
            %dir = 3;
          default:
            continue;
        }
        %dir -= %angleOffset;
        if(%dir < 0)
          %dir += 4;
        switch(%dir)
        {
          case 0:
            %output = "fireRelayNorth";
          case 1:
            %output = "fireRelayWest";
          case 2:
            %output = "fireRelaySouth";
          case 3:
            %output = "fireRelayEast";
        }
        %brick.eventOutput[%num] = %output;
        %brick.eventOutputIDX[%num] = outputEvent_GetOutputEventIdx("fxDTSBrick",%output);
      }
    }
  }
}

function readEventsFromFile(%name) {
  %file = "add-ons/server_farming/events/" @ %name @ ".txt";
  if(!isFile(%file)) {
    return "";
  }
  %f = new FileObject();
  %f.openForRead(%file);
  %str = "";   
  while(!%f.isEOF()) {
    %line = %f.readLine();
    %str = strlen(%str) > 0 ? %str NL %line : %line;
  }
  %f.close();
  return %str;
}

function applyEventsFromFile(%brick, %file) {
  %events = readEventsFromFile(%file);
  if(strlen(%events)) {
    applyEventsFromText(%brick, %events);
  }
  return getLineCount(%events);
}

/*
  Applies bot events to a bot brick
  Usage: /npcevents npc/potatofarmer
  See the files in the `events/` folder to see what's available/add more
 */
function servercmdnpcevents(%cl, %file) {
  if(!%cl.isAdmin) return;
  if(!%cl.player) return;

  %pl = %cl.player;
  %start = %pl.getEyePoint();
  %vec = vectorScale(%pl.getMuzzleVector(0), 10);
  %brick = containerraycast(%start, vectorAdd(%start, %vec), $TypeMasks::FxBrickAlwaysObjectType);

  if(!%brick) return;

  if(%brick.getDatablock() == BrickBlockheadBot_HoleSpawnData.getId()) {
    %num = applyEventsFromFile(%brick, %file);
    %cl.chatMessage("\c6Read \c3" @ %num @ " events \c6from file \c3" @ %file);
    %brick.setItem(0);
    %brick.setCollision(0);
    %brick.setRaycasting(0);
    %brick.setRendering(0);
    %brick.respawnBot();
  }
}
