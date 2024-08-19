using DS.ScriptableObjects;

using UnityEngine;
using UnityEngine.Events;

namespace GameJam {
  [System.Serializable]
  public class Level {
    [SerializeField]
    public GameObject ParentObject;

    [SerializeField]
    public int UsersNeededForLevel = 0;

    [SerializeField]
    public int UsersPerPortAtLevel = 0;

        [Header("Events")]
    [SerializeField]
    public UnityEvent Event;

    [field: Header("Dialog")]
    [field: SerializeField]
    public DSDialogueSO LevelStartDialogNode { get; set; }
  }
}
