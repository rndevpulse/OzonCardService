import {ICategory, IOrganization} from "../../models/org";
import Select from "react-select";
import * as React from "react";
import {useState} from "react";


interface IBatchesProps{
    organization: IOrganization
}

export function Batches({organization}:IBatchesProps){
    const [categories, setCategories] = useState<ICategory[]>([]);


    return(
        <div>
            <label htmlFor="categories">Фильтр категорий</label>
            <Select
                id='categories'
                onChange={values => setCategories(values as ICategory[])}
                value={categories}
                options={organization?.categories}
                getOptionLabel={option => option.name}
                getOptionValue={option => option.id}
                placeholder='Категории'
                isMulti
            />
        </div>
    )
};