using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosedDoor : NPCBehavior
{
    [SerializeField] private AudioSource _SFX;
    [SerializeField] private GameObject _openedDoor;
    [SerializeField] private TextAsset _questCompleteDialog;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>().CanMove == true)
        {
            _animator.SetBool("IsTalk", false);
        }
    }

    public override void SomeAction()
    {
        _SFX.Play();
        PeachesVerification();
        _animator.SetBool("IsTalk", true);
    }

    private void PeachesVerification()
    {
        if (SwitchParametres.PeachCounter > 15)
        {
            _textDialog = _questCompleteDialog;
            _openedDoor.SetActive(true);
        }
    }
}
