枪械的数据管理用GunData_SO。

枪械名称、弹匣最大装弹量、当前弹匣装弹量、备用子弹量。

画面左下角显示枪械名称和子弹信息的UI。



gunData

bulletData

bulletPrefab

casingPrefab



动画状态机：

移动的位置变换逻辑与射击是分离的，所以动画切换时，待机、走路都可以切换成别的枪械动画。

- idle：有wasd输入时进入walk
- walk：有lshift输入时进入run
- run：reload和fire可以打断run状态，此时播放对应对话，移动速度变为walk
- fire：idle、walk、run状态下可进入
- holster（换枪）：任何状态下可进入



装弹动画百分比

- 弹匣不为空：25%拆下（+在备用弹里面），65%装回去（弹匣填充）
- 弹匣为空：17%拆下，45%装回去，74%拉好枪栓
