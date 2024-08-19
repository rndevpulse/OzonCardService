import {FC, useContext, useEffect, useState} from "react";
import {observer} from "mobx-react-lite";
import * as React from "react";
import {Tab, TabList, TabPanel, Tabs} from "react-tabs";
import Select from "react-select";
import {IOrganization} from "../../models/org";
import {Context} from "../../index";
import {Batches} from "../../components/batch";
import {IBatch} from "../../models/batch";
import PropsService from "../../services/PropsService";
import {toast} from "react-toastify";


const PatternsPage: FC = () => {

    const {organizationStore} = useContext(Context);
    const [batches, setBatches] = useState<IBatch[]>([]);


    const [organization, setOrganization] = useState<IOrganization>()

    const onOrganizationSelectChange = (organization : IOrganization) => {
        setOrganization(organization)
    }

    async function init(){
        await organizationStore.requestOrganizations();
        const org = organizationStore.organizations[0];
        setOrganization(org);
        const response = await PropsService.getBatches();
        setBatches(response.data)
    }
    useEffect(() => {
        init()
    }, []);

    async function onOrganizationBatchesChanged(batch: IBatch) {
        const response = await PropsService.setBatch(batch)
        const temp = batches.find(x=>x.id === response.data.id)
        if (temp !== undefined)
        {
            temp.name = response.data.name
            temp.properties = response.data.properties
            setBatches(batches.map(x=>x))
            toast.info("Шаблон изменен")
            return
        }
        setBatches(batches.concat(response.data))
        toast.info("Шаблон добавлен")

    }

    return (
        <div className="center form-group col-md-12">
            <h1>Шаблоны</h1>

            <label htmlFor="organizations">Организации</label>
            <Select
                id='organizations'
                value={organization}
                options={organizationStore.organizations}
                getOptionLabel={option => option.name}
                getOptionValue={option => option.id}
                onChange={organization => onOrganizationSelectChange(organization as IOrganization)}
            />

            <Tabs>
                <TabList>
                    <Tab>Отчеты</Tab>
                    <Tab>Задачи</Tab>
                </TabList>
                <TabPanel>
                    <Batches
                        organization={organization as IOrganization}
                        batches={batches.filter(x=>x.organization === organization?.id)}
                        onBatchesChanged={async batch=> await onOrganizationBatchesChanged(batch) }
                    />

                </TabPanel>
                <TabPanel>
                    <h1>Данный раздел еще находится в разработке</h1>
                </TabPanel>
            </Tabs>
        </div>
    )
}

export default observer(PatternsPage);