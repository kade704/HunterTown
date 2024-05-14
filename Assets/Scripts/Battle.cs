using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] private AvatarCustomize[] _avatarCustomize = new AvatarCustomize[4];

    public void SetAvatarCustomize(int index, AvatarCustomize avatarCustomize)
    {
        if (avatarCustomize != null)
        {
            _avatarCustomize[index].ShowAvatar(true);
            _avatarCustomize[index].BodyClothSprite = avatarCustomize.BodyClothSprite;
            _avatarCustomize[index].HairSprite = avatarCustomize.HairSprite;
            _avatarCustomize[index].LeftSleeveSprite = avatarCustomize.LeftSleeveSprite;
            _avatarCustomize[index].RightSleeveSprite = avatarCustomize.RightSleeveSprite;
            _avatarCustomize[index].LeftPantSprite = avatarCustomize.LeftPantSprite;
            _avatarCustomize[index].RightPantSprite = avatarCustomize.RightPantSprite;
            _avatarCustomize[index].HairColor = avatarCustomize.HairColor;
        }
        else
        {
            _avatarCustomize[index].ShowAvatar(false);
        }
    }
}
