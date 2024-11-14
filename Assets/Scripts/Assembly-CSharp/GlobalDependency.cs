using System.Collections.Generic;
using UnityEngine;

public class GlobalDependency
{
	public ThreadPriority Priority = ThreadPriority.Normal;

	public Dictionary<PrefabDependencyType, List<PrefabDependency>> m_dependency = new Dictionary<PrefabDependencyType, List<PrefabDependency>>();
}
