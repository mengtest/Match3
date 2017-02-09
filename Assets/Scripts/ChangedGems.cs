using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class is used to describe gems that recently changed (i.e. were collapsed or created)
/// </summary>
public class ChangedGems
{
    private List<GameObject> gems { get; set; }
    public int MaxDistance { get; set; }

    public IEnumerable<GameObject> Gems
    {
        get
        {
            return gems.Distinct();
        }
    }

    public void Add(GameObject go)
    {
        if (!gems.Contains(go))
            gems.Add(go);
    }

    public ChangedGems()
    {
        gems = new List<GameObject>();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (GameObject gemGO in gems)
        {
            var gem = gemGO.GetComponent<Gem>();
            if (gem != null)
            {
                sb.Append("[" + gem.Row + "][" + gem.Column + "], ");
            }
        }
        if (sb.Length > 0)
            return sb.ToString().Substring(0, sb.Length - 2);
        return string.Empty;
    }
}