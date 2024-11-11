using UnityEngine;

public class test : MonoBehaviour
{

    public GameObject gameObject;
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
                GameObject go = Instantiate(gameObject);
                go.transform.position = new Vector3(i + ox, 0, oz + j);
            }
        }

    }
}