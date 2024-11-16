import {IExtensionProperty, IHandlerInfo, PropertyType} from "../../models/request";
import {IOrganization} from "../../models/org";
import {ExtensionDateTimeProperty} from "./ExtensionDateTimeProperty";
import React from "react";
import {ExtensionBoolProperty} from "./ExtensionBoolProperty";
import {ExtensionStringProperty} from "./ExtensionStringProperty";
import {ExtensionGuidProperty} from "./ExtensionGuidProperty";

interface HandlerProps {
    handler:IHandlerInfo
    props:IExtensionProperty[]
    organization?:IOrganization
}


export function Extensions({handler, props, organization}:HandlerProps) {

    const castViewProp = (property:IExtensionProperty) => {

        switch (property.type) {
            case PropertyType.ExtensionBoolProperty:
                return <ExtensionBoolProperty property={property}/>

            case PropertyType.ExtensionStringProperty:
                return <ExtensionStringProperty property={property}/>

            case PropertyType.ExtensionDateTimeProperty:
                return <ExtensionDateTimeProperty property={property}/>

            case PropertyType.ExtensionGuidProperty:
                return <ExtensionGuidProperty property={property} organization={organization}/>

            default: return <></>
        }
    }

    return (
        <ul>
            {props && props.map((prop,i) => {
                return (
                    <li key={`${prop.type}_${i}`}>
                        {castViewProp(prop)}
                    </li>
                )
            })}
        </ul>
    )
}