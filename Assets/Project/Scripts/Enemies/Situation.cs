

//Stephan Ennen 7/27/15

//Enum for identifying what situation any given enemy is in.
namespace Enemies
{

	public enum Situation 
	{
		InControl = 0,			//Enemy is in full control. Nothing is going on.
		BeingPushedByHook = 1,  //Enemy is being pushed by the hook head's inital contact.
		Hooked = 2,				//Enemy is being swung around by the player's control.
		BeingSlammed = 3,		//Enemy is in the middle of being thrown through the air due to a slam.
		//Stunned = 4,			//Enemy is regathering its senses after being slammed.
		//Not all enemies will use the below.. Might be changed later
		//HasIdol = 5; //Enemy has the idol and is trying to run away....
	}
}