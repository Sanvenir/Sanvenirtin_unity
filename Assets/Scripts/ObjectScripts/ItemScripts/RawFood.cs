using ObjectScripts.CharSubstance;
using UtilScripts.Text;

namespace ObjectScripts.ItemScripts
{
    /// <inheritdoc cref="SingularObject" />
    /// <inheritdoc cref="IConsumableItem"/>>
    /// <summary>
    /// A basic food
    /// </summary>
    public class RawFood: SingularObject, IConsumableItem
    {
        public void DoConsume(Character character)
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetEatItemLog(TextName, character.TextName));
            Weight -= 1;
            character.Hunger -= 10;
            
            if (Weight > 0) return;
            SceneManager.Instance.Print(
                GameText.Instance.GetEatUpItemLog(TextName, character.TextName));
            Destroy(gameObject);
        }
    }
}