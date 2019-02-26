package TreasureChestReward
{
  function fxDTSBrick::openTreasureChest(%brick, %pl)
  {
    %ret = parent::openTreasureChest(%brick, %pl);
    %cl = %pl.client;
    %cl.setScore(%cl.score + 100);
    messageClient(%cl, '', "\c6You got \c2$100\c6 for finding the chest!");
    return %ret;
  }
};
activatePackage(TreasureChestReward);
