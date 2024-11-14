using System.IO;

public interface IModule
{
	void Load(BinaryReader br);

	void Free();
}
