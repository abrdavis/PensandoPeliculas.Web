using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Configs
{
    public class AzureBlobStorageConfig
    {
        public required string BlobStorageUrl { get; set; }
        public required string DefaultContainer { get; set; }
    }
}
