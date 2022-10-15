using UnityEngine;

public class LearningSkills : NPCBehavior
{
    [SerializeField] private AudioSource _pickUpSound;
    private CharacterControl _characterControl;
    private enum ParameterType
    {
        jump,
        doubleJump,
        dash,
        attack
    };

    [SerializeField] private ParameterType _parameterType = ParameterType.jump;

    private void Start()
    {
        var characterGO = GameObject.Find("Character");
        _characterControl = characterGO.GetComponent<CharacterControl>();
    }

    public override void SomeAction()
    {
        switch (_parameterType) {

            case ParameterType.jump:
                SwitchParametres.CanJump = true;
                _characterControl.isCanJump = true;
                break;

            case ParameterType.doubleJump:
                SwitchParametres.CanDoubleJump = true;
                _characterControl._isCanDoubleJump = true;
                break;

            case ParameterType.dash:
                SwitchParametres.CanDash = true;
                _characterControl.isCanDash = true;
                break;

            case ParameterType.attack:
                SwitchParametres.CanAttack = true;
                _characterControl.isCanAttack = true;
                break;
        }
        _pickUpSound.Play();
    }
}
