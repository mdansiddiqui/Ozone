export class ActivityLogModel implements IActivityLogModel {
   
    Id: number;
    ApprovalStatusId: number | undefined;
  
    TableRowId: number | undefined;
 
    
    Remarks: string | undefined;
    RemarksById: number | undefined;
    
    
    constructor(data?: ActivityLogModel) {
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

    static fromJS(data: any): ActivityLogModel {
        data = typeof data === 'object' ? data : {};
        let result = new ActivityLogModel();
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

    clone(): ActivityLogModel {
        const json = this.toJSON();
        let result = new ActivityLogModel();
        result.init(json);
        return result;
    }
}

// export interface ILocationModel {
   
//     Id: number;
//     Code: string | undefined;
//     Name: string | undefined;
//     Description: string | undefined;
//     StartDate: string | undefined;
//     Remarks: boolean | undefined;
//     RemarksById: boolean | undefined;
  
// } {
//         if (data) {
//             for (var property in data) {
//                 if (data.hasOwnProperty(property))
//                     (<any>this)[property] = (<any>data)[property];
//             }
//         }
//     }

//     init(data?: any) {
//         if (data) {
//             this.code = data["code"];
//             this.name = data["name"];
//             this.id = data["id"];
//         }
//     }

//     static fromJS(data: any): StandardModel {
//         data = typeof data === 'object' ? data : {};
//         let result = new StandardModel();
//         result.init(data);
//         return result;
//     }

//     toJSON(data?: any) {
//         data = typeof data === 'object' ? data : {};
//         data["Id"] = this.Id;
//         data["Code"] = this.Code;
//         data["Name"] = this.Name;
//         data["Description"] = this.Description;
//         data["StartDate"] = this.StartDate;
//         data["Remarks"] = this.Remarks;
//         data["RemarksById"] = this.RemarksById;
        
//         return data; 
//     }

//     clone(): StandardModel {
//         const json = this.toJSON();
//         let result = new StandardModel();
//         result.init(json);
//         return result;
//     }
// }

export interface IActivityLogModel {
    Id: number;
    ApprovalStatusId: number | undefined;
  
    TableRowId: number | undefined;

    
    Remarks: string | undefined;
    RemarksById: number | undefined;


    
   
}