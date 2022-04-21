import { ICategoryResponse } from "./ICategoryResponse";
import { ICorporateNutritionResponse } from "./ICorporateNutritionResponse";

export interface IOrganizationResponse {
    id: string;
    name: string;
    categories: ICategoryResponse[];
    corporateNutritions: ICorporateNutritionResponse[];
}