export class AuditDocumentsModel implements IAuditDocumentsModel {
   
    Id: number | undefined
    Code: string | undefined;
    Name: string | undefined;
    IsActive: boolean | undefined;
    IsDeleted: boolean | undefined;
    Description: string | undefined;
    CreatedBy: number | undefined;
    constructor(data?: AuditDocumentsModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }y
    
    code: string;
    name: string;
    id: number;

    init(data?: any) {
        if (data) {
            this.Id = data["Id"];
            this.Code = data["Code"];
            this.Name = data["Name"];
            this.IsActive = data["IsActive"];
            this.IsDeleted = data["IsDeleted"];
            this.Description=data["Description"];
            this.CreatedBy = data["CreatedBy"];

        }
    }

    static fromJS(data: any): AuditDocumentsModel {
        data = typeof data === 'object' ? data : {};
        let result = new AuditDocumentsModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.Id;
        data["Code"] = this.Code;
        data["Name"] = this.Name;
        data["IsActive"] = this.IsActive;
        data["IsDeleted"] = this.IsDeleted;
       data["Description"]=this.Description;
       data["CreatedBy"] = this.CreatedBy;

        return data; 
    }

    clone(): AuditDocumentsModel {
        const json = this.toJSON();
        let result = new AuditDocumentsModel();
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
//     IsActive: boolean | undefined;
//     IsDeleted: boolean | undefined;
  
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
//         data["IsActive"] = this.IsActive;
//         data["IsDeleted"] = this.IsDeleted;
        
//         return data; 
//     }

//     clone(): StandardModel {
//         const json = this.toJSON();
//         let result = new StandardModel();
//         result.init(json);
//         return result;
//     }
// }

export interface IAuditDocumentsModel {
    Id: number | undefined;
    Code: string | undefined;
    Name: string | undefined;
    IsActive: boolean | undefined;
    IsDeleted: boolean | undefined;
    Description: string | undefined;
    CreatedBy: number | undefined;

    
   
}
