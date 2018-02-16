Insert the object you want to animate *underneath* the "AnimationObjectHolder"-gameObject. 
This way, your object will not be scaled up/down, but will still be animated, 
as specified by the selected "Animator Override Controller" that is assigned to the "AnimationObjectHolder"-gameObject.

***HIERARCHY EXAMPLE***
DefaultAnim_Move_DownUp
	AnimationObjectHolder (has the Animator Override Controller, which selects the relevant Animation Clip to play)
		[ADD YOUR OBJECTS HERE]