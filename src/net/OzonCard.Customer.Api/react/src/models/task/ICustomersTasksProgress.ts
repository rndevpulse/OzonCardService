import {IProgress} from "./IProgress";


export interface ICustomersTasksProgress extends IProgress {
    CountAll :number
    CountNew: number
    CountFail: number
    CountBalance: number
    CountCategory: number
    CountProgram: number
}