import { ICategoryResponce } from "./ICategoryResponce";
import { ICorporateNutritionResponce } from "./ICorporateNutritionResponce";

export interface IOrganizationResponce {
    Id: string;
    Name: string;
    Categories: ICategoryResponce[];
    CorporateNutritions: ICorporateNutritionResponce[];
}