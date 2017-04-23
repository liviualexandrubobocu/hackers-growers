using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeForm
{
	private double lifeSpan { get; set; }
	private double growingRate { get; set; }
	private double attackPower { get; set; }
	private double defensePower { get; set; }
	private double mutationTime { get; set; }

    int actionRadius { get; set; }

	public enum LifeType
	{
		CarbonBased,
		SilliconBased,
		MetalBased
	}

	public LifeForm(
		double lifeSpan,
		double growingRate,
		double attackPower,
		double defensePower,
		double mutationTime
	){
		this.lifeSpan = lifeSpan;
		this.growingRate = growingRate;
		this.attackPower = attackPower;
		this.defensePower = defensePower;
		this.mutationTime = mutationTime;
	}

	public void attackLifeForm(LifeForm alienLifeForm){
		if (this.attackPower > (alienLifeForm.defensePower + alienLifeForm.lifeSpan)){
			alienLifeForm.die ();
		} else {
			alienLifeForm.lifeSpan -= (this.attackPower - alienLifeForm.defensePower);
		}
	}

	public void defendItself(){

	}

	public void divide(){
		
	}

	public void mutate(){

	}

	public void die(){
		//~LifeForm();

	}

    public int getRadius()
    {
        return actionRadius;
    }

    public void setRadius(int val)
    {
        actionRadius = val;
    }
}


