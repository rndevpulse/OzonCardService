import { ICategoryResponse } from "./ICategoryResponcse";
import { ICorporateNutritionResponse } from "./ICorporateNutritionResponse";

export interface IOrganizationResponse {
    Id: string;
    Name: string;
    Categories: ICategoryResponse[];
    CorporateNutritions: ICorporateNutritionResponse[];
}