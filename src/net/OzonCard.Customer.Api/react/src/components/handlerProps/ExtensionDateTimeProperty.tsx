import {ExtensionProps} from "./ExtensionProps";
import {PropertyBehaviour} from "../../models/request";
import {DatePickerFrom, DatePickerTo} from "../datePicker";
import DatePicker from "react-datepicker";
import * as React from "react";


export function ExtensionDateTimeProperty({property, onChangeValue}:ExtensionProps){

    const castBehaviour = () => {
        switch (property.behaviour){
            case PropertyBehaviour.DateStart:
                return <DatePickerFrom
                    value={property.value}
                    onChange={data => onChangeValue(property, data)}
                />
            case PropertyBehaviour.DateEnd:
                return <DatePickerTo
                    value={property.value}
                    onChange={data => onChangeValue(property, data)}
                />
            default:
                return <div>
                    <label htmlFor={`formDate_${property.name}`} className="form-label">{property.label}</label>
                    <DatePicker
                        id={`formDate_${property.name}`}
                        dateFormat='dd MMMM yyyy'
                        selected={property.value}
                        onChange={data => onChangeValue(property, data)}
                        locale='ru'
                        placeholderText={property.label}
                    />
                </div>
        }
    }

    return(<>{castBehaviour()}</>)

}