namespace RPG.Core.UI.Dragging
{
  public interface IDragSource<T> where T : class
  {
    T Item { get; }
    int Number { get; }
    void RemoveItems(int number);
  }
}