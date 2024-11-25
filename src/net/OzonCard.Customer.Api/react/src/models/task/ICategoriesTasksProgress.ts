import {IProgress} from "./IProgress";

export interface ICategoriesTasksProgress  extends  IProgress{
    Log: string
    All: number
    Processed: number
}