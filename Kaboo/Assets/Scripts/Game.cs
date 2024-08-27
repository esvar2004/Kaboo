using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public List<int> turn;
    CardDisplay cardDisplay;

    public Game () {
        this.turn = new List<int>(){0, 1, 2, 3};
        cardDisplay = new CardDisplay();
    }

    // Start is called before the first frame update
    void Start()
    {
        Game game = new Game();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
