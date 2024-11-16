import {PropertyBehaviour} from "./PropertyBehaviour";
import {PropertyType} from "./PropertyType";

export interface IExtensionProperty {
    label: string
    name: string
    behaviour: PropertyBehaviour | undefined
    type: PropertyType | undefined
    value:any
}