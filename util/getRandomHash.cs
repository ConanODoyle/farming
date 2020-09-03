// generates a random hash, optionally appended with a string to "group" hashes
function getRandomHash(%str) {
    return sha1(getRandom(0, 999999) @ getRandom(0, 999999) @ getRandom(0, 999999) @ getRandom(0, 999999) @ %str);
}
