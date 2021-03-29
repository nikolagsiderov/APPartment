using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.Utilities;
using APPartment.UI.ViewModels.Clingons.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class GridItemViewModel : IBaseObject
    {
        [GridFieldDisplay]
        public long ID { get; set; }

        [GridFieldDisplay]
        public virtual string Name { get; set; }

        [GridFieldDisplay]
        public string Details { get; set; }

        public long ObjectID { get; set; }

        public long ObjectTypeID { get; set; }

        public long CreatedByID { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedByID { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<string> Comments { get; set; }

        public List<ImagePostViewModel> Images { get; set; }

        public List<string> ActionsHtml { get; set; } = new List<string>();

        public IEnumerable<PropertyUIInfo> Properties
        {
            get
            {
                var properties = this
                    .GetType()
                    .GetProperties()
                    .Where(p => p.IsDefined(typeof(GridFieldDisplayAttribute), true))
                    .Select(p => new PropertyUIInfo()
                    {
                        Property = p,
                        DisplayName = p.GetCustomAttributes(typeof(DisplayAttribute), true).Any() ? p.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().Name : p.Name
                    });

                return properties;
            }
        }
    }
}
