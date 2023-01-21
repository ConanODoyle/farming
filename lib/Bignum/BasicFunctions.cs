// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Base Functions			//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		WHAT YOU ARE DOING		---	//
// ------------------------------------	//

//Function Purpose:	Performs basic, bare-bones addition on two integers.
//Function Inputs:	Num1: The number to add to.
//					Num2: The number to add onto the first.
//Requisites:		For correct results, Num1 and Num2 must be strings containing only the characters 0-9.
function IMath_Add(%Num1, %Num2)
{
	%Length1 = strLen(%Num1);
	%Length2 = strLen(%Num2);
	%Max = getMax(%Length1,%Length2);

	if(%Length2 != %Length1)
	{
		//Equalize the length of both numbers
		%x = equalizeLengths(%Num1, %Num2);
		%Num1 = getword(%x, 0);
		%Num2 = getword(%x, 1);
	}

	//Set up our variables
	%Carry = 0;
	%a = %Max - 9;

	//We automatically break out of the loop later so we can just use while(true) safely.
	while(true)
	{
		//Get the correct parts of the numbers
		if(%a >= 0)
		{
			%Current1 = getSubStr(%Num1, %a, 9) | 0; //The | 0 after these getsubstr calls forces torque
			%Current2 = getSubStr(%Num2, %a, 9) | 0; //to convert it to an integer, which speeds things up a good bit.
		}
		else
		{
			%x = %a + 9;
			%Current1 = getsubstr(%Num1, 0, %x) | 0;
			%Current2 = getsubstr(%Num2, 0, %x) | 0;
		}

		%Temp = (((%Current1 | 0) + (%Current2 | 0)) | 0) + (%Carry | 0) | 0; //The | 0's in here prevent torque from switching numbers to floats.
		%l = strlen(%Temp);

		//If the length of the number is greater than 9, that means we have to carry over.
		if(%l > 9)
		{
			%Temp = (%Temp | 0) - 1000000000 | 0; //Subtraction in this case is faster than modulus, and they do the same thing.
			%l = strlen(%Temp);
			%Carry = 1;
		}
		else
			%Carry = 0;

		%a -= 9;

		if(%a > -9)
		{
			%l = 9 - %l;
			if(%l > 0) //Add 0's to the temporary answer so that it displays right in the result.
				%Temp = getsubstr("00000000",0, %l) @ %Temp;

			%Result = %Temp @ %Result;
		}
		else
		{
			if(%Carry == 0)
				%Result = %Temp @ %Result;
			else
			{
				%l = 9 - %l;
				if(%l > 0)
					%Temp = getsubstr("00000000",0, %l) @ %Temp;

				//If there's a carry active, that means we need to add a one. Otherwise the answer would be incorrect, obviously.
				%Result = "1" @ %Temp @ %Result;
			}

			//Since we're all done, break out of the loop and return the result.
			break;
		}
	}

	return %Result;
}

//Function Purpose:	Performs basic, bare-bones subtraction on two integers.
//Function Inputs:	Num1: The number to subtract from.
//					Num2: The number to subtract from the first.
//Requisites:		For correct results, Num1 and Num2 must be strings containing only the characters 0-9.
function IMath_Subtract(%Num1, %Num2)
{
	if(aGreaterThanb(%Num2, %Num1))
		return "-" @ IMath_Subtract(%Num2, %Num1);

	%Length1 = strLen(%Num1);
	%Length2 = strLen(%Num2);
	%Max = getMax(%Length1,%Length2);

	if(%Length2 != %Length1)
	{
		//Equalize the length of both numbers
		%x = equalizeLengths(%Num1, %Num2);
		%Num1 = getword(%x, 0);
		%Num2 = getword(%x, 1);
	}

	%count = -1;

	for(%a = %max - 9; true; %a -= 9)
	{
		if(%a >= 0)
		{
			%n1[%count++] = getSubStr(%num1, %a, 9) | 0; //As stated before, the | 0 makes it faster.
			%n2[%count] = getSubStr(%num2, %a, 9) | 0;
		}
		else
		{
			%x = %a + 9;
			%n1[%count++] = getsubstr(%num1, 0, %x) | 0;
			%n2[%count] = getsubstr(%num2, 0, %x) | 0;
			break;
		}
	}

	%count++;

	for(%a = 0; %a < %count; %a++)
	{
		%res = ((%n1[%a]|0) - (%n2[%a]|0)) | 0;

		//For some reason the number has to be converted to a string in order for it to be compared like this.
		//If the result is less than 0, then we need to carry.
		if(%res @ "" < 0)
		{
			//Perform carrying. I'm not really going to explain this.
			for(%b = %a + 1; %b < %count; %b++)
			{
				if(((%n1[%b]|0) - (%n2[%b]|0) | 0) @ "" > 0)
				{
					%n1[%b] = ((%n1[%b] | 0) - 1) | 0;
					%n1[%a] = ((%n1[%a] | 0) + 1000000000) | 0;

					for(%c = %b - 1; %c > %a; %c--)
						%n1[%c] = ((%n1[%c] | 0) + 999999999) | 0;

					break;
				}
			}

			//Re-assign the result variable
			%res = ((%n1[%a]|0) - (%n2[%a]|0)) | 0;
		}

		if(%a + 1 != %count) //Add zeros so it displays properly in the result
			%Ans = ((%x=9-strlen(%res)) > 0 ? (getsubstr("00000000",0,%x) @ %res) : %res) @ %Ans;
		else
		{
			if(%res != 0)
				%Ans = %res @ %Ans;
			else //Trim leading zeroes
			{
				%l = strLen(%ans) - 1;
				while(getsubstr(%ans, 0, 1) $= "0" && %l > 1)
				{
					%l--;
					%ans = getsubstr(%ans, 1, 9999);
				}
			}
		}
	}

	return %Ans;
}

//Function Purpose:	Performs basic, bare-bones multiplication on two integers.
//Function Inputs:	Num1: The first number to multiply.
//					Num2: The number to multiply by the first.
//Requisites:		For correct results, Num1 and Num2 must be strings containing only the characters 0-9.
function IMath_Multiply(%Num1, %Num2)
{
	%Length2 = strLen(%Num2);
	%Length1 = strLen(%Num1);
	%firstCount = -1;

	//Get all of the first number into an array. These are set up so we do as few getsubstr operations as possible.
	for(%a = %Length1 - 4; %a >= 0; %a -= 4)
		%n1[%firstCount++] = getSubStr(%Num1, %a, 4) | 0;

	if(%a > -4)
		%n1[%firstCount++] = getSubStr(%Num1, 0, %a + 4) | 0;

	%tmp2 = 0;
	%firstCount++;
	%totalCount = 0;

	//Loop through every 4 digits of the 2nd number only once, never putting the digits into the array
	for(%a = %Length2 - 4; %a > -4; %a -= 4)
	{
		if(%a >= 0)
			%n2 = getSubStr(%Num2, %a, 4) | 0;
		else
			%n2 = getSubStr(%Num2, 0, %a + 4) | 0;

		for(%b = 0; %b < %firstCount; %b++)
		{
			//Since n2 and n1 have already both been | 0'd, we only need to do it at the end of the expression.
			//Calculate the product of the two numbers
			%tmp = %n2 * %n1[%b] | 0;

			%l = strLen(%tmp);

			%tt = %tmp2 + %b | 0;

			if(%l > 4)
			{
				%l -= 4;
				%rem = (%tmp|0) % 10000 | 0;

				%tmps[%tt] = %tmps[%tt] + %rem | 0;
				%tmps[%tt + 1] = %tmps[%tt + 1] + (((%tmp|0) - (%rem|0) | 0)/10000) | 0;
				%totalCount = getMax(%totalCount, %tt+1);
			}
			else
			{
				%tmps[%tt] = %tmps[%tt] + %tmp | 0;
				%totalCount = getMax(%totalCount, %tt);
			}

			while(%tmps[%tt] >= 10000)
			{
				%tmps[%tt+1] = %tmps[%tt+1] + 1 | 0;
				%tmps[%tt] = %tmps[%tt] - 10000 | 0;
				%tt++;
				%totalCount = getMax(%totalCount, %tt);
			}
		}
		
		//We use a lookup table here to get rid of the extra getsubstrs. The less getsubstr's we have to use the better.
		%Result = $NORMAL[%tmps[%tmp2]] @ %Result;

		%tmp2++;
	}

	if((%tmps[%totalCount]|0) != 0)
		%totalCount++;

	for(%a = %tmp2; %a < %totalCount; %a++)
	{
		if(%a + 1 != %totalCount)
			%tmps[%a] = $NORMAL[%tmps[%a]];

		%Result = %tmps[%a] @ %Result;
	}

	return %Result;
}