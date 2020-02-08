using SIS.MvcFramework.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.ViewModels.InputViewModels.Tracks
{
    public class TrackCreateInputViewModel
    {
        public string AlbumId { get; set; }
        
        [StringLengthValidation("Not proper length",5,10)]
        public string Name { get; set; }

        [StringLengthValidation("Not proper length", 5, 10)]
        public string Link { get; set; }
      
        [RangeValidation(10,20,"Price not between 10 and 20")]
        public decimal Price { get; set; }
    }
}
