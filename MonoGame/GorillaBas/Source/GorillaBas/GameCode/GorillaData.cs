using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GorillaBas.GameCode
{
	public class GorillaData
	{
		public string Name { get; set; }
		public Rectangle Area { get; set; }
		public float Angle { get; set; }
		public int DirectionModifier { get; set; }
		public float Velocity { get; set; }
		public int Score { get; set; }

		public GorillaData()
		{
		}

		public GorillaData(string name, Rectangle area, float angle, int directionModifier, float velocity)
		{
			Name = name;
			Reset(area, angle, directionModifier, velocity);
		}

		public void Reset(Rectangle area, float angle, int directionModifier, float velocity)
		{
			Area = area;
			Angle = angle;
			DirectionModifier = directionModifier;
			Velocity = velocity;
		}
	}
}
