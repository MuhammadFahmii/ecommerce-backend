// ------------------------------------------------------------------------------------
// MockData.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// MockData
    /// </summary>
    public static class MockData
    {
        /// <summary>
        /// GetUserAttibute
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, List<string>>> GetUserAttibute()
        {
            await Task.Delay(0);
            var result = new Dictionary<string, List<string>>();
            var plants = new List<string>();
            List<string> WorkCenters;
            WorkCenters = GetWorkCenters().Select(w => w.Key).ToList();
            result.Add(Constants.WorkCenterFieldName, WorkCenters);
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
        /// GetAssignmentMechanicListResponse
        /// </summary>
        /// <returns></returns>
        public static async Task<List<UserClientIdInfo>> getUserByAttribute()
        {
            await Task.Delay(0);
            return new List<UserClientIdInfo>()
            {
                new UserClientIdInfo()
                {
                    UserId = new Guid("49f140da-707a-459e-4e60-08d708dc37c0"),
                    clientId = "b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0",
                    isCustomer = false
                },
                new UserClientIdInfo()
                {
                    UserId = new Guid("b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0"),
                    clientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    isCustomer = false
                },
                new UserClientIdInfo()
                {
                    UserId = new Guid("c8374c2f-08f7-4156-4e75-08d708dc37c0"),
                    clientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    isCustomer = false
                },
                new UserClientIdInfo()
                {
                    UserId = new Guid("6178aba0-dd0a-4615-4e7e-08d708dc37c0"),
                    clientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    isCustomer = false
                },
                new UserClientIdInfo()
                {
                    UserId = new Guid("647c1e9c-6fe4-4800-25d7-08d771620e86"),
                    clientId = "3ea6f2ac-92aa-4a4c-8725-daa9350be5c8",
                    isCustomer = false
                }
            };
        }

        /// <summary>
        /// GetAuthorizedUser
        /// </summary>
        /// <returns></returns>
        public static List<UserMangementUser> GetListMechanics()
        {
            var result = new List<UserMangementUser>()
            {
                new UserMangementUser
                {
                    UserId = new Guid("49f140da-707a-459e-4e60-08d708dc37c0"),
                    UserName = "80107002@unitedtractors.com",
                    Email = "80107002@unitedtractors.com",
                    FirstName = "Agus",
                    LastName = "LISANTO"
                }, new UserMangementUser
                {
                    UserId = new Guid("b1a48ae8-3c71-4ae4-4e7b-08d708dc37c0"),
                    UserName = "70316028@unitedtractors.com",
                    Email = "inengahsuarma54@gmail.com",
                    FirstName = "I NENGAH",
                    LastName = "I NENGAH"
                }, new UserMangementUser
                {
                    UserId = new Guid("c8374c2f-08f7-4156-4e75-08d708dc37c0"),
                    UserName = "81300006@unitedtractors.com",
                    Email = "81300006@unitedtractors.com",
                    FirstName = "HENDRAWAN",
                    LastName = "HENDRAWAN"
                }, new UserMangementUser
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
        public static List<KeyValuePair<string, string>> GetWorkCenters()
        {
            return new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("M-ADRTP","FMC Adaro Tutupan"),
                new KeyValuePair<string, string>("FM-BDIPM","FMC Bendili PAMA"),
                new KeyValuePair<string, string>("FM-BIUPM","FMC Batukajang PAMA"),
                new KeyValuePair<string, string>("FM-BKJBU","FMC Batukajang BUMA"),
                new KeyValuePair<string, string>("FM-BKJSJ","FMC Batukajang SIMS"),
                new KeyValuePair<string, string>("FM-BNEPM","FMC Bontang East Block Site - KITADIN"),
                new KeyValuePair<string, string>("FM-DMIPM","FMC Damai Pama"),
                new KeyValuePair<string, string>("FM-JBYPM","FMC Jembayan PAMA"),
                new KeyValuePair<string, string>("FM-JKTMB","FMC Jakarta On Road Mayasari Bhakti"),
                new KeyValuePair<string, string>("FM-JKTTJ","FMC Jakarta On Road Trans Jakarta"),
                new KeyValuePair<string, string>("FM-LTIBU","FMC Lati Buma"),
                new KeyValuePair<string, string>("FM-MLWSM","FMC Muaralawa SIMS"),
                new KeyValuePair<string, string>("FM-SRKVL","FMC Soroako"),
                new KeyValuePair<string, string>("FM-TJGBU","FMC Tanjung Buma"),
                new KeyValuePair<string, string>("FM-TJGSI","FMC Tanjung Sis"),
                new KeyValuePair<string, string>("FM-BGLKP","FMC Bengalon KPP"),
                new KeyValuePair<string, string>("FM-BGLKW","FMC Bengalon KWN"),
                new KeyValuePair<string, string>("FM-MTBPM","FMC MTBU Pama"),
                new KeyValuePair<string, string>("FM-TBGKW","FMC Tabang KWN")
            };
        }
    }
}
