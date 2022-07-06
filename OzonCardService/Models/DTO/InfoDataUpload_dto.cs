using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;

namespace OzonCardService.Models.DTO
{
    public class InfoDataUpload_dto : IInfoData
    {
        public int CountCustomersAll { get; set; }
        public int CountCustomersNew { get; set; }
        public int CountCustomersFail { get; set; }
        public int CountCustomersBalance { get; set; }
        public int CountCustomersCategory { get; set; }
        public int CountCustomersCorporateNutritions { get; set; }

        public bool isCompleted { get; set; }
        public bool isCancel { get; set; }
        public TimeSpan TimeCompleted { get; set; }

        public static InfoDataUpload_dto operator +(InfoDataUpload_dto left, InfoDataUpload_dto right)  
        {
            left.CountCustomersNew += right.CountCustomersNew;
            left.CountCustomersFail += right.CountCustomersFail;
            left.CountCustomersBalance += right.CountCustomersBalance;
            left.CountCustomersCategory += right.CountCustomersCategory;
            left.CountCustomersCorporateNutritions += right.CountCustomersCorporateNutritions;
            return left;
        }
        

    }
}
