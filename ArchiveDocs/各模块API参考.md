# Managers 各种管理器

## GameManager 游戏管理器

继承于泛型单例

字段：

- playerStats：玩家数据
- followCamera：跟随相机
- endGameObservers：通知游戏结束的观察者链表

方法：

- Awake：生成单例对象，并且在加载新场景时不销毁该对象。
- RegisterPlayer：将玩家数据赋值给字段，并且设置相机跟随和LookAt。
- AddObserver：在链表中注册观察者
- RemoveObserver：在链表中注销观察者
- NotifyObservers：通知所有观察者并执行各自的虚函数
- GetEntrance：获取destinationTag为ENTER的传送门，返回该传送门的锚点的transform。



## MouseManager 鼠标管理器

继承于泛型单例

字段：

- 各种鼠标贴图
- hitInfo：鼠标在屏幕位置发出的射线所击中的GameObject的信息
- 事件OnMouseClicked：鼠标点击时发生的事件
- 事件OnEnemyClicked：鼠标点击敌人时发生的事件

方法：

- Awake：生成单例对象，并且在加载新场景时不销毁该对象。
- Update：执行下面的2个方法
- SetCursorTexture：创建屏幕上的鼠标位置点射出的射线，利用该射线得到该射线与游戏世界相交的信息hitInfo，根据hitInfo中的碰撞体的游戏对象标签来设置指针的不同图标。
- MouseControl：当按下鼠标左键且鼠标点击的物体为碰撞体时，根据该碰撞体的标签，调用类中2个事件订阅的函数。



## SaveManager 存档管理器

继承于泛型单例

字段：

- sceneName：用于做PlayerPrefs的键，该变量具体值是什么并不重要

方法：

- Awake：生成单例对象，并且在加载新场景时不销毁该对象。
- Update：按下Esc返回主菜单，按下S存档，按下L读档
- Save：传入要存储的数据和键，将其转换为Json数据后存入PlayerPrefs中，并且存储当前场景名。
- Load：若PlayerPrefs中有要加载的键，就读取Json文件来读档。
- SavePlayerData：调用前述的Save方法，以GM中的玩家数据对象名称为键存储玩家数据
- LoadPlayerData：调用前述的Load方法，键为GM中玩家数据对象名称。



# Characters 各种角色类

## PlayerController 玩家控制器

字段：

- agent：导航代理
- anim：动画控制器
- characterStats：玩家数据
- attackTarget：攻击目标
- lastAttackTime：上次攻击时间，即CD
- isDead：玩家是否死亡
- stopDistance：记录玩家距离点按的目标多少距离时停止

方法：

- Awake：获取agent、anim、characterStats这几个component，并将停止距离设置为agent的stoppingDistance。
- OnEnable：鼠标管理器的OnMouseClicked事件订阅函数MoveToTarget，OnEnemyClicked订阅函数EventAttack；通过GM注册该玩家数据。
- Start：从存档管理器中读档
- OnDisable：取消OnEnable中订阅的函数
- Update：根据玩家数据中的当前生命值判断是否死亡，若死亡则通过GM通知所有GM中注册的观察者们。切换玩家动画状态；减少lastAttackTime。
- SwitchAnimation：通过动画控制器设置动画状态机中的一些参数变量，从而切换动画。只修改了速度和死亡这2个参数。
- MoveToTarget：通过设置agent相关信息，令玩家移动到target的位置
- EventAttack：设置attackTarget为传入的target，并且用随机数判断玩家是否产生暴击，同时调用协程来移动到攻击目标处
- 协程MoveToAttackTarget：令玩家面向目标，在while循环中判断玩家与目标的距离是否大于设定的停止距离，大于的话就一直更新agent的目标，并从协程返回。如果小于的话则说明玩家已抵达目标处，可以执行攻击，此时判断CD是否结束，然后通过动画控制器传入暴击值和攻击触发器，并且更新CD。
- 动画事件Hit：在动画的关键帧中调用该函数，如果攻击目标的tag为Attackable，就获取攻击目标的各种组件来完成伤害计算或反击的功能。



## EnemyController 敌人控制器

继承于IEndGameObserver接口。

字段：

- enemyStates：枚举敌人状态
- agent：导航代理
- anim：动画控制器
- coll：敌人碰撞体组件
- characterStats：玩家数据
- sightRadius：视野半径
- isGuard：是否是站桩警戒状态
- speed：敌人移动的速度
- attackTarget：攻击目标
- lookAtTime：敌人离开视野范围后的原地等待时间
- remainLookAtTime：敌人离开视野范围后的剩余原地等待时间
- lastAttackTime：上次攻击的时间，即CD
- guardRotation：记录敌人初始的方向朝向
- patrolRange：巡逻范围，用于获取巡逻区内的一个随机点
- isWalk, isChase, isFollow, isDead：敌人动画控制器相关状态
- playerDead：玩家死亡状态

方法：

- Awake：获取各种组件，设定speed初始值为agent的速度，guardPos和guardRotation为初始transform组件的值，初始化remainLookAtTime=lookAtTime。

- Start：如果设定了是站装怪，则切换敌人枚举状态为GUARD，否则为PATROL，并且获取一个新的巡逻目标点。通过GM注册观察者。

- OnDisable：若GM已经初始化了，则注销观察者；未初始化则直接返回。

- Update：判断敌人是否死亡。若玩家未死亡，则更新状态、切换动画、更新CD值。

- SwitchAnimation：根据动画控制器相关状态设置动画控制器的参数。

- SwitchStates：若死亡则更新枚举值，否则检查玩家是否能被敌人看到，能的话就更新为追击状态。然后在Switch语句中根据不同的状态执行不同命令。站桩状态判断敌人是否返回了原来的站桩位置；巡逻状态判断是否抵达了巡逻目标点，然后获取下一个新的巡逻目标点；追击状态判断玩家是否脱离视线，是的话则更新等待时间，等待时间完了再更新状态，否则就通过agent的相关变量来追击玩家，当玩家在攻击范围或者技能范围内时，执行攻击；死亡状态时关闭敌人的碰撞体组件，将敌人的agent的半径设置为0，并在一定时间后销毁该敌人的游戏对象。

- Attack：根据敌人在技能范围或攻击范围内，激活动画控制器对应的触发器

- TargetInAttackRange：判断玩家与敌人的距离是否在近战攻击距离内

- TargetInSkillRange：判断玩家与敌人的距离是否在技能攻击距离内

- FoundPlayer：获取敌人sightRadius内的所有碰撞体，如果其中有碰撞体的标签为玩家，则设定攻击目标为玩家，并且该函数返回true。

- GetNewWayPoint：获取新的巡逻点，同时运用SamplePosition方法来避免新的巡逻点在Unwalkable的物体上。

- OnDrawGizmoSelected：画出sightRadius对应的球体，方便在游戏世界中进行调整

- Hit：动画关键帧调用，若敌人攻击时，敌人面向玩家，则进行伤害计算和生命值扣除。

- EndNotify：玩家死亡时，GM通过注册的观察者通知敌人，敌人切换动画机状态为win，且更改其他状态，来实现敌人在原地欢呼雀跃的效果。



## Golem 石头巨人

继承于EnemyController

字段：

- kickForce：击退力

- rockPrefab：投掷所用的石头的预制件

- handPos：投掷时手的位置

方法：

- KickOff：在扔石头动画的某个关键帧触发，根据石头扔出的方向和kickForce，更改玩家的velocity，并且设置玩家的动画为晕眩，表示玩家受到严重伤害。
- ThrowRock：在扔石头动画的某个关键帧触发，在手部坐标实例化石头。



## Rock 石头巨人所扔的石头

字段：

- rb：刚体组件
- rockStates：石头的枚举状态
- force：石头所受的力
- damage：石头造成的伤害
- target：目标游戏对象
- direction：移动方向
- breakEffect：配合破碎粒子效果来实现玩家反击石头扔到石头巨人身上后，大石头破碎成各个小石头的效果。

方法：

- Start：获取刚体组件，并设置刚体速度为Vector3.one，这是为了避免其在FixedUpdate中切换状态时判断错误而切换到错误的状态。枚举状态切换为HitPlayer。执行飞向目标函数。
- FixedUpdate：当石头速度的模的平方小于1时，切换枚举状态为HitNothing。
- FlyToTarget：根据target的位置和石头的位置得到石头指向target的方向向量，根据方向向量和force，为刚体施加瞬时力。
- OnCollisionEnter：根据石头状态执行不同的命令：HitPlayer，攻击到玩家，则更新玩家导航agent的设置和动画控制器状态并施加伤害，然后切换状态为HitNothing。若状态为HitEnemy，则判断击中的对象是否为石头巨人，是的话则执行伤害计算，并实例化粒子效果，然后销毁石头的GO。



## Grunt 绿巨人

继承于EnemyController。

与石头巨人类似，其中一个攻击方式会击退玩家



# Character Stats 角色数据

## CharacterStats

通过属性获取角色的最大生命值、当前生命值、基础防御力、当前防御力，以及攻击相关的数值。根据攻击者是否暴击来完成不同的伤害计算。

## CharacterData_SO

ScriptableObject，实际存储角色的各种属性，被击杀时给予的经验值和经验值和升级等。



# Combat 战斗数据

## AttackData_SO

包含角色攻击相关的数据，如攻击距离、技能距离、冷却时间、最小伤害、最大伤害、暴击乘数、暴击率等。



# Transition 场景切换

## SceneController 场景控制器

继承与泛型单例和IEndGameObserver接口。

字段：

- playerPrefab：玩家预制件
- sceneFaderPrefab：实现场景渐入渐出的预制件
- fadeFinished：布尔值，表示fade是否结束
- player：玩家
- playerAgent：玩家的导航代理

方法：

- Awake：生成单例对象，并且在加载新场景时不销毁该对象。
- Start：注册观察者，初始化fadeFinished为true。
- TransitionToDestination：根据传送门的目的地是同场景的不同传送门还是不同场景的传送门，来创建转换的协程，但协程参数不同。
- 协程Transition：转换前先通过存档管理器保存玩家数据。然后判断传送的场景是否为当前场景，若是则通过GM获取到player的导航代理，先禁用，然后设置player的位置和朝向，完成后再启用导航代理，然后从协程返回。若传送场景是另一个场景，则先执行场景异步加载，然后在新场景传送门的预定位置实例化玩家，然后并且读取玩家数据，最后从协程退出。
- GetDestination：获取所有的传送门Tag，和传入的参数Tag对比，一致则返回。
- 协程LoadMain：加载主界面，先实例化场景渐入渐出的预制件，然后启动渐出的协程，再进行异步场景加载，再启动渐入的协程，最终退出协程。
- 协程LoadLevel：创建渐出协程、异步场景加载、实例化玩家、存档玩家数据、创建渐出协程，退出协程。
- TransitionToMain：调用LoadMain协程
- TransitionToLoadGame：调用LoadLevel协程，参数为存档管理器的场景名
- TransitionToFirstLevel：调用LoadLevel，参数为“Game”，即第一个场景名称。
- EndNotify：观察者通知函数，游戏结束时调用，当fadeFinished为true时，修改其为false并开启LoadMain协程。



## TransitionDestination 场景传送门的Tag

该类仅用于存储传送门目的地的枚举值。

字段：

- 枚举destinationTag：枚举值表示不同的传送目的地。



## TransitionPoint 传送点

字段：

- 枚举transitionType，标记是传送目的地是同场景还是不同场景
- sceneName：传送目的地的场景名
- destinationTag：传送目的地的传送门标签
- canTrans：是否能传送的布尔标记

方法：

- Update：按下E且canTrans时，调用场景控制器的传送到目的地方法
- OnTriggerStay：玩家进入碰撞体中，则设置canTrans为true
- OnTriggerExit：玩家离开碰撞体，设置canTrans为false



# Tools 工具类

## Singleton 泛型单例类

泛型单例，A类使用时继承于Singleton\<A\>。



## IEndGameObserver 游戏结束观察者接口

只包含一个EndNotify接口，用于当玩家死亡游戏结束时，场景中各种对象执行对应的操作。



## ExtensionMethod 扩展方法

IsFacingTarget方法：在Transform类中的扩展，用于判断敌人是否面朝玩家。



# UI 用户界面

## HealthBarUI 生命条UI

字段：

- healthUIPrefab：生命条预制件
- barPoint：显示在怪物头上的生命条点位
- alwaysVisible：是否一直可见
- visibleTime：可见时间
- timeLeft：剩余可见时间
- healthSlider：Image类型，生命条滑动图像
- UIbar：生命条的位置
- cam：主相机位置
- currentStats：当前敌人角色的数据信息

方法：

- Awake：获取角色数据组件，组件中的UpdateHealthBarOnAttack事件订阅UpdateHealthBar方法
- OnEnable：获取主相机位置，对场景中所有世界空间渲染的画布（目前只有生命条画布是在世界空间）进行实例化，并获取其图像作为healthSlider。
- UpdateHealthBar：更新时，设置生命条为活跃，根据当前生命值和最大生命值的比值更新fillAmount，从而实现生命条变化的效果。
- LateUpdate：更新UIbar的位置和朝向，并根据timeLeft设置UIbar组件的GO是否为激活



## MainMenu 游戏主界面

字段：

- 三个按钮：newGameBtn，continueBtn，quitBtn

- PlayableDirector director，时间条动画组件

方法：

- Awake：获取各个按钮组件，并且为各个按钮组件添加监听的函数；获取PlayableDirector，并将NewGame函数添加为director组件停止后所引发的事件。
- PlayTimeLine：播放时间线director。
- NewGame：新游戏函数，删除所有PlayerPrefs，并且通过场景控制器转移到第一关
- ContinueGame：通过场景控制器加载存档游戏
- QuitGame：退出游戏，关闭应用。



## PlayerHealthUI 玩家生命条UI

字段：

- levelText：玩家等级的文本
- healthSlider：生命条滑动的图像
- expSlider：经验值滑动的图像

方法：

- Awake：获取各个组件给各个字段
- Update：更新玩家等级文本，并调用更新生命条和经验条的函数
- UpdateHealth：通过GM获取到玩家的当前生命值和最大生命值的比值，将其赋值给healthSlider的fillAmount
- UpdateExp：通过GM后渠道玩家的当前经验值和升级所需经验值的比值，将其赋值给expSlider的fillAmount



## SceneFader 场景渐入渐出

字段：

- canvasGrout：画布组
- fadeInDuration, fadeOutDuration：渐入、渐出的持续时间

方法：

- Awake：获取所有画布组，并且该GO设置为加载新场景时不销毁
- 协程FadeOutIn：通过调用下面2个协程来实现渐出渐入
- 协程FadeOut：渐出效果，通过增加画布组的alpha值到1来实现。
- 协程FadeIn：渐入效果，通过减少画布组的alpha值到0来实现。