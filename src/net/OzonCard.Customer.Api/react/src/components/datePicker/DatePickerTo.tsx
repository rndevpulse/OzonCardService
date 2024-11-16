import {IDatePickerProps} from "./IDatePickerProps";
import DatePicker, {registerLocale} from "react-datepicker";
import * as React from "react";
import {ru} from "date-fns/locale/ru";

export function DatePickerTo({value, start, onChange}: IDatePickerProps) {
    registerLocale("ru", ru)
    return(
        <>
            <label htmlFor="formDateTo" className="form-label">по</label>
            <DatePicker
                id='formDateTo'
                dateFormat='dd MMMM yyyy'
                selected={value}
                selectsEnd
                startDate={start}
                // endDate={value}
                minDate={start}
                onChange={date => onChange(date as Date)}
                locale='ru'
                placeholderText="Период по"
            />
        </>
    )
}