// ------------------------------------------------------------------------------------
// GeneralParameterInfo.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// GeneralParameterInfo
    /// </summary>
    public class GeneralParameterInfo
    {
        /// <summary>
        /// GeneralParameterId
        /// </summary>
        /// <value></value>
        public Guid GeneralParameterId { get; set; }

        /// <summary>
        /// GeneralParameterCode
        /// </summary>
        /// <value></value>
        public string GeneralParameterCode { get; set; }

        /// <summary>
        /// GeneralParameterValue1
        /// </summary>
        /// <value></value>
        public string GeneralParameterValue1 { get; set; }

        /// <summary>
        /// GeneralParameterValue2
        /// </summary>
        /// <value></value>
        public string GeneralParameterValue2 { get; set; }

        /// <summary>
        /// GeneralParameterValue3
        /// </summary>
        /// <value></value>
        public string GeneralParameterValue3 { get; set; }

        /// <summary>
        /// GeneralParameterValue4
        /// </summary>
        /// <value></value>
        public string GeneralParameterValue4 { get; set; }

        /// <summary>
        /// GeneralParameterValue5
        /// </summary>
        /// <value></value>
        public string GeneralParameterValue5 { get; set; }
    }

    /// <summary>
    /// UserDeviceInfo
    /// </summary>
    public class UserDeviceInfo
    {
        /// <summary>
        /// DeviceId
        /// </summary>
        /// <value></value>
        public string DeviceId { get; set; }

        /// <summary>
        /// AppVersion
        /// </summary>
        /// <value></value>
        public string AppVersion { get; set; }

        /// <summary>
        /// OsVersion
        /// </summary>
        /// <value></value>
        public string OsVersion { get; set; }
    }

    /// <summary>
    /// UserEmailInfo
    /// </summary>
    public class UserEmailInfo
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        public string Email { get; set; }
        
        /// <summary>
        /// FirstName
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        /// <value></value>
        public string LastName { get; set; }
    }

    /// <summary>
    /// UserClientIdInfo
    /// </summary>   
    public class UserClientIdInfo
    {
        /// <summary>
        /// UserClientIdInfo
        /// </summary>
        /// <value></value>
        public Guid UserId { get; set; }

        /// <summary>
        /// clientId
        /// </summary>
        /// <value></value>
        public string clientId { get; set; }

        /// <summary>
        /// isCustomer
        /// </summary>
        /// <value></value>
        public bool isCustomer { get; set; }
    }
    
    /// <summary>
    /// UserMangementUser
    /// </summary>
    public class UserMangementUser
    {
        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        public Guid UserId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        /// <value></value>
        public string LastName { get; set; }

        /// <summary>
        /// CustomerCode
        /// </summary>
        /// <value></value>
        public string CustomerCode { get; set; }

        /// <summary>
        /// ContactNumber
        /// </summary>
        /// <value></value>
        public string ContactNumber { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>
        /// <value></value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        /// <value></value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// UpdatedBy
        /// </summary>
        /// <value></value>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedDate
        /// </summary>
        /// <value></value>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// ForgotPasswordToken
        /// </summary>
        /// <value></value>
        public string ForgotPasswordToken { get; set; }

        /// <summary>
        /// ForgotPasswordTokenExpiryDate
        /// </summary>
        /// <value></value>
        public string ForgotPasswordTokenExpiryDate { get; set; }
    }
    
    /// <summary>
    /// AuthorizedUser
    /// </summary>
    public class AuthorizedUser
    {
        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// UserName
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        /// <value></value>
        public string UserFullName { get; set; }

        /// <summary>
        /// CustomerCode
        /// </summary>
        /// <value></value>

        public string CustomerCode { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        /// <value></value>
        public string ClientId { get; set; }
        
        /// <summary>
        /// RoleLevel
        /// </summary>
        /// <value></value>
        public int RoleLevel { get; set; }
    }
}
