package positionPersistenceFix
{
   function GameConnection::loadPersistence(%client)
   {
      %ret = parent::loadPersistence(%client);

      if (isObject(%client.player))
      {
         %x = mFloor(getWord(%client.player.getTransform(), 0) * 10) / 10;
         %y = mFloor(getWord(%client.player.getTransform(), 1) * 10) / 10;
         %z = mFloor(getWord(%client.player.getTransform(), 2) * 10) / 10;
         %rot = getWords(%client.player.getTransform(), 3, 10);
         %client.player.setTransform(mFloor(%x) SPC mFloor(%y) SPC mCeil(%z) SPC %rot);
         %client.player.setVelocity("0 0 0");
      }

      return %ret;
   }
};
schedule(1000, 0, activatePackage, positionPersistenceFix);