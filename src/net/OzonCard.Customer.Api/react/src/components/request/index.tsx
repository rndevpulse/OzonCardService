import {IOrganization} from "../../models/org";
import * as React from "react";
import {useToast} from "../toast";
import {useEffect, useRef, useState} from "react";
import {IExtensionProperty, IHandlerInfo} from "../../models/request";
import RequestService from "../../services/RequestService";
import Select from "react-select";
import {DatePickerWithTime} from "../datePicker";
import {Extensions} from "../handlerProps";
import {makeAutoObservable} from "mobx";

interface IRequestHandlersProps{
    organization: IOrganization
}

export function RequestHandlers({organization}:IRequestHandlersProps){
    const toast = useToast();
    const [lastOrganization, setLastOrganization] = useState<string>('');
    const selectInputRef = useRef()

    const [handlers, setHandlers] = useState<IHandlerInfo[]>();
    const [handler, setHandler] = useState<IHandlerInfo>();
    const [handlerProps, setHandlerProps] = useState<IExtensionProperty[]>([]);

    const [schedule, setSchedule] = useState<Date>(new Date());
    //-(new Date().getTimezoneOffset())

    const onClear = () => {
        (selectInputRef.current as any).clearValue();
    };
    async function selectedHandler(handler:IHandlerInfo|undefined){
        if (handler !== undefined){
            setHandler(handler)
            // toast.show(handler.key)
            // RequestService.getHandlerProps(handler.key)
            //     .then(result =>
            //         setHandlerProps(result.data))
            // makeAutoObservable(handlerProps)
            let result = await RequestService.getHandlerProps(handler.key)
            setHandlerProps(result.data)
        }
    }
    useEffect(() => {
        if (lastOrganization !== organization?.id)
        {
            setLastOrganization(organization?.id ?? '')
            // onClear();
        }
    });
    useEffect( () => {
        RequestService.getHandlers()
            .then(result=>
            {
                setHandlers(result.data)
                // toast.show("Запросы загружены")
            })
    }, []);
    return(
        <>
            <Select
                id='handlers'
                onChange={values => selectedHandler(values as IHandlerInfo | undefined)}
                value={handler}
                options={handlers}
                getOptionLabel={option => option.name}
                getOptionValue={option => option?.key ?? "1"}
                placeholder='Укажите запрос'
                isClearable={true}
                ref={selectInputRef as any}
            />
            <DatePickerWithTime value={schedule} onChange={setSchedule}/>
            {handler && handlerProps && <Extensions
                organization={organization}
                handler={handler}
                props={handlerProps}
                />}

        </>
    )
}