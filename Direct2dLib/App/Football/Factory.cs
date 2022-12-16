using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using SharpDX;

namespace Direct2dLib.App.Football
{
    public class Factory
    {
        public Factory()
        {

        }

        public GameObject CreatePlayerByIndex(int index, string spriteLink, Vector3 startPosition)
        {
            GameObject player = new GameObject(startPosition);
            player.AddComponent(new SpriteRenderer(player,
                DX2D.Instance.LoadBitmap(spriteLink),
                100, 100));
            player.AddComponent(new CircleCollider2D(player, 40, true));
            player.AddComponent(new Player(player, index));

            return player;
        } 
    }
}
