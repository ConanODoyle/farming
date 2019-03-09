function servercmdclearitems(%cl)
{
  if (%cl.isAdmin)
  {
    %pos = %cl.player.position;
    initcontainerradiussearch(%pos, 300, $Typemasks::itemobjecttype);
    while (isobject(%next = containerSearchNext()))
    {
      if (!%next.isStatic())
        %next.delete();
    }
  }
  messageAll('MsgClearBricks', "\c2" @%cl.name @ " cleared nearby items.");
}
