import {IExtensionProperty} from "../../models/request";
import {IOrganization} from "../../models/org";

export interface ExtensionProps{
    property:IExtensionProperty
    organization?:IOrganization
}