using UnityEngine;

namespace GamePlugins
{
	public class SingletonScreen<T> : BaseScreen where T : BaseScreen
	{
		protected static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = ScreenManager.Instance.CheckExistScreen<T>();
					if (_instance == null)
					{
						_instance = ScreenManager.Instance.CheckInstanceScreenPrebab<T>();
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
					_instance = ScreenManager.Instance.CheckExistScreen<T>();
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
