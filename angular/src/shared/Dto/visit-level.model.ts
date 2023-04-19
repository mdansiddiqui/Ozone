export class VisitLevel implements IVisitLevel {
    Id: number | undefined
    Code: string | undefined;
    Name: string | undefined;
    IsActive: boolean | undefined;
    IsDeleted: boolean | undefined;
    Description: string | undefined;

    constructor(data?: VisitLevel) {
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
        }
    }

    static fromJS(data: any): VisitLevel {
        data = typeof data === 'object' ? data : {};
        let result = new VisitLevel();
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
        return data;
    }

    clone(): VisitLevel {
        const json = this.toJSON();
        let result = new VisitLevel();
        result.init(json);
        return result;
    }
}



export interface IVisitLevel {
    Id: number;
    Code: string | undefined;
    Name: string | undefined;
    IsActive: boolean | undefined;
    IsDeleted: boolean | undefined;
    Description: string | undefined;
}
