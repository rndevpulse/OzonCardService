
export interface IBatch {
    id?: string
    organization: string
    name: string
    properties:IBatchProp[]
}

export interface IBatchProp {
    name: string
    aggregations: string[]
}