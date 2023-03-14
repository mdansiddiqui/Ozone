export class ProjectSA8000Model implements IProjectSA8000Model {
   
    Id: number;
    ApprovalStatusId: number | undefined;
  
    TableRowId: number | undefined;
 
    
    Remarks: string | undefined;
    RemarksById: number | undefined;
    
    
    constructor(data?: ProjectSA8000Model) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }y
    
   
    tableRowId: string;
    approvalStatusId: number;
    id: number;

    init(data?: any) {
        if (data) {

            this.Id = data["Id"];
            this.ApprovalStatusId = data["ApprovalStatusId"];
          
         
            this.TableRowId=data["TableRowId"];
            
            this.Remarks = data["Remarks"];
            this.RemarksById = data["RemarksById"];

            
        }
    }

    static fromJS(data: any): ProjectSA8000Model {
        data = typeof data === 'object' ? data : {};
        let result = new ProjectSA8000Model();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};

        data["Id"] = this.Id;
        data["ApprovalStatusId"] = this.ApprovalStatusId;
       
       
        data["TableRowId"]=this.TableRowId;
       
        data["Remarks"] = this.Remarks;
        data["RemarksById"] = this.RemarksById;
        
     
       
        return data; 
    }

    clone(): ProjectSA8000Model {
        const json = this.toJSON();
        let result = new ProjectSA8000Model();
        result.init(json);
        return result;
    }
}



export interface IProjectSA8000Model {
    Id: number;
    ApprovalStatusId: number | undefined;
  
    TableRowId: number | undefined;

    
    Remarks: string | undefined;
    RemarksById: number | undefined;


    
   
}