// ------------------------------------------------------------------------------------
// MsTeamTemplate.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// MsTeamTemplate
    /// </summary>
    public class MsTeamTemplate
    {
        /// <summary>
        /// summary
        /// </summary>
        public string summary { get; set;} = Constants.MsTeamsSummaryError;
        /// <summary>
        /// themeColor
        /// </summary>
        public string themeColor { get; set;} = Constants.MsTeamsThemeColorError;

        /// <summary>
        /// sections
        /// </summary>
        public List<Section> sections {get; set;}
    }

    /// <summary>
    /// Section
    /// </summary>
    public class Section
    {
        /// <summary>
        /// activityTitle
        /// </summary>
        /// <value></value>
        public string activityTitle { get; set;}

        /// <summary>
        /// activitySubtitle
        /// </summary>
        /// <value></value>
        public string activitySubtitle { get; set;}

        /// <summary>
        /// activityImage
        /// </summary>
        /// <value></value>
        public string activityImage {get; set;}

        /// <summary>
        /// Facts
        /// </summary>
        /// <value></value>
        public List<Fact> Facts {get; set;}
    }

    /// <summary>
    /// Fact
    /// </summary>
    public class Fact
    {
        /// <summary>
        /// name
        /// </summary>
        /// <value></value>
        public string name { get; set;}

        /// <summary>
        /// value
        /// </summary>
        /// <value></value>
        public string value { get; set;}
    }
    
    /// <summary>
    /// CacheMSTeam
    /// </summary>
    public class CacheMSTeam
    {
        /// <summary>
        /// Counter
        /// </summary>
        public int Counter { get; set; } = 100;
        
        /// <summary>
        /// Hours
        /// </summary>
        public double Hours => 24;
        
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
