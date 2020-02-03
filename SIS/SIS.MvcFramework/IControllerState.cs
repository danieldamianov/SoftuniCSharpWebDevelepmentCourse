using SIS.MvcFramework.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IControllerState
    {
        ModelStateDictionary ModelStateDictionary { get; set; }

        void Reset();

        void InitializeInnerState(Controller controller);

        void SetStateOfController(Controller controller);
    }
}
