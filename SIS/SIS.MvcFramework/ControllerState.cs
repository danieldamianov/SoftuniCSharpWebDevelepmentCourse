using System;
using System.Collections.Generic;
using System.Text;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework
{
    public class ControllerState : IControllerState
    {
        public ModelStateDictionary ModelStateDictionary { get; set; }

        public ControllerState()
        {
            this.Reset();
        }

        public void InitializeInnerState(Controller controller)
        {
            this.ModelStateDictionary = controller.ModelState;
        }

        public void Reset()
        {
            this.ModelStateDictionary = new ModelStateDictionary();
        }

        public void SetStateOfController(Controller controller)
        {
            controller.ModelState = this.ModelStateDictionary;
        }
    }
}
