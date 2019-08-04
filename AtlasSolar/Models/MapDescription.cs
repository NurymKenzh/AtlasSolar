using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("MapDescriptions", Schema = "public")]
    public class MapDescription
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Code")]
        public string LayersCode { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionForUser")]
        public string DescriptionForUser { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameEN")]
        public string NameEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameKZ")]
        public string NameKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameRU")]
        public string NameRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionEN")]
        public string DescriptionEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionKZ")]
        public string DescriptionKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionRU")]
        public string DescriptionRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AppointmentEN")]
        public string AppointmentEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AppointmentKZ")]
        public string AppointmentKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AppointmentRU")]
        public string AppointmentRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "SourceEN")]
        public string SourceEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "SourceKZ")]
        public string SourceKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "SourceRU")]
        public string SourceRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AdditionalEN")]
        public string AdditionalEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AdditionalKZ")]
        public string AdditionalKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "AdditionalRU")]
        public string AdditionalRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ResolutionEN")]
        public string ResolutionEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ResolutionKZ")]
        public string ResolutionKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ResolutionRU")]
        public string ResolutionRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string Name
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return NameEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return NameKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return NameRU;
                            }
                        }
                        else
                        {
                            return NameEN;
                        }
                    }
                    else
                    {
                        return NameEN;
                    }
                }
                else
                {
                    return NameEN;
                }
                return NameEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Appointment")]
        public string Appointment
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return AppointmentEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return AppointmentKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return AppointmentRU;
                            }
                        }
                        else
                        {
                            return AppointmentEN;
                        }
                    }
                    else
                    {
                        return AppointmentEN;
                    }
                }
                else
                {
                    return AppointmentEN;
                }
                return AppointmentEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Source")]
        public string Source
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return SourceEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return SourceKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return SourceRU;
                            }
                        }
                        else
                        {
                            return SourceEN;
                        }
                    }
                    else
                    {
                        return SourceEN;
                    }
                }
                else
                {
                    return SourceEN;
                }
                return SourceEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Additional")]
        public string Additional
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return AdditionalEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return AdditionalKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return AdditionalRU;
                            }
                        }
                        else
                        {
                            return AdditionalEN;
                        }
                    }
                    else
                    {
                        return AdditionalEN;
                    }
                }
                else
                {
                    return AdditionalEN;
                }
                return AdditionalEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Resolution")]
        public string Resolution
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return ResolutionEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return ResolutionKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return ResolutionRU;
                            }
                        }
                        else
                        {
                            return ResolutionEN;
                        }
                    }
                    else
                    {
                        return ResolutionEN;
                    }
                }
                else
                {
                    return ResolutionEN;
                }
                return ResolutionEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Description")]
        public string Description
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                return DescriptionEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return DescriptionKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return DescriptionRU;
                            }
                        }
                        else
                        {
                            return DescriptionEN;
                        }
                    }
                    else
                    {
                        return DescriptionEN;
                    }
                }
                else
                {
                    return DescriptionEN;
                }
                return DescriptionEN;
            }
        }
    }
}