using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interfaces are class templates
// They are used to make the code more generic as different classes can "derive" from the same interface
// When creating an Interface, it's common practice to name it starting with an "I"
public interface ISnapSurface {

	// Interfaces do not contain actual variables,
	// instead they contain the definition of the variables a class MUST include when deriving from it 
	// Defined variables come with {} containing the kind of usage that can be done of a variable
	// "get;" means that a variable is read only
	Transform snapPoint { get; }

	// "get; set;" means that a variable is read/write
	Pickable snappedHere { get; set; }

	// Same goes for the functions defined in the interface
	void Snap(Pickable pickable);

}