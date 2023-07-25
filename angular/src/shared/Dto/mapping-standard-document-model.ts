export class MappingStandardDocumentModel implements IMappingStandardDocumentModel {

    Id: number | undefined
    StandardId: number | undefined;
    DocumentTypeId: number | undefined;
    VisitLevelId: number | undefined
    IsActive: boolean | undefined;
    IsRequired: boolean | undefined;
    IsDeleted: boolean | undefined;
    DocumentForReviewer: boolean | undefined;
    DocumentAssignId :number | undefined;

    constructor(data?: MappingStandardDocumentModel) {
        if (data) {
            for (let property in data) {
                if (data.hasOwnProperty(property))
                    (this as any)[property] = (data as any)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.Id = data["Id"]
            this.StandardId = data["StandardId"];
            this.DocumentTypeId = data["DocumentTypeId"];
            this.VisitLevelId = data["VisitLevelId"];
            this.IsActive = data["IsActive"];
            this.IsRequired = data["IsRequired"];
            this.IsDeleted = data["IsDeleted"];
            this.DocumentForReviewer=data["DocumentForReviewer"];
            this.DocumentForReviewer=data["DocumentAssignId"];
            
        }
    }

    static fromJS(data: any): MappingStandardDocumentModel {
        data = typeof data === 'object' ? data : {};
        let result = new MappingStandardDocumentModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.Id;
        data["StandardId"] = this.StandardId;
        data["DocumentTypeId"] = this.DocumentTypeId;
        data["VisitLevelId"] = this.VisitLevelId;
        data["IsActive"] = this.IsActive;
        data["IsRequired"] = this.IsRequired;
        data["IsDeleted"] = this.IsDeleted;
        data["DocumentForReviewer"]=this.DocumentForReviewer;
        data["DocumentAssignId"]=this.DocumentAssignId;
        
        return data;
    }

    clone(): MappingStandardDocumentModel {
        const json = this.toJSON();
        let result = new MappingStandardDocumentModel();
        result.init(json);
        return result;
    }
}

export interface IMappingStandardDocumentModel {
    Id: number | undefined;
    StandardId: number | undefined;
    DocumentTypeId: number | undefined;
    VisitLevelId: number | undefined;
    IsActive: boolean | undefined;
    IsRequired: boolean | undefined
    IsDeleted: boolean | undefined;
    DocumentForReviewer:boolean | undefined;
    DocumentAssignId : number | undefined;
}
