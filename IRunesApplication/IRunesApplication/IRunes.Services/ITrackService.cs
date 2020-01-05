using IRunes.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface ITrackService
    {
        Track GetTrackById(string id);
    }
}
