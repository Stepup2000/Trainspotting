using UnityEngine;

public class TreeAnimationEVApplier : BaseEVApplier
{
    private Animator animator;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.ToggleTree.AddListener(TriggerTree);
        TriggerTree(false);
        TryGetComponent<Animator>(out animator);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleTree.RemoveListener(TriggerTree);
    }


    /// <summary>
    /// Starts or stops the tree bending effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the tree bending effect.</param>
    /// </summary>
    public void TriggerTree(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff == true) return;
        animator?.SetBool("ToggleTree", onOrOff);
    }
}
