function serverCmdBuilder(%cl, %target)
{
  if (%cl.isSuperAdmin)
  {
    if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
    {
      messageClient(%cl, '', "\c6You gave \c3" @ %targ.name @ "\c6 builder mode!");
      %cl = %targ;
    }
    %cl.bypassRestrictions = 1;
    %cl.player.setDatablock(PlayerStandardArmor);
    messageClient(%cl, '', "\c6You are now a builder!");
  }
}

function serverCmdUnbuilder(%cl, %target)
{
  if (%cl.isSuperAdmin)
  {
    if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
    {
      messageClient(%cl, '', "\c6You removed \c3" @ %targ.name @ "\c6's builder mode!");
      %cl = %targ;
    }
    %cl.bypassRestrictions = 0;
    %cl.player.setDatablock(isObject(%cl.playerDatablock) ? %cl.playerDatablock : PlayerNoJet);
    messageClient(%cl, '', "\c6You are not a builder anymore!");
  }
}
