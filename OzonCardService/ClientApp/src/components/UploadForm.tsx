import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { IOrganizationResponse } from '../models/IOrganizationResponse';
import OrganizationService from '../services/OrganizationServise';
import { useEffect } from 'react';

const UploadForm: FC = () => {
    const [organizations, setOrganizations] = useState<IOrganizationResponse[]>([])
    async function getOrganizations() {
        try {
            const response = await OrganizationService.getMyOrganizations();
            console.log('UploadForm response.data = ',response.data)
            setOrganizations(response.data);
        }
        catch (e) {
            console.log(e);
        }
    }
    useEffect(() => {
        getOrganizations();
        console.log('UploadForm',organizations)
    }, [])
    return (
        <div>
            <h1>UPLOAD FORM fdg</h1>
            {organizations.map(org =>
                <div key={org?.Id ?? '1'}>{org.Name}</div>

            )}
        </div>
    );
};
export default UploadForm;
