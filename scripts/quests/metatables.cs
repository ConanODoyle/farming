// TODO: manually create a list of "metatables" for quest-givers
//
// metatables are just a string of table names so "vegetableTableA fruitTableA
// fishTableA" might be a valid metatable
//
// the sort of thing i'm looking for:
// quest request metatable for each difficulty level, potentially one set for
// each quest-giver
// quest reward metatable for each difficulty level, definitely one set for each
// quest-giver
//
// notes: metatables are just a way to combine tables of various categories into
// a larger table so that players can get enhanced variety of requests - how
// these are split up is arbitrary
//
// metatables must contain at least 3 tables so that quests can roll up to 3
// unique item types without running out of tables to draw from
//
// tables can be members of more than one metatable, but metatables' tables
// should not have any common items, or else quests like "get 75 potatoes and
// 32 potatoes" would happen because of the way quest generation works
