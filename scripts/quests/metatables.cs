// TODO: manually create a list of metatables for quest-givers
//
// the sort of thing i'm looking for:
// quest request metatable for each difficulty level, potentially one for each quest-giver, but only one is really necessary
// quest reward metatable for each difficulty level, one for each quest-giver
//
// possible considerations: metatables of various types e.g. fish, crops, minerals
// notes: metatables should be essentially just a way to split up tables into at least 3 categories so that players get quests with varied requests and rewards
// more categories is better and will enhance variety of quests
// tables can be members of more than one metatable, but metatables' tables should not have any intersections
// i.e. a metatable containing two tables should not have potatoes in both of those tables
// if this happens, a quest might be given that requests or rewards
