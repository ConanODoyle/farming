ForceRequiredAddOn("Support_Player_Persistence");

if(!isFunction(RegisterPersistenceVar))
{
   error("ERROR: Script_Player_Persistence - Unable to load required add-on 'Support_Player_Persistence' - persistence will NOT work properly");
}

package PlayerPersistencePackage
{
   function serverCmdChangeMap(%client, %mapName)
   {
      saveAllClientPersistence();
      Parent::serverCmdChangeMap(%client, %mapName);
   }

   function GameConnection::savePersistence(%client)
   {
      if(!%client.hasSpawnedOnce)
         return;

      //client connected, but did not auth, seems like hasSpawnedOnce should catch this but it apparently doesn't
      if(%client.getBLID() < 0)
         return;

      //open file
      %file = new FileObject();
      %filename = "config/server/persistence/" @ %client.getBLID() @ ".txt";
      %file.openForWrite(%filename);

      if(!%file)
      {
         error("ERROR: GameConnection::savePersistence(" @ %client @ ") - failed to open file '" @ %filename @ "' for write");
         %file.delete();
         return;
      }

      echo("Saving persistence for BLID " @ %client.getBLID());


      //save all registered client tagged fields
      %file.writeLine(">CLIENT");
      Persistence::SaveTaggedFields(%client, %file);
      

      //save player object
      %player = %client.player;
      if(isObject(%player))
      {
         %file.writeLine(">PLAYER");
         %file.writeLine("datablock" TAB %player.getDataBlock().getName());
         %file.writeLine("transformPerMap" TAB $MapSaveName TAB %player.getTransform());
         %file.writeLine("velocity" TAB %player.getVelocity());
         %file.writeLine("damagePercent" TAB %player.getDamagePercent());
         %file.writeLine("scale" TAB %player.getScale());
         Persistence::SaveTaggedFields(%player, %file);
      }
      else
      {
         //dead?
         //save camera orbit point
         %camera = %client.camera;
         if(isObject(%camera))
         {
            %file.writeLine(">CAMERA"); 
            %file.writeLine("transform" TAB %camera.getTransform());            
            %file.writeLine("orbitPoint" TAB %camera.getOrbitPoint());
            %file.writeLine("orbitDistance" TAB %camera.getOrbitDistance());
            Persistence::SaveTaggedFields(%camera, %file);
         }
      }

      //close file
      %file.close();
      %file.delete();
   }

   function Persistence::SaveTaggedFields(%obj, %file)
   {
      %idx = 0;
      %taggedField = "";
      while(1)
      {
         %taggedField = %obj.getTaggedField(%idx);
         if(%taggedField $= "")
            break;
         
         %name  = getField (%taggedField, 0);
         %value = getFields(%taggedField, 1, 999);

         %saveLine = false;
         if($Persistence::MatchName[%name] $= "1")
         {
            %saveLine = true;               
         }
         else
         {
            for(%i = 0; %i < $Persistence::MatchAll_Count; %i++)
            {
               %len = strlen($Persistence::MatchAll_Entry[%i]);
               if(strnicmp($Persistence::MatchAll_Entry[%i], %name, %len) != 0)
                  continue;

               %saveLine = true;
               break;
            }
         }
         
         if(%saveLine)
         {
            //echo(%idx @ " saving " @ %name @ " => " @ %value);

            if($Persistence::MatchDatablock[%name] !$= "" && %value !$= "0")
            {
               //before saving a datablock, we make sure it is the expected datablock type
               //if you hit this error you are probably doing something wrong
               if(!isObject(%value))
               {
                  error("ERROR: Persistence::SaveTaggedFields(" @ %obj @ ", " @ %file @ ") - tagged field " @ %name @ " => " @ %value @ " is not an object");
               }
               else if(%value.getClassName() $= $Persistence::MatchDatablock[%name])
               {
                  %dbName = %value.getName();
                  %file.writeLine(%name TAB %dbName);
               }
               else
               {
                  error("ERROR: Persistence::SaveTaggedFields(" @ %obj @ ", " @ %file @ ") - tagged field " @ %name @ " => " @ %value @ " is not a " @ $Persistence::MatchDatablock[%name]);
               }
            }
            else
            {
               %file.writeLine(%taggedField);
            }
         }

         %idx++;
      }
   }     

   function GameConnection::loadPersistence(%client)
   {
      //open file
      %file = new FileObject();
      %filename = "config/server/persistence/" @ %client.getBLID() @ ".txt";
      %file.openForRead(%filename);

      if(!%file)
      {
         error("ERROR: GameConnection::loadPersistence(" @ %client @ " (BLID: " @ %client.getBLID() @ ")) - failed to open file '" @ %filename @ "' for write");
         %file.delete();
         return;
      }

      echo("Loading persistence for BLID " @ %client.getBLID());

      //read and assign data
      %currObj = 0;
      %gotPlayer = false;
      %gotCamera = false;
      while(!%file.isEOF())
      {
         %line = %file.readLine();
         
         if(%line $= ">CLIENT")
         {
            %currObj = %client;
            continue;
         }
         else if(%line $= ">PLAYER")
         {
            %gotPlayer = true;
            %currObj = %client.player;
            continue;
         }
         else if(%line $= ">CAMERA")
         {
            %gotCamera = true;
            %currObj = %client.camera;
            continue;
         }

         if(!isObject(%currObj))
            continue;

         %name  = getField (%line, 0);
         %value = getFields(%line, 1, 999);

         //special handling of some values
         if(%name $= "transform")
         {
            %currObj.setTransform(%value);
         }
         else if(%name $= "transformPerMap")
         {
            %map   = getField(%value, 0);
            %trans = getField(%value, 1);
            if($mapSaveName $= %map)
               %currObj.setTransform(%trans);
         }
         else if(%name $= "velocity")
         {
            %currObj.setVelocity(%value);
         }
         else if(%name $= "damagedPercent")
         {
            %currObj.setDamagedPercent(%value);
         }
         else if(%name $= "datablock")
         {
            if(!isObject(%value))
            {
               warn("WARNING: GameConnection::loadPersistence(" @ %client @ " (BLID: " @ %client.getBLID() @ ")) - datablock " @ %value @ " does not exist");
               continue;
            }

            if(%currObj.getDataBlock().getClassName() !$= %value.getClassName())
            {
               warn("WARNING: GameConnection::loadPersistence(" @ %client @ " (BLID: " @ %client.getBLID() @ ")) - datablock " @ %value @ " is not a " @ %currObj.getDataBlock().getClassName());
               continue;
            }

            %currObj.setDataBlock(%value);
         }
         else if(%name $= "scale")
         {
            %currObj.setScale(%value);
         }
         else
         {
            //convert back from datablock name?
            if($Persistence::MatchDataBlock[%name] !$= "")
            {
               if(%value !$= "0")
               {
                  if(!isObject(%value))
                  {
                     //attempted to load a datablock reference that doesn't exist
                     //save was probably made with an addon enabled that is currently disabled
                     warn("WARNING: GameConnection::loadPersistence(" @ %client @ " (BLID: " @ %client.getBLID() @ ")) - loading " @ %name @ " => " @ %value @ " as datablock, '" @ %value @ "' is not an object");
                     continue;
                  }

                  %value = %value.getId();
                  if(%value.getClassName() !$= $Persistence::MatchDataBlock[%name])
                  {
                     warn("WARNING: GameConnection::loadPersistence(" @ %client @ " (BLID: " @ %client.getBLID() @ ")) - loading " @ %name @ " => " @ %value @ " as datablock, '" @ %value @ "' is not a " @ $Persistence::MatchDataBlock[%name]);
                     continue;
                  }
               }
            }

            %cmd = "%currObj." @ %name @ " = \"" @ %value @ "\";";
            eval(%cmd);
         }  
      }

      //close file
      %file.close();
      %file.delete();

      %client.applyPersistence(%gotPlayer, %gotCamera);
      if(isObject(%client.player))
         commandToClient(%client, 'ShowEnergyBar', %client.player.getDataBlock().showEnergyBar);
      %client.schedulePersistenceSave(1);
   }

   function GameConnection::applyPersistence(%client, %gotPlayer, %gotCamera)
   {
      //this function is called after persistence has been loaded
      //if you want to restore client owned objects then this is the function you package into
      %camera = %client.camera;
      %player = %client.player;

      echo("Applying persistence" SPC %gotPlayer SPC %gotCamera);

      if(!%gotPlayer && %gotCamera)
      {
         %client.setControlObject(%camera);      
         %camera.setOrbitPointMode(%camera.orbitPoint, %camera.orbitDistance);

         if(isObject(%player))
            %player.delete();
      }
      else if(%gotPlayer)
      {
         //tell client about inventory
         %toolCount = %player.getDataBlock().maxTools;
         for(%i = 0; %i < %toolCount; %i++)
         {
            messageClient(%client, 'MsgItemPickup', "", %i, %player.tool[%i], 1); //the last 1 = silent
         }

         //if player is inside bricks, move up
         %mask = $TypeMasks::FxBrickOBjectType;
         %center = %player.getWorldBoxCenter();
         %wb = %player.getWorldBox();
         %xsize = (getWord(%wb, 3) - getWord(%wb, 0)) / 4 - 0.2;
         %ysize = (getWord(%wb, 4) - getWord(%wb, 1)) / 4 - 0.2;
         %zsize = (getWord(%wb, 5) - getWord(%wb, 2)) / 4 - 0.2;
         //echo(%xsize SPC %ysize SPC %zsize);
         %blockedOnce = false;
         for(%i = 0; %i < 1000; %i++)
         {
            if(containerBoxClear(%mask, %center, %xsize, %ysize, %zsize))
            {
               break;
            }

            //echo("blocked");
            %center = vectorAdd(%center, "0 0" SPC %zsize);
            %blockedOnce = true;
            //echo(%center);
         }
         if(%blockedOnce)
         {
            %rot = getWords(%player.getTransform(), 3, 6);
            %pos = vectorSub(%center, "0 0" SPC %zsize);
            %player.setTransform(%pos SPC %rot);
         }

         //make player use the last tool they had out
         if(%player.currTool >= 0 && %player.currTool < %toolCount)
         {
            commandToClient(%client, 'setActiveTool', %player.currTool);
         }
         else if(%client.currInv >= 0 && %client.currInv < %player.getDataBlock().maxItems)
         {
            commandToClient(%client, 'SetActiveBrick', %client.currInv);
         }
      }
   
   }

   function GameConnection::onClientEnterGame(%client)
   {
      if(%client.inventory0 !$= "" && %client.inventory0 !$= "0")
         commandToClient(%client, 'CancelAutoBrickBuy');
      Parent::onClientEnterGame(%client);
      %client.loadPersistence();
   }

   function GameConnection::onClientLeaveGame(%client)
   {
      %client.savePersistence();
      Parent::onClientLeaveGame(%client);
   }

   function doQuitGame()
   {
      //if we're hosting and hit "quit", save all clients first
      if(isObject(ServerGroup))
      {
         saveAllClientPersistence();
      }

      Parent::doQuitGame();
   }  

   function saveAllClientPersistence()
   {
      %count = clientGroup.getCount();
      for(%i = 0; %i < %count; %i++)
      {
         %cl = clientGroup.getObject(%i);
         %cl.savePersistence();
      }
   }

   function loadAllClientPersistence()
   {
      %count = clientGroup.getCount();
      for(%i = 0; %i < %count; %i++)
      {
         %cl = clientGroup.getObject(%i);
         if(!%cl.hasSpawnedOnce)
            %cl.loadPersistence();
      }
   }

   function GameConnection::schedulePersistenceSave(%client, %firstTime)
   {
      if(isEventPending(%client.persistenceSchedule))
         cancel(%client.persistenceSchedule);

      if(!%firstTime)                                     //don't save immediately after loading
         if(!isEventPending($LoadSaveFile_Tick_Schedule)) //don't save during brick loading
            %client.savePersistence();

      //save every 5 minutes
      %client.persistenceSchedule = %client.schedule(5 * 60 * 1000, schedulePersistenceSave);
   }

};
activatePackage(PlayerPersistencePackage);


if(isFunction(RegisterPersistenceVar))
{
   //RegisterPersistenceVar(varname, matchAll, matchDatablock)
   //matchAll = true means to match any tagged field that starts with this string, not just the exact name.  This is expensive so only use if necessary.
   RegisterPersistenceVar("score", false, "");
   RegisterPersistenceVar("mode", false, "");

   RegisterPersistenceVar("currInv", false, "");
   RegisterPersistenceVar("currInvSlot", false, "");

   RegisterPersistenceVar("inventory0", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory1", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory2", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory3", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory4", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory5", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory6", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory7", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory8", false, "fxDTSBrickData");
   RegisterPersistenceVar("inventory9", false, "fxDTSBrickData");
   RegisterPersistenceVar("instantUseData", false, "fxDTSBrickData");

   RegisterPersistenceVar("tool0", false, "ItemData");   
   RegisterPersistenceVar("tool1", false, "ItemData");   
   RegisterPersistenceVar("tool2", false, "ItemData");   
   RegisterPersistenceVar("tool3", false, "ItemData");   
   RegisterPersistenceVar("tool4", false, "ItemData");   
   RegisterPersistenceVar("tool5", false, "ItemData");   
   RegisterPersistenceVar("tool6", false, "ItemData");   
   RegisterPersistenceVar("tool7", false, "ItemData");   
   RegisterPersistenceVar("tool8", false, "ItemData");   
   RegisterPersistenceVar("tool9", false, "ItemData");   
   RegisterPersistenceVar("tool10", false, "ItemData");   
   RegisterPersistenceVar("tool12", false, "ItemData");   
   RegisterPersistenceVar("tool13", false, "ItemData");   
   RegisterPersistenceVar("tool14", false, "ItemData");   
   RegisterPersistenceVar("tool15", false, "ItemData");   
   RegisterPersistenceVar("tool16", false, "ItemData");   
   RegisterPersistenceVar("tool17", false, "ItemData");   
   RegisterPersistenceVar("tool18", false, "ItemData");   
   RegisterPersistenceVar("tool19", false, "ItemData");   
   RegisterPersistenceVar("currTool", false, "");      
}



