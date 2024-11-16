import {ExtensionProps} from "./ExtensionProps";
import * as React from "react";


export function ExtensionBoolProperty({property, onChangeValue}:ExtensionProps) {

return(
    <label htmlFor={`checkbox_${property.name}`} className="label-checkbox">
        <input id={`checkbox_${property.name}`} type='checkbox' checked={property.value}
               onChange={e => onChangeValue(property, e.target.value)}
        />
        {property.label}
        <i className="material-icons red-text">
            {property.value ? 'check_box' : 'check_box_outline_blank'}
        </i>
    </label>
)
}