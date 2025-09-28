
package PlayerPersistencePackage
{
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

      // remove echo due to increased save frequency
      // echo("Saving persistence for BLID " @ %client.getBLID());


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

   function GameConnection::schedulePersistenceSave(%client, %firstTime)
   {
      if(isEventPending(%client.persistenceSchedule))
         cancel(%client.persistenceSchedule);

      if(!%firstTime)                                     //don't save immediately after loading
         if(!isEventPending($LoadSaveFile_Tick_Schedule)) //don't save during brick loading
            %client.savePersistence();

      //save every 5 seconds scaled up based on playercount
      %client.persistenceSchedule = %client.schedule((ClientGroup.getCount() * 500) + 5000, schedulePersistenceSave);
   }

};
activatePackage(PlayerPersistencePackage);