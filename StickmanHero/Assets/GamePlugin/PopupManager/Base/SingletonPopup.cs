namespace GamePlugins
{
	public class SingletonPopup<T> : BasePopup where T : BasePopup
	{
		protected static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = PopupManager.Instance.CheckExistPopup<T>();
					if (_instance == null)
					{
						_instance = PopupManager.Instance.CheckInstancePopupPrebab<T>();
					}
				}
				return _instance;
			}
		}

		public static T CheckInstance
		{
			get
			{
				if (_instance == null)
				{
					_instance = PopupManager.Instance.CheckExistPopup<T>();
				}
				return _instance;
			}
		}

		public override void Awake()
		{
			base.Awake();
		}
	}
}
