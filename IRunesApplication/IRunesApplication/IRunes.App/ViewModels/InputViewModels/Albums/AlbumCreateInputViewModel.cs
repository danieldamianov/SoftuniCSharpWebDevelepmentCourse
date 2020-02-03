using SIS.MvcFramework.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.ViewModels.InputViewModels.Albums
{
    public class AlbumCreateInputViewModel
    {
        [StringLengthValidationAttribute("Name not between 5 and 9",5,9)]
        public string Name { get; set; }

        [StringLengthValidationAttribute("Cover not between 10 and 150", 10, 150)]
        public string Cover { get; set; }
    }
}
