﻿using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.Tools;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Base
{
    public abstract class GridItemViewModel : IBaseObject
    {
        [GridFieldDisplay(Order = 1)]
        public long ID { get; set; }

        [GridFieldDisplay(Order = 2)]
        public virtual string Name { get; set; }

        [GridFieldDisplay(Order = 3)]
        public virtual string Details { get; set; }

        public long ObjectID { get; set; }

        public long ObjectTypeID { get; set; }

        public long? HomeID { get; set; }

        public long CreatedByID { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedByID { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public long MainID { get; set; }

        public List<string> ActionsHtml { get; set; } = new List<string>();

        public bool HideItem { get; set; } = false;

        [JsonIgnore]
        [IgnoreDataMember]
        public IEnumerable<PropertyUIInfo> Properties
        {
            get
            {
                var properties = this
                    .GetType()
                    .GetProperties()
                    .Where(p => p.IsDefined(typeof(GridFieldDisplayAttribute), true))
                    .Select(x => new
                    {
                        Property = x,
                        Attribute = (GridFieldDisplayAttribute)Attribute.GetCustomAttribute(x, typeof(GridFieldDisplayAttribute), true)
                    })
                    .OrderBy(x => x.Attribute.Order)
                    .Select(p => new PropertyUIInfo(p.Property)
                    {
                        DisplayName = p.Property.GetCustomAttributes(typeof(DisplayAttribute), true).Any() ? p.Property.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().Name : p.Property.Name
                    });

                return properties;
            }
        }
    }
}
