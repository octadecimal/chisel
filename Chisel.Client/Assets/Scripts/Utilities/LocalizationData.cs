using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class LocalizationData
{
    public LocalizationItem[] Cells;
    public LocalizationItem[] Buildings;
    public LocalizationItem[] Resources;
}

[System.Serializable]
public class LocalizationItem
{
    public string k;
    public string v;
}