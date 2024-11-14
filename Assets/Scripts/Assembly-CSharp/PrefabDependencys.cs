using System.Collections.Generic;

public class PrefabDependencys
{
	public PrefabDependency m_prefab = new PrefabDependency();

	public Dictionary<PrefabDependencyType, List<short>> m_dependency = new Dictionary<PrefabDependencyType, List<short>>();
}
