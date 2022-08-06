namespace RPG.Core.UI.Dragging
{
  public interface IDragDestination<T> where T : class
  {
    int MaxAcceptable(T item);
    void AddItems(T item, int number);
  }
}