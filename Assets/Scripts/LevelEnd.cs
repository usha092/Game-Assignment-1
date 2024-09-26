using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    // Every level has it own Level End object. This object knows what is going to be the next level our game
    // will open. The name of the next level is in this variable.
    public string nextLevel; 

    // For example in Level1 the value of this nextLevel is "Level2" in Level2 the value is "Level3" and so on.

    // You need to make Unity scenes and name them as "Level1" Level2" and "Level3"

    // When Player hits this Level End object it will read the value of nextLevel and then open the scene. 
    // Rememeber to put ALL you scenes to the list in Build Settings. 


}
