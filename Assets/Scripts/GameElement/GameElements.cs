using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class GameElement
{
    public virtual string name {
        get;set;
    }
    public virtual int value {
        get;set;
    }
    public virtual bool enabled {
        get;set;
    }

    public virtual string description {
        get;set;
    }

    public GameElement() {
    }

    public GameElement(string name, int value) : this(name, value, true)
    {
    }
    public GameElement(string name, int value, bool enabled) : this(name, value, enabled, "")
    {
    }

    public GameElement(string name, int value, bool enabled, string description)
    {
        this.name = name;
        this.value = value;
        this.enabled = enabled;
        this.description = description;
    }
}

[Serializable]
public class Staff : GameElement
{
    public List<GameElement> consumeList;
    public List<GameElement> produceList;

    //StringBuilder sb = new StringBuilder();

    public Staff(string name, int value, bool enabled) : base(name, value, enabled)
    {
        consumeList = new List<GameElement>();
        produceList = new List<GameElement>();
        //description = GetDescription();
    }

    public override string description {
        get
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name).AppendLine( "每单位人力");
            sb.Append("消耗：");
            if (consumeList.Count <= 0)
            {
                sb.Append("无");
            }
            else
            {
                foreach (GameElement ge in consumeList)
                {
                    sb.AppendFormat(ge.name).AppendFormat("-{0} ", ge.value);
                }
            }
            sb.Append("\n生产：");
            if (produceList.Count <= 0)
            {
                sb.Append("无");
            }
            else
            {
                foreach (GameElement ge in produceList)
                {
                    sb.AppendFormat(ge.name).AppendFormat("+{0} ", ge.value);
                }
            }
            return sb.ToString();
        }
    }

}
[Serializable]
public class Good : GameElement
{
    public List<GameElement> materialList;
    public GameElement item;

    public bool isInfinite { get; set; }

    public Good(string name, int value, bool isInfinite, bool enabled, string desc) : base(name, value, enabled,desc)
    {
        this.isInfinite = isInfinite;
        materialList = new List<GameElement>();
    }
}
[Serializable]
public abstract class Building : GameElement {
    public string activatingBuildingInfo
    {
        get
        {
            return activateInfo;
        }
    }
    protected string activateInfo;

    public Building(string name, int value, bool enabled) : base(name, value, enabled)
    {
    }
}
[Serializable]
public class Shelter : Building
{
    /// <summary>
    /// the number of HR will increase
    /// </summary>
    public int capacity;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value">shelter already have</param>
    /// <param name="enabled">allow to  be build?</param>
    /// /// <param name="capacity">the number of HR will increase</param>
    public Shelter(string name, int value, bool enabled, int capacity)
        : base(name, value, enabled)
    {
        this.capacity = capacity;
        activateInfo = "你建造了" + name + ", 殖民地可居住人数上升了" + capacity;
    }
}

[Serializable]
public class ProductionBuilding : Building
{
    /// <summary>
    /// HR type name
    /// </summary>
    public string staffName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value">useless here.Allow only one exists</param>
    /// <param name="enabled">allow to  be build?</param>
    /// /// <param name="staffName">HR type name</param>
    public ProductionBuilding(string name, int value, bool enabled, string staffName)
        : base(name, value, enabled)
    {
        this.staffName = staffName;
        activateInfo = "你建造了" + name + ", 你现在可以招募" + staffName + "了";
    }
}
[Serializable]
public class RosterItem : GameElement {

    public int maxValue;
    public BattleUnit battleUnit;

    public RosterItem(BattleUnit unit, int value, int maxValue, bool enabled) 
    {
        if (unit != null)
        {
            this.battleUnit = unit;
            this.maxValue = maxValue;

            this.name = unit.name;
            this.value = value;
            this.enabled = enabled;
            this.description = unit.description;
        }
    }
}

[Serializable]
public class BattleUnit : GameElement
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float life { get; set; }

    public float maxLife { get; set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public float atk { get; set; }
    /// <summary>
    /// 防御力
    /// </summary>
    public float def { get; set; }
    /// <summary>
    /// 攻击频率，times per second
    /// </summary>
    public float dps { get; set; }


    /// <summary>
    /// value is useless here
    /// </summary>
    /// <param name="name"></param>
    /// <param name="desc"></param>
    /// <param name="enabled"></param>
    /// <param name="maxLife"></param>
    /// <param name="atk"></param>
    /// <param name="def"></param>
    /// <param name="dps"></param>
    public BattleUnit(string name, string desc, bool enabled, float maxLife, float atk, float def, float dps)
        : base(name, 0, enabled)
    {
        this.description = desc;
        this.maxLife = maxLife;
        this.life = maxLife;
        this.atk = atk;
        this.def = def;
        this.dps = dps;
    }
}
[Serializable]
public class Enemy : BattleUnit
{
    /// <summary>
    /// 掉落物品
    /// </summary>
    public List<GameElement> drop;

    public Enemy(string name, string desc, bool enabled, float maxLife, float atk, float def, float dps)
        : base(name, desc, enabled, maxLife, atk, def, dps)
    {
        drop = new List<GameElement>();
    }
}