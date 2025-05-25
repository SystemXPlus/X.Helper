using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Enums
{
    public enum HttpContentType
    {
        /// <summary>
        /// text/html
        /// </summary>
        [Display(Name ="text/html")]
        TEXT_HTML,
        /// <summary>
        /// text/plan
        /// </summary>
        [Display(Name = "text/plain")]
        TEXT_PLAN,
        /// <summary>
        /// text/json
        /// </summary>
        [Display(Name = "text/json")]
        TEXT_JSON,
        /// <summary>
        /// text/css
        /// </summary>
        //[Display(Name = "text/css")]
        //TEXT_CSS,
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        [Display(Name = "application/x-www-form-urlencoded")]
        APPLICATION_X_WWW_FORM_URLENCODED,
        /// <summary>
        /// application/json
        /// </summary>
        [Display(Name = "application/json")]
        APPLICATION_JSON,
        /// <summary>
        /// application/xml
        /// </summary>
        //[Display(Name = "application/xml")]
        //APPLICATION_XML,
        /// <summary>
        /// application/javascript
        /// </summary>
        //[Display(Name = "application/javascript")]
        //APPLICATION_JAVASCRIPT,
        /// <summary>
        /// application/octet-stream
        /// </summary>
        [Display(Name = "application/octet-stream")]
        APPLICATION_OCTET_STREAM,
        /// <summary>
        /// multipart/form-data
        /// </summary>
        [Display(Name = "multipart/form-data")]
        MULTIPART_FORM_DATA,
    }
}
