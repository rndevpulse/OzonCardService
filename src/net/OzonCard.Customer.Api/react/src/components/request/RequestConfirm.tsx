import {IExtensionProperty, IHandlerInfo} from "../../models/request";
import * as React from "react";
import {IOrganization} from "../../models/org";


interface IRequestConfirmProps {
    handler: IHandlerInfo
    handlerProps: IExtensionProperty[]
    organization: IOrganization
    schedule: Date
    onConfirmed: (description: string) => void
}

export function RequestConfirm({handler, handlerProps, schedule, organization, onConfirmed}:IRequestConfirmProps){

    function GetCategory():string{
        let catProp = handlerProps.find(x=>x.name === "category")
        return organization.categories.find(x=>x.id === catProp?.value)?.name ?? "undefined"
    }

    const castViewHandler = () => {
        switch(handler.key){
            case "d.request.companies.block":
                return ViewConfirmBlock();
            case "d.request.companies.unblock":
                return ViewConfirmUnblock();
        }
    }
    const ViewConfirmBlock = () => {
        let company = GetCategory()
        return(
            <>
                {`Компания '${company}' будет заблокирована\nСотрудники '${company}' не смогут получать питание по пропускам начиная с ${schedule.toLocaleString()}`}
            </>
        )
    }
    const ViewConfirmUnblock = () => {
        let company = GetCategory()
        return(
            <>
                {`Компания '${company}' будет разблокирована\nСотрудники '${company}' смогут получать питание и дотации по пропускам начиная с ${schedule.toLocaleString()}`}
            </>
        )
    }


    return(
        <div>
            <div className="description-confirmation">
                {castViewHandler()}

            </div>
            <button className="button"
                    onClick={() => onConfirmed(GetCategory())}>
                Продолжить
            </button>
        </div>

    )
}