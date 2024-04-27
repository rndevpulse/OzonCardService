import { makeAutoObservable } from 'mobx';
import OrganizationService from '../services/OrganizationServise';
import {IOrganization} from "../api/models/org/IOrganization";



export default class OrganizationStore {
    organizations: IOrganization[] = []
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
            const response = await OrganizationService.getMyOrganizations();
            this.organizations = response.data;
            //console.log(response);
        }
        catch (e) {
            //console.log(e);
        }
        finally {
            this.setLoading(false)
        }
        
    }

    async updateOrganization(organizationId: string) {
        this.setLoading(true)
        try {
            const response = await OrganizationService.updateOrganization(organizationId)
            this.organizations = this.organizations.filter(f => f.id !== response.data.id)
            this.organizations.push(response.data)
            //console.log(response)
        }
        catch (e) {
            //console.log(e);
        }
        finally {
            this.setLoading(false)
        }
    }
    async createOrganization(login: string, password:string) {
        this.setLoading(true)
        try {
            const response = await OrganizationService.createOrganization(login, password)
            this.organizations.push(response.data)
            console.log(response)
        }
        catch (e) {
            //console.log(e);
        }
        finally {
            this.setLoading(false)
        }
    }

    

}

