public interface UIContainer
{
	void DrawSprite(UISpriteX sprite);

	void SendEvent(UIControl control, int command, float wparam, float lparam);

	void Add(UIControl control);
}
