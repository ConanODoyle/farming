$FakeClient = new ScriptObject(FakeClients) {isAdmin = 1; isSuperAdmin = 1;};

function loadLastAutosave()
{
  fcn(Conan).isBuilder = 1;
  fcn(Zeustal).isBuilder = 1;
  serverCmdLoadAutosave($FakeClient, "last");

  $Pref::FloatingBricks::AdminOnly = 1;
  $Pref::FloatingBricks::Enabled = 0;

  schedule(25000, 0, setAllWaterLevelsFull);
}
//DO NOT schedule the growTick call - it will break loading plants!

package brickDeath
{
  function fxDTSBrick::onDeath(%obj)
  {
    %ret = parent::onDeath(%obj);
    %obj.isDead = 1;
    return %ret;
  }

  function fxDTSBrick::onRemove(%obj)
  {
    %ret = parent::onRemove(%obj);
    return %ret;
  }
};
activatePackage(brickDeath);