using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{

	public class Drive_Control_Input_00_Base_CS : MonoBehaviour
	{

		protected Drive_Control_CS controlScript;

        protected int speedStep;


        public virtual void Prepare(Drive_Control_CS controlScript)
		{
			this.controlScript = controlScript;
		}


		public virtual void Drive_Input()
		{
		}

    }

}
