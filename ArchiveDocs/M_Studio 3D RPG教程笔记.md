可用协程来实现跟踪的功能：

```cs
IEnumerator funcName(...)
{
    // some loop code to track
    {
        ...
        yield return null
    }
    
    // attack
    // some code...
}
```



P17：**运用动画关键帧触发事件调用函数方法完成攻击相关计算，没有用物理碰撞的方法。**



P19 Unity单例模式实现：

```cs
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
```



P27 HealthBarUI.cs中的OnEnable()方法中foreach里面的if语句直接判断canvas是不是在worldspace渲染，若世界中有其他worldspace设置的canvas的话则会出错，解决方案是 创建一个单独的变量，并拖拽拿到我们想要的canvas；或者用名字的方式找到它。



P29 获取UI的子组件的时候是用GetChild()方法而不是拖拽的方式。



P30 创建新场景时留意：

- Canvas
- 升级到URP材质
- 烘焙Navigation





