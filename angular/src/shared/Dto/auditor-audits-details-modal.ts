export class AuditorAuditsDetailsModel implements IAuditorAuditsDetailsModel {
   
    Id: number | undefined;
    LeadAuditorName: string | undefined;
    ProjectCode: string | undefined;
    VisitLevel: string | undefined;
    NCS: string | undefined;
    AvgTAT_1: string | undefined;
   
    constructor(data?: AuditorAuditsDetailsModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }
  
    init(data?: any) {
        if (data) {
            this.Id = data["Id"];
            this.LeadAuditorName = data["LeadAuditorName"];
            this.ProjectCode = data["ProjectCode"];
            this.VisitLevel = data["VisitLevel"];
            this.NCS = data["NCS"];
            this.AvgTAT_1 = data["AvgTAT_1"];
            
        }
    }

    static fromJS(data: any): AuditorAuditsDetailsModel {
        data = typeof data === 'object' ? data : {};
        let result = new AuditorAuditsDetailsModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.Id;
        data["LeadAuditorName"] = this.LeadAuditorName;
        data["ProjectCode"] = this.ProjectCode;
        data["VisitLevel"] = this.VisitLevel;
        data["NCS"] = this.NCS;
        data["AvgTAT_1"] = this.AvgTAT_1;
        
        return data; 
    }

    clone(): AuditorAuditsDetailsModel {
        const json = this.toJSON();
        let result = new AuditorAuditsDetailsModel();
        result.init(json);
        return result;
    }
}
export interface IAuditorAuditsDetailsModel {
    Id: number| undefined
    LeadAuditorName: string | undefined;
    ProjectCode: string | undefined;
    VisitLevel: string | undefined;
    NCS: string | undefined;
    AvgTAT_1: string | undefined;
   // IsActive: boolean | undefined;
    //IsDeleted: boolean | undefined;


    
   
}