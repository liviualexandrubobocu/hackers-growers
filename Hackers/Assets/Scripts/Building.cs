using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
	private double shieldDefense { get; set;}
	private double resourceCost { get; set; }
	private double lifeSpan { get; set; }
	private double increaseLifeSpanRatio { get; set; }

	public Building(
		double shieldDefense,
		double resourceCost,
		double lifeSpan,
		double increaseLifeSpanRatio
	){
		this.shieldDefense = shieldDefense;
		this.resourceCost = resourceCost;
		this.lifeSpan = lifeSpan;
		this.increaseLifeSpanRatio = increaseLifeSpanRatio;
	}

	public void increaseLife(LifeForm alienLifeForm){
		//alienLifeForm.lifeSpan += ((this.increaseLifeSpanRatio * alienLifeForm.lifeSpan) / 100);
	}

	public void destroy(){
		//~Building();
	}
}


