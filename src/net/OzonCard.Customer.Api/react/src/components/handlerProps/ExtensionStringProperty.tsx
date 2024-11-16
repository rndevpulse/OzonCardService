import {ExtensionProps} from "./ExtensionProps";
import * as React from "react";



export function ExtensionStringProperty({property, onChangeValue}:ExtensionProps) {
    return (
            <label className='extensionString' htmlFor={`textbox_${property.name}`}>
                {property.label}
                <input
                    id={`textbox_${property.name}`}
                    onChange={e => onChangeValue(property, e.target.value)}
                    value={property.value}
                    type='text'
                    placeholder={property.label}
                />
            </label>
    )
}
