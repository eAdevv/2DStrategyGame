using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewBuildData", menuName = "BuildInformation/BuildData")]
public class BuildInformationData : ScriptableObject
{
    public string BuildName;
    public Sprite BuildIcon;
    public bool CanProduceUnit;
}
