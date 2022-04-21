import { FC, useContext, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Context } from '..';
import { IOrganizationResponse } from '../models/IOrganizationResponse';
import { ICategoryResponse } from '../models/ICategoryResponse';
import { ICorporateNutritionResponse } from '../models/ICorporateNutritionResponse';
import { isFunctionOrConstructorTypeNode } from 'typescript';

interface item_option {
    id: string
    name:string
}

const UploadForm: FC = () => {
    const { organizationstore } = useContext(Context);

    const [organizationId, setOrganizationId] = useState('');

    const [categories, setCategories] = useState<ICategoryResponse[]>([]);
    const [categoryId, setCategoryId] = useState('');
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);
    const [corporateNutritionId, setCorporateNutritionId] = useState('');

    const [balance,setBalance] = useState<number>(0)

    const CustomSelect = ({ id, value, options, onChange }) => {
        return (
            <select className="custom-select" id={id} value={value} onChange={onChange}>
                {options.map(option =>
                    <option key={option.id} value={option.id}>{option.name}</option>
                )}
            </select>
        )
    }
    const onOrganizationSelectChange = (e) => {
        
        const orgId = e.target.options[e.target.selectedIndex].value
        const organization = organizationstore.organizations.find(org => org.id === orgId);

        setOrganizationId(organization?.id ?? '')
        setCategories(organization?.categories ?? [])
        setCategoryId(organization?.categories[0]?.id ?? '')
        setCorporateNutritions(organization?.corporateNutritions ?? [])
        setCorporateNutritionId(organization?.corporateNutritions[0]?.id ?? '')
    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setOrganizationId(organizationstore.organizations[0]?.id ?? []);

        setCategories(organizationstore.organizations[0]?.categories ?? []);
        setCategoryId(organizationstore.organizations[0]?.categories[0]?.id ?? '');

        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')



        organizationstore.setLoading(false);
        console.log('isLoading false')

    }

    useEffect(() => {
        firstInit();
        console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }
    console.log('UploadForm return')
    return (

        <div>
            <h1>UPLOAD FORM</h1>
            
            <p>организация: {organizationId}</p>
            <p>категория: {categoryId}</p>
            <p>корпит: {corporateNutritionId}</p>


            <div className="form-group col-md-6">
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations} onChange={onOrganizationSelectChange} />
            </div>
            <div className="form-group col-md-6">
                <label htmlFor="categories">Категории</label>
                <CustomSelect id="categories" value={categoryId} options={categories} onChange={event => setCategoryId(event.target.value)}/>
            </div>
            <div className="form-group col-md-6">
                <label htmlFor="corporateNutritions">Программы питания</label>
                <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions} onChange={event => setCorporateNutritionId(event.target.value)} />
            </div>

            <div className="form-group col-md-6">
                 <label htmlFor="balance">Баланс</label>
                <input
                    id='balance'
                    onChange={e => setBalance(parseInt(e.target.value))}
                    value={balance}
                    type='number'
                     placeholder='Баланс'
                />
            </div>

        </div>
    );
};
export default observer(UploadForm);
