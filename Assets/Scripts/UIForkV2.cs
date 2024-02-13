using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIForkV2 : MonoBehaviour
{
    /*
    public List<Sprite> arrowSprites;

    List<UIForkPanel> panels = new();

    private void Awake()
    {
        panels = TooManyFuncts.GetComponentsInChildrenParametric<UIForkPanel>(transform, "Panel", null, null);
    }

    public void CallFork(Space forkSpace)
    {
        for (int i = 0; i < forkSpace.next.Count; i++)
        {
            panels[i]?.CallPanel(forkSpace.next[i], in arrowSprites);
        }
    }

    public void ReturnFork(UIForkPanel panel)
    {
        InputFork.Instance.OnChooseDirection(panels.IndexOf(panel));
        foreach(var p in panels)
        {
            if (p != panel)
                p.DeactivatePanel();
        }

    }
    */
}
