$FakeClient = new ScriptObject(FakeClients) {isAdmin = 1; isSuperAdmin = 1;};

function loadLastAutosave()
{
  fcn(Conan).bypassRestrictions = 1;
  fcn(Zeustal).bypassRestrictions = 1;
  serverCmdLoadAutosave($FakeClient, "last");

  schedule(15000, 0, setAllWaterLevelsFull);
}
//DO NOT schedule the growTick call - it will break loading plants!

package brickDeath
{
  function fxDTSBrick::onDeath(%obj)
  {
    %ret = parent::onDeath(%obj);
    %obj.isDead = 1;
    // if (%obj.getDatablock().isSprinkler)
    // {
    //  SprinklerSimSet.remove(%obj);
    // }
    return %ret;
  }

  function fxDTSBrick::onRemove(%obj)
  {
    %ret = parent::onRemove(%obj);
    // if (%obj.getDatablock().isSprinkler)
    // {
    //  SprinklerSimSet.remove(%obj);
    // }
    return %ret;
  }
};
activatePackage(brickDeath);