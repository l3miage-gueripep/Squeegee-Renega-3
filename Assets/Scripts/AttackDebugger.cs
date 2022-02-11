using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDebugger : MonoBehaviour
{
    GameObject hitboxVisualiser;
    private void Start()
    {
        hitboxVisualiser = Resources.Load<GameObject>("Sphere");
    }
    public void ShowAttackHitbox(Attack attack)
    {
        StartCoroutine(DoShowAttackHitbox(attack));
    }
    IEnumerator DoShowAttackHitbox(Attack attack)
    {
        GameObject hitboxVisualiserInstance = CreateHitboxVisualiserInstance(attack);
        for (int i = 0; i < attack.activeFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        DestroyHitboxVisualiserInstance(hitboxVisualiserInstance);
    }
    public GameObject CreateHitboxVisualiserInstance(Attack attack)
    {
        GameObject hitboxVisualizerInstance;
        hitboxVisualizerInstance = Instantiate(hitboxVisualiser, attack.hurtboxCenter.position, new Quaternion(0, 0, 0, 0), attack.transform);
        hitboxVisualizerInstance.transform.localScale = hitboxVisualizerInstance.transform.localScale * attack.range / 5;
        return hitboxVisualizerInstance;
    }
    public void DestroyHitboxVisualiserInstance(GameObject hitboxVisualiserInstance)
    {
        Destroy(hitboxVisualiserInstance);
    }

}
