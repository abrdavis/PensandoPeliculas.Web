using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensadoPeliculas.Images.Models
{
    public class ImageServiceConfig
    {
        public string ServiceUrl { get; set; }
        public ImageServiceConfig() { }
        public  ImageServiceConfig(string serviceUrl)
        {
            this.ServiceUrl = serviceUrl;
        }
    }
}
