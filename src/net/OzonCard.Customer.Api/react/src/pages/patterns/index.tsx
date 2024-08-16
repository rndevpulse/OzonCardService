import {FC, useContext, useEffect, useState} from "react";
import {observer} from "mobx-react-lite";
import * as React from "react";
import {Tab, TabList, TabPanel, Tabs} from "react-tabs";
import Select from "react-select";
import {ICategory, IOrganization, IProgram} from "../../models/org";
import {Context} from "../../index";
import {Batches} from "../../components/batch";


const PatternsPage: FC = () => {

    const {organizationStore, taskStore} = useContext(Context);


    const [organization, setOrganization] = useState<IOrganization>()

    const onOrganizationSelectChange = (organization : IOrganization) => {
        setOrganization(organization)
    }

    async function init(){
        await organizationStore.requestOrganizations();
        const org = organizationStore.organizations[0];
        setOrganization(org);
    }
    useEffect(() => {
        init()
    }, []);
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
                    />

                </TabPanel>
                <TabPanel>

                </TabPanel>
            </Tabs>
        </div>
    )
}

export default observer(PatternsPage);