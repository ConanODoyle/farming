function serverCmdMessageBoxNo(%client)
{
    if(isFunction(%client.messageBoxYesNoCallback))
    {
        call(%client.messageBoxYesNoCallback, %client);
        %client.messageBoxYesNoCallback = "";
    }
}