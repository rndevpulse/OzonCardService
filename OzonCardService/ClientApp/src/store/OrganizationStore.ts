import axios from 'axios';
import { makeAutoObservable } from 'mobx';
import { IOrganizationResponse } from '../models/IOrganizationResponse';
import OrganizationServise from '../services/OrganizationServise';



export default class OrganizationStore {
    organizations: IOrganizationResponse[] = []
    isLoading = false;
    public constructor() {
        makeAutoObservable(this);
    }

    setLoading(bool: boolean) {
        this.isLoading = bool;
    }

    async requestOrganizations() {
        this.setLoading(true);
        try {
            const response = await OrganizationServise.getMyOrganizations();
            this.organizations = response.data;
            console.log(response);
        }
        catch (e) {
            console.log(e);
        }
        
    }


}

