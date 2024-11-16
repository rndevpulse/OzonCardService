import {ExtensionProps} from "./ExtensionProps";
import * as React from "react";


export function ExtensionStringProperty({property}:ExtensionProps) {
    return (
            <label id={`ex_${property.name}`} htmlFor={`textbox_${property.name}`}>
                {property.label}
                <input
                    id={`textbox_${property.name}`}
                    onChange={e => property.value = e.target.value}
                    value={property.value}
                    type='text'
                    placeholder={property.label}
                />
            </label>
    )
}
