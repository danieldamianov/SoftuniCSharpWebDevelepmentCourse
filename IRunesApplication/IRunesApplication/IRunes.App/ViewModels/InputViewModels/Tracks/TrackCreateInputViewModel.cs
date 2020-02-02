using SIS.MvcFramework.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.ViewModels.InputViewModels.Tracks
{
    public class TrackCreateInputViewModel
    {
        public string AlbumId { get; set; }
        
        [StringLengthValidationAttribute("Not proper length",5,10)]
        public string Name { get; set; }
    
        public string Link { get; set; }
      
        public decimal Price { get; set; }
    }
}
