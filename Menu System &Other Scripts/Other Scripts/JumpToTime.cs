using UnityEngine;
using UnityEngine.Playables;

public class JumpToTime : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public double targetTime;

    private bool hasJumped = false;

    void Start()
    {
        // Ensure we have a reference to the PlayableDirector
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    void Update()
    {
        if (!hasJumped && gameObject.activeInHierarchy)
        {
            JumpToTargetTime();
            hasJumped = true;
        }
    }

    void JumpToTargetTime()
    {
        if (playableDirector != null)
        {
            playableDirector.time = targetTime;
            playableDirector.Evaluate(); // Evaluate to jump to the specific time
            playableDirector.Play(); // Resume playback
        }
    }
}
