import {ExtensionProps} from "./ExtensionProps";
import * as React from "react";
import {PropertyBehaviour} from "../../models/request";
import Select from "react-select";
import {useEffect, useState} from "react";
import {IBatch} from "../../models/batch";
import PropsService from "../../services/PropsService";

export function ExtensionGuidProperty({property, organization, onChangeValue}:ExtensionProps) {
    const [batches, setBatches] = useState<IBatch[]>([]);

    useEffect(() => {
        if (property.behaviour === PropertyBehaviour.OrganizationId
            && organization?.id !== property.value){

            onChangeValue(property, organization?.id)
        }
    });
    useEffect(() => {
        PropsService.getBatches().then(response=>
            setBatches(response.data))
    }, []);
    const castBehaviour = () => {
        console.log(`ExtensionGuidProperty: ${property.behaviour}`)
        switch (property.behaviour)
        {
            // case PropertyBehaviour.OrganizationId:
            //     return <>
            //         <label htmlFor={`organization_${property.name}`}>{property.label}</label>
            //         <Select
            //             id={`organization_${property.name}`}
            //             defaultValue={organization}
            //             onChange={value => onChangeValue(property, value?.id)}
            //             options={[organization] as IOrganization[]}
            //             getOptionLabel={option => option.name}
            //             getOptionValue={option => option.id}
            //             placeholder={property.label}
            //         />
            //     </>

            case PropertyBehaviour.CategoryId:
                return <>
                    <label htmlFor={`categories_${property.name}`}>{property.label}</label>
                    <Select
                        id={`categories_${property.name}`}
                        onChange={value => onChangeValue(property, value?.id)}
                        value={organization?.categories.filter(x => x.id === property.value as string)}
                        options={organization?.categories}
                        getOptionLabel={option => option.name}
                        getOptionValue={option => option.id}
                        placeholder={property.label}
                    />
                </>

            case PropertyBehaviour.ProgramId:
                return <>
                    <label htmlFor={`programs_${property.name}`}>{property.label}</label>
                    <Select
                        id={`programs_${property.name}`}
                        onChange={value => onChangeValue(property, value?.id)}
                        value={organization?.programs.filter(x=>x.id === property.value  as string)}
                        options={organization?.programs}
                        getOptionLabel={option => option.name}
                        getOptionValue={option => option.id}
                        placeholder={property.label}
                    />
                </>

            case PropertyBehaviour.BatchId:
                return <>
                    <label htmlFor={`batches_${property.name}`}>{property.label}</label>
                    <Select
                        id={`batches_${property.name}`}
                        onChange={value => onChangeValue(property, value?.id)}
                        value={batches.filter(x=>
                            x.organization === organization?.id
                            && x.id === property.value as string)}
                        options={batches.filter(x=>x.organization === organization?.id)}
                        getOptionLabel={option => option.name}
                        getOptionValue={option => option.id ?? "1"}
                        placeholder={property.label}
                        isClearable={true}
                    />
                </>

            default: return(<></>)
        }

    }


    return(<>{castBehaviour()}</>)
}