import {IOrganization} from "../../models/org";
import * as React from "react";
import {useToast} from "../toast";
import {useContext, useEffect, useRef, useState} from "react";
import {IExtensionProperty, IHandlerInfo, IRequestModel} from "../../models/request";
import RequestService from "../../services/RequestService";
import Select from "react-select";
import {DatePickerWithTime} from "../datePicker";
import {Extensions} from "../handlerProps";
import {Context} from "../../index";
import {Modal} from "../modal";
import {ModalContext} from "../../context/modal";
import {RequestConfirm} from "./RequestConfirm";
import "./index.css"

interface IRequestHandlersProps{
    organization: IOrganization
}

export function RequestHandlers({organization}:IRequestHandlersProps){
    const { taskStore} = useContext(Context);

    const toast = useToast();
    const [lastOrganization, setLastOrganization] = useState<string>('');
    const selectInputRef = useRef()

    const [handlers, setHandlers] = useState<IHandlerInfo[]>();
    const [handler, setHandler] = useState<IHandlerInfo>();
    const [handlerProps, setHandlerProps] = useState<IExtensionProperty[]>([]);

    const [schedule, setSchedule] = useState<Date>(new Date());
    //-(new Date().getTimezoneOffset())

    const {modal, open, close}=useContext(ModalContext)

    const onClear = () => {
        (selectInputRef.current as any).clearValue();
    };
    async function selectedHandler(handler:IHandlerInfo|undefined){
        if (handler !== undefined){
            setHandler(handler)
            let result = await RequestService.getHandlerProps(handler.key)
            setHandlerProps(result.data)
        }
    }
    function onChangePropValue(property:IExtensionProperty, value:any){
        // console.log(property)
        // console.log(value)
        property.value = value
        setHandlerProps(handlerProps.map(x=>x))
        // console.log(property)

    }
    async function tryConfirm(){
        if (handler === null)
        {
            toast.show("Отложенный запрос не указан")
            return
        }
        if (handler!.key.includes("companies")){
            open()
        }
        else {
            await appendBatch()
        }
    }
    async function appendBatch(description: string = "") {

        let task = await RequestService.append(
            handler!.key,{
                schedule:schedule,
                timeOffset: -(new Date().getTimezoneOffset()),
                properties: handlerProps
            },
            `${organization.name} отложено на ${schedule.toLocaleString()}: ` + handler?.name + ' ' + description)
        close()
        if (task === undefined){
            return
        }
        taskStore.onAddTask(task.data)
        // navigate(`/tasks`)
        toast.show("Отложенный запрос создан")

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
                // isClearable={true}
                ref={selectInputRef as any}
            />
            <DatePickerWithTime value={schedule} onChange={setSchedule}/>
            {handler && handlerProps && <Extensions
                organization={organization}
                handler={handler}
                props={handlerProps}
                onChangeValue={onChangePropValue}
            />}
            {handler && handlerProps && <button className="button"
                    onClick={tryConfirm}
            >
                Добавить
            </button>}
            {modal && handler && handlerProps && <Modal title={'Подтвердите действия'} onClose={close}>
                <RequestConfirm
                    handler={handler}
                    handlerProps={handlerProps}
                    schedule={schedule}
                    organization={organization}
                    onConfirmed={appendBatch}
                    />
            </Modal>}
        </>
    )
}