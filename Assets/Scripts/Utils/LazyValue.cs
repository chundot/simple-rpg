using System;

namespace RPG.Utils
{
  /// <summary>
  /// 封装容器类型, 使用之前强制初始化
  /// </summary>
  public class LazyValue<T>
  {
    T _value;
    bool _initialized = false;
    readonly Func<T> _initializer;

    /// <summary>
    /// 初始化容器, 不初始化值.
    /// </summary>
    /// <param name="initializer"> 
    /// 初始化函数.
    /// </param>
    public LazyValue(Func<T> initializer)
    {
      _initializer = initializer;
    }

    /// <summary>
    /// 获取或设定值.
    /// </summary>
    public T Value
    {
      get
      {
        // 保证值的初始化.
        ForceInit();
        return _value;
      }
      set
      {
        // 手动初始化, 不再使用默认初始化.
        _initialized = true;
        _value = value;
      }
    }

    /// <summary>
    /// 强制初始化.
    /// </summary>
    public void ForceInit()
    {
      if (!_initialized)
      {
        _value = _initializer();
        _initialized = true;
      }
    }
  }
}