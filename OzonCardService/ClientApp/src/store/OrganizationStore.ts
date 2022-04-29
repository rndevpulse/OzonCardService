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

    async updateOrganization(organizationId: string) {
        this.setLoading(true)
        try {
            const response = await OrganizationServise.updateOrganization(organizationId)
            this.organizations = this.organizations.filter(f => f.id !== response.data.id)
            this.organizations.push(response.data)
            console.log(response)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            this.setLoading(false)
        }
    }
    async createOrganization(email: string, password:string) {
        this.setLoading(true)
        try {
            const response = await OrganizationServise.createOrganization(email, password)
            this.organizations.push(response.data)
            console.log(response)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            this.setLoading(false)
        }
    }

}

