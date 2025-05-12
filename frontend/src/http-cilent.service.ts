import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable, NgModule } from "@angular/core";
import { ApiEndpoints } from "./app/core/api-endpoints";
import { catchError, of } from "rxjs";

@Injectable({providedIn: 'root'})
export class HttpClientService{

    constructor(private httpClient: HttpClient){

    }

    public getShortenedUrl(originalUrl: string){
        let url = ApiEndpoints.UrlShortener;
        const params = new HttpParams().set("originalUrl", originalUrl)
        return this.httpClient.post(url, null, {params}).pipe(
            catchError((error) => {
                if(error.status == 429){
                    let msg = "Too many requests";
                    return of({message: msg});
                }
                return of({});
            })
        );
    }
}