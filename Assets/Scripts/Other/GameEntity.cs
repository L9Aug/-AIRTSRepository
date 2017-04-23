// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEntity : MonoBehaviour
{
    #region Variables

    #region Public

    /// <summary>
    /// This Tiles HexTransform.
    /// </summary>
    public HexTransform hexTransform;

    /// <summary>
    /// The ID of the team that this entity belongs to.
    /// </summary>
    [Tooltip("The ID of the team that this entity belongs to.")]
    public int TeamID;

    /// <summary>
    /// The maximum health this entity can have.
    /// </summary>
    [Tooltip("The maximum health this entity can have.")]
    public float MaxHealth;

    /// <summary>
    /// The current health this entity has.
    /// </summary>
    [Tooltip("The current health this entity has.")]
    public float Health;

    #endregion

    #endregion

    #region Functions

    #region Public
    public void DealDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Do death things, then use base.Die()
    /// </summary>
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region Protected

    protected virtual void Start()
    {
        Health = MaxHealth;
    }

    protected virtual void Update()
    {

    }

    #endregion

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(GameEntity), true)]
[CanEditMultipleObjects]
public class GameEntityEditor : Editor
{
    protected static bool UseCustomInpector;

    private GameEntity myGETarget;

    protected virtual void OnEnable()
    {
        myGETarget = (GameEntity)target;
    }

    public override void OnInspectorGUI()
    {
        UseCustomInpector = EditorGUILayout.Toggle("Use Custom Inspector?", UseCustomInpector);

        if (UseCustomInpector)
        {
            EditorGUILayout.LabelField("Team ID:", myGETarget.TeamID.ToString());
            EditorGUILayout.LabelField("Health:", myGETarget.Health.ToString() + " / " + myGETarget.MaxHealth.ToString());
        }
        else
        {
            base.OnInspectorGUI();
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

}
#endif