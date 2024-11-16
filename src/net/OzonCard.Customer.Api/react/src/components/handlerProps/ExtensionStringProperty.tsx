import {ExtensionProps} from "./ExtensionProps";
import * as React from "react";
import "./index.css";


export function ExtensionStringProperty({property}:ExtensionProps) {
    return (
            <label className='extensionString' htmlFor={`textbox_${property.name}`}>
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
