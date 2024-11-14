public abstract class GameSubUI
{
	private GameUI GameUI;

	private GameUIListener mGameUIListener = EmptyGameUIlistener.Self;

	public void Create(GameUI gameUI)
	{
		GameUI = gameUI;
		OnCreate();
	}

	protected abstract void OnCreate();

	public void Destroy()
	{
		OnDestroy();
		GameUI = null;
	}

	protected virtual void OnDestroy()
	{
	}

	public void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}

	protected GameUI GetGameUI()
	{
		return GameUI;
	}

	public void ResumeFromHide()
	{
		OnResume();
	}

	protected virtual void OnResume()
	{
	}

	public void Hide()
	{
		OnHide();
	}

	protected virtual void OnHide()
	{
	}

	public void SetListener(GameUIListener listener)
	{
		if (listener == null)
		{
			mGameUIListener = EmptyGameUIlistener.Self;
		}
		else
		{
			mGameUIListener = listener;
		}
	}

	public GameUIListener GetListener()
	{
		return mGameUIListener;
	}
}
