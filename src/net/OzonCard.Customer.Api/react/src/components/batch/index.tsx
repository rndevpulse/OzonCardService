import {IOrganization} from "../../models/org";
import Select from "react-select";
import * as React from "react";
import {useEffect, useRef, useState} from "react";
import {IBatch, IBatchProp} from "../../models/batch";
import {useToast} from "../toast";
import "./index.css"


interface IBatchesProps{
    organization: IOrganization,
    batches: IBatch[],
    onBatchesChanged: (batch : IBatch) => Promise<IBatch>,
    onPropRemove: (id: string) => void,
}

export function Batches({organization, batches, onBatchesChanged, onPropRemove}:IBatchesProps){

    const toast = useToast();
    const [lastOrganization, setLastOrganization] = useState<string>('');

    const [batch, setBatch] = useState<IBatch>();


    const [batchName, setBatchName] = useState<string>('');
    const [batchProps, setBatchProps] = useState<IBatchProp[]>([]);

    const selectInputRef = useRef()
    const selectedBatch = (batch :IBatch | undefined) => {
        setBatch(batch ?? undefined)
        setBatchName(batch?.name ?? '')
        setBatchProps(batch?.properties ?? [])
    }
    const onClear = () => {
        (selectInputRef.current as any).clearValue();
    };

    function appendBatch() {
        if (batchProps.find(x=>x.name == '') !== undefined){
            toast.show("Уже есть пустой шаблон", "warning")
            return
        }
        setBatchProps(
            batchProps.concat({
                name:'',
                aggregations:[]
            })
        )
    }
    function removeBatch(prop: IBatchProp) {
        setBatchProps(
            batchProps.filter(x=>x.name !== prop.name)
        )
    }
    function updateBatchName(prop: IBatchProp, value: string) {
        prop.name = value
        setBatchProps(batchProps.map(x=>x))
    }
    function updateBatchAggregations(prop: IBatchProp, value: string[]) {
        prop.aggregations = value
        setBatchProps(batchProps.map(x=>x))
    }

    const onViewBatches = (props:IBatchProp[]) => {
        return (
            props.map(prop=>
            <ul>
                <li>
                    <>
                        <label htmlFor="batchName">Наименование пакета
                            <input
                                id="batchName"
                                onChange={e => updateBatchName(prop, e.target.value)}
                                value={prop.name}
                                type='text'
                                placeholder='пакета'
                            />
                        </label>
                        <button className="button red"
                                onClick={e=>removeBatch(prop)}
                        >Убрать
                        </button>
                    </>

                </li>
                <li>
                    <Select
                        id='batchCategories'
                        onChange={values =>updateBatchAggregations(prop, values.map(x=>x.id))}
                        value={organization.categories.filter(x=>prop.aggregations.includes(x.id))}
                        options={organization.categories}
                        getOptionLabel={option => option.name}
                        getOptionValue={option => option.id}
                        placeholder='Категории'
                        isMulti
                    />
                </li>
            </ul>
        ))
    }


    async function sendBatch(id: string | undefined = undefined) {
        if (organization === undefined) {
            toast.show("Не указана организация", "warning")
            return
        }
        if (batchName.length === 0){
            toast.show("Не указано наименование шаблона", "warning")
            return
        }
        let temp = await onBatchesChanged({
            id:id,
            organization:organization?.id,
            name:batchName,
            properties:batchProps,
        })
        if (id === undefined) {
            selectedBatch(temp)
        }

    }
    function removeProp(id: string | undefined) {
        if (id === undefined) {
            toast.show("Не указан шаблон для удаления", "warning")
            return
        }
        onPropRemove(id);
        selectedBatch(undefined)
        onClear();
    }

    useEffect(() => {
        if (lastOrganization !== organization?.id)
        {
            setLastOrganization(organization?.id ?? '')
            selectedBatch(undefined)
            onClear();
        }
    });
    return (
        <>
            <Select
                id='batches'
                onChange={values => selectedBatch(values as IBatch | undefined)}
                value={batch}
                options={batches}
                getOptionLabel={option => option.name}
                getOptionValue={option => option.id ?? "1"}
                placeholder='Сохраненный шаблон'
                isClearable={true}
                ref={selectInputRef as any}
            />
            <div className="batch-container">
                <div className="container-row-customer container-row-wrap">
                    <label htmlFor="patternName">Наименование шаблона
                        <input
                            id="patternName"
                            onChange={e => setBatchName(e.target.value)}
                            value={batchName}
                            type='text'
                            placeholder='Шаблон'
                        />
                    </label>
                    <button className="button"
                            onClick={e => sendBatch()}
                    >Создать
                    </button>
                    <button className="button"
                            onClick={e => sendBatch(batch?.id)}
                    >Изменить
                    </button>
                    <button className="button red"
                            onClick={e => removeProp(batch?.id)}
                    >Удалить
                    </button>
                </div>
                {batchProps && onViewBatches(batchProps)}
                <div>
                    <button className="button"
                            onClick={appendBatch}
                    >
                        Добавить
                    </button>
                </div>
            </div>
        </>

    )
};