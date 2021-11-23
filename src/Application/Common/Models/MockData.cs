// ------------------------------------------------------------------------------------
// MockData.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// MockData
    /// </summary>
    public static class MockData
    {
        /// <summary>
        /// GetUserAttribute
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<string>>? GetUserAttribute()
        {
            var result = new Dictionary<string, List<string>>();
            var plants = new List<string>();
            var workCenters = GetWorkCenters().Select(w => w.Key).ToList();
            result.Add(Constants.WorkCenterFieldName, workCenters);
            plants.Add(Constants.All);
            result.Add(Constants.PlantFieldName, plants);
            return result;
        }

        /// <summary>
        /// GetAuthorizedUser
        /// </summary>
        /// <returns></returns>
        public static AuthorizedUser GetAuthorizedUser()
        {
            return new AuthorizedUser()
            {
                UserId = Guid.NewGuid(),
                UserName = Constants.SystemEmail,
                UserFullName = Constants.SystemName,
                CustomerCode = Constants.SystemName,
                ClientId = Constants.SystemClientId,
                RoleLevel = 0
            };
        }

        /// <summary>
        /// GetUserByAttributeAsync
        /// </summary>
        /// <returns></returns>
        public static List<UserClientIdInfo> GetUserByAttribute()
        {
            return new List<UserClientIdInfo>()
            {
                new()
                {
                    UserId = new Guid("49f140da-707a-459e-4e60-08d708dc37c0"),
                    ClientId = "b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0",
                    IsCustomer = false
                },
                new()
                {
                    UserId = new Guid("b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0"),
                    ClientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    IsCustomer = false
                },
                new()
                {
                    UserId = new Guid("c8374c2f-08f7-4156-4e75-08d708dc37c0"),
                    ClientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    IsCustomer = false
                },
                new()
                {
                    UserId = new Guid("6178aba0-dd0a-4615-4e7e-08d708dc37c0"),
                    ClientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    IsCustomer = false
                },
                new()
                {
                    UserId = new Guid("647c1e9c-6fe4-4800-25d7-08d771620e86"),
                    ClientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    IsCustomer = false
                }
            };
        }

        /// <summary>
        /// GetAuthorizedUser
        /// </summary>
        /// <returns></returns>
        public static List<UserManagementUser> GetListMechanics()
        {
            var result = new List<UserManagementUser>()
            {
                new()
                {
                    UserId = new Guid("49f140da-707a-459e-4e60-08d708dc37c0"),
                    UserName = "80107002@unitedtractors.com",
                    Email = "80107002@unitedtractors.com",
                    FirstName = "Agus",
                    LastName = "LISANTO"
                },
                new()
                {
                    UserId = new Guid("b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0"),
                    UserName = "70316028@unitedtractors.com",
                    Email = "inengahsuarma54@gmail.com",
                    FirstName = "I NENGAH",
                    LastName = "I NENGAH"
                },
                new()
                {
                    UserId = new Guid("c8374c2f-08f7-4156-4e75-08d708dc37c0"),
                    UserName = "81300006@unitedtractors.com",
                    Email = "81300006@unitedtractors.com",
                    FirstName = "HENDRAWAN",
                    LastName = "HENDRAWAN"
                },
                new()
                {
                    UserId = new Guid("6178aba0-dd0a-4615-4e7e-08d708dc37c0"),
                    UserName = "80101070@unitedtractors.com",
                    Email = "saiful.ongisnade2016@gmail.com",
                    FirstName = "SAIFUL",
                    LastName = "HADI"
                }
            };

            return result;
        }

        /// <summary>
        /// RandomNum
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNum(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// GetWorkCenters
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetWorkCenters()
        {
            return new List<KeyValuePair<string, string>>
            {
                new("M-ADRTP", "FMC Adaro Tutupan"),
                new("FM-BDIPM", "FMC Bendili PAMA"),
                new("FM-BIUPM", "FMC Batukajang PAMA"),
                new("FM-BKJBU", "FMC Batukajang BUMA"),
                new("FM-BKJSJ", "FMC Batukajang SIMS"),
                new("FM-BNEPM", "FMC Bontang East Block Site - KITADIN"),
                new("FM-DMIPM", "FMC Damai Pama"),
                new("FM-JBYPM", "FMC Jembayan PAMA"),
                new("FM-JKTMB", "FMC Jakarta On Road Mayasari Bhakti"),
                new("FM-JKTTJ", "FMC Jakarta On Road Trans Jakarta"),
                new("FM-LTIBU", "FMC Lati Buma"),
                new("FM-MLWSM", "FMC Muaralawa SIMS"),
                new("FM-SRKVL", "FMC Soroako"),
                new("FM-TJGBU", "FMC Tanjung Buma"),
                new("FM-TJGSI", "FMC Tanjung Sis"),
                new("FM-BGLKP", "FMC Bengalon KPP"),
                new("FM-BGLKW", "FMC Bengalon KWN"),
                new("FM-MTBPM", "FMC MTBU Pama"),
                new("FM-TBGKW", "FMC Tabang KWN")
            };
        }
    }
}
