using System;

namespace OzonCardService.Models.DTO
{
    public class InfoDataUpload_dto
    {
        public int CountCustomersAll { get; set; }
        public int CountCustomersUpload { get; set; }
        public int CountCustomersFail { get; set; }
        public int CountCustomersBalance { get; set; }
        public int CountCustomersCategory { get; set; }
        public int CountCustomersCorporateNutritions { get; set; }

        public bool isCompleted { get; set; }
        public TimeSpan TimeCompleted { get; set; }

        public static InfoDataUpload_dto operator +(InfoDataUpload_dto left, InfoDataUpload_dto right)  
        {
            var a = new InfoDataUpload_dto();
            a.CountCustomersAll = left.CountCustomersAll;
            a.CountCustomersUpload = left.CountCustomersUpload + right.CountCustomersUpload;
            a.CountCustomersFail = left.CountCustomersFail + right.CountCustomersFail;
            a.CountCustomersBalance = left.CountCustomersBalance + right.CountCustomersBalance;
            a.CountCustomersCategory = left.CountCustomersCategory + right.CountCustomersCategory;
            a.CountCustomersCorporateNutritions = left.CountCustomersCorporateNutritions + right.CountCustomersCorporateNutritions;
            return a;
        }
        public static InfoDataUpload_dto operator -(InfoDataUpload_dto left, InfoDataUpload_dto right)
        {
            var a = new InfoDataUpload_dto();
            a.CountCustomersAll = left.CountCustomersAll;
            a.CountCustomersUpload = left.CountCustomersUpload - right.CountCustomersUpload;
            a.CountCustomersFail = left.CountCustomersFail - right.CountCustomersFail;
            a.CountCustomersBalance = left.CountCustomersBalance - right.CountCustomersBalance;
            a.CountCustomersCategory = left.CountCustomersCategory - right.CountCustomersCategory;
            a.CountCustomersCorporateNutritions = left.CountCustomersCorporateNutritions - right.CountCustomersCorporateNutritions;
            return a;
        }

    }
}
