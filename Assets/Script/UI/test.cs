using UnityEngine;

public class test : MonoBehaviour
{

    public GameObject gameObject;
    public GameObject mask;
    public Vector3Int pos;
    public int l;
    public int w;
    [ContextMenu("hhh")]
    public void Foo()
    {
        int ox = pos.x - l / 2;
        int oz = pos.z - w / 2;
        for (int i = 0; i < l; i++)
        {
            for (int j = 0; j < w; j++)
            {
                GameObject go = Instantiate(gameObject, mask.transform, false);
                go.transform.position = new Vector3(i + ox, 0.001f, oz + j);
            }
        }

    }
    [ContextMenu("clear")]
    public void Clear()
    {
        int count = mask.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var c = mask.transform.GetChild(i);
            Destroy(c);
        }
    }
}   