dtween 



/* ABOUT */
dtween is the simple tweening solution you have been looking for.

- Animate any public object property.
- Browse through eight commented demos that show you the ins and outs of the Framework.
- Improve your animations with two easy to use components.
- Setup beautiful animations chains with dtweens start, update and complete events.



/* GET STARTED */
- Open one of the available scenes in the Scenes folder.
- Every scene contains a canvas element with an "image" GameObject.
- The GameObject contains the demo components. You can find them in the Demo folder.



/* HOW IT WORKS */
The base class "DTween" acts as container element for all your tweens. Add a tween by calling
the "To" or "From" function of a class instance (returns a Tween object).

You can update every tween of the DTween instance by calling its "Update" function in the MonoBehaviour
"FixedUpdate" or "Update" call.

To change a transform position value, you can listen to the OnUpdate callback of the Tween object and
set the Vector value on every callback. Alternatively you can use the Mutate Component to set
those values directly (See "0_DemoBasicTo" or "2_DemoMutate").



/* SIMPLE EXAMPLE */
// Tweens the public property "value" from 0 to 100 in 2 seconds, after 1 second.

private DTween dTween = new DTween();
public float value = 0;

public void Start()
{
	dTween.To( this, 2, new { delay = 1, value = 100 }, Back.EaseInOut );
}

public void FixedUpdate()
{
	dTween.Update();
}



/* WEBSITE *
http://davidochmann.de