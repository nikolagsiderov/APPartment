using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Clingons.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class PostViewModel : IBaseObject
    {
        [Required]
        [APPUIHint(Templates.Input, Row = 1, Col = 6, Order = 1)]
        public virtual string Name { get; set; }

        [APPUIHint(Templates.TextArea, Row = 2, Col = 10)]
        public string Details { get; set; }

        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long ID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectTypeID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long CreatedByID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime CreatedDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long? ModifiedByID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime? ModifiedDate { get; set; }
        #endregion

        public List<string> Comments { get; set; }

        public List<ImagePostViewModel> Images { get; set; }

        public string LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdate { get; set; }

        [JsonIgnore]
        public List<PropertyUIInfo> Properties
        {
            get
            {
                var properties = this.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => Attribute.IsDefined(p, typeof(APPUIHintAttribute), true))
                    .Where(p =>
                        p.GetCustomAttributesData()
                            .Where(x => x.AttributeType.Equals(typeof(APPUIHintAttribute))).Any() ? (!p.GetCustomAttributesData()
                            .Where(x => x.AttributeType.Equals(typeof(APPUIHintAttribute)))
                            .FirstOrDefault()
                            .ConstructorArguments
                            .FirstOrDefault()
                            .Value
                            .ToString()
                            .Equals(Templates.Hidden)) : false);

                var orderedProperties = properties
                    .Select(x => new
                    {
                        Property = x,
                        Attribute = (APPUIHintAttribute)Attribute.GetCustomAttribute(x, typeof(APPUIHintAttribute), true)
                    })
                    .Where(x => x.Attribute.Row != 0)
                    .Where(x => x.Attribute.Col != 0)
                    .OrderBy(x => x.Attribute.Row)
                    .ThenBy(x => x.Attribute.Col)
                    .ThenBy(x => x.Attribute.Order)
                    .Select(x => new PropertyUIInfo(x.Property)
                    {
                        Row = x.Attribute.Row,
                        Col = x.Attribute.Col
                    })
                    .ToList();

                return orderedProperties;
            }
        }

        [JsonIgnore]
        public IEnumerable<PropertyInfo> HiddenProperties
        {
            get
            {
                var properties = this.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public);

                var hiddenProperties = properties
                    .Where(p =>
                        p.GetCustomAttributesData()
                            .Where(x => x.AttributeType.Equals(typeof(APPUIHintAttribute))).Any() ? (p.GetCustomAttributesData()
                            .Where(x => x.AttributeType.Equals(typeof(APPUIHintAttribute)))
                            .FirstOrDefault()
                            .ConstructorArguments
                            .FirstOrDefault()
                            .Value
                            .ToString()
                            .Equals(Templates.Hidden)) : false);

                return hiddenProperties;
            }
        }
    }
}
