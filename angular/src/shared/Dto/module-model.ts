export class ModuleModel implements IModuleModel {
   
    Id: number | undefined
    Name: string | undefined;
    IsActive: boolean | undefined;
    Code: string | undefined;
    Description: string | undefined;
    //IsDeleted: boolean | undefined;
    
    
    constructor(data?: ModuleModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }
    code: string;
    name: string;
    id: number;

    init(data?: any) {
        if (data) {
            this.Id = data["Id"];
            this.Name = data["Name"];
            this.IsActive = data["IsActive"];
            this.Code=data["Code"];
            this.Description=data["Description"];
           // this.IsDeleted = data["IsDeleted"];
            
        }
    }

    static fromJS(data: any): ModuleModel {
        data = typeof data === 'object' ? data : {};
        let result = new ModuleModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.Id;
        data["Name"] = this.Name;
        data["IsActive"] = this.IsActive;
        data["Code"]=this.Code;
        data["Description"]=this.Description;
       // data["IsDeleted"] = this.IsDeleted;
       
        return data; 
    }

    clone(): ModuleModel {
        const json = this.toJSON();
        let result = new ModuleModel();
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

export interface IModuleModel {
    Id: number;
    Name: string | undefined;
    IsActive: boolean | undefined;
    Code: string | undefined;
    Description: string | undefined;
  //  IsDeleted: boolean | undefined;


    
   
}