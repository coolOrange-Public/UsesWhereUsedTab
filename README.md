Installation:
1.) copy the Vault folder to the according DS Vault folder
2.) modify the default.ps1 and add the command OnTabContextChanged_UsesWhereUsed somwhere in the function OnTabContextChanged like this:

function OnTabContextChanged
{
...
	OnTabContextChanged_UsesWhereUsed
...
	
}

