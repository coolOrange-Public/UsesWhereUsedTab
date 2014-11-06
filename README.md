Installation
============
- copy the Vault folder to the according DS Vault folder
- modify the default.ps1 and add the command OnTabContextChanged_UsesWhereUsed somwhere in the function OnTabContextChanged like this:
```
function OnTabContextChanged
{
...
    OnTabContextChanged_UsesWhereUsed
...
	
}
```
