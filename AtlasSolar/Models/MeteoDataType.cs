using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("MeteoDataTypes", Schema = "public")]
    public class MeteoDataType
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Source")]
        public MeteoDataSource MeteoDataSource { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Source")]
        public int MeteoDataSourceId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Periodicity")]
        public MeteoDataPeriodicity MeteoDataPeriodicity { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Periodicity")]
        public int MeteoDataPeriodicityId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameEN")]
        public string NameEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameKZ")]
        public string NameKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameRU")]
        public string NameRU { get; set; }
        
        [Display(ResourceType = typeof(Resources.Common), Name = "RowNameEN")]
        public string AdditionalEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "RowNameKZ")]
        public string AdditionalKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "RowNameRU")]
        public string AdditionalRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "GroupEN")]
        public string GroupEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "GroupKZ")]
        public string GroupKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "GroupRU")]
        public string GroupRU { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionEN")]
        public string DescriptionEN { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionKZ")]
        public string DescriptionKZ { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionRU")]
        public string DescriptionRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string CodeName
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
                                return Code + " (" + NameEN + ")";
                            }
                            if (cookie.Value == "kk")
                            {
                                return Code + " (" + NameKZ + ")";
                            }
                            if (cookie.Value == "ru")
                            {
                                return Code + " (" + NameRU + ")";
                            }
                        }
                        else
                        {
                            return Code + " (" + NameEN + ")";
                        }
                    }
                    else
                    {
                        return Code + " (" + NameEN + ")";
                    }
                }
                else
                {
                    return Code + " (" + NameEN + ")";
                }
                return Code + " (" + NameEN + ")";
            }
        }

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

        [Display(ResourceType = typeof(Resources.Common), Name = "MeteoDataGroup")]
        public string Group
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
                                return GroupEN;
                            }
                            if (cookie.Value == "kk")
                            {
                                return GroupKZ;
                            }
                            if (cookie.Value == "ru")
                            {
                                return GroupRU;
                            }
                        }
                        else
                        {
                            return GroupEN;
                        }
                    }
                    else
                    {
                        return GroupEN;
                    }
                }
                else
                {
                    return GroupEN;
                }
                return GroupEN;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string NameFull
        {
            get
            {
                string r = "";
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                if(!string.IsNullOrEmpty(GroupEN))
                                {
                                    r += GroupEN;
                                }
                                if(!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameEN}";
                                if (!string.IsNullOrEmpty(AdditionalEN))
                                {
                                    r += $" {AdditionalEN}";
                                }
                                r += $" ({Code})";
                                return r;
                            }
                            if (cookie.Value == "kk")
                            {
                                if (!string.IsNullOrEmpty(GroupKZ))
                                {
                                    r += GroupKZ;
                                }
                                if (!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameKZ}";
                                if (!string.IsNullOrEmpty(AdditionalKZ))
                                {
                                    r += $" {AdditionalKZ}";
                                }
                                r += $" ({Code})";
                                return r;
                            }
                            if (cookie.Value == "ru")
                            {
                                if (!string.IsNullOrEmpty(GroupRU))
                                {
                                    r += GroupRU;
                                }
                                if (!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameRU}";
                                if (!string.IsNullOrEmpty(AdditionalRU))
                                {
                                    r += $" {AdditionalRU}";
                                }
                                r += $" ({Code})";
                                return r;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(GroupEN))
                            {
                                r += GroupEN;
                            }
                            if (!string.IsNullOrEmpty(r))
                            {
                                r += ": ";
                            }
                            r += $"{NameEN}";
                            if (!string.IsNullOrEmpty(AdditionalEN))
                            {
                                r += $" {AdditionalEN}";
                            }
                            r += $" ({Code})";
                            return r;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(GroupEN))
                        {
                            r += GroupEN;
                        }
                        if (!string.IsNullOrEmpty(r))
                        {
                            r += ": ";
                        }
                        r += $"{NameEN}";
                        if (!string.IsNullOrEmpty(AdditionalEN))
                        {
                            r += $" {AdditionalEN}";
                        }
                        r += $" ({Code})";
                        return r;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GroupEN))
                    {
                        r += GroupEN;
                    }
                    if (!string.IsNullOrEmpty(r))
                    {
                        r += ": ";
                    }
                    r += $"{NameEN}";
                    if (!string.IsNullOrEmpty(AdditionalEN))
                    {
                        r += $" {AdditionalEN}";
                    }
                    r += $" ({Code})";
                    return r;
                }
                if (!string.IsNullOrEmpty(GroupEN))
                {
                    r += GroupEN;
                }
                if (!string.IsNullOrEmpty(r))
                {
                    r += ": ";
                }
                r += $"{NameEN}";
                if (!string.IsNullOrEmpty(AdditionalEN))
                {
                    r += $" {AdditionalEN}";
                }
                r += $" ({Code})";
                return r;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string NameGroupAdditional
        {
            get
            {
                string r = "";
                if (HttpContext.Current != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                    if (cookie != null)
                    {
                        if (cookie.Value != null)
                        {
                            if (cookie.Value == "en")
                            {
                                if (!string.IsNullOrEmpty(GroupEN))
                                {
                                    r += GroupEN;
                                }
                                if (!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameEN}";
                                if (!string.IsNullOrEmpty(AdditionalEN))
                                {
                                    r += $" {AdditionalEN}";
                                }
                                return r;
                            }
                            if (cookie.Value == "kk")
                            {
                                if (!string.IsNullOrEmpty(GroupKZ))
                                {
                                    r += GroupKZ;
                                }
                                if (!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameKZ}";
                                if (!string.IsNullOrEmpty(AdditionalKZ))
                                {
                                    r += $" {AdditionalKZ}";
                                }
                                return r;
                            }
                            if (cookie.Value == "ru")
                            {
                                if (!string.IsNullOrEmpty(GroupRU))
                                {
                                    r += GroupRU;
                                }
                                if (!string.IsNullOrEmpty(r))
                                {
                                    r += ": ";
                                }
                                r += $"{NameRU}";
                                if (!string.IsNullOrEmpty(AdditionalRU))
                                {
                                    r += $" {AdditionalRU}";
                                }
                                return r;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(GroupEN))
                            {
                                r += GroupEN;
                            }
                            if (!string.IsNullOrEmpty(r))
                            {
                                r += ": ";
                            }
                            r += $"{NameEN}";
                            if (!string.IsNullOrEmpty(AdditionalEN))
                            {
                                r += $" {AdditionalEN}";
                            }
                            return r;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(GroupEN))
                        {
                            r += GroupEN;
                        }
                        if (!string.IsNullOrEmpty(r))
                        {
                            r += ": ";
                        }
                        r += $"{NameEN}";
                        if (!string.IsNullOrEmpty(AdditionalEN))
                        {
                            r += $" {AdditionalEN}";
                        }
                        return r;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GroupEN))
                    {
                        r += GroupEN;
                    }
                    if (!string.IsNullOrEmpty(r))
                    {
                        r += ": ";
                    }
                    r += $"{NameEN}";
                    if (!string.IsNullOrEmpty(AdditionalEN))
                    {
                        r += $" {AdditionalEN}";
                    }
                    return r;
                }
                if (!string.IsNullOrEmpty(GroupEN))
                {
                    r += GroupEN;
                }
                if (!string.IsNullOrEmpty(r))
                {
                    r += ": ";
                }
                r += $"{NameEN}";
                if (!string.IsNullOrEmpty(AdditionalEN))
                {
                    r += $" {AdditionalEN}";
                }
                return r;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Code")]
        public string CodeAdditional
        {
            get
            {
                return Code + (string.IsNullOrEmpty(AdditionalEN) ? "" : $" ({AdditionalEN})");
            }
        }
    }
}