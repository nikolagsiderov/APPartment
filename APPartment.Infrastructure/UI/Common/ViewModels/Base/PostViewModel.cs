using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using APPartment.Infrastructure.UI.Web.Tools;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Base
{
    public abstract class PostViewModel : IBaseObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<string> Sections { get; } = new List<string>() { "General Information" };

        [APPUIHint(Templates.Input, Row = 1, Col = 6, Order = 1, Section = "General Information")]
        public virtual string Name { get; set; }

        [APPUIHint(Templates.TextArea, Row = 2, Col = 10, Section = "General Information")]
        public virtual string Details { get; set; }

        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long ID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectTypeID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long? HomeID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long CreatedByID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime CreatedDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long? ModifiedByID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime? ModifiedDate { get; set; }
        #endregion

        public List<string> CommentsHtml { get; set; } = new List<string>();

        public List<ImagePostViewModel> Images { get; set; } = new List<ImagePostViewModel>();

        public List<string> EventsHtml { get; set; } = new List<string>();

        public List<string> ObjectLinksHtml { get; set; } = new List<string>();

        public List<ObjectParticipantPostViewModel> Participants { get; set; } = new List<ObjectParticipantPostViewModel>();

        public List<string> ActionsHtml = new List<string>();

        [JsonIgnore]
        [IgnoreDataMember]
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
                    .ThenBy(x => x.Attribute.Order)
                    .Select(x => new PropertyUIInfo(x.Property)
                    {
                        Template = x.Attribute.Template,
                        Row = x.Attribute.Row,
                        Col = x.Attribute.Col,
                        DisplayName = x.Property.GetCustomAttributes(typeof(DisplayAttribute), true).Any() ? x.Property.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().Name : x.Property.Name,
                        Section = x.Attribute.Section,
                        SelectList = x.Attribute.SelectList
                    })
                    .ToList();

                return orderedProperties;
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
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
