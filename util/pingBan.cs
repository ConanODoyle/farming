function serverCmdPingBan(%cl, %blid)
{

   if (!%cl.isAdmin)
   {
      return;
   }

   if (%blid $= "")
   {
      messageClient(%cl, '', "Usage: /pingBan blid");
      return;
   }

   if (!isObject(%target = findClientByBL_ID(%blid)))
   {
      messageClient(%cl, '', "Cannot find player with BLID " @ %blid);
      return;
   }
   
   if (%target.pingbanned)
   {
      %target.pingBanned = 0;
      messageclient(%cl, '', "Pingban removed from " @ %target.name);
      if (isObject(%target))
      {
         messageClient(%target, '', "\c5You can ping players again.");
      }
   }
   else 
   {
      %target.pingBanned = 1;
      messageclient(%cl, '', "Pingbanned " @ %target.name);
      if (isObject(%target))
      {
         messageClient(%target, '', "\c5You can no longer ping players.");
      }
   }
 
}

RegisterPersistenceVar("pingBanned", false, "");