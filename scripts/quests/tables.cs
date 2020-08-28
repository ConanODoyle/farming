// loot tables - rewards and requests

function farmingTableClear(%tableName) {
    $Farming::TableSize[%tableName] = 0;
    $Farming::TableTotalWeight[%tableName] = 0;
}

function farmingTableAdd(%tableName, %item, %weight) {
    if (%weight <= 0) {
        error("ERROR: Can't add item with weight <= 0");
        return;
    }

    $Farming::TableItem[%tableName, $Farming::TableSize[%tableName]] = %item;
    $Farming::TableWeight[%tableName, $Farming::TableSize[%tableName]] = %weight;

    $Farming::TableSize[%tableName]++;
    $Farming::TableTotalWeight[%tableName] += %weight;
}

function farmingTableGetIndex(%tableName, %item) {
    for (%i = 0; %i < $Farming::TableSize[%tableName]; %i++) {
        if ($Farming::TableItem[%tableName, %i] $= %item) {
            return %i;
        }
    }

    return -1;
}

function farmingTableRemove(%tableName, %item, %weight) {
    %index = farmingTableGetIndex(%tableName, %item);

    if (%index == -1) {
        error("ERROR: Item not in table, can't remove");
        return 0;
    }

    $Farming::TableSize[%tableName]--;
    $Farming::TableTotalWeight[%tableName] -= $Farming::TableWeight[%tableName, %index];

    for (%i = %index; %i < $Farming::TableSize[%tableName]; %i++) {
        $Farming::TableItem[%tableName, %i] = $Farming::TableItem[%tableName, %i + 1];
        $Farming::TableWeight[%tableName, %i] = $Farming::TableWeight[%tableName, %i + 1];
    }

    return 1;
}

// give a value between 0 and 1 (0 inclusive, 1 exclusive) or the empty string
// returns an item in the table based on the value or randomly
function farmingTableGetItem(%tableName, %random) {
    if ($Farming::TableSize[%tableName] == 0) {
        warn("Warning: attempted to get from empty table");
        return "";
    }

    while (%random $= "" || %random < 0 || %random >= 1) {
        if (%random < 0) {
            warn("Warning: attempted to get from table with value < 0; using random value");
        } else if (%random >= 1) {
            warn("Warning: attempted to get from table with value >= 1; using random value");
        }

        %random = getRandom();
    }

    %position = %random * $Farming::TableTotalWeight[%tableName];
    %currWeight = 0;

    for (%i = 0; %i < $Farming::TableSize[%tableName]; %i++) {
        %item = $Farming::TableItem[%tableName, %i];
        %weight = $Farming::TableWeight[%tableName, %i];

        %currWeight += %weight;

        if (%position < %currWeight) {
            return %item;
        }
    }

    error("ERROR: Malformed table - no item found, stored total weight may be larger than real total weight");
    return "";
}
