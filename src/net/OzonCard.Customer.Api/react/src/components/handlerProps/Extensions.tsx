import {IExtensionProperty, IHandlerInfo, PropertyType} from "../../models/request";
import {IOrganization} from "../../models/org";
import {ExtensionDateTimeProperty} from "./ExtensionDateTimeProperty";
import React from "react";
import {ExtensionBoolProperty} from "./ExtensionBoolProperty";
import {ExtensionStringProperty} from "./ExtensionStringProperty";
import {ExtensionGuidProperty} from "./ExtensionGuidProperty";
import "./index.css"


interface HandlerProps {
    handler:IHandlerInfo
    props:IExtensionProperty[]
    organization?:IOrganization
    onChangeValue:(property:IExtensionProperty, value:any) => void
}


export function Extensions({handler, props, organization, onChangeValue}:HandlerProps) {

    const castViewProp = (property:IExtensionProperty) => {

        switch (property.type) {
            case PropertyType.ExtensionBoolProperty:
                return <ExtensionBoolProperty property={property} onChangeValue={onChangeValue}/>

            case PropertyType.ExtensionStringProperty:
                return <ExtensionStringProperty property={property} onChangeValue={onChangeValue}/>

            case PropertyType.ExtensionDateTimeProperty:
                return <ExtensionDateTimeProperty property={property} onChangeValue={onChangeValue}/>

            case PropertyType.ExtensionGuidProperty:
                return <ExtensionGuidProperty
                    property={property}
                    organization={organization}
                    onChangeValue={onChangeValue}
                />

            default: return <></>
        }
    }

    return (
        <ul className=''>
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