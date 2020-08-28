// loot tables - rewards and requests

function farmingTableRemove(%tableName) {
    deleteDataIDArray("QuestTable_" @ %tableName);
}

function farmingTableAdd(%tableName, %item, %weight) {
    if (%weight <= 0) {
        error("ERROR: Can't add item with weight <= 0 (attempted to use weight " @ %weight @ ")");
        return;
    }

    %dataIDArrayName = "QuestTable_" @ %tableName;

    %tableSize = getDataIDArrayCount(%dataIDArrayName) + 1;
    setDataIDArrayCount(%dataIDArrayName, %tableSize);
    addToDataIDArray(%dataIDArrayName, %item SPC %weight);
    setDataIDArrayTagValue(%dataIDArrayName, "weight", getDataIDArrayTagValue(%dataIDArrayName, "weight") + %weight);
}

function farmingTableGetIndex(%tableName, %item) {
    %dataIDArrayName = "QuestTable_" @ %tableName;
    %tableLength = getDataIDArrayCount(%dataIDArrayName);
    for (%i = 0; %i < %tableLength; %i++) {
        if (getWord(getDataIDArrayValue(%dataIDArrayName), 0) $= %item) {
            return %i;
        }
    }

    return -1;
}

function farmingTableRemove(%tableName, %item) {
    %index = farmingTableGetIndex(%tableName, %item);

    if (%index == -1) {
        error("ERROR: Item" SPC %item SPC "not in table" SPC %tableName @ ", can't remove");
        return 0;
    }

    %dataIDArrayName = "QuestTable_" @ %tableName;
    %tableLength = getDataIDArrayCount(%dataIDArrayName);

    %weight = getWord(getDataIDArrayValue(%dataIDArrayName, %index), 1);

    for (%i = %index; %i < %tableLength; %i++) {
        setDataIDArrayValue(%dataIDArrayName, %i, getDataIDArrayValue(%dataIDArrayName, %i + 1));
    }

    setDataIDArrayTagValue(%dataIDArrayName, "weight", getDataIDArrayTagValue(%dataIDArrayName, "weight") - %weight);

    return 1;
}

// give a value between 0 and 1 (0 inclusive, 1 exclusive) or the empty string
// returns an item in the table based on the value or randomly
function farmingTableGetItem(%tableName, %random) {
    %dataIDArrayName = "QuestTable_" @ %tableName;
    %tableLength = getDataIDArrayCount(%dataIDArrayName);

    if (%tableLength == 0) {
        warn("Warning: attempted to get from empty table" SPC %tableName);
        return "";
    }

    while (%random $= "" || %random < 0 || %random >= 1) {
        if (%random < 0) {
            warn("Warning: attempted to get from table" SPC %tableName SPC "with value < 0; using random value");
        } else if (%random >= 1) {
            warn("Warning: attempted to get from table" SPC %tableName SPC "with value >= 1; using random value");
        }

        %random = getRandom();
    }

    %position = %random * getDataIDArrayTagValue(%dataIDArrayName, "weight");
    %currWeight = 0;

    for (%i = 0; %i < %tableLength; %i++) {
        %entry = getDataIDArrayValue(%dataIDArrayName, %i);
        %item = getWord(%entry, 0);
        %weight = getWord(%entry, 1);

        %currWeight += %weight;

        if (%position < %currWeight) {
            return %item;
        }
    }

    error("ERROR: Malformed table" SPC %tableName SPC "- no item found, stored total weight may be larger than real total weight");
    return "";
}
