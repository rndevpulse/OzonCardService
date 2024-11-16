import {IDatePickerProps} from "./IDatePickerProps";
import DatePicker, {registerLocale} from "react-datepicker";
import * as React from "react";
import {ru} from "date-fns/locale/ru";

export function DatePickerFrom({value, end,onChange}: IDatePickerProps) {
    registerLocale("ru", ru)
    return(
        <>
            <label htmlFor="formDateFrom" className="form-label">Период с</label>
            <DatePicker
                id="formDateFrom"
                dateFormat='dd MMMM yyyy'
                selected={value}
                selectsStart
                startDate={value}
                endDate={end}
                onChange={date => onChange(date as Date)}
                locale='ru'
                placeholderText="Период с"
            />
        </>
    )
}