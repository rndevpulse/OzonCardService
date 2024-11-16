import {IExtensionProperty} from "./IExtensionProperty";

export interface IRequestModel {
    schedule:Date
    timeOffset:number
    properties:IExtensionProperty[]
}