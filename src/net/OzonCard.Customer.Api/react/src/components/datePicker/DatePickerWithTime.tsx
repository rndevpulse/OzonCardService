import {IDatePickerProps} from "./IDatePickerProps";
import DatePicker, {registerLocale} from "react-datepicker";
import {ru} from "date-fns/locale/ru";
import * as React from "react";

export function DatePickerWithTime({value, onChange}: IDatePickerProps) {
    registerLocale("ru", ru)
    return(
        <>
            <label htmlFor="datePickerWithTime" className="form-label">Запланированное время</label>
            <DatePicker
                id='datePickerWithTime'
                timeCaption='Время'
                dateFormat='dd MMMM yyyy в HH:mm'
                showTimeSelect
                timeFormat='HH:mm'
                timeIntervals={30}
                selected={value}
                onChange={date => onChange(date as Date)}
                locale='ru'
                placeholderText="Запланированное время"
            />
        </>
)
}