using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Core.UI.Dragging
{
  public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
      where T : class
  {
    Vector3 _startPosition;
    Transform _originalParent;
    IDragSource<T> _source;

    Canvas _parentCanvas;

    void Awake()
    {
      _parentCanvas = GetComponentInParent<Canvas>();
      _source = GetComponentInParent<IDragSource<T>>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
      _startPosition = transform.position;
      _originalParent = transform.parent;
      // 
      GetComponent<CanvasGroup>().blocksRaycasts = false;
      transform.SetParent(_parentCanvas.transform, true);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
      transform.position = _startPosition;
      GetComponent<CanvasGroup>().blocksRaycasts = true;
      transform.SetParent(_originalParent, true);

      var container = !EventSystem.current.IsPointerOverGameObject() ? _parentCanvas.GetComponent<IDragDestination<T>>() : GetContainer(eventData);
      if (container != null)
        DropItemIntoContainer(container);
    }

    IDragDestination<T> GetContainer(PointerEventData eventData)
    {
      if (eventData.pointerEnter)
      {
        var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
        return container;
      }
      return null;
    }

    void DropItemIntoContainer(IDragDestination<T> destination)
    {
      if (ReferenceEquals(destination, _source)) return;

      if (destination is not IDragContainer<T> destinationContainer || _source is not IDragContainer<T> sourceContainer ||
          destinationContainer.Item == null ||
          ReferenceEquals(destinationContainer.Item, sourceContainer.Item))
      {
        AttemptSimpleTransfer(destination);
        return;
      }

      AttemptSwap(destinationContainer, sourceContainer);
    }

    void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
    {
      var removedSourceNumber = source.Number;
      var removedSourceItem = source.Item;
      var removedDestinationNumber = destination.Number;
      var removedDestinationItem = destination.Item;

      source.RemoveItems(removedSourceNumber);
      destination.RemoveItems(removedDestinationNumber);

      var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
      var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

      if (sourceTakeBackNumber > 0)
      {
        source.AddItems(removedSourceItem, sourceTakeBackNumber);
        removedSourceNumber -= sourceTakeBackNumber;
      }
      if (destinationTakeBackNumber > 0)
      {
        destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
        removedDestinationNumber -= destinationTakeBackNumber;
      }

      if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
          destination.MaxAcceptable(removedSourceItem) < removedSourceNumber)
      {
        destination.AddItems(removedDestinationItem, removedDestinationNumber);
        source.AddItems(removedSourceItem, removedSourceNumber);
        return;
      }

      if (removedDestinationNumber > 0)
        source.AddItems(removedDestinationItem, removedDestinationNumber);
      if (removedSourceNumber > 0)
        destination.AddItems(removedSourceItem, removedSourceNumber);
    }

    bool AttemptSimpleTransfer(IDragDestination<T> destination)
    {
      var draggingItem = _source.Item;
      var draggingNumber = _source.Number;

      var acceptable = destination.MaxAcceptable(draggingItem);
      var toTransfer = Mathf.Min(acceptable, draggingNumber);

      if (toTransfer > 0)
      {
        _source.RemoveItems(toTransfer);
        destination.AddItems(draggingItem, toTransfer);
        return false;
      }

      return true;
    }

    int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
    {
      var takeBackNumber = 0;
      var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

      if (destinationMaxAcceptable < removedNumber)
      {
        takeBackNumber = removedNumber - destinationMaxAcceptable;

        var sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

        if (sourceTakeBackAcceptable < takeBackNumber)
          return 0;
      }
      return takeBackNumber;
    }
  }
}