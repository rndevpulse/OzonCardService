import {IProgress} from "./IProgress";


export interface IReportsTasksProgress extends IProgress {
    Description :string
    Progress: number
}