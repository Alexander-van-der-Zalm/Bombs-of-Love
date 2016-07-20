using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentFollowPath : AgentSteering
{
    public Vector2 PathOffset = new Vector2(0.5f, 0);
    public float CloseEnough = 0.3f;

    private int index = 1;
    private List<Node> m_Path;

    public List<Node> Path { set { m_Path = value; MoveToIndexOnPath(0); } get { return m_Path; } }

    public override void Init()
    {
        base.Init();
        if (Path != null)
            MoveToIndexOnPath(index);
    }

    public override void UpdateSteering()
    {
        float distance = (transform.position - Target.position).magnitude;
        if (distance < CloseEnough)
        {
            MoveToIndexOnPath(++index);
        }
        base.UpdateSteering();
    }

    private void MoveToIndexOnPath(int i)
    {
        if (Path == null || i >= Path.Count)
            return;

        Debug.Log("NextNode " + i);
        Target.position = Path[i].Pos + PathOffset;
        index = i;
    }
}
