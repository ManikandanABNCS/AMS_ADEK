using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ACS.AMS.DAL
{
    public class RequiredBindingMetadataProvider
    {
        public void GetBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context.PropertyAttributes.OfType<RequiredAttribute>().Any())
            {
                context.BindingMetadata.IsBindingRequired = true;
            }
        }
    }
}
